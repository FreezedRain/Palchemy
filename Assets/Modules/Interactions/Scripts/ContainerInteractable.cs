namespace Potions.Gameplay
{
    /// <summary>
    /// General class for all interactables that deal with items (chests, cauldrons, etc)
    /// </summary>
    public abstract class ContainerInteractable : BaseInteractable
    {
        public override bool CanInteract(Interactor interactor, InteractionType type = InteractionType.Any) => CanHolderInteract(interactor.Character.ItemHolder, type);
        public override bool CanSkip(Interactor interactor) => CanHolderSkip(interactor.Character.ItemHolder);

        protected virtual bool CanHolderInteract(ItemHolder holder, InteractionType type = InteractionType.Any) => true;

        protected virtual bool CanHolderSkip(ItemHolder holder) => false;

        protected override void OnInteract(Interactor interactor)
        {
            var holder = interactor.Character.ItemHolder;
            string heldItemId = holder.ItemId;
            holder.SetItem(GetItem());
            OnItemAdded(heldItemId);
            base.OnInteract(interactor);
        }

        protected virtual string GetItem() => null;

        protected virtual void OnItemAdded(string id) { }
    }
}