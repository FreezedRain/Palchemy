using System;
using Potions.Gameplay;
using Potions.Global;
using UnityEngine;

namespace Potions.Level
{
    public class TransitionTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.attachedRigidbody.gameObject.TryGetComponent<PlayerInputProvider>(out var player))
            {
                if (!String.IsNullOrEmpty(_levelId))
                    GameManager.Instance.Transitions.LoadLevel(_levelId);
            }
        }

        [SerializeField]
        private string _levelId;
    }
}