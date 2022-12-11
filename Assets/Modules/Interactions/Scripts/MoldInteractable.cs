using System;
using System.Collections.Generic;
using Potions.Global;
using UnityEngine;

namespace Potions.Gameplay
{
    public class MoldInteractable : ContainerInteractable
    {
        protected override bool CanHolderInteract(ItemHolder holder, InteractionType type) => !_inProgress && holder.ItemId == "golem_heart";

        protected override string GetItem() => null;

        protected override void OnItemAdded(string id)
        {
            _inProgress = true;
            var seq = LeanTween.sequence();
            _closeClip.Play(transform.position);
            seq.append(() => _spriteRenderer.sprite = _closedSprite);
            seq.append(1f);
            seq.append(() =>
            {
                _spriteRenderer.sprite = _openSprite;
                _animationHelper.Bump();
                ParticleManager.Spawn(ParticleType.Mold, transform.position);
                _openClip.Play(transform.position);
                Instantiate(_golemPrefab, _golemSpawnPoint.position, Quaternion.identity);
                _inProgress = false;
            });
        }

        [SerializeField]
        private CharacterLogic _golemPrefab;
        [SerializeField]
        private Transform _golemSpawnPoint;
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private Sprite _openSprite;
        [SerializeField]
        private Sprite _closedSprite;
        [Header("Audio")]
        [SerializeField]
        private AudioClipData _openClip;
        [SerializeField]
        private AudioClipData _closeClip;
        private bool _inProgress;
    }
}