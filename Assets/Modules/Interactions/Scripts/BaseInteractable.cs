using System;
using System.Collections.Generic;
using UnityEngine;

namespace Potions.Gameplay
{
    public abstract class BaseInteractable : MonoBehaviour
    {
        public enum InteractionType
        {
            Any, Pickup, Drop
        }
        
        public static List<BaseInteractable> Interactables = new();
        public float Range => _range;

        public virtual bool CanInteract(Interactor interactor, InteractionType type) => true;
        
        public virtual bool CanSkip(Interactor interactor) => false;
        
        public virtual bool CanAltInteract(Interactor interactor) => false;

        public virtual void SetActive(bool active) => _bubble.IsActive = active;

        public void Interact(Interactor interactor) => OnInteract(interactor);
        public void AltInteract(Interactor interactor) => OnAltInteract(interactor);

        protected virtual void OnInteract(Interactor interactor)
        {
            // Do bubble animations and stuff
            if (interactor.ShowBubbles)
                _bubble.Blink();
            if (_animationHelper)
                _animationHelper.Bump();
        }
        
        protected virtual void OnAltInteract(Interactor interactor)
        {
            if (interactor.ShowBubbles)
                _bubble.Blink();
            if (_animationHelper)
                _animationHelper.Bump();
        }

        private void OnEnable() => Interactables.Add(this);

        private void OnDisable() => Interactables.Remove(this);

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _range);
        }

        [SerializeField]
        private float _range;
        [SerializeField]
        private InteractableBubble _bubble;
        [SerializeField]
        protected AnimationHelper _animationHelper;
    }
}