using System;

namespace Potions.Level
{
    [Serializable]
    public struct ItemGoalData
    {
        public string ItemId;
        public int NumericGoal;
        public float Timespan;
    }
}