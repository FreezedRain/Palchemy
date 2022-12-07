using UnityEngine;

namespace Potions.Gameplay
{
    public abstract class ContainerInteractable : BaseInteractable
    {
        public override bool CanInteract(Interactor interactor) => CanHolderInteract(interactor.Character.ItemHolder);
        public override bool CanSkip(Interactor interactor) => CanHolderSkip(interactor.Character.ItemHolder);

        protected virtual bool CanHolderInteract(ItemHolder holder) => true;

        protected virtual bool CanHolderSkip(ItemHolder holder) => false;

        protected override void OnInteract(Interactor interactor)
        {
            base.OnInteract(interactor);
            
            var holder = interactor.Character.ItemHolder;
            string heldItemId = holder.ItemId;
            holder.SetItem(GetItem());
            OnItemAdded(heldItemId);
        }

        protected virtual string GetItem() => null;

        protected virtual void OnItemAdded(string id) { }
    }
}