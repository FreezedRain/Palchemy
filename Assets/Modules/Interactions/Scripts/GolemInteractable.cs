using System;

namespace Potions.Gameplay
{
    public class GolemInteractable : BaseInteractable
    {
        public GolemInputProvider Golem => _golem;
        
        public override bool CanInteract(Interactor interactor) => true;
        
        protected override void OnInteract(Interactor interactor)
        {
            base.OnInteract(interactor);
            _golem.Interact(interactor);
        }

        private void Awake()
        {
            _golem = GetComponent<GolemInputProvider>();
        }

        private GolemInputProvider _golem;
    }
}