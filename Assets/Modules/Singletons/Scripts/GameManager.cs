using System;
using UnityEngine;

namespace Potions.Global
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public TransitionManager Transitions { get; private set; }
        public SaveData SaveData { get; private set; }

        public void RecordLevelCompleted(string id)
        {
            if (SaveData.CompletedLevels.Contains(id)) return;

            print($"Completed level {id}!");
            SaveData.CompletedLevels.Add(id);
            SaveGame();
        }

        protected override void Awake()
        {
            base.Awake();
            Transitions = GetComponent<TransitionManager>();
            LoadGame();
        }

        private void LoadGame()
        {
            string saveString = PlayerPrefs.GetString("save");
            SaveData = String.IsNullOrEmpty(saveString) ? new SaveData() : JsonUtility.FromJson<SaveData>(saveString);
        }

        private void SaveGame()
        {
            string saveString = JsonUtility.ToJson(SaveData);
            PlayerPrefs.SetString("save", saveString);
        }
    }
}