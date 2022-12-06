using UnityEngine;

namespace Potions.Gameplay
{
    public class ChestInteractable : ContainerInteractable
    {
        protected override bool CanHolderInteract(ItemHolder holder) => true;
        protected override string GetItem() => _itemId;
        public override bool CanSkip(Interactor interactor) => !CanInteract(interactor);

        private void Start() => _previewItem.SetItem(_itemId);

        [SerializeField]
        private string _itemId;
        [SerializeField]
        private ItemHolder _previewItem;
    }
}