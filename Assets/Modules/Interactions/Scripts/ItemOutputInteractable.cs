using System;

namespace Potions.Gameplay
{
    public class ItemOutputInteractable : ContainerInteractable
    {
        public event Action ItemTaken;
        
        public void SetOutput(string itemId) => _output = itemId; 
            
        protected override bool CanHolderInteract(ItemHolder holder, InteractionType type) => holder.ItemId == null && _output != null;

        protected override string GetItem() => _output;

        protected override void OnItemAdded(string id)
        {
            _output = null;
            ItemTaken?.Invoke();
        }

        private string _output;
    }
}