using System;
using System.Collections.Generic;
using Potions.Gameplay;
using Potions.Global;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Potions.Level
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        private void Start()
        {
            // Listen to altars
            foreach (var altar in FindObjectsOfType<AltarInteractable>())
            {
                altar.ItemAdded += OnAltarItemAdded;
            }
            
            // Setup recipe cards
            _goals = new();
            foreach (var goalData in _goalsData)
            {
                var card = _levelUI.CreateRecipeCard();
                _goals.Add(new ItemGoal(goalData, card));
            }
        }

        private void Update()
        {
            if (_isComplete) return;
            UpdateGoals();
        }

        private void OnAltarItemAdded(string id)
        {
            foreach (var goal in _goals)
            {
                if (goal.CanAcceptItem(id))
                    goal.AcceptItem(id);
            }
        }

        private void UpdateGoals()
        {
            bool allComplete = true;
            foreach (var goal in _goals)
            {
                goal.Update();
                allComplete &= goal.IsComplete;
            }

            // If all goals are complete
            if (allComplete)
            {
                // Proceed to next level?
                foreach (var goal in _goals)
                    goal.SetFinished(true);
                _isComplete = true;
                GameManager.Instance.RecordLevelCompleted(SceneManager.GetActiveScene().name);
                LeanTween.delayedCall(1f, () => GameManager.Instance.Transitions.LoadLevel("Bedroom"));
            }
        }

        [SerializeField]
        private LevelUI _levelUI;
        [SerializeField]
        private List<ItemGoalData> _goalsData;

        private List<ItemGoal> _goals;
        private bool _isComplete;
    }
}