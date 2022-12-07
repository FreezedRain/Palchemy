using System;

namespace Potions.Gameplay
{
    public interface IInputProvider
    {
        public event Action Interacted;
        public event Action AltInteracted;
        
        public InputState GetState();
    }
}