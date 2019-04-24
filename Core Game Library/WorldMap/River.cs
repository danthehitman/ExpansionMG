using System.Collections.Generic;
using HML.Expansion.WorldMap.Tile;

namespace HML.Expansion.WorldMap
{
    public enum Direction
    {
        Left,
        Right,
        Top,
        Bottom
    }

    public class River
    {

        public int Length;
        public List<WorldTile> Tiles;
        public int ID;

        public int Intersections;
        public float TurnCount;
        public Direction CurrentDirection;

        public River(int id)
        {
            ID = id;
            Tiles = new List<WorldTile>();
        }

        public void AddTile(WorldTile tile)
        {
            tile.SetRiverPath(this);
            Tiles.Add(tile);
        }
    }

    public class RiverGroup
    {
        public List<River> Rivers = new List<River>();
    }
}
