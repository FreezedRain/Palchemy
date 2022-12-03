using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Potions.Gameplay
{
    public class Interactor : MonoBehaviour
    {
        public event Action<BaseInteractable> Interacted;

        public bool ShowBubbles => _showBubbles;
        public CharacterLogic Character => _character;
        public BaseInteractable Interactable => _activeInteractable;

        public void Setup(CharacterLogic character) => _character = character;

        public void Interact()
        {
            if (_activeInteractable != null)
            {
                _activeInteractable.Interact(this);
                Interacted?.Invoke(_activeInteractable);
            }
        }

        private void Awake()
        {
            _ownInteractables = GetComponents<BaseInteractable>().ToList();
        }

        private void Update()
        {
            var newInteractable = FindInteractable();
            if (newInteractable != _activeInteractable)
            {
                if (_showBubbles)
                    _activeInteractable?.SetActive(false);
                _activeInteractable = newInteractable;
                if (_showBubbles)
                    _activeInteractable?.SetActive(true);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Handles.matrix = transform.localToWorldMatrix;
            Vector3 from = Quaternion.AngleAxis(-_allowedAngle * 0.5f, Vector3.forward) * Vector3.up;
            Handles.DrawWireArc(Vector3.zero, Vector3.forward, from, _allowedAngle, 1f);
        }

        private BaseInteractable FindInteractable()
        {
            bool IsFacingAngle(BaseInteractable interactable, out float distance)
            {
                Vector3 diff = (interactable.transform.position - transform.position);
                if (Mathf.Acos(Vector3.Dot(diff.normalized, _character.Forward)) * Mathf.Rad2Deg <= _allowedAngle)
                {
                    distance = diff.magnitude;
                    return distance <= interactable.Range;
                }

                distance = 0f;
                return false;
            }
            
            float minDistance = float.MaxValue;
            BaseInteractable closest = null;
            foreach (var interactable in BaseInteractable.Interactables)
            {
                if (!_ownInteractables.Contains(interactable) && IsFacingAngle(interactable, out float distance)
                                                             && distance <= minDistance
                                                             && interactable.CanInteract(this))
                {
                    minDistance = distance;
                    closest = interactable;
                }
            }

            return closest;
        }

        private CharacterLogic _character;
        private BaseInteractable _activeInteractable;
        private List<BaseInteractable> _ownInteractables;

        [SerializeField]
        private float _allowedAngle;
        [SerializeField]
        private bool _showBubbles;
    }
}