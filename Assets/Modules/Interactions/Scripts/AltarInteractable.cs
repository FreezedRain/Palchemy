using System;
using UnityEngine;

namespace Potions.Gameplay
{
    public class AltarInteractable : ContainerInteractable
    {
        public event Action<string> ItemAdded;

        protected override bool CanHolderInteract(ItemHolder holder, InteractionType type)
        {
            // Only accept items that can be destroyed
            return holder.ItemId != null && holder.Item.CanDestroy;
        }

        protected override string GetItem() => null;

        protected override void OnItemAdded(string id)
        {
            ItemAdded?.Invoke(id);
            var ghost = Instantiate(_itemGhostPrefab, _ghostOrigin);
            ghost.transform.localPosition = Vector3.zero;
            ghost.Setup(id);
        }

        [SerializeField] private ItemGhost _itemGhostPrefab;
        [SerializeField] private Transform _ghostOrigin;
    }
}