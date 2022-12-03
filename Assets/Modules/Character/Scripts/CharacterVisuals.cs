using UnityEngine;

namespace Potions.Gameplay
{
    public class CharacterVisuals : MonoBehaviour
    {
        public void FaceDirection(Vector2 dir)
        {
            if (dir == Vector2.zero)
                return;
            
            (Sprite sprite, bool flip) = _spriteSet.GetSprite(dir);
            _spriteRenderer.sprite = sprite;
            _spriteRenderer.flipX = flip;
        }

        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private SpriteSet _spriteSet;
    }
}