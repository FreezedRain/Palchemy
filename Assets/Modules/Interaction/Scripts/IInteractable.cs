namespace Potions.Gameplay
{
    public interface IInteractable
    {
        bool CanInteract(Interactor interactor);
        void OnInteract(Interactor interactor);
    }
}