using System;
using System.Collections.Generic;
using UnityEngine;

namespace Potions.Gameplay
{
    public class BaseInteractable : MonoBehaviour, IInteractable
    {
        public static List<BaseInteractable> Interactables = new();
        public float Range => _range;

        public virtual bool CanInteract(Interactor interactor) => true;

        public virtual void OnInteract(Interactor interactor)
        {
            // Do bubble animations and stuff
        }

        private void OnEnable()
        {
            Interactables.Add(this);
        }

        private void OnDisable()
        {
            Interactables.Remove(this);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _range);
        }

        [SerializeField]
        private float _range;
    }
}