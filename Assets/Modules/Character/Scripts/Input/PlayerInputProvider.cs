using System;
using Potions.Global;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Potions.Gameplay
{
    public class PlayerInputProvider : MonoBehaviour, IInputProvider
    {
        public event Action Interacted;
        public event Action AltInteracted;

        public InputState GetState() => _currentState;

        private void OnMove(InputValue value) => _currentState.Direction = value.Get<Vector2>();
        private void OnInteract() => Interacted?.Invoke();
        private void OnAltInteract() => AltInteracted?.Invoke();
        private void OnRestart() => GameManager.Instance.Transitions.LoadLevel("LevelSelect");

        private InputState _currentState;
    }
}