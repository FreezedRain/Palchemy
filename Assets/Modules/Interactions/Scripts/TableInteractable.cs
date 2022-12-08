using System;
using UnityEngine;

namespace Potions.Gameplay
{
    public class TableInteractable : ContainerInteractable
    {
        protected override bool CanHolderInteract(ItemHolder holder) => _itemId != null ^ holder.ItemId != null;
        protected override string GetItem() => _itemId;

        protected override void OnItemAdded(string id)
        {
            _itemId = id;
            _itemHolder.SetItem(id);
        }

        private void Start()
        {
            if (!String.IsNullOrEmpty(_overrideItemId))
                OnItemAdded(_overrideItemId);
        }

        [SerializeField]
        private ItemHolder _itemHolder;
        [SerializeField]
        private string _overrideItemId;
        private string _itemId;
    }
}