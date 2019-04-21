
namespace HML.Expansion.Model.Item
{
    public class BaseItem : IInventoryObject
    {
        public string Name { get; set; }
        public string DisplayText { get; set; }
        public string InventorySpriteName { get; set; }
        public float Quality { get; set; }

        public BaseItem(string name)
        {
            Name = name;
        }

        public string GetDisplayText()
        {
            if (DisplayText == null)
                return Name;
            return DisplayText;
        }

        public string GetInventorySprite()
        {
            return SpriteContants.DEFAULT_ITEM_SPRITE;
        }
    }

}
