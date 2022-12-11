using UnityEngine;
using UnityEngine.Audio;

namespace Potions.Global
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        public void PlaySound(AudioClip clip, Vector3 pos, float pitch, float volume)
        {
            var source = CreateSource(pos);
            source.loop = false;
            source.pitch = pitch;
            source.volume = volume;
            source.outputAudioMixerGroup = _effectsMixerGroup;
            // source.spatialBlend = 0.5f;
            source.PlayOneShot(clip);
            Destroy(source.gameObject, clip.length + 0.5f);
        }
        
        private AudioSource CreateSource(Vector3 position)
        {
            var source = new GameObject("Oneshot").AddComponent<AudioSource>();
            source.transform.position = position;
            return source;
        }

        [SerializeField]
        private AudioSource _effectsSource;
        [SerializeField]
        private AudioMixerGroup _effectsMixerGroup;
    }
}