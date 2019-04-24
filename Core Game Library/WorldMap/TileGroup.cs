using System.Collections.Generic;
using HML.Expansion.WorldMap.Tile;

namespace HML.Expansion.WorldMap
{
    public enum TileGroupType
    {
        Water,
        Land
    }

    public class TileGroup
    {

        public TileGroupType Type;
        public List<WorldTile> Tiles;

        public TileGroup()
        {
            Tiles = new List<WorldTile>();
        }
    }
}
