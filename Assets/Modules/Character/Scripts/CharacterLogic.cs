using UnityEngine;

namespace Potions.Gameplay
{
    [RequireComponent(typeof(CharacterMovement), typeof(CharacterVisuals))]
    public class CharacterLogic : MonoBehaviour
    {
        private void Awake()
        {
            _input = GetComponent<IInputProvider>();
            _movement = GetComponent<CharacterMovement>();
            _visuals = GetComponent<CharacterVisuals>();
        }

        private void Update()
        {
            var inputState = _input.GetState();

            _movement.Move(inputState.Direction);
            _visuals.FaceDirection(inputState.Direction);
        }

        private IInputProvider _input;
        private CharacterMovement _movement;
        private CharacterVisuals _visuals;
    }
}