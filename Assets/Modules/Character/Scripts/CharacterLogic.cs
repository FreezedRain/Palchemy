using UnityEngine;

namespace Potions.Gameplay
{
    [RequireComponent(typeof(CharacterMovement), typeof(CharacterVisuals), typeof(Interactor))]
    public class CharacterLogic : MonoBehaviour
    {
        public Vector2 Forward => _forward;
        
        private void Awake()
        {
            _input = GetComponent<IInputProvider>();
            _movement = GetComponent<CharacterMovement>();
            _visuals = GetComponent<CharacterVisuals>();
            _interactor = GetComponent<Interactor>();
            
            _interactor.Setup(this);
            _input.Interacted += _interactor.Interact;
        }

        private void Update()
        {
            var inputState = _input.GetState();
            if (inputState.Direction != Vector2.zero)
            {
                _forward = inputState.Direction;
            }

            _movement.Move(inputState.Direction);
            _visuals.FaceDirection(inputState.Direction);
        }

        private Vector2 _forward;

        private IInputProvider _input;
        private CharacterMovement _movement;
        private CharacterVisuals _visuals;
        private Interactor _interactor;
    }
}