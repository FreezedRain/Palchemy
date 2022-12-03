using System;
using UnityEngine;

namespace Potions.Gameplay
{
    public class CharacterVisuals : MonoBehaviour
    {
        public bool IsMoving;
        
        public void FaceDirection(Vector2 dir)
        {
            if (dir == Vector2.zero) return;
            
            (Sprite sprite, bool flip) = _spriteSet.GetSprite(dir);
            _spriteRenderer.sprite = sprite;
            _spriteRenderer.flipX = flip;
        }

        public void Bump() => _currentBumpAmount = _bumpAmount;

        private void Update()
        {
            _time += (IsMoving ? 2 : 1) * Time.deltaTime * _squashSpeed;
            float squashAmount = (IsMoving ? 2 : 1) * _squashAmount;
            
            Vector2 bumpDeformation = new Vector2(_currentBumpAmount / 10, -_currentBumpAmount / 10);
            Vector2 squashDeformation = new Vector2(
                1 + Mathf.Sin(_time) * squashAmount,
                1 - Mathf.Sin(_time) * squashAmount);

            _visualsParent.localScale = bumpDeformation + squashDeformation;

            _currentBumpAmount = Mathf.Lerp(_currentBumpAmount, 0f, 16f * Time.deltaTime);
        }

        private float _time;
        private float _currentBumpAmount;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private SpriteSet _spriteSet;
        [SerializeField]
        private Transform _visualsParent;
        [Header("Animation")]
        [SerializeField]
        private float _squashSpeed;
        [SerializeField]
        private float _squashAmount;
        [SerializeField]
        private float _bumpAmount;
    }
}