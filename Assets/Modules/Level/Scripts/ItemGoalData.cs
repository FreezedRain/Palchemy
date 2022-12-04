using System;

namespace Potions.Level
{
    [Serializable]
    public struct ItemGoalData
    {
        public string ItemId;
        public float Goal;
        public float Decay;
    }
}