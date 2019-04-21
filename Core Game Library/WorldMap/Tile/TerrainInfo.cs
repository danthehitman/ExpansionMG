using System.Collections.Generic;

namespace HML.Expansion.WorldMap.Tile
{
    public class TerrainInfo
    {
        public HeightType HeightType { get; set; }
        public HeatType HeatType { get; set; }
        public MoistureType MoistureType { get; set; }
        public BiomeType BiomeType { get; set; }

        public float HeightValue { get; set; }
        public float HeatValue { get; set; }
        public float MoistureValue { get; set; }

        public int Bitmask { get; set; }
        public int BiomeBitmask { get; set; }

        public bool Collidable { get; set; }
        public bool FloodFilled { get; set; }

        public List<River> Rivers { get; set; }

        public int RiverSize { get; set; }
    }

}
