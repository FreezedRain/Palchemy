using UnityEngine;

namespace Potions.Global
{
    [CreateAssetMenu(menuName = "Audio/Clip")]
    public class AudioClipData : ScriptableObject
    {
        public virtual void Play(Vector3 position)
        {
            AudioManager.Instance.PlaySound(_clips[Random.Range(0, _clips.Length)],
                position, Random.Range(_pitchMin, _pitchMax), _volume);
        }

        [SerializeField] protected AudioClip[] _clips;
        [SerializeField, Min(0)] private float _volume = 1;
        [SerializeField, Min(0)] private float _pitchMin = 1;
        [SerializeField, Min(0)] private float _pitchMax = 1;
    }
}