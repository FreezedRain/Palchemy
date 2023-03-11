namespace Potions.Gameplay
{
    /// <summary>
    /// Disposes of whatever the interactor is holding
    /// </summary>
    public class EmptyInteractable : ContainerInteractable
    {
        protected override bool CanHolderInteract(ItemHolder holder, InteractionType type) => holder.ItemId != null;

        protected override string GetItem() => null;
    }
}