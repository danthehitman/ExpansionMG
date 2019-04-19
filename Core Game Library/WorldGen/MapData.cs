using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HML.Expansion.WorldGen
{
    public class MapData
    {
        public double[,] Data;
        public double Min { get; set; }
        public double Max { get; set; }

        public MapData(int width, int height)
        {
            Data = new double[width, height];
            Min = double.MaxValue;
            Max = double.MinValue;
        }
    }

}
