using System;
using System.Collections.Generic;
using System.ComponentModel;
using HML.Expansion.Entity;
using HML.Expansion.Model;
using HML.Expansion.Common.Extensions;
using HML.Expansion.Common;
using HML.Expansion.Model.Item;

namespace HML.Expansion.WorldMap.Tile
{

    public class BaseTile : INotifyPropertyChanged
    {
        protected Random random = new Random();

        public TerrainInfo TerrainData { get; set; }
        public TileResourceInfo TileResourceData { get; set; }

        public BaseTile Left { get; set; }
        public BaseTile Right { get; set; }
        public BaseTile Top { get; set; }
        public BaseTile Bottom { get; set; }

        public TileCache Cache { get; set; }

        public int TimesExplored { get; set; }
        public int TimesSurveyed { get; set; }

        public EventHandler TileDataChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public GameWorld World { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public BaseTile(GameWorld world, int x, int y)
        {
            World = world;
            X = x;
            Y = y;
            TerrainData = new TerrainInfo();
            TileResourceData = new TileResourceInfo();
            TerrainData.Rivers = new List<River>();
            Cache = new TileCache();
        }

        public void ExploreTile(object entity = null)
        {
            var human = entity as HumanEntity;
            if (human != null)
            {
                AddInventoryToCache(ExploreTile(human, this).ExplorationInventory);
            }
            TimesExplored++;
            Console.WriteLine("Explored tile.");
        }

        public void AddInventoryToCache(Inventory inventory)
        {
            Cache.CacheInventory = inventory;
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateBiomeBitmask()
        {
            int count = 0;

            if (TerrainData.Collidable && Top != null && Top.TerrainData.BiomeType == TerrainData.BiomeType)
                count += 1;
            if (TerrainData.Collidable && Bottom != null && Bottom.TerrainData.BiomeType == TerrainData.BiomeType)
                count += 4;
            if (TerrainData.Collidable && Left != null && Left.TerrainData.BiomeType == TerrainData.BiomeType)
                count += 8;
            if (TerrainData.Collidable && Right != null && Right.TerrainData.BiomeType == TerrainData.BiomeType)
                count += 2;

            TerrainData.BiomeBitmask = count;
        }

        public void UpdateBitmask()
        {
            int count = 0;

            if (TerrainData.Collidable && Top != null && Top.TerrainData.HeightType == TerrainData.HeightType)
                count += 1;
            if (TerrainData.Collidable && Right != null && Right.TerrainData.HeightType == TerrainData.HeightType)
                count += 2;
            if (TerrainData.Collidable && Bottom != null && Bottom.TerrainData.HeightType == TerrainData.HeightType)
                count += 4;
            if (TerrainData.Collidable && Left != null && Left.TerrainData.HeightType == TerrainData.HeightType)
                count += 8;

            TerrainData.Bitmask = count;
        }

        public bool HasRiverNeighbor()
        {
            if (Left != null && Left.TerrainData.Rivers.Count > 0)
                return true;
            if (Right != null && Right.TerrainData.Rivers.Count > 0)
                return true;
            if (Top != null && Top.TerrainData.Rivers.Count > 0)
                return true;
            if (Bottom != null && Bottom.TerrainData.Rivers.Count > 0)
                return true;
            return false;
        }

        public int GetRiverNeighborCount(River river)
        {
            int count = 0;
            if (Left != null && Left.TerrainData.Rivers.Count > 0 && Left.TerrainData.Rivers.Contains(river))
                count++;
            if (Right != null && Right.TerrainData.Rivers.Count > 0 && Right.TerrainData.Rivers.Contains(river))
                count++;
            if (Top != null && Top.TerrainData.Rivers.Count > 0 && Top.TerrainData.Rivers.Contains(river))
                count++;
            if (Bottom != null && Bottom.TerrainData.Rivers.Count > 0 && Bottom.TerrainData.Rivers.Contains(river))
                count++;
            return count;
        }

        public void SetRiverPath(River river)
        {
            if (!TerrainData.Collidable)
                return;

            if (!TerrainData.Rivers.Contains(river))
            {
                TerrainData.Rivers.Add(river);
            }
        }

        private void SetRiverTile(River river)
        {
            SetRiverPath(river);
            TerrainData.HeightType = HeightType.River;
            TerrainData.HeightValue = 0;
            TerrainData.Collidable = false;
        }

        // This function got messy.  Sorry.
        public void DigRiver(River river, int size)
        {
            SetRiverTile(river);
            TerrainData.RiverSize = size;

            if (size == 1)
            {
                if (Bottom != null)
                {
                    Bottom.SetRiverTile(river);
                    if (Bottom.Right != null) Bottom.Right.SetRiverTile(river);
                }
                if (Right != null) Right.SetRiverTile(river);
            }

            if (size == 2)
            {
                if (Bottom != null)
                {
                    Bottom.SetRiverTile(river);
                    if (Bottom.Right != null) Bottom.Right.SetRiverTile(river);
                }
                if (Right != null)
                {
                    Right.SetRiverTile(river);
                }
                if (Top != null)
                {
                    Top.SetRiverTile(river);
                    if (Top.Left != null) Top.Left.SetRiverTile(river);
                    if (Top.Right != null) Top.Right.SetRiverTile(river);
                }
                if (Left != null)
                {
                    Left.SetRiverTile(river);
                    if (Left.Bottom != null) Left.Bottom.SetRiverTile(river);
                }
            }

            if (size == 3)
            {
                if (Bottom != null)
                {
                    Bottom.SetRiverTile(river);
                    if (Bottom.Right != null) Bottom.Right.SetRiverTile(river);
                    if (Bottom.Bottom != null)
                    {
                        Bottom.Bottom.SetRiverTile(river);
                        if (Bottom.Bottom.Right != null) Bottom.Bottom.Right.SetRiverTile(river);
                    }
                }
                if (Right != null)
                {
                    Right.SetRiverTile(river);
                    if (Right.Right != null)
                    {
                        Right.Right.SetRiverTile(river);
                        if (Right.Right.Bottom != null) Right.Right.Bottom.SetRiverTile(river);
                    }
                }
                if (Top != null)
                {
                    Top.SetRiverTile(river);
                    if (Top.Left != null) Top.Left.SetRiverTile(river);
                    if (Top.Right != null) Top.Right.SetRiverTile(river);
                }
                if (Left != null)
                {
                    Left.SetRiverTile(river);
                    if (Left.Bottom != null) Left.Bottom.SetRiverTile(river);
                }
            }

            if (size == 4)
            {

                if (Bottom != null)
                {
                    Bottom.SetRiverTile(river);
                    if (Bottom.Right != null) Bottom.Right.SetRiverTile(river);
                    if (Bottom.Bottom != null)
                    {
                        Bottom.Bottom.SetRiverTile(river);
                        if (Bottom.Bottom.Right != null) Bottom.Bottom.Right.SetRiverTile(river);
                    }
                }
                if (Right != null)
                {
                    Right.SetRiverTile(river);
                    if (Right.Right != null)
                    {
                        Right.Right.SetRiverTile(river);
                        if (Right.Right.Bottom != null) Right.Right.Bottom.SetRiverTile(river);
                    }
                }
                if (Top != null)
                {
                    Top.SetRiverTile(river);
                    if (Top.Right != null)
                    {
                        Top.Right.SetRiverTile(river);
                        if (Top.Right.Right != null) Top.Right.Right.SetRiverTile(river);
                    }
                    if (Top.Top != null)
                    {
                        Top.Top.SetRiverTile(river);
                        if (Top.Top.Right != null) Top.Top.Right.SetRiverTile(river);
                    }
                }
                if (Left != null)
                {
                    Left.SetRiverTile(river);
                    if (Left.Bottom != null)
                    {
                        Left.Bottom.SetRiverTile(river);
                        if (Left.Bottom.Bottom != null) Left.Bottom.Bottom.SetRiverTile(river);
                    }

                    if (Left.Left != null)
                    {
                        Left.Left.SetRiverTile(river);
                        if (Left.Left.Bottom != null) Left.Left.Bottom.SetRiverTile(river);
                        if (Left.Left.Top != null) Left.Left.Top.SetRiverTile(river);
                    }

                    if (Left.Top != null)
                    {
                        Left.Top.SetRiverTile(river);
                        if (Left.Top.Top != null) Left.Top.Top.SetRiverTile(river);
                    }
                }
            }
        }

        public TileExplorationResults ExploreTile(HumanEntity entity, BaseTile tile)
        {
            var results = new TileExplorationResults();
            //Determine tile properties
            RevealTile(entity, tile);
            //Determine foraging haul
            results.ExplorationInventory = ForageTile(entity, tile);
            //Determine events (lost, special, etc...)
            DetermineExplorationEvents(entity, tile, results);
            //Determine effects of events
            return results;
        }

        private void DetermineExplorationEvents(HumanEntity entity, BaseTile tile,
            TileExplorationResults explorationResults)
        {
            var roll = 0f;
            var baseExplorationTime = GetExplorationTime(tile, entity);

            //Do we get lost?
            roll = random.NextFloatInRange(0f, .5f);
            roll = roll + DetermineTileLostAndSpecialDiscoveryModifier(tile);
            if (roll > entity.AdjustedNavigationSkill)
            {
                //Lost, what happened to us based on our survival skill?
            }
            roll = random.NextFloatInRange(0f, .5f);
            roll = roll + DetermineTileLostAndSpecialDiscoveryModifier(tile, true);
            if (roll < entity.AdjustedExplorationSkill)
            {
                //We made a special discovery
            }

            DetermineCatastrophe(tile, entity, explorationResults);
        }

        private void DetermineCatastrophe(BaseTile tile, HumanEntity entity,
            TileExplorationResults explorationResults)
        {
            var severityPercent = random.NextFloatInRange(0f, 1f);
            var roll = 0f;
            var biomeType = tile.TerrainData.BiomeType;
            var heightType = tile.TerrainData.HeightType;

            var elapsedTime = 0f;
            var healthLost = 0f;
            var fatigueLost = 0f;
            var moraleLost = 0f;

            if (heightType == HeightType.River)
            {
                //TODO: Account for river.
            }
            else if (heightType >= HeightType.Rock)
            {
                roll = random.NextFloatInRange(0f, .5f);
                var mountainCatastrophe = false;
                if (heightType > HeightType.Rock)
                {
                    if (roll + .5f > entity.AdjustedMountaineeringSkill)
                        mountainCatastrophe = true;
                }
                else
                {
                    if (roll + .45f > entity.AdjustedMountaineeringSkill)
                        mountainCatastrophe = true;
                }
                if (mountainCatastrophe)
                {
                    explorationResults.ExplorationEntries.Add(
                        new ExplorationStoryEntry()
                        {
                            Title = "The mountains were not kind.",
                            Description = "During your exploration in the mountains you suffered a fall and were injured."
                        });

                    elapsedTime = MathUtilities.GetFloatAtPercentBetween(3f, 24f, severityPercent);
                    healthLost = MathUtilities.GetFloatAtPercentBetween(1f, 3f, severityPercent);
                    fatigueLost = MathUtilities.GetFloatAtPercentBetween(1f, 3f, severityPercent);
                    moraleLost = MathUtilities.GetFloatAtPercentBetween(2f, 4f, severityPercent);
                }
            }
            else
            {
                roll = random.NextFloatInRange(0f, 1f);
                switch (biomeType)
                {
                    case BiomeType.Desert:
                        if (roll > .9f)
                        {
                            explorationResults.ExplorationEntries.Add(
                                new ExplorationStoryEntry()
                                {
                                    Title = "The desert was not kind.",
                                    Description = "During your exploration in the desert you were stung by an extremely poisonous snake and grew ill."
                                });

                            elapsedTime = MathUtilities.GetFloatAtPercentBetween(1f, 6f, severityPercent);
                            healthLost = MathUtilities.GetFloatAtPercentBetween(.5f, 1f, severityPercent);
                            fatigueLost = MathUtilities.GetFloatAtPercentBetween(1f, 2f, severityPercent);
                            moraleLost = MathUtilities.GetFloatAtPercentBetween(1f, 2f, severityPercent);
                        }
                        break;
                    case BiomeType.Savanna:
                        if (roll > .9f)
                        {

                        }
                        break;
                    case BiomeType.TropicalRainforest:
                        break;
                    case BiomeType.Grassland:
                        break;
                    case BiomeType.Woodland:
                        break;
                    case BiomeType.SeasonalForest:
                        break;
                    case BiomeType.TemperateRainforest:
                        break;
                    case BiomeType.BorealForest:
                        break;
                    case BiomeType.Tundra:
                        break;
                    case BiomeType.Ice:
                        break;
                }
            }
            explorationResults.ElapsedTimeHours += elapsedTime;
            entity.Health -= healthLost;
            entity.Fatigue -= fatigueLost;
            entity.Morale -= moraleLost;

        }

        private static float DetermineTileLostAndSpecialDiscoveryModifier(BaseTile tile, bool ignoreNeighborModifiers = false)
        {
            var biomeType = tile.TerrainData.BiomeType;
            var heightType = tile.TerrainData.HeightType;
            var result = 0f;

            if (heightType == HeightType.River)
            {
                //TODO: Account for river.
            }
            else
            {
                switch (biomeType)
                {
                    case BiomeType.Desert:
                        result = .3f;
                        break;
                    case BiomeType.Savanna:
                        result = .1f;
                        break;
                    case BiomeType.TropicalRainforest:
                        result = .5f;
                        break;
                    case BiomeType.Grassland:
                        result = .1f;
                        break;
                    case BiomeType.Woodland:
                        result = .2f;
                        break;
                    case BiomeType.SeasonalForest:
                        result = .4f;
                        break;
                    case BiomeType.TemperateRainforest:
                        result = .5f;
                        break;
                    case BiomeType.BorealForest:
                        result = .7f;
                        break;
                    case BiomeType.Tundra:
                        result = .4f;
                        break;
                    case BiomeType.Ice:
                        result = .5f;
                        break;
                }
            }
            if (!ignoreNeighborModifiers && (tile.HasRiverNeighbor() || tile.TerrainData.HeightType == HeightType.Shore))
                result = result - 0.3f;

            return result;
        }

        private static float GetExplorationTime(BaseTile tile, HumanEntity entity)
        {
            var biomeType = tile.TerrainData.BiomeType;
            var heightType = tile.TerrainData.HeightType;
            //Adjustment will be either posative or negative based on various conditions.  Negative will lessen the time
            //spent exploring this tile.
            var adjustment = 0f;

            var compass = (ItemCompass)entity.EntityInventory.GetInventoryObjectOfType(typeof(ItemCompass));
            if (compass != null)
            {
                adjustment -= compass.Quality;
            }

            var result = 0f;

            if (heightType == HeightType.River)
            {
                result = 5f;
                result -= (entity.AdjustedSwimmingSkill - 1) * 2;
            }
            else if (heightType >= HeightType.Rock)
            {

            }
            else
            {
                switch (biomeType)
                {
                    case BiomeType.Desert:
                        result = 8f;
                        break;
                    case BiomeType.Savanna:
                        result = 12f;
                        break;
                    case BiomeType.TropicalRainforest:
                        result = 24f;
                        break;
                    case BiomeType.Grassland:
                        result = 10f;
                        break;
                    case BiomeType.Woodland:
                        result = 10f;
                        break;
                    case BiomeType.SeasonalForest:
                        result = 24f;
                        break;
                    case BiomeType.TemperateRainforest:
                        result = 30f;
                        break;
                    case BiomeType.BorealForest:
                        result = 20f;
                        break;
                    case BiomeType.Tundra:
                        result = 12f;
                        break;
                    case BiomeType.Ice:
                        result = 12f;
                        break;
                }
            }

            return result + adjustment;
        }

        private static Inventory ForageTile(HumanEntity entity, BaseTile tile)
        {
            var inventory = new Inventory();
            var resourceHaul = 0;
            ////Bark
            //resourceHaul = CalculateResourceHaul(12f, tile.TileResourceData.Bark, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new Bark());
            //}
            ////Berries
            //resourceHaul = CalculateResourceHaul(24f, tile.TileResourceData.Berries, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new Berries());
            //}
            ////Bone
            //resourceHaul = CalculateResourceHaul(8f, tile.TileResourceData.SmallAnimals, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new Bone() { Size = Resource.ResourceSize.Small });
            //}
            //resourceHaul = CalculateResourceHaul(8f, tile.TileResourceData.LargeAnimals, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new Bone() { Size = Resource.ResourceSize.Large });
            //}
            ////Cactus
            //resourceHaul = CalculateResourceHaul(12f, tile.TileResourceData.CactusStuffs, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new Cactus());
            //}
            ////Egg
            //resourceHaul = CalculateResourceHaul(6f, tile.TileResourceData.Birds, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new Egg());
            //}
            ////Fruit
            //resourceHaul = CalculateResourceHaul(12f, tile.TileResourceData.Fruit, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new Fruit());
            //}
            ////Grass
            //resourceHaul = CalculateResourceHaul(24f, tile.TileResourceData.Grasses, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new Grass());
            //}
            ////Hide
            //resourceHaul = CalculateResourceHaul(2f, tile.TileResourceData.SmallAnimals, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new Hide() { Size = Resource.ResourceSize.Small });
            //}
            //resourceHaul = CalculateResourceHaul(2f, tile.TileResourceData.LargeAnimals, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new Hide() { Size = Resource.ResourceSize.Large });
            //}
            ////Broad Leaf
            //resourceHaul = CalculateResourceHaul(24f, tile.TileResourceData.BroadLeaf, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new BroadLeaf());
            //}
            ////Long Leaf
            //resourceHaul = CalculateResourceHaul(24f, tile.TileResourceData.LongLeaf, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new LongLeaf());
            //}
            ////Small Leaf
            //resourceHaul = CalculateResourceHaul(24f, tile.TileResourceData.SmallLeaf, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new SmallLeaf());
            //}
            ////Lose Fur
            //resourceHaul = CalculateResourceHaul(4f, tile.TileResourceData.LargeAnimals, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new LoseFur());
            //}
            ////Meat
            //resourceHaul = CalculateResourceHaul(2f, tile.TileResourceData.SmallAnimals, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new Meat() { Size = Resource.ResourceSize.Small });
            //}
            //resourceHaul = CalculateResourceHaul(4f, tile.TileResourceData.LargeAnimals, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new Meat() { Size = Resource.ResourceSize.Large });
            //}
            ////Moss
            //resourceHaul = CalculateResourceHaul(6f, tile.TileResourceData.Mosses, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new Moss());
            //}
            ////Nuts
            //resourceHaul = CalculateResourceHaul(18f, tile.TileResourceData.Nuts, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new Nuts());
            //}
            ////Rocks
            //resourceHaul = CalculateResourceHaul(24f, tile.TileResourceData.SmallRocks, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new Rock() { Size = Resource.ResourceSize.Small });
            //}
            //resourceHaul = CalculateResourceHaul(12f, tile.TileResourceData.LargeRocks, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new Rock() { Size = Resource.ResourceSize.Large });
            //}
            ////Roots
            //resourceHaul = CalculateResourceHaul(24f, tile.TileResourceData.Roots, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new Roots());
            //}
            ////Seeds
            //resourceHaul = CalculateResourceHaul(24f, tile.TileResourceData.Seeds, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new Seeds());
            //}
            ////Sticks
            //resourceHaul = CalculateResourceHaul(24f, tile.TileResourceData.Sticks, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new Stick());
            //}
            ////Teeth
            //resourceHaul = CalculateResourceHaul(12f, tile.TileResourceData.SmallAnimals, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new Teeth() { Size = Resource.ResourceSize.Small });
            //}
            //resourceHaul = CalculateResourceHaul(12f, tile.TileResourceData.LargeAnimals, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new Teeth() { Size = Resource.ResourceSize.Large });
            //}
            ////Weeds
            //resourceHaul = CalculateResourceHaul(24f, tile.TileResourceData.Weeds, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new Weed());
            //}
            ////Wood
            //resourceHaul = CalculateResourceHaul(12f, tile.TileResourceData.Wood, entity.AdjustedForagingSkill);
            //for (int i = 0; i < resourceHaul; i++)
            //{
            //    inventory.AddMaterial(new Wood());
            //}

            return inventory;
        }

        private static int CalculateResourceHaul(float resourceBase, float tileMultiplier, float entityMultiplier)
        {
            var resourceFinal = (int)System.Math.Round((resourceBase * tileMultiplier) * entityMultiplier);
            return resourceFinal;
        }

        private void RevealTile(HumanEntity entity, BaseTile tile)
        {
            var biomeType = tile.TerrainData.BiomeType;
            switch (biomeType)
            {
                case BiomeType.Desert:
                    RevealDesert(tile, entity);
                    break;
                case BiomeType.Savanna:
                    RevealSavanna(tile, entity);
                    break;
                case BiomeType.TropicalRainforest:
                    break;
                case BiomeType.Grassland:
                    break;
                case BiomeType.Woodland:
                    break;
                case BiomeType.SeasonalForest:
                    break;
                case BiomeType.TemperateRainforest:
                    break;
                case BiomeType.BorealForest:
                    break;
                case BiomeType.Tundra:
                    break;
                case BiomeType.Ice:
                    break;
            }
        }

        private void RevealDesert(BaseTile tile, HumanEntity entity)
        {
            tile.TileResourceData.Bark = CalculateResourceAvailabilityMultiplier(
                0.5f);
            tile.TileResourceData.Berries = CalculateResourceAvailabilityMultiplier(
                1f);
            tile.TileResourceData.SmallAnimals = CalculateResourceAvailabilityMultiplier(
                1.25f);
            tile.TileResourceData.LargeAnimals = CalculateResourceAvailabilityMultiplier(
                0.75f);
            tile.TileResourceData.CactusStuffs = CalculateResourceAvailabilityMultiplier(
                2f);
            tile.TileResourceData.Birds = CalculateResourceAvailabilityMultiplier(
                0.75f);
            tile.TileResourceData.Fruit = CalculateResourceAvailabilityMultiplier(
                0.75f);
            tile.TileResourceData.Grasses = CalculateResourceAvailabilityMultiplier(
                0.75f);
            tile.TileResourceData.LongLeaf = CalculateResourceAvailabilityMultiplier(
                0.5f);
            tile.TileResourceData.SmallLeaf = CalculateResourceAvailabilityMultiplier(
                0.75f);
            tile.TileResourceData.BroadLeaf = CalculateResourceAvailabilityMultiplier(
                0f);
            tile.TileResourceData.Mosses = CalculateResourceAvailabilityMultiplier(
                0f);
            tile.TileResourceData.Nuts = CalculateResourceAvailabilityMultiplier(
                0.1f);
            tile.TileResourceData.LargeRocks = CalculateResourceAvailabilityMultiplier(
                1f);
            tile.TileResourceData.SmallRocks = CalculateResourceAvailabilityMultiplier(
                0.5f);
            tile.TileResourceData.Seeds = CalculateResourceAvailabilityMultiplier(
                0f);
            tile.TileResourceData.Sticks = CalculateResourceAvailabilityMultiplier(
                0.75f);
            tile.TileResourceData.Weeds = CalculateResourceAvailabilityMultiplier(
                0.5f);
            tile.TileResourceData.Wood = CalculateResourceAvailabilityMultiplier(
                0.25f);
        }

        private void RevealSavanna(BaseTile tile, HumanEntity entity)
        {
            tile.TileResourceData.Bark = CalculateResourceAvailabilityMultiplier(
                1f);
            tile.TileResourceData.Berries = CalculateResourceAvailabilityMultiplier(
                1f);
            tile.TileResourceData.SmallAnimals = CalculateResourceAvailabilityMultiplier(
                1.5f);
            tile.TileResourceData.LargeAnimals = CalculateResourceAvailabilityMultiplier(
                1.5f);
            tile.TileResourceData.CactusStuffs = CalculateResourceAvailabilityMultiplier(
                1.5f);
            tile.TileResourceData.Birds = CalculateResourceAvailabilityMultiplier(
                1.25f);
            tile.TileResourceData.Fruit = CalculateResourceAvailabilityMultiplier(
                0.75f);
            tile.TileResourceData.Grasses = CalculateResourceAvailabilityMultiplier(
                1.5f);
            tile.TileResourceData.LongLeaf = CalculateResourceAvailabilityMultiplier(
                0.75f);
            tile.TileResourceData.SmallLeaf = CalculateResourceAvailabilityMultiplier(
                1f);
            tile.TileResourceData.BroadLeaf = CalculateResourceAvailabilityMultiplier(
                0.25f);
            tile.TileResourceData.Mosses = CalculateResourceAvailabilityMultiplier(
                0.25f);
            tile.TileResourceData.Nuts = CalculateResourceAvailabilityMultiplier(
                .75f);
            tile.TileResourceData.LargeRocks = CalculateResourceAvailabilityMultiplier(
                1f);
            tile.TileResourceData.SmallRocks = CalculateResourceAvailabilityMultiplier(
                1f);
            tile.TileResourceData.Seeds = CalculateResourceAvailabilityMultiplier(
                .2f);
            tile.TileResourceData.Sticks = CalculateResourceAvailabilityMultiplier(
                1f);
            tile.TileResourceData.Weeds = CalculateResourceAvailabilityMultiplier(
                1f);
            tile.TileResourceData.Wood = CalculateResourceAvailabilityMultiplier(
                1f);
        }

        private float CalculateResourceAvailabilityMultiplier(float resourceBase)
        {
            var roll = random.NextFloatInRange(0.5f, 2f);
            var resourceFinal = (float)(resourceBase * roll);
            return resourceFinal;
        }
    }
}
