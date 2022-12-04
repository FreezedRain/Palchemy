using System;
using System.Collections.Generic;
using Potions.Gameplay;
using UnityEngine;

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
            foreach (var goal in _goals)
                goal.Update();
        }

        private void OnAltarItemAdded(string id)
        {
            print($"Delivered {id}");
            foreach (var goal in _goals)
            {
                if (goal.CanAcceptItem(id))
                    goal.AcceptItem(id);
            }
        }

        [SerializeField]
        private LevelUI _levelUI;
        [SerializeField]
        private List<ItemGoalData> _goalsData;

        private List<ItemGoal> _goals;
    }
}