using System;
using UnityEngine;

namespace Potions.Gameplay
{
    public enum ParticleType
    {
        None,
        Splash,
        Mold
    }

    public class ParticleManager : MonoSingleton<ParticleManager>
    {
        public static void Spawn(ParticleType type, Vector3 position, Transform parent = null)
        {
            var prefab = Instance.GetParticleObject(type);
            if (prefab)
                Instantiate(prefab, position, Quaternion.identity, parent);
        }

        private GameObject GetParticleObject(ParticleType type) => type switch
        {
            ParticleType.None => null,
            ParticleType.Splash => _splash,
            ParticleType.Mold => _mold,
            _ => throw new ArgumentOutOfRangeException(nameof(type), $"VFX {type} doesn't exist!"),
        };

        [Header("Particles")]
        [SerializeField] private GameObject _splash;
        [SerializeField] private GameObject _mold;
    }
}