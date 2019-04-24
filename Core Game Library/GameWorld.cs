using HML.Expansion.WorldMap;
using HML.Expansion.WorldMap.Tile;

namespace HML.Expansion
{
    public class GameWorld
    {
        private WorldTile[,] tiles;
        private int key = -1;
        public GameWorld()
        {
            WrappingWorldGenerator worldGen = new WrappingWorldGenerator(this, 100, 100, key);
            tiles = worldGen.Tiles;
        }

        public WorldTile GetTileAt(int x, int y)
        {
            try
            {
                return tiles[x, y];
            }
            catch
            {
                return null;
            }
        }

        public WorldTile GetTileAtDirection(int x, int y, TileDirectionEnum direction)
        {
            switch (direction)
            {
                case TileDirectionEnum.Left:
                    return GetTileAt(x - 1, y);
                case TileDirectionEnum.Right:
                    return GetTileAt(x + 1, y);
                case TileDirectionEnum.Up:
                    return GetTileAt(x, y + 1);
                case TileDirectionEnum.Down:
                    return GetTileAt(x, y - 1);
                default:
                    return null;
            }
        }
    }
}
