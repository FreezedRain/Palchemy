using System;
using System.Collections.Generic;

namespace Potions.Global
{
    [Serializable]
    public class SaveData
    {
        public List<string> CompletedLevels;

        public SaveData()
        {
            CompletedLevels = new();
        }
    }
}