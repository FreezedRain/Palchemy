using System;

namespace Potions.Gameplay
{
    public interface IInputProvider
    {
        public event Action Interacted;
        
        public InputState GetState();
    }
}