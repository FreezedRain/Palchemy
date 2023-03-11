using Potions.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Potions.Global
{
    public class TransitionManager : MonoBehaviour
    {
        public void LoadLevel(string sceneName) => TransitionTo(sceneName);

        private void TransitionTo(string sceneName)
        {
            if (_inTransition)
            {
                Debug.LogWarning($"Tried loading scene {sceneName} while loading another scene!");
                return;
            }

            _inTransition = true;
            var sequence = LeanTween.sequence();
            Vector2 scale = _screenFade.rectTransform.parent.GetComponent<RectTransform>().sizeDelta;
            _screenFade.material.SetFloat("_Ratio", scale.x / scale.y);
            UpdateTransitionOffset(SceneManager.GetActiveScene().name, scale);

            sequence.append(LeanTween
                .value(gameObject, f => _screenFade.material.SetFloat("_Fade", f), 1.2f, -1.2f, 0.7f).setEaseOutQuad());
            sequence.append(() => SceneManager.LoadScene(sceneName));
            sequence.append(() => { UpdateTransitionOffset(sceneName, scale); });
            sequence.append(LeanTween
                .value(gameObject, f => _screenFade.material.SetFloat("_Fade", f), -1.2f, 1.2f, 0.7f).setEaseOutQuad());
            sequence.append(() => _inTransition = false);
        }

        private void UpdateTransitionOffset(string name, Vector2 scale)
        {
            if (name != "Bedroom")
            {
                var playerPos =
                    Camera.main.WorldToScreenPoint(FindObjectOfType<PlayerInputProvider>().transform.position);
                _screenFade.material.SetFloat("_OffsetX", playerPos.x / scale.x);
                _screenFade.material.SetFloat("_OffsetY", playerPos.y / scale.y);
            }
            else
            {
                _screenFade.material.SetFloat("_OffsetX", 0.5f);
                _screenFade.material.SetFloat("_OffsetY", 0.5f);
            }
        }

        private void Start()
        {
            _screenFade.material = Instantiate(_screenFade.material);
        }

        [SerializeField] private Image _screenFade;

        private bool _inTransition;
    }
}