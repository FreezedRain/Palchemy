using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Potions.Gameplay
{
    [RequireComponent(typeof(Sprite))]
    public class ItemHolder : MonoBehaviour
    {
        public ItemData Item => ItemDatabase.Get(_id);
        public string ItemId => _id;
        
        public void SetItem(string id, bool animate = true)
        {
            _id = id;
            _spriteRenderer.sprite = Item ? Item.Sprite : null;

            if (animate)
            {
                LeanTween.cancel(_spriteRenderer.gameObject);
                _spriteRenderer.transform.localScale = Vector3.one * 1.2f;
                LeanTween.scale(_spriteRenderer.gameObject, Vector3.one, 0.15f).setEaseOutCubic();
                // _spriteRenderer.color = Color.clear;
                // LeanTween.color(_spriteRenderer.gameObject, Color.white, 0.1f);
                // transform.localScale = Vector3.zero;
                // LeanTween.scale(gameObject, Vector3.one, 0.175f).setEaseInCubic();
            }
        }

        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        private string _id;
    }
}