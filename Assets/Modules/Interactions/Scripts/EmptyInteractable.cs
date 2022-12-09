namespace Potions.Gameplay
{
    public class EmptyInteractable : ContainerInteractable
    {
        protected override bool CanHolderInteract(ItemHolder holder, InteractionType type) => holder.ItemId != null;

        protected override string GetItem() => null;
    }
}