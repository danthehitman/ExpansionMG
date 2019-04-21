using HML.Expansion.Common;
using HML.Expansion.WorldMap.Tile;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HML.Expansion.WorldMap
{

    public abstract class Generator
    {
        protected int Seed;

        private GameWorld world;
        private Random rng;
        protected int width = 100;
        protected int height = 100;

        // Adjustable variables for Unity Inspector

        //Original Config
        //Height Map
        public static int TerrainOctaves = 6;
        public static float TerrainFrequency = 1.25f;
        public static float DeepWater = 0.2f;
        public static float ShallowWater = 0.4f;
        public static float Shore = 0.43f;
        public static float Sand = 0.5f;
        public static float Grass = 0.7f;
        public static float Forest = 0.8f;
        public static float Rock = 0.9f;

        //Less water, more mountains?
        //Height Map
        //protected int TerrainOctaves = 6;
        //protected double TerrainFrequency = 1.25;
        //protected float DeepWater = 0.2f;
        //protected float ShallowWater = 0.3f;
        //protected float Sand = 0.4f;
        //protected float Grass = 0.5f;
        //protected float Forest = 0.7f;
        //protected float Rock = 0.9f;

        //Heat Map
        //Original
        //protected int HeatOctaves = 4;
        protected int HeatOctaves = 4;
        protected float HeatFrequency = 3.0f;
        protected float ColdestValue = 0.05f;
        protected float ColderValue = 0.18f;
        protected float ColdValue = 0.4f;
        protected float WarmValue = 0.6f;
        protected float WarmerValue = 0.8f;

        //Moisture Map
        protected int MoistureOctaves = 4;
        //Bumping this up made a big difference for more grasslands and deserts
        protected float MoistureFrequency = 4.0f;
        protected float DryerValue = 0.27f;
        protected float DryValue = 0.4f;
        protected float WetValue = 0.6f;
        protected float WetterValue = 0.8f;
        protected float WettestValue = 0.9f;

        //Original
        //Moisture Map
        //protected int MoistureOctaves = 4;
        //protected double MoistureFrequency = 3.0;
        //protected float DryerValue = 0.27f;
        //protected float DryValue = 0.4f;
        //protected float WetValue = 0.6f;
        //protected float WetterValue = 0.8f;
        //protected float WettestValue = 0.9f;

        //Rivers
        private int RiverCount;
        private int MinRiverTurns;
        private float MinRiverHeight = 0.6f;
        private int MinRiverLength;
        private int MaxRiverIntersections = 2;
        private int MaxRiverSize;

        protected MapData HeightData;
        protected MapData HeatData;
        protected MapData MoistureData;
        protected MapData Clouds1;
        protected MapData Clouds2;

        public BaseTile[,] Tiles;

        protected List<TileGroup> Waters = new List<TileGroup>();
        protected List<TileGroup> Lands = new List<TileGroup>();

        protected List<River> Rivers = new List<River>();
        protected List<RiverGroup> RiverGroups = new List<RiverGroup>();

        protected BiomeType[,] BiomeTable = new BiomeType[6, 6] {   
        //COLDEST        //COLDER          //COLD                  //HOT                          //HOTTER                       //HOTTEST
        { BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland,    BiomeType.Desert,              BiomeType.Desert,              BiomeType.Desert },              //DRYEST
        { BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland,    BiomeType.Desert,              BiomeType.Desert,              BiomeType.Desert },              //DRYER
        { BiomeType.Ice, BiomeType.Tundra, BiomeType.Woodland,     BiomeType.Grassland,            BiomeType.Savanna,             BiomeType.Savanna },             //DRY
        { BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.Woodland,            BiomeType.Grassland,             BiomeType.Savanna },             //WET
        { BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.SeasonalForest,      BiomeType.TropicalRainforest,  BiomeType.TropicalRainforest },  //WETTER
        { BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.TemperateRainforest, BiomeType.TropicalRainforest,  BiomeType.TropicalRainforest }   //WETTEST
    };

        //protected BiomeType[,] BiomeTable = new BiomeType[6,6] {   
        //    //COLDEST        //COLDER          //COLD                  //HOT                          //HOTTER                       //HOTTEST
        //    { BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland,    BiomeType.Desert,              BiomeType.Desert,              BiomeType.Desert },              //DRYEST
        //    { BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland,    BiomeType.Desert,              BiomeType.Desert,              BiomeType.Desert },              //DRYER
        //    { BiomeType.Ice, BiomeType.Tundra, BiomeType.Woodland,     BiomeType.Woodland,            BiomeType.Savanna,             BiomeType.Savanna },             //DRY
        //    { BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.Woodland,            BiomeType.Savanna,             BiomeType.Savanna },             //WET
        //    { BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.SeasonalForest,      BiomeType.TropicalRainforest,  BiomeType.TropicalRainforest },  //WETTER
        //    { BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.TemperateRainforest, BiomeType.TropicalRainforest,  BiomeType.TropicalRainforest }   //WETTEST
        //};

        public Generator(GameWorld world, int height, int width, int seed)
        {
            this.world = world;
            this.height = height;
            this.width = width;
            RiverCount = (width / 32) * 3;
            MinRiverTurns = (width / 256) * 10;
            MinRiverLength = (width / 32) * 3;
            MaxRiverSize = (width / 128);

            Console.WriteLine("RiverCount: " + RiverCount + " MinRiverTurns: " + MinRiverTurns + " MinRiverLength: " +
                MinRiverLength + " MaxRiverSize: " + MaxRiverSize);

            //Generate a random seed if none is provided.
            if (seed == -1)
                Seed = new Random().Next(0, int.MaxValue);
            else
                Seed = seed;
            rng = new Random(Seed);

            //HeightMapRenderer = transform.Find ("HeightTexture").GetComponent<MeshRenderer> ();
            //HeatMapRenderer = transform.Find ("HeatTexture").GetComponent<MeshRenderer> ();
            //MoistureMapRenderer = transform.Find ("MoistureTexture").GetComponent<MeshRenderer> ();
            //BiomeMapRenderer = transform.Find ("BiomeTexture").GetComponent<MeshRenderer> ();

            Initialize();
            Generate();
        }

        protected abstract void Initialize();
        protected abstract void GetData();

        protected abstract BaseTile GetTop(BaseTile tile);
        protected abstract BaseTile GetBottom(BaseTile tile);
        protected abstract BaseTile GetLeft(BaseTile tile);
        protected abstract BaseTile GetRight(BaseTile tile);

        protected virtual void Generate()
        {
            GetData();
            LoadTiles();
            UpdateNeighbors();

            GenerateRivers();
            BuildRiverGroups();
            DigRiverGroups();
            AdjustMoistureMap();

            UpdateBitmasks();
            FloodFill();

            GenerateBiomeMap();
            UpdateBiomeBitmask();

            //HeightMapRenderer.materials [0].mainTexture = TextureGenerator.GetHeightMapTexture (Width, Height, Tiles);
            //HeatMapRenderer.materials[0].mainTexture = TextureGenerator.GetHeatMapTexture (Width, Height, Tiles);
            //MoistureMapRenderer.materials[0].mainTexture = TextureGenerator.GetMoistureMapTexture (Width, Height, Tiles);
            //BiomeMapRenderer.materials[0].mainTexture = TextureGenerator.GetBiomeMapTexture (Width, Height, Tiles, ColdestValue, ColderValue, ColdValue);
        }

        private void UpdateBiomeBitmask()
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    Tiles[x, y].UpdateBiomeBitmask();
                }
            }
        }

        public BiomeType GetBiomeType(BaseTile tile)
        {
            return BiomeTable[(int)tile.TerrainData.MoistureType, (int)tile.TerrainData.HeatType];
        }

        private void GenerateBiomeMap()
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {

                    if (!Tiles[x, y].TerrainData.Collidable) continue;

                    BaseTile t = Tiles[x, y];
                    t.TerrainData.BiomeType = GetBiomeType(t);
                }
            }
        }

        private void AddMoisture(BaseTile t, int radius)
        {
            int startx = MathUtilities.Mod(t.X - radius, width);
            int endx = MathUtilities.Mod(t.X + radius, width);
            Vector2 center = new Vector2(t.X, t.Y);
            int curr = radius;

            while (curr > 0)
            {

                int x1 = MathUtilities.Mod(t.X - curr, width);
                int x2 = MathUtilities.Mod(t.X + curr, width);
                int y = t.Y;

                AddMoisture(Tiles[x1, y], 0.025f / (center - new Vector2(x1, y)).Length());

                for (int i = 0; i < curr; i++)
                {
                    AddMoisture(Tiles[x1, MathUtilities.Mod(y + i + 1, height)], 0.025f / (center - new Vector2(x1, MathUtilities.Mod(y + i + 1, height))).Length());
                    AddMoisture(Tiles[x1, MathUtilities.Mod(y - (i + 1), height)], 0.025f / (center - new Vector2(x1, MathUtilities.Mod(y - (i + 1), height))).Length());

                    AddMoisture(Tiles[x2, MathUtilities.Mod(y + i + 1, height)], 0.025f / (center - new Vector2(x2, MathUtilities.Mod(y + i + 1, height))).Length());
                    AddMoisture(Tiles[x2, MathUtilities.Mod(y - (i + 1), height)], 0.025f / (center - new Vector2(x2, MathUtilities.Mod(y - (i + 1), height))).Length());
                }
                curr--;
            }
        }

        private void AddMoisture(BaseTile t, float amount)
        {
            MoistureData.Data[t.X, t.Y] += amount;
            t.TerrainData.MoistureValue += amount;
            if (t.TerrainData.MoistureValue > 1)
                t.TerrainData.MoistureValue = 1;

            //set moisture type
            if (t.TerrainData.MoistureValue < DryerValue) t.TerrainData.MoistureType = MoistureType.Dryest;
            else if (t.TerrainData.MoistureValue < DryValue) t.TerrainData.MoistureType = MoistureType.Dryer;
            else if (t.TerrainData.MoistureValue < WetValue) t.TerrainData.MoistureType = MoistureType.Dry;
            else if (t.TerrainData.MoistureValue < WetterValue) t.TerrainData.MoistureType = MoistureType.Wet;
            else if (t.TerrainData.MoistureValue < WettestValue) t.TerrainData.MoistureType = MoistureType.Wetter;
            else t.TerrainData.MoistureType = MoistureType.Wettest;
        }

        private void AdjustMoistureMap()
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {

                    BaseTile t = Tiles[x, y];
                    if (t.TerrainData.HeightType == HeightType.River)
                    {
                        AddMoisture(t, (int)60);
                    }
                }
            }
        }

        private void DigRiverGroups()
        {
            for (int i = 0; i < RiverGroups.Count; i++)
            {

                RiverGroup group = RiverGroups[i];
                River longest = null;

                //Find longest river in this group
                for (int j = 0; j < group.Rivers.Count; j++)
                {
                    River river = group.Rivers[j];
                    if (longest == null)
                        longest = river;
                    else if (longest.Tiles.Count < river.Tiles.Count)
                        longest = river;
                }

                if (longest != null)
                {
                    //Dig out longest path first
                    DigRiver(longest);

                    for (int j = 0; j < group.Rivers.Count; j++)
                    {
                        River river = group.Rivers[j];
                        if (river != longest)
                        {
                            DigRiver(river, longest);
                        }
                    }
                }
            }
        }

        private void BuildRiverGroups()
        {
            //loop each tile, checking if it belongs to multiple rivers
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    BaseTile t = Tiles[x, y];

                    if (t.TerrainData.Rivers.Count > 1)
                    {
                        // multiple rivers == intersection
                        RiverGroup group = null;

                        // Does a rivergroup already exist for this group?
                        for (int n = 0; n < t.TerrainData.Rivers.Count; n++)
                        {
                            River tileriver = t.TerrainData.Rivers[n];
                            for (int i = 0; i < RiverGroups.Count; i++)
                            {
                                for (int j = 0; j < RiverGroups[i].Rivers.Count; j++)
                                {
                                    River river = RiverGroups[i].Rivers[j];
                                    if (river.ID == tileriver.ID)
                                    {
                                        group = RiverGroups[i];
                                    }
                                    if (group != null) break;
                                }
                                if (group != null) break;
                            }
                            if (group != null) break;
                        }

                        // existing group found -- add to it
                        if (group != null)
                        {
                            for (int n = 0; n < t.TerrainData.Rivers.Count; n++)
                            {
                                if (!group.Rivers.Contains(t.TerrainData.Rivers[n]))
                                    group.Rivers.Add(t.TerrainData.Rivers[n]);
                            }
                        }
                        else   //No existing group found - create a new one
                        {
                            group = new RiverGroup();
                            for (int n = 0; n < t.TerrainData.Rivers.Count; n++)
                            {
                                group.Rivers.Add(t.TerrainData.Rivers[n]);
                            }
                            RiverGroups.Add(group);
                        }
                    }
                }
            }
        }

        public float GetHeightValue(BaseTile tile)
        {
            if (tile == null)
                return int.MaxValue;
            else
                return tile.TerrainData.HeightValue;
        }

        private void GenerateRivers()
        {
            int rivercount = RiverCount;
            Rivers = new List<River>();
            int attempts = 0;

            foreach (int x in Enumerable.Range(0, width).OrderBy(r => rng.Next()))
            {
                foreach (int y in Enumerable.Range(0, height).OrderBy(rr => rng.Next()))
                {
                    BaseTile tile = Tiles[x, y];
                    attempts++;
                    // validate the tile
                    if (!tile.TerrainData.Collidable) continue;
                    if (tile.TerrainData.Rivers.Count > 0) continue;

                    if (tile.TerrainData.HeightValue > MinRiverHeight)
                    {
                        // Tile is good to start river from
                        River river = new River(rivercount);

                        // Figure out the direction this river will try to flow
                        river.CurrentDirection = GetLowestNeighbor(tile);

                        // Recursively find a path to water
                        FindPathToWater(tile, river.CurrentDirection, ref river);

                        // Validate the generated river 
                        if (river.TurnCount < MinRiverTurns || river.Tiles.Count < MinRiverLength || river.Intersections > MaxRiverIntersections)
                        {
                            //Validation failed - remove this river
                            for (int iii = 0; iii < river.Tiles.Count; iii++)
                            {
                                BaseTile t = river.Tiles[iii];
                                t.TerrainData.Rivers.Remove(river);
                            }
                        }
                        else if (river.Tiles.Count >= MinRiverLength)
                        {
                            //Validation passed - Add river to list
                            Rivers.Add(river);
                            tile.TerrainData.Rivers.Add(river);
                            rivercount--;
                        }
                        if (rivercount == 0)
                            goto End;
                    }
                }
            }
        End:
            Console.WriteLine(rivercount.ToString() + " : " + attempts);
        }

        // Dig river based on a parent river vein
        private void DigRiver(River river, River parent)
        {
            int intersectionID = 0;
            int intersectionSize = 0;

            // determine point of intersection
            for (int i = 0; i < river.Tiles.Count; i++)
            {
                BaseTile t1 = river.Tiles[i];
                for (int j = 0; j < parent.Tiles.Count; j++)
                {
                    BaseTile t2 = parent.Tiles[j];
                    if (t1 == t2)
                    {
                        intersectionID = i;
                        intersectionSize = t2.TerrainData.RiverSize;
                    }
                }
            }

            int counter = 0;
            int intersectionCount = river.Tiles.Count - intersectionID;
            int size = rng.Next(intersectionSize, 5);
            river.Length = river.Tiles.Count;

            // randomize size change
            int two = river.Length / 2;
            int three = two / 2;
            int four = three / 2;
            int five = four / 2;

            int twomin = two / 3;
            int threemin = three / 3;
            int fourmin = four / 3;
            int fivemin = five / 3;

            // randomize length of each size
            int count1 = rng.Next(fivemin, five);
            if (size < 4)
            {
                count1 = 0;
            }
            int count2 = count1 + rng.Next(fourmin, four);
            if (size < 3)
            {
                count2 = 0;
                count1 = 0;
            }
            int count3 = count2 + rng.Next(threemin, three);
            if (size < 2)
            {
                count3 = 0;
                count2 = 0;
                count1 = 0;
            }
            int count4 = count3 + rng.Next(twomin, two);

            // Make sure we are not digging past the river path
            if (count4 > river.Length)
            {
                int extra = count4 - river.Length;
                while (extra > 0)
                {
                    if (count1 > 0) { count1--; count2--; count3--; count4--; extra--; }
                    else if (count2 > 0) { count2--; count3--; count4--; extra--; }
                    else if (count3 > 0) { count3--; count4--; extra--; }
                    else if (count4 > 0) { count4--; extra--; }
                }
            }

            // adjust size of river at intersection point
            if (intersectionSize == 1)
            {
                count4 = intersectionCount;
                count1 = 0;
                count2 = 0;
                count3 = 0;
            }
            else if (intersectionSize == 2)
            {
                count3 = intersectionCount;
                count1 = 0;
                count2 = 0;
            }
            else if (intersectionSize == 3)
            {
                count2 = intersectionCount;
                count1 = 0;
            }
            else if (intersectionSize == 4)
            {
                count1 = intersectionCount;
            }
            else
            {
                count1 = 0;
                count2 = 0;
                count3 = 0;
                count4 = 0;
            }

            // dig out the river
            for (int i = river.Tiles.Count - 1; i >= 0; i--)
            {

                BaseTile t = river.Tiles[i];

                if (counter < count1 && MaxRiverSize > 3)
                {
                    t.DigRiver(river, 4);
                }
                else if (counter < count2 && MaxRiverSize > 2)
                {
                    t.DigRiver(river, 3);
                }
                else if (counter < count3 && MaxRiverSize > 1)
                {
                    t.DigRiver(river, 2);
                }
                else if (counter < count4 && MaxRiverSize > 0)
                {
                    t.DigRiver(river, 1);
                }
                else
                {
                    t.DigRiver(river, 0);
                }
                counter++;
            }
        }

        // Dig river
        private void DigRiver(River river)
        {
            int counter = 0;

            // How wide are we digging this river?
            int size = rng.Next(1, 5);
            //DEF: Try this
            //int size = 1;
            river.Length = river.Tiles.Count;

            // randomize size change
            int two = river.Length / 2;
            int three = two / 2;
            int four = three / 2;
            int five = four / 2;

            int twomin = two / 3;
            int threemin = three / 3;
            int fourmin = four / 3;
            int fivemin = five / 3;

            // randomize lenght of each size
            int count1 = rng.Next(fivemin, five);
            if (size < 4)
            {
                count1 = 0;
            }
            int count2 = count1 + rng.Next(fourmin, four);
            if (size < 3)
            {
                count2 = 0;
                count1 = 0;
            }
            int count3 = count2 + rng.Next(threemin, three);
            if (size < 2)
            {
                count3 = 0;
                count2 = 0;
                count1 = 0;
            }
            int count4 = count3 + rng.Next(twomin, two);

            // Make sure we are not digging past the river path
            if (count4 > river.Length)
            {
                int extra = count4 - river.Length;
                while (extra > 0)
                {
                    if (count1 > 0) { count1--; count2--; count3--; count4--; extra--; }
                    else if (count2 > 0) { count2--; count3--; count4--; extra--; }
                    else if (count3 > 0) { count3--; count4--; extra--; }
                    else if (count4 > 0) { count4--; extra--; }
                }
            }

            // Dig it out
            for (int i = river.Tiles.Count - 1; i >= 0; i--)
            {
                BaseTile t = river.Tiles[i];

                if (counter < count1 && MaxRiverSize > 3)
                {
                    t.DigRiver(river, 4);
                }
                else if (counter < count2 && MaxRiverSize > 2)
                {
                    t.DigRiver(river, 3);
                }
                else if (counter < count3 && MaxRiverSize > 1)
                {
                    t.DigRiver(river, 2);
                }
                else if (counter < count4 && MaxRiverSize > 0)
                {
                    t.DigRiver(river, 1);
                }
                else
                {
                    t.DigRiver(river, 0);
                }
                counter++;
            }
        }

        private void FindPathToWater(BaseTile tile, Direction direction, ref River river)
        {
            if (tile.TerrainData.Rivers.Contains(river))
                return;

            // check if there is already a river on this tile
            if (tile.TerrainData.Rivers.Count > 0)
                river.Intersections++;

            river.AddTile(tile);

            // get neighbors
            BaseTile left = GetLeft(tile);
            BaseTile right = GetRight(tile);
            BaseTile top = GetTop(tile);
            BaseTile bottom = GetBottom(tile);

            float leftValue = int.MaxValue;
            float rightValue = int.MaxValue;
            float topValue = int.MaxValue;
            float bottomValue = int.MaxValue;

            // query height values of neighbors
            if (left != null && left.GetRiverNeighborCount(river) < 2 && !river.Tiles.Contains(left))
                leftValue = left.TerrainData.HeightValue;
            if (right != null && right.GetRiverNeighborCount(river) < 2 && !river.Tiles.Contains(right))
                rightValue = right.TerrainData.HeightValue;
            if (top != null && top.GetRiverNeighborCount(river) < 2 && !river.Tiles.Contains(top))
                topValue = top.TerrainData.HeightValue;
            if (bottom != null && bottom.GetRiverNeighborCount(river) < 2 && !river.Tiles.Contains(bottom))
                bottomValue = bottom.TerrainData.HeightValue;

            // if neighbor is existing river that is not this one, flow into it
            if (bottom != null && bottom.TerrainData.Rivers.Count == 0 && !bottom.TerrainData.Collidable)
                bottomValue = 0;
            if (top != null && top.TerrainData.Rivers.Count == 0 && !top.TerrainData.Collidable)
                topValue = 0;
            if (left != null && left.TerrainData.Rivers.Count == 0 && !left.TerrainData.Collidable)
                leftValue = 0;
            if (right != null && right.TerrainData.Rivers.Count == 0 && !right.TerrainData.Collidable)
                rightValue = 0;

            // override flow direction if a tile is significantly lower
            if (direction == Direction.Left)
                if (Math.Abs(rightValue - leftValue) < 0.1f)
                    rightValue = int.MaxValue;
            if (direction == Direction.Right)
                if (Math.Abs(rightValue - leftValue) < 0.1f)
                    leftValue = int.MaxValue;
            if (direction == Direction.Top)
                if (Math.Abs(topValue - bottomValue) < 0.1f)
                    bottomValue = int.MaxValue;
            if (direction == Direction.Bottom)
                if (Math.Abs(topValue - bottomValue) < 0.1f)
                    topValue = int.MaxValue;

            // find mininum
            float min = Math.Min(Math.Min(Math.Min(leftValue, rightValue), topValue), bottomValue);

            // if no minimum found - exit
            if (min == int.MaxValue)
                return;

            //Move to next neighbor
            if (min == leftValue)
            {
                if (left != null && left.TerrainData.Collidable)
                {
                    if (river.CurrentDirection != Direction.Left)
                    {
                        river.TurnCount++;
                        river.CurrentDirection = Direction.Left;
                    }
                    FindPathToWater(left, direction, ref river);
                }
            }
            else if (min == rightValue)
            {
                if (right != null && right.TerrainData.Collidable)
                {
                    if (river.CurrentDirection != Direction.Right)
                    {
                        river.TurnCount++;
                        river.CurrentDirection = Direction.Right;
                    }
                    FindPathToWater(right, direction, ref river);
                }
            }
            else if (min == bottomValue)
            {
                if (bottom != null && bottom.TerrainData.Collidable)
                {
                    if (river.CurrentDirection != Direction.Bottom)
                    {
                        river.TurnCount++;
                        river.CurrentDirection = Direction.Bottom;
                    }
                    FindPathToWater(bottom, direction, ref river);
                }
            }
            else if (min == topValue)
            {
                if (top != null && top.TerrainData.Collidable)
                {
                    if (river.CurrentDirection != Direction.Top)
                    {
                        river.TurnCount++;
                        river.CurrentDirection = Direction.Top;
                    }
                    FindPathToWater(top, direction, ref river);
                }
            }
        }

        // Build a Tile array from our data
        private void LoadTiles()
        {
            Tiles = new BaseTile[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    BaseTile t = new BaseTile(world, x, y);
                    t.X = x;
                    t.Y = y;

                    //set heightmap value
                    float heightValue = HeightData.Data[x, y];
                    heightValue = (heightValue - HeightData.Min) / (HeightData.Max - HeightData.Min);
                    t.TerrainData.HeightValue = heightValue;


                    if (heightValue < DeepWater)
                    {
                        t.TerrainData.HeightType = HeightType.DeepWater;
                        t.TerrainData.Collidable = false;
                    }
                    else if (heightValue < ShallowWater)
                    {
                        t.TerrainData.HeightType = HeightType.ShallowWater;
                        t.TerrainData.Collidable = false;
                    }
                    else if (heightValue < Shore)
                    {
                        t.TerrainData.HeightType = HeightType.Shore;
                        t.TerrainData.Collidable = true;
                    }
                    else if (heightValue < Sand)
                    {
                        t.TerrainData.HeightType = HeightType.Sand;
                        t.TerrainData.Collidable = true;
                    }
                    else if (heightValue < Grass)
                    {
                        t.TerrainData.HeightType = HeightType.Grass;
                        t.TerrainData.Collidable = true;
                    }
                    else if (heightValue < Forest)
                    {
                        t.TerrainData.HeightType = HeightType.Forest;
                        t.TerrainData.Collidable = true;
                    }
                    else if (heightValue < Rock)
                    {
                        t.TerrainData.HeightType = HeightType.Rock;
                        t.TerrainData.Collidable = true;
                    }
                    else
                    {
                        t.TerrainData.HeightType = HeightType.Snow;
                        t.TerrainData.Collidable = true;
                    }


                    //adjust moisture based on height
                    if (t.TerrainData.HeightType == HeightType.DeepWater)
                    {
                        MoistureData.Data[t.X, t.Y] += 8f * t.TerrainData.HeightValue;
                    }
                    else if (t.TerrainData.HeightType == HeightType.ShallowWater)
                    {
                        MoistureData.Data[t.X, t.Y] += 3f * t.TerrainData.HeightValue;
                    }
                    else if (t.TerrainData.HeightType == HeightType.Shore)
                    {
                        MoistureData.Data[t.X, t.Y] += 1f * t.TerrainData.HeightValue;
                    }
                    else if (t.TerrainData.HeightType == HeightType.Sand)
                    {
                        MoistureData.Data[t.X, t.Y] += 0.2f * t.TerrainData.HeightValue;
                    }

                    //Moisture Map Analyze	
                    float moistureValue = MoistureData.Data[x, y];
                    moistureValue = (moistureValue - MoistureData.Min) / (MoistureData.Max - MoistureData.Min);
                    t.TerrainData.MoistureValue = moistureValue;

                    //set moisture type
                    if (moistureValue < DryerValue) t.TerrainData.MoistureType = MoistureType.Dryest;
                    else if (moistureValue < DryValue) t.TerrainData.MoistureType = MoistureType.Dryer;
                    else if (moistureValue < WetValue) t.TerrainData.MoistureType = MoistureType.Dry;
                    else if (moistureValue < WetterValue) t.TerrainData.MoistureType = MoistureType.Wet;
                    else if (moistureValue < WettestValue) t.TerrainData.MoistureType = MoistureType.Wetter;
                    else t.TerrainData.MoistureType = MoistureType.Wettest;


                    // Adjust Heat Map based on Height - Higher == colder
                    if (t.TerrainData.HeightType == HeightType.Forest)
                    {
                        HeatData.Data[t.X, t.Y] -= 0.1f * t.TerrainData.HeightValue;
                    }
                    else if (t.TerrainData.HeightType == HeightType.Rock)
                    {
                        HeatData.Data[t.X, t.Y] -= 0.25f * t.TerrainData.HeightValue;
                    }
                    else if (t.TerrainData.HeightType == HeightType.Snow)
                    {
                        HeatData.Data[t.X, t.Y] -= 0.4f * t.TerrainData.HeightValue;
                    }
                    else
                    {
                        HeatData.Data[t.X, t.Y] += 0.01f * t.TerrainData.HeightValue;
                    }

                    // Set heat value
                    float heatValue = HeatData.Data[x, y];
                    heatValue = (heatValue - HeatData.Min) / (HeatData.Max - HeatData.Min);
                    t.TerrainData.HeatValue = heatValue;

                    // set heat type
                    if (heatValue < ColdestValue) t.TerrainData.HeatType = HeatType.Coldest;
                    else if (heatValue < ColderValue) t.TerrainData.HeatType = HeatType.Colder;
                    else if (heatValue < ColdValue) t.TerrainData.HeatType = HeatType.Cold;
                    else if (heatValue < WarmValue) t.TerrainData.HeatType = HeatType.Warm;
                    else if (heatValue < WarmerValue) t.TerrainData.HeatType = HeatType.Warmer;
                    else t.TerrainData.HeatType = HeatType.Warmest;

                    Tiles[x, y] = t;
                }
            }
        }

        private void UpdateNeighbors()
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    BaseTile t = Tiles[x, y];

                    t.Top = GetTop(t);
                    t.Bottom = GetBottom(t);
                    t.Left = GetLeft(t);
                    t.Right = GetRight(t);
                }
            }
        }

        private void UpdateBitmasks()
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    Tiles[x, y].UpdateBitmask();
                }
            }
        }

        private void FloodFill()
        {
            // Use a stack instead of recursion
            Stack<BaseTile> stack = new Stack<BaseTile>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {

                    BaseTile t = Tiles[x, y];

                    //Tile already flood filled, skip
                    if (t.TerrainData.FloodFilled) continue;

                    // Land
                    if (t.TerrainData.Collidable)
                    {
                        TileGroup group = new TileGroup();
                        group.Type = TileGroupType.Land;
                        stack.Push(t);

                        while (stack.Count > 0)
                        {
                            FloodFill(stack.Pop(), ref group, ref stack);
                        }

                        if (group.Tiles.Count > 0)
                            Lands.Add(group);
                    }
                    // Water
                    else
                    {
                        TileGroup group = new TileGroup();
                        group.Type = TileGroupType.Water;
                        stack.Push(t);

                        while (stack.Count > 0)
                        {
                            FloodFill(stack.Pop(), ref group, ref stack);
                        }

                        if (group.Tiles.Count > 0)
                            Waters.Add(group);
                    }
                }
            }
        }

        private void FloodFill(BaseTile tile, ref TileGroup tiles, ref Stack<BaseTile> stack)
        {
            // Validate
            if (tile == null)
                return;
            if (tile.TerrainData.FloodFilled)
                return;
            if (tiles.Type == TileGroupType.Land && !tile.TerrainData.Collidable)
                return;
            if (tiles.Type == TileGroupType.Water && tile.TerrainData.Collidable)
                return;

            // Add to TileGroup
            tiles.Tiles.Add(tile);
            tile.TerrainData.FloodFilled = true;

            // floodfill into neighbors
            BaseTile t = GetTop(tile);
            if (t != null && !t.TerrainData.FloodFilled && tile.TerrainData.Collidable == t.TerrainData.Collidable)
                stack.Push(t);
            t = GetBottom(tile);
            if (t != null && !t.TerrainData.FloodFilled && tile.TerrainData.Collidable == t.TerrainData.Collidable)
                stack.Push(t);
            t = GetLeft(tile);
            if (t != null && !t.TerrainData.FloodFilled && tile.TerrainData.Collidable == t.TerrainData.Collidable)
                stack.Push(t);
            t = GetRight(tile);
            if (t != null && !t.TerrainData.FloodFilled && tile.TerrainData.Collidable == t.TerrainData.Collidable)
                stack.Push(t);
        }

        private Direction GetLowestNeighbor(BaseTile tile)
        {
            float left = GetHeightValue(tile.Left);
            float right = GetHeightValue(tile.Right);
            float bottom = GetHeightValue(tile.Bottom);
            float top = GetHeightValue(tile.Top);

            if (left < right && left < top && left < bottom)
                return Direction.Left;
            else if (right < left && right < top && right < bottom)
                return Direction.Right;
            else if (top < left && top < right && top < bottom)
                return Direction.Top;
            else if (bottom < top && bottom < right && bottom < left)
                return Direction.Bottom;
            else
                return Direction.Bottom;
        }
    }

}
