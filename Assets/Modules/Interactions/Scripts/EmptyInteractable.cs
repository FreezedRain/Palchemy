namespace Potions.Gameplay
{
    public class EmptyInteractable : ContainerInteractable
    {
        protected override bool CanHolderInteract(ItemHolder holder) => holder.ItemId != null;

        protected override string GetItem() => null;
    }
}