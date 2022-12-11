using UnityEngine;

namespace Potions.Gameplay
{
    // Loop an AudioSource and play the initial segment first
    [RequireComponent(typeof(AudioSource))]
    public class AudioLoop : MonoBehaviour
    {
        public void Awake()
        {
            _source = GetComponent<AudioSource>();
            if (_playOnAwake)
                Play();
        }

        public void Play()
        {
            _source.Stop();
            _source.PlayOneShot(_initialClip);
            _source.PlayScheduled(AudioSettings.dspTime + _initialClip.length);
        }

        private AudioSource _source;
        [SerializeField]
        private bool _playOnAwake;
        [SerializeField]
        private AudioClip _initialClip;
    }
}