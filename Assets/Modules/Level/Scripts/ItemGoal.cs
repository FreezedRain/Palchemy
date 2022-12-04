using System;
using UnityEngine;

namespace Potions.Level
{
    public abstract class ItemGoal
    {
        public abstract bool CanAcceptItem(string id);
        public abstract void AcceptItem(string id);
        public abstract bool IsComplete();
    }
}
