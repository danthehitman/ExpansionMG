using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HML.Expansion.WorldMap.Tile
{
    using System.Collections.Generic;
    using HML.Expansion.Model;

    public class TileExplorationResults
    {
        public TileExplorationResults()
        {
            ExplorationEntries = new List<ExplorationStoryEntry>();
        }
        public Inventory ExplorationInventory { get; set; }
        public float ElapsedTimeHours { get; set; }
        public List<ExplorationStoryEntry> ExplorationEntries { get; set; }

    }

    public class ExplorationStoryEntry
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

}
