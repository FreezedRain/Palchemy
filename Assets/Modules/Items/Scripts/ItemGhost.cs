using Potions.Global;
using UnityEngine;

namespace Potions.Gameplay
{
    [RequireComponent(typeof(ItemHolder))]
    public class ItemGhost : MonoBehaviour
    {
        public void Setup(string id)
        {
            GetComponent<ItemHolder>().SetItem(id);
            // seq.append(0.25f);
            var seq = LeanTween.sequence();
            LeanTween.delayedCall(gameObject, 0.45f,
                () => ParticleManager.Spawn(ParticleType.Splash, transform.position));
            seq.append(LeanTween.moveLocalY(gameObject, transform.localPosition.y + 0.7f, 0.3f).setEaseOutCubic());
            seq.append(LeanTween.scale(gameObject, Vector3.one * 1.85f, 0.2f).setEaseInCubic());
            seq.insert(LeanTween.alpha(gameObject, 0, 0.15f));
            seq.append(() =>
            {
                if (gameObject)
                {
                    _popClip.Play(transform.position);
                    Destroy(gameObject);
                }
            });
        }

        private void OnDestroy()
        {
            LeanTween.cancel(gameObject);
        }

        [SerializeField] private AudioClipData _popClip;
    }
}