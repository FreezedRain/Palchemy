using System;
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
        }

        public void Update()
        {
            if (_progress >= _data.Goal)
            {
                _isComplete = true;
                _card.SetShine(true);
            }

            if (_isComplete) return;
            
            _progress = Mathf.Clamp(_progress - _data.Decay * Time.deltaTime, 0, _data.Goal);
            _card.SetFill(_progress / _data.Goal);
        }

        public ItemGoal(ItemGoalData data, RecipeCard card)
        {
            _data = data;
            _card = card;
            _card.SetSprite(ItemDatabase.FindRecipeByOutcome(_data.ItemId).PageSprite);
        }

        private float _progress;
        private bool _isComplete;

        private RecipeCard _card;
        private ItemGoalData _data;
    }
}
