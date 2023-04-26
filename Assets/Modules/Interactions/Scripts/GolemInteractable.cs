namespace Potions.Gameplay
{
    public class GolemInteractable : BaseInteractable
    {
        public GolemBrain Golem => _golem;

        public override bool CanInteract(Interactor interactor, InteractionType type) => true;
        public override bool CanAltInteract(Interactor interactor) => true;

        protected override void OnInteract(Interactor interactor)
        {
            base.OnInteract(interactor);
            _golem.Interact(interactor);
        }

        protected override void OnAltInteract(Interactor interactor)
        {
            base.OnAltInteract(interactor);
            _golem.AltInteract(interactor);
        }

        private void Awake()
        {
            _golem = GetComponent<GolemBrain>();
        }

        private GolemBrain _golem;
    }
}