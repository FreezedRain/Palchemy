using System.Collections.Generic;
using Potions.Global;
using UnityEngine;

namespace Potions.Gameplay
{
    /// <summary>
    /// Accepts ingredients and outputs a potion after a while which can be collected with a bottle.
    /// 
    /// Features a simple state machine with 3 states: Empty, Cook, Full.
    /// Transition logic:
    /// Empty -> (drop item) -> Cook (until no item is dropped in X seconds) -> Full -> (pick up with a bottle) -> Empty
    /// </summary>
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
                    var e = _boilParticles.emission;
                    e.enabled = true;
                    LeanTween.cancel(gameObject);
                    LeanTween.value(gameObject, f => _boilSource.volume = f, 0f, 0.1f, 0.4f);
                    _cookTimer = _cookDuration;
                    _spriteRenderer.sprite = _cookSprite;
                    break;
                case State.Full:
                    var e2 = _boilParticles.emission;
                    e2.enabled = false;
                    LeanTween.cancel(gameObject);
                    LeanTween.value(gameObject, f => _boilSource.volume = f, 0.1f, 0f, 0.4f)
                        .setOnComplete(_boilSource.Stop);
                    _cookTimer = 0;
                    _cookedItemId = ItemDatabase.Instance.FindRecipeByIngredients(_ingredients)?.ResultId ?? "mistake";
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

        [SerializeField] private float _cookDuration;
        [Header("Sprites")]
        [SerializeField] private Sprite _cookSprite;
        [SerializeField] private Sprite _fullSprite;
        [SerializeField] private Sprite _emptySprite;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private ParticleSystem _boilParticles;
        [Header("Audio")]
        [SerializeField] private AudioClipData _pickupClip;
        [SerializeField] private AudioClipData _dropClip;
        [SerializeField] private AudioSource _boilSource;

        private State _currentState = State.Empty;

        private List<string> _ingredients = new();
        private float _cookTimer;
        private string _cookedItemId;
    }
}