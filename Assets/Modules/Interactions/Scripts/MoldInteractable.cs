using System;
using System.Collections.Generic;
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
            seq.append(() => _spriteRenderer.sprite = _closedSprite);
            seq.append(1f);
            seq.append(() =>
            {
                _spriteRenderer.sprite = _openSprite;
                _animationHelper.Bump();
                ParticleManager.Spawn(ParticleType.Mold, transform.position);
                Instantiate(_golemPrefab, _golemSpawnPoint.position, Quaternion.identity);
                _inProgress = false;
            });
        }

        protected override bool CanHolderSkip(ItemHolder holder)
        {
            if (holder.ItemId != "golem_heart")
                return true;

            return !_inProgress;
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
        private bool _inProgress;
    }
}