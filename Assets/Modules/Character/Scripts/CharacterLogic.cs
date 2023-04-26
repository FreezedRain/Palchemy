using System.Collections.Generic;
using UnityEngine;

namespace Potions.Gameplay
{
    [RequireComponent(typeof(CharacterMovement), typeof(CharacterVisuals), typeof(Interactor))]
    public class CharacterLogic : MonoBehaviour
    {
        public static List<CharacterLogic> AllCharacters = new();

        public Vector2 Forward => _forward;
        public bool IsMoving => _brain.GetState().Direction != Vector2.zero;
        public ItemHolder ItemHolder => _itemHolder;
        public Interactor Interactor { get; private set; }
        public CharacterMovement Movement { get; private set; }
        public CharacterVisuals Visuals { get; private set; }

        public void LookTowards(Vector2 dir) => _lookDirection = dir;

        private void Awake()
        {
            _brain = GetComponent<IBrain>();
            Movement = GetComponent<CharacterMovement>();
            Visuals = GetComponent<CharacterVisuals>();
            Interactor = GetComponent<Interactor>();

            Interactor.Setup(this);
            _brain.Interacted += Interactor.Interact;
            _brain.AltInteractStarted += Interactor.NormalAltInteract;
        }

        private void OnEnable() => AllCharacters.Add(this);
        private void OnDisable() => AllCharacters.Remove(this);

        private void Update()
        {
            var inputState = _brain.GetState();
            if (inputState.Direction != Vector2.zero)
            {
                _forward = inputState.Direction;
            }

            Movement.Move(inputState.Direction);
            Visuals.FaceDirection(_lookDirection ?? inputState.Direction);
            Visuals.IsMoving = inputState.Direction != Vector2.zero;

            _lookDirection = null;
        }

        [SerializeField] private ItemHolder _itemHolder;
        private Vector2 _forward;
        private Vector2? _lookDirection;
        private IBrain _brain;
    }
}