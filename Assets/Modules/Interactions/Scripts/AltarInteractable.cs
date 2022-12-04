using System;
using System.Collections.Generic;
using UnityEngine;

namespace Potions.Gameplay
{
    public class AltarInteractable : ContainerInteractable
    {
        public event Action Complete;
        
        protected override bool CanHolderInteract(ItemHolder holder)
        {
            return _isActive && holder.ItemId != null && _allowedItems.Contains(holder.ItemId);
        }

        protected override string GetItem() => null;
        
        protected override void OnItemAdded(string id)
        {
            _progress += 1f;
        }

        private void Update()
        {
            if (_isActive)
            {
                _progress = Mathf.Clamp(_progress - _productionDecrease * Time.deltaTime, 0, _productionGoal);

                if (_progress >= _productionGoal)
                {
                    Complete?.Invoke();
                    _isActive = false;
                }
            }
        }

        // TODO: Add activation function
        public bool _isActive;

        private float _progress;

        [SerializeField]
        private float _productionGoal;
        [SerializeField]
        private float _productionDecrease;
        [SerializeField]
        private List<string> _allowedItems;
    }
}