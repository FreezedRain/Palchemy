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
        
        public void SetItem(string id, Vector3? itemPosition = null)
        {
            _id = id;
            _spriteRenderer.sprite = Item ? Item.Sprite : null;

            if (itemPosition.HasValue)
            {
                LeanTween.cancel(_spriteRenderer.gameObject);
                // _spriteRenderer.transform.localScale = Vector3.one * 1.2f;
                // LeanTween.scale(_spriteRenderer.gameObject, Vector3.one, 0.15f).setEaseOutCubic();

                int order = _spriteRenderer.sortingOrder;
                _spriteRenderer.enabled = true;
                _spriteRenderer.sortingOrder = order + 1;
                _spriteRenderer.transform.position = itemPosition.Value;
                _spriteRenderer.transform.SetParent(transform.parent.parent, true);
                _spriteRenderer.transform.localScale = Vector3.one;
                // _spriteRenderer.transform.SetParent(transform, true);
                Vector3 goal = transform.parent.parent.InverseTransformPoint(transform.position);
                // _spriteRenderer.transform.localPosition = _spriteRenderer.transform.InverseTransformPoint(itemPosition.Value);
                LeanTween.moveLocal(_spriteRenderer.gameObject, goal, 0.15f)
                    .setEaseInCubic()
                    .setOnComplete(() =>
                    {
                        _spriteRenderer.transform.SetParent(transform, true);
                        _spriteRenderer.sortingOrder = order;
                        if (_isInvisible)
                        {
                            LeanTween.scale(_spriteRenderer.gameObject, Vector3.zero, 0.15f)
                                .setEaseInCubic()
                                .setOnComplete(() => _spriteRenderer.enabled = false);
                        }
                    });

                // LeanTween.scale(_spriteRenderer.gameObject, Vector3.one, 0.15f).setEaseOutCubic();

                // _spriteRenderer.color = Color.clear;
                // LeanTween.color(_spriteRenderer.gameObject, Color.white, 0.1f);
                // transform.localScale = Vector3.zero;
                // LeanTween.scale(gameObject, Vector3.one, 0.175f).setEaseInCubic();
            }
        }

        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private bool _isInvisible;
        private string _id;
    }
}