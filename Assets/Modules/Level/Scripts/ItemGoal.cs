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
            // _itemLog.Add(_timer);
            _count = Mathf.Clamp(_count + 1, 0, _data.NumericGoal);
            _timer = 0f;
        }

        public void Update()
        {
            // Update logic
            if (_count > 0)
            {
                _timer += Time.deltaTime;
                if (_timer > _data.Timespan)
                {
                    _timer -= _data.Timespan;
                    _count = 0;
                }
            }
            else
            {
                _timer = _data.Timespan;
            }
            // _itemLog = _itemLog.Where(i => _timer - i <= _data.Timespan).ToList();
            // int itemsOverTimespan = _itemLog.Count;
            
            _isComplete = _count >= _data.NumericGoal;
            

            
            // Update card visuals
            _card.IsShining = _isComplete;
            _card.SetProgress(_count, _data.NumericGoal);
            _card.Fill = 1.0f - Mathf.Clamp01(_timer / _data.Timespan);
        }

        public void SetFinished(bool finished)
        {
            _isFinished = finished;
            if (finished)
            {
                _card.Fill = 1f;
                _card.IsShining = true;
            }
        }

        public ItemGoal(ItemGoalData data, RecipeCard card)
        {
            _data = data;
            _card = card;
            _card.SetBase(ItemDatabase.FindRecipeByOutcome(_data.ItemId).PageSprite);
            _itemLog = new();
        }

        private bool _isComplete;
        private bool _isFinished;
        private float _timer;
        private int _count;

        private RecipeCard _card;
        private ItemGoalData _data;

        private List<float> _itemLog;
    }
}
