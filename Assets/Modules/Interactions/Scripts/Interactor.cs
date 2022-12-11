using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Potions.Gameplay
{
    public class Interactor : MonoBehaviour
    {
        public event Action<BaseInteractable> Interacted;

        public bool ShowBubbles => _showBubbles;
        public CharacterLogic Character => _character;
        public BaseInteractable ClosestInteractable => _closestInteractable;
        public bool CanInteract(BaseInteractable.InteractionType type = BaseInteractable.InteractionType.Any) => _closestInteractable
                                                                          && _closestInteractable.CanInteract(this, type);
        public bool CanAltInteract => _closestInteractable && _closestInteractable.CanAltInteract(this);

        public void Setup(CharacterLogic character) => _character = character;

        public void Interact()
        {
            if (!CanInteract()) return;
            
            _closestInteractable.Interact(this);
            Interacted?.Invoke(_closestInteractable);
            Character.Visuals.Bump();
        }

        public void StartAltInteraction()
        {
            if (!CanAltInteract) return;
            _altInteractable = _closestInteractable;
            _altInteractionTimer = 0f;
        }

        public void StopAltInteraction()
        {
            if (_altInteractable)
                _altInteractable.SetFill(0f);
            _altInteractable = null;
        }

        public void AltInteract()
        {
            if (_altInteractable == null) return;
            _altInteractable.AltInteract(this);
            Interacted?.Invoke(_altInteractable);
            Character.Visuals.Bump();
        }
        
        public void NormalAltInteract()
        {
            if (!CanAltInteract) return;
            _closestInteractable.AltInteract(this);
            Interacted?.Invoke(_closestInteractable);
            Character.Visuals.Bump();
        }

        private void Awake()
        {
            _ownInteractables = GetComponents<BaseInteractable>().ToList();
        }

        private void Update()
        {
            var newInteractable = FindInteractable();
            if (newInteractable != _closestInteractable)
            {
                if (_showBubbles)
                {
                    _closestInteractable?.SetActive(false);
                    if (newInteractable && newInteractable.CanInteract(this, BaseInteractable.InteractionType.Any))
                        newInteractable.SetActive(true);
                }
                _closestInteractable = newInteractable;
            }
            else
            {
                bool canInteractNow = CanInteract();

                if (_canInteractBefore != canInteractNow)
                {
                    if (_showBubbles)
                        _closestInteractable?.SetActive(canInteractNow);
                    _canInteractBefore = canInteractNow;
                }
            }

            if (_altInteractable)
            {
                _altInteractionTimer += Time.deltaTime;
                _altInteractable.SetFill(_altInteractionTimer / _altInteractionDuration);
                if (_altInteractionTimer >= _altInteractionDuration)
                {
                    AltInteract();
                    StopAltInteraction();
                }
            }
        }

        // private void OnDrawGizmosSelected()
        // {
        //     Handles.matrix = transform.localToWorldMatrix;
        //     Vector3 from = Quaternion.AngleAxis(-_allowedAngle * 0.5f, Vector3.forward) * Vector3.up;
        //     Handles.DrawWireArc(Vector3.zero, Vector3.forward, from, _allowedAngle, 1f);
        // }

        private BaseInteractable FindInteractable()
        {
            bool IsFacingAngle(BaseInteractable interactable, out float distance)
            {
                Vector3 diff = (interactable.transform.position - transform.position);
                if (_ignoreAngle)
                {
                    distance = diff.magnitude;
                    return distance <= interactable.Range;
                }
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
                // Skip golems and own interactables
                if (!_allowGolems && interactable is GolemInteractable || _ownInteractables.Contains(interactable))
                    continue;
                if (IsFacingAngle(interactable, out float distance)
                    && distance <= minDistance)
                {
                    minDistance = distance;
                    closest = interactable;
                }
            }

            return closest;
        }

        private CharacterLogic _character;
        private BaseInteractable _closestInteractable;
        private List<BaseInteractable> _ownInteractables;
        private bool _canInteractBefore;
        private BaseInteractable _altInteractable;
        private float _altInteractionTimer;

        [SerializeField]
        private float _allowedAngle;
        [SerializeField]
        private bool _ignoreAngle;
        [SerializeField]
        private bool _showBubbles;
        [SerializeField]
        private bool _allowGolems;
        [SerializeField]
        private float _altInteractionDuration;
    }
}