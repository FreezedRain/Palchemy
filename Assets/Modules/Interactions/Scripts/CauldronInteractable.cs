using System.Collections.Generic;
using Potions.Global;
using UnityEngine;

namespace Potions.Gameplay
{
    public class CauldronInteractable : ContainerInteractable
    {
        protected override bool CanHolderInteract(ItemHolder holder, InteractionType type)
        {
            if (holder.ItemId == null)
            {
                return false;
            }
            
            if (_currentState is State.Cook or State.Empty)
            {
                return holder.Item.IsIngredient;
            }
            
            if (_currentState == State.Full)
            {
                return holder.ItemId == "bottle";
            }

            return false;
        }

        protected override bool CanHolderSkip(ItemHolder holder)
        {
            return false;
            if (holder.ItemId == null)
                return true;

            if (_currentState == State.Full)
            {
                return holder.ItemId != "bottle";
            }

            if (_currentState == State.Empty)
            {
                return !holder.Item.IsIngredient;
            }
            
            if (_currentState == State.Cook && _ingredients.Count >= _maxIngredients)
            {
                return holder.ItemId != "bottle";
            }

            return false;
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
        
        protected override void PlayInteractAudio(Interactor interactor)
        {
            var clip = interactor.Character.ItemHolder.Item == null ? _dropClip : _pickupClip;
            clip.Play(transform.position);
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
                    _boilSource.volume = 0f;
                    _boilSource.Play();
                    LeanTween.cancel(gameObject);
                    LeanTween.value(gameObject, f => _boilSource.volume = f, 0f, 0.1f, 0.4f);
                    _cookTimer = _cookDuration;
                    _spriteRenderer.sprite = _cookSprite;
                    break;
                case State.Full:
                    LeanTween.cancel(gameObject);
                    LeanTween.value(gameObject, f => _boilSource.volume = f, 0.1f, 0f, 0.4f)
                        .setOnComplete(_boilSource.Stop);
                    _cookTimer = 0;
                    _cookedItemId = ItemDatabase.FindRecipeByIngredients(_ingredients)?.ResultId ?? "mistake";
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
        [SerializeField]
        private int _maxIngredients;
        [Header("Sprites")]
        [SerializeField]
        private Sprite _cookSprite;
        [SerializeField]
        private Sprite _fullSprite;
        [SerializeField]
        private Sprite _emptySprite;
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [Header("Audio")]
        [SerializeField]
        private AudioClipData _pickupClip;
        [SerializeField]
        private AudioClipData _dropClip;
        [SerializeField]
        private AudioSource _boilSource;

        private State _currentState = State.Empty;
        
        private List<string> _ingredients;
        private float _cookTimer;
        private string _cookedItemId;
    }
}