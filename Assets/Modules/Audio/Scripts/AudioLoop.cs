using UnityEngine;

namespace Potions.Gameplay
{
    /// <summary>
    /// Loop an AudioSource and play the initial segment first
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioLoop : MonoBehaviour
    {
        public void Start()
        {
            _source = GetComponent<AudioSource>();
            if (_playOnStart)
                Play();
        }

        public void Play()
        {
            _source.Stop();

            var seq = LeanTween.sequence();
            seq.append(0.25f);
            seq.append(() =>
            {
                _source.PlayOneShot(_initialClip);
                _source.PlayScheduled(AudioSettings.dspTime + _initialClip.length);
            });
            seq.insert(LeanTween.value(gameObject, f => _source.volume = f, 0, 1, 1.5f));
        }

        [SerializeField] private bool _playOnStart;
        [SerializeField] private AudioClip _initialClip;
        private AudioSource _source;
    }
}