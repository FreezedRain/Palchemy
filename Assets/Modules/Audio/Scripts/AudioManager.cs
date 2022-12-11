using UnityEngine;

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
            source.PlayOneShot(clip);
        }
        
        private AudioSource CreateSource(Vector3 position)
        {
            var source = new GameObject("Oneshot").AddComponent<AudioSource>();
            source.transform.position = position;
            return source;
        }
    }
}