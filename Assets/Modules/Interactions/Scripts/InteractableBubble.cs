using UnityEngine;

namespace Potions.Gameplay
{
    public class InteractableBubble : MonoBehaviour
    {
        public bool IsActive
        {
            set
            {
                LeanTween.cancel(_visuals);
                if (value)
                {
                    LeanTween.scale(_visuals, Vector3.one, 0.21f).setEaseOutBack();
                }
                else
                {
                    LeanTween.scale(_visuals, Vector3.zero, 0.21f).setEaseInBack();
                }
            }
        }

        public void Blink()
        {
            LeanTween.cancel(_visuals);
            _visuals.transform.localScale = Vector3.one * 1.15f;
            LeanTween.scale(_visuals, Vector3.one, 0.15f).setEaseInCubic();
        }

        public void SetFill(float value)
        {
            _fill.transform.localScale = Vector3.one * 0.275f * value;
        }

        private void Awake()
        {
            _visuals.transform.localScale = Vector3.zero;
        }

        [SerializeField] private GameObject _visuals;
        [SerializeField] private Transform _fill;
    }
}