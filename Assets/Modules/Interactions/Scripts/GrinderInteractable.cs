using System;
using System.Collections.Generic;
using UnityEngine;

namespace Potions.Gameplay
{
    public class GrinderInteractable : ContainerInteractable
    {
        protected override bool CanHolderInteract(ItemHolder holder, InteractionType type)
        {
            return _currentState == State.Empty && holder.Item != null && holder.Item.GrindedId != null;
        }

        protected override string GetItem() => null;

        protected override void OnItemAdded(string id)
        {
            _ingredient = id;
            SetState(State.Grind);
        }

        private void Awake()
        {
            _outputInteractable.ItemTaken += OnItemTaken;
        }

        private void Update()
        {
            if (_currentState == State.Grind)
            {
                _grindTimer -= Time.deltaTime;
                if (_grindTimer <= 0f)
                    SetState(State.Full);
            }
        }

        private void SetState(State state)
        {
            switch (state)
            {
                case State.Empty:
                    _spriteRenderer.sprite = _emptySprite;
                    break;
                case State.Grind:
                    _grindTimer = _grindDuration;
                    _spriteRenderer.sprite = _grindSprite;
                    break;
                case State.Full:
                    _outputInteractable.SetOutput(ItemDatabase.Get(_ingredient).GrindedId);
                    _grindTimer = 0;
                    _ingredient = null;
                    _spriteRenderer.sprite = _fullSprite;
                    break;
            }

            _currentState = state;
        }

        private void OnItemTaken()
        {
            SetState(State.Empty);
        }

        private enum State
        {
            Empty,
            Grind,
            Full
        }

        [SerializeField]
        private float _grindDuration;
        [SerializeField]
        private ItemOutputInteractable _outputInteractable;
        [Header("Sprites")]
        [SerializeField]
        private Sprite _emptySprite;
        [SerializeField]
        private Sprite _grindSprite;
        [SerializeField]
        private Sprite _fullSprite;
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private State _currentState = State.Empty;

        private string _ingredient;
        private float _grindTimer;
    }
}