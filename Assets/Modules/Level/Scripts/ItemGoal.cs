using Potions.Gameplay;
using UnityEngine;

namespace Potions.Level
{
    public class ItemGoal
    {
        public ItemGoal(ItemGoalData data, RecipeCard card)
        {
            _data = data;
            _card = card;
            _card.SetBase(ItemDatabase.Instance.FindRecipeByOutcome(_data.ItemId).PageSprite);
            _card.SetProgress(0, _data.NumericGoal);
        }

        public bool IsComplete => _isComplete;

        public bool CanAcceptItem(string id) => _data.ItemId == id;

        public void AcceptItem(string id)
        {
            _count = Mathf.Clamp(_count + 1, 0, _data.NumericGoal);
            _card.SetProgress(_count, _data.NumericGoal);
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
                    _card.SetProgress(_count, _data.NumericGoal);
                }
            }
            else
            {
                _timer = _data.Timespan;
            }

            _isComplete = _count >= _data.NumericGoal;

            // Update card visuals
            _card.IsShining = _isComplete;
            _card.Fill = 1.0f - Mathf.Clamp01(_timer / _data.Timespan);
        }

        public void SetFinished(bool finished)
        {
            if (finished)
            {
                _card.Fill = 1f;
                _card.IsShining = true;
            }
        }

        private bool _isComplete;
        private float _timer;
        private int _count;

        private RecipeCard _card;
        private ItemGoalData _data;
    }
}