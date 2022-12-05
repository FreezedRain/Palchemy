using System;

namespace Potions.Global
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public TransitionManager Transitions { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Transitions = GetComponent<TransitionManager>();
        }
    }
}