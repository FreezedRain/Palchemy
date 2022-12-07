using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Potions.Gameplay
{
    public class AnimationHelper : MonoBehaviour
    {
        private void Awake()
        {
            _defaultScale = transform.localScale;
        }

        public void Bump()
        {
            LeanTween.cancel(gameObject);
            transform.localScale = new Vector3(_defaultScale.x * 1.1f, _defaultScale.y * 0.9f);
            LeanTween
                .scale(gameObject, _defaultScale, 0.14f)
                .setEaseInQuad();
        }
        
        private Vector3 _defaultScale;
    }
}