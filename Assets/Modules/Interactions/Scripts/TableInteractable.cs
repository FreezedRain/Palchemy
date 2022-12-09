using System;
using UnityEngine;

namespace Potions.Gameplay
{
    public class TableInteractable : ContainerInteractable
    {
        protected override bool CanHolderInteract(ItemHolder holder, InteractionType type)
        {
            switch (type)
            {
                case InteractionType.Any:
                    return _itemId != null ^ holder.ItemId != null;
                case InteractionType.Pickup:
                    return _itemId != null && holder.ItemId == null;
                case InteractionType.Drop:
                    return _itemId == null && holder.ItemId != null;
            }

            return false;
        }

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