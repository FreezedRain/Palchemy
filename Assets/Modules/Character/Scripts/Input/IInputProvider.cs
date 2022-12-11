using System;

namespace Potions.Gameplay
{
    public interface IInputProvider
    {
        public event Action Interacted;
        public event Action AltInteractStarted;
        public event Action AltInteractFinished;
        
        public InputState GetState();
    }
}