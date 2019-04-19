using System.Collections.Generic;

namespace HML.Expansion.WorldGen
{
    public enum TileGroupType
    {
        Water,
        Land
    }

    public class TileGroup
    {

        public TileGroupType Type;
        public List<BaseTile> Tiles;

        public TileGroup()
        {
            Tiles = new List<BaseTile>();
        }
    }
}
