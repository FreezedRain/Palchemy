using System.Collections.Generic;
using UnityEngine;

namespace Potions.Gameplay
{
    public class ItemDatabase : MonoSingleton<ItemDatabase>
    {
        public static ItemData Get(string id) => id == null ? null : Instance._items[id];
        
        protected override void Awake()
        {
            base.Awake();
            LoadItems();
        }

        private void LoadItems()
        {
            _items = new();
            var loadedItems = Resources.LoadAll<ItemData>(_itemsFolder);
            foreach (var item in loadedItems)
            {
                _items.Add(item.Id, item);
            }
            
            Debug.Log($"Loaded {_items.Count} items.");
        }

        [SerializeField]
        private string _itemsFolder;
        
        private Dictionary<string, ItemData> _items;
    }
}