using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HML.Expansion.Model.Resource
{
    public class BaseResource : IInventoryObject
    {
        public enum ResourceSize
        {
            Small,
            Medium,
            Large
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayText { get; set; }
        public string InventorySpriteName { get; set; }

        public BaseResource(string name)
        {
            Name = name;
        }

        public BaseResource(string name, string displayText, string description, string inventorySprite)
        {
            Name = name;
            DisplayText = displayText;
            InventorySpriteName = inventorySprite;
        }

        public virtual string GetDisplayText()
        {
            if (DisplayText == null)
                return Name;
            return DisplayText;
        }

        public string GetInventorySprite()
        {
            return SpriteContants.DEFAULT_RESOURCE_SPRITE;
        }
    }

}
