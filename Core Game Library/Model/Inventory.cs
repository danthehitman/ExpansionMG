using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using HML.Expansion.Model.Item;
using HML.Expansion.Model.Resource;

namespace HML.Expansion.Model
{

    public class Inventory : INotifyPropertyChanged
    {
        private List<BaseItem> items;
        private List<BaseResource> materials;

        public const string ItemsPropertyName = "Items";
        public List<BaseItem> Items
        {
            get
            {
                return items;
            }

            set
            {
                items = value;
                OnPropertyChanged(ItemsPropertyName);
                OnPropertyChanged(InventoryObjectsPropertyName);
            }
        }

        public const string MaterialsPropertyName = "Materials";
        public List<BaseResource> Materials
        {
            get
            {
                return materials;
            }

            set
            {
                materials = value;
                OnPropertyChanged(MaterialsPropertyName);
                OnPropertyChanged(InventoryObjectsPropertyName);
            }
        }

        public void AddItem(BaseItem item)
        {
            Items.Add(item);
            OnPropertyChanged(ItemsPropertyName);
            OnPropertyChanged(InventoryObjectsPropertyName);
        }

        public void AddMaterial(BaseResource material)
        {
            Materials.Add(material);
            OnPropertyChanged(MaterialsPropertyName);
            OnPropertyChanged(InventoryObjectsPropertyName);
        }

        public const string InventoryObjectsPropertyName = "InventoryObjects";
        public List<IInventoryObject> InventoryObjects
        {
            get
            {
                return Items.Cast<IInventoryObject>().Concat(Materials.Cast<IInventoryObject>()).ToList();
            }
        }

        public Inventory()
        {
            items = new List<BaseItem>();
            materials = new List<BaseResource>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        public IInventoryObject GetInventoryObjectOfType(Type type)
        {
            return InventoryObjects.FirstOrDefault(e => e.GetType() == type);
        }
    }

}
