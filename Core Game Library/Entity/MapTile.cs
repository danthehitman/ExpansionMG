using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HML.Expansion.Graphics;
using HML.Expansion.WorldMap;
using HML.Expansion.WorldMap.Tile;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HML.Expansion.Entity
{
    public class MapTile
    {
        public Sprite Texture { get; set; }
        public WorldTile WorldTile { get; set; }

        public MapTile(Sprite texture, WorldTile worldTile)
        {
            Texture = texture;
            WorldTile = worldTile;
        }

        public static Texture2D GetTileSprite(WorldTile tile, System.Random rand, )
        {
            BiomeType value = tile.TerrainData.BiomeType;
            Texture2D sprite = null;

            if (tile.TerrainData.HeightType == HeightType.DeepWater)
            {
                sprite = SpriteManager.Instance.GetSpriteByName(Constants.TILE_DEEP_WATER);
            }
            else if (tile.TerrainData.HeightType == HeightType.ShallowWater)
            {
                sprite = SpriteManager.Instance.GetSpriteByName(Constants.TILE_SHALLOW_WATER);
            }
            else if (tile.TerrainData.HeightType == HeightType.Rock)
            {
                sprite = SpriteManager.Instance.GetSpriteByName(Constants.TILE_MOUNTAIN);
            }
            else if (tile.TerrainData.HeightType == HeightType.River)
            {
                sprite = SpriteManager.Instance.GetSpriteByName(Constants.TILE_RIVER);
            }
            else
            {
                switch (value)
                {
                    case BiomeType.Ice:
                        sprite = SpriteManager.Instance.GetSpriteByName(Constants.TILE_ICE);
                        break;
                    case BiomeType.BorealForest:
                        sprite = SpriteManager.Instance.GetSpriteByName(Constants.TILE_BOREAL_FOREST);
                        break;
                    case BiomeType.Desert:
                        sprite = GetRandomTileVarietySpriteByWeight(
                            SpriteManager.Instance.GetSpritesByKey(Constants.TILE_KEY_DESERT), rand);
                        break;
                    case BiomeType.Grassland:
                        sprite = SpriteManager.Instance.GetSpriteByName(Constants.TILE_GRASSLAND);
                        break;
                    case BiomeType.SeasonalForest:
                        sprite = SpriteManager.Instance.GetSpriteByName(Constants.TILE_SEASONAL_FOREST);
                        break;
                    case BiomeType.Tundra:
                        sprite = SpriteManager.Instance.GetSpriteByName(Constants.TILE_TUNDRA);
                        break;
                    case BiomeType.Savanna:
                        sprite = SpriteManager.Instance.GetSpriteByName(Constants.TILE_SAVANNA);
                        break;
                    case BiomeType.TemperateRainforest:
                        sprite = SpriteManager.Instance.GetSpriteByName(Constants.TILE_TEMPERATE_RAINFOREST);
                        break;
                    case BiomeType.TropicalRainforest:
                        sprite = SpriteManager.Instance.GetSpriteByName(Constants.TILE_TROPICAL_RAINFOREST);
                        break;
                    case BiomeType.Woodland:
                        sprite = SpriteManager.Instance.GetSpriteByName(Constants.TILE_WOODLAND);
                        break;
                }
            }

            return sprite;
        }
    }
}
