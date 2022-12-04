using System;
using System.Collections.Generic;
using UnityEngine;

namespace Potions.Gameplay
{
    public class AltarInteractable : ContainerInteractable
    {
        public event Action<string> ItemAdded;

        protected override bool CanHolderInteract(ItemHolder holder) => holder.ItemId != null;

        protected override string GetItem() => null;
        
        protected override void OnItemAdded(string id)
        {
            ItemAdded?.Invoke(id);
        }
    }
}