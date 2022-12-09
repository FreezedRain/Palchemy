using System;
using System.Collections.Generic;
using System.Linq;
using Potions.Gameplay;
using UnityEngine;

namespace Potions.Level
{
    public class ItemGoal
    {
        public bool IsComplete => _isComplete;
        
        public bool CanAcceptItem(string id) => _data.ItemId == id;

        public void AcceptItem(string id)
        {
            _progress += 1;
            items.Add(_timer2);
        }

        public void Update()
        {
            int itemCount = items.Count(i => _timer2 - i <= 30f);
            
            _isComplete = itemCount >= _data.Goal;
            _card.IsShining = _isComplete;

            _timer += Time.deltaTime;
            _timer2 += Time.deltaTime;

            
            
            while (_timer >= 1f)
            {
                // Debug.Log($"IP30s: {items.Count(i => _timer2 - i <= 30f)} IPS: {items.Count(i => _timer2 - i <= 30f) / 30f} for {_data.ItemId}");

                _progress = Mathf.Clamp(_progress - _data.Decay * _timer, 0, _data.Goal + 1);
                _timer = 0f;
            }
            
            _card.SetProgress(itemCount, (int) _data.Goal);
            _card.Fill = itemCount / _data.Goal;

            // _card.Fill = Mathf.Clamp01(_progress / _data.Goal);
        }

        public ItemGoal(ItemGoalData data, RecipeCard card)
        {
            _data = data;
            _card = card;
            _card.SetBase(ItemDatabase.FindRecipeByOutcome(_data.ItemId).PageSprite);
            items = new();
        }

        private float _progress;
        private bool _isComplete;
        private float _timer;

        private RecipeCard _card;
        private ItemGoalData _data;
        private float _timer2;
        
        private List<float> items;
    }
}
