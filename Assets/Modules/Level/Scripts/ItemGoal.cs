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
            if (_progress >= _data.Goal)
            {
                _isComplete = true;
                _card.IsShining = true;
                _card.Fill = 1f;
            }

            if (_isComplete) return;

            _timer += Time.deltaTime;
            _timer2 += Time.deltaTime;
            
            Debug.Log($"IP10s: {items.Count(i => _timer2 - i <= 10f)} IPS: {items.Count(i => _timer2 - i <= 10f) / 10f} for {_data.ItemId}");

            while (_timer >= 1f)
            {
                _progress = Mathf.Clamp(_progress - _data.Decay * _timer, 0, _data.Goal);
                _timer = 0f;
                _card.Fill = _progress / _data.Goal;
            }
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
