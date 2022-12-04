using System;
using System.Collections.Generic;
using UnityEngine;

namespace Potions.Gameplay
{
    public class CauldronInteractable : ContainerInteractable
    {
        protected override bool CanHolderInteract(ItemHolder holder)
        {
            if (_currentState == State.Full)
            {
                return holder.ItemId == "bottle";
            }

            return holder.ItemId != null && holder.Item.IsIngredient;
        }

        protected override string GetItem() => _cookedItemId;

        protected override void OnItemAdded(string id)
        {
            if (_currentState != State.Full)
            {
                _ingredients.Add(id);
                SetState(State.Cook);
            }
            else
            {
                SetState(State.Empty);
            }
        }

        private void Awake()
        {
            _ingredients = new();
        }

        private void Update()
        {
            if (_currentState == State.Cook)
            {
                _cookTimer -= Time.deltaTime;
                if (_cookTimer <= 0f)
                    SetState(State.Full);
            }
        }

        private void SetState(State state)
        {
            switch (state)
            {
                case State.Empty:
                    _cookedItemId = null;
                    _spriteRenderer.sprite = _emptySprite;
                    break;
                case State.Cook:
                    _cookTimer = _cookDuration;
                    _spriteRenderer.sprite = _cookSprite;
                    break;
                case State.Full:
                    _cookTimer = 0;
                    _cookedItemId = ItemDatabase.FindRecipeByIngredients(_ingredients)?.ResultId ?? "crap";
                    _ingredients.Clear();
                    _spriteRenderer.sprite = _fullSprite;
                    break;
            }

            _currentState = state;
        }

        private enum State
        {
            Empty,
            Cook,
            Full
        }

        [SerializeField]
        private float _cookDuration;
        [Header("Sprites")]
        [SerializeField]
        private Sprite _cookSprite;
        [SerializeField]
        private Sprite _fullSprite;
        [SerializeField]
        private Sprite _emptySprite;
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private State _currentState = State.Empty;
        
        private List<string> _ingredients;
        private float _cookTimer;
        private string _cookedItemId;
    }
}