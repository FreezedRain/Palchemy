using UnityEngine;

namespace Potions.Gameplay
{
    public class AnimationHelper : MonoBehaviour
    {
        private void Awake()
        {
            _defaultScale = transform.localScale;
        }

        public void Bump()
        {
            LeanTween.cancel(gameObject);
            transform.localScale = new Vector3(_defaultScale.x * (1 + 0.1f * _multiplier),
                _defaultScale.y * (1f - 0.1f * _multiplier));
            LeanTween.scale(gameObject, _defaultScale, 0.14f)
                .setEaseInQuad();
        }

        [SerializeField] private float _multiplier = 1f;
        private Vector3 _defaultScale;
    }
}