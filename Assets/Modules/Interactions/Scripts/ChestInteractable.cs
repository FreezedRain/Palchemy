using UnityEngine;

namespace Potions.Gameplay
{
    public class ChestInteractable : ContainerInteractable
    {
        protected override bool CanHolderInteract(ItemHolder holder, InteractionType type)
        {
            return holder.ItemId == null;
        }

        protected override string GetItem() => _itemId;

        private void Start() => _itemHolder.SetItem(_itemId, animate: false);

        [SerializeField] private string _itemId;
        [SerializeField] private ItemHolder _itemHolder;
    }
}