using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HML.Expansion.Utilities
{
    public class AssetLoader
    {
        public static Dictionary<string, Texture2D> LoadTextures(ContentManager content)
        {
            Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

            textures.Add(SpriteContants.TILE_GRASSLAND,
                content.Load<Texture2D>(StringContants.CONTENT_TERRAIN_PATH + SpriteContants.TILE_GRASSLAND));

            textures.Add(SpriteContants.TILE_KEY_DESERT,
                content.Load<Texture2D>(StringContants.CONTENT_TERRAIN_PATH + SpriteContants.TILE_KEY_DESERT));
            textures.Add(SpriteContants.TILE_MOUNTAIN,
                content.Load<Texture2D>(StringContants.CONTENT_TERRAIN_PATH + SpriteContants.TILE_MOUNTAIN));
            textures.Add(SpriteContants.TILE_SAVANNA,
                content.Load<Texture2D>(StringContants.CONTENT_TERRAIN_PATH + SpriteContants.TILE_SAVANNA));
            textures.Add(SpriteContants.TILE_ICE,
                content.Load<Texture2D>(StringContants.CONTENT_TERRAIN_PATH + SpriteContants.TILE_ICE));
            textures.Add(SpriteContants.TILE_TUNDRA,
                content.Load<Texture2D>(StringContants.CONTENT_TERRAIN_PATH + SpriteContants.TILE_TUNDRA));
            textures.Add(SpriteContants.TILE_WOODLAND,
                content.Load<Texture2D>(StringContants.CONTENT_TERRAIN_PATH + SpriteContants.TILE_WOODLAND));
            textures.Add(SpriteContants.TILE_BOREAL_FOREST,
                content.Load<Texture2D>(StringContants.CONTENT_TERRAIN_PATH + SpriteContants.TILE_BOREAL_FOREST));
            textures.Add(SpriteContants.TILE_SEASONAL_FOREST,
                content.Load<Texture2D>(StringContants.CONTENT_TERRAIN_PATH + SpriteContants.TILE_SEASONAL_FOREST));
            textures.Add(SpriteContants.TILE_TEMPERATE_RAINFOREST,
                content.Load<Texture2D>(StringContants.CONTENT_TERRAIN_PATH + SpriteContants.TILE_TEMPERATE_RAINFOREST));
            textures.Add(SpriteContants.TILE_TROPICAL_RAINFOREST,
                content.Load<Texture2D>(StringContants.CONTENT_TERRAIN_PATH + SpriteContants.TILE_TROPICAL_RAINFOREST));
            textures.Add(SpriteContants.TILE_DEEP_WATER,
                content.Load<Texture2D>(StringContants.CONTENT_TERRAIN_PATH + SpriteContants.TILE_DEEP_WATER));
            textures.Add(SpriteContants.TILE_SHALLOW_WATER,
                content.Load<Texture2D>(StringContants.CONTENT_TERRAIN_PATH + SpriteContants.TILE_SHALLOW_WATER));
            textures.Add(SpriteContants.TILE_RIVER,
                content.Load<Texture2D>(StringContants.CONTENT_TERRAIN_PATH + SpriteContants.TILE_RIVER));

            textures.Add(SpriteContants.TILE_RIVER_BEND,
                content.Load<Texture2D>(StringContants.CONTENT_TERRAIN_PATH + SpriteContants.TILE_RIVER_BEND));
            textures.Add(SpriteContants.TILE_RIVER_CROSS,
                content.Load<Texture2D>(StringContants.CONTENT_TERRAIN_PATH + SpriteContants.TILE_RIVER_CROSS));
            textures.Add(SpriteContants.TILE_RIVER_HOR,
                content.Load<Texture2D>(StringContants.CONTENT_TERRAIN_PATH + SpriteContants.TILE_RIVER_HOR));
            textures.Add(SpriteContants.TILE_RIVER_TERM_HOR,
                content.Load<Texture2D>(StringContants.CONTENT_TERRAIN_PATH + SpriteContants.TILE_RIVER_TERM_HOR));
            textures.Add(SpriteContants.TILE_RIVER_TERM_VERT,
                content.Load<Texture2D>(StringContants.CONTENT_TERRAIN_PATH + SpriteContants.TILE_RIVER_TERM_VERT));
            textures.Add(SpriteContants.TILE_RIVER_T,
                content.Load<Texture2D>(StringContants.CONTENT_TERRAIN_PATH + SpriteContants.TILE_RIVER_T));
            textures.Add(SpriteContants.TILE_RIVER_VERT,
                content.Load<Texture2D>(StringContants.CONTENT_TERRAIN_PATH + SpriteContants.TILE_RIVER_VERT));
            textures.Add(SpriteContants.TILE_RIVER_SINGLE,
                content.Load<Texture2D>(StringContants.CONTENT_TERRAIN_PATH + SpriteContants.TILE_RIVER_SINGLE));

            return textures;
        }
    }
}
