using UnityEngine;

namespace Potions.Gameplay
{
    public class TableInteractable : ContainerInteractable
    {
        protected override bool CanHolderInteract(ItemHolder holder) => _itemId != null || holder.ItemId != null;
        protected override string GetItem() => _itemId;

        protected override void OnItemAdded(string id)
        {
            _itemId = id;
            _itemHolder.SetItem(id);
        }

        [SerializeField]
        private ItemHolder _itemHolder;
        private string _itemId;
    }
}