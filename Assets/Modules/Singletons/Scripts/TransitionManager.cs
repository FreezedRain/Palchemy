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
            // sequence.append(LeanTween.alpha(_screenFade.rectTransform, 1f, 0.5f));
            _screenFade.material.SetFloat("_Invert", 0f);
            sequence.append(LeanTween.value(gameObject, f => _screenFade.material.SetFloat("_Fade", f), 1.2f, -1.15f, 0.5f));
            sequence.append(() => SceneManager.LoadScene(sceneName));
            sequence.append(0.5f);
            sequence.append(() => _screenFade.material.SetFloat("_Invert", 1f));
            sequence.append(LeanTween.value(gameObject, f => _screenFade.material.SetFloat("_Fade", f), -1.15f, 1.2f, 0.5f));

            // sequence.append(LeanTween.alpha(_screenFade.rectTransform, 0f, 0.5f));
            sequence.append(() => _inTransition = false);
        }
        
        [SerializeField]
        private Image _screenFade;

        private bool _inTransition;
    }
}