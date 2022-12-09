using System;
using System.Collections.Generic;
using UnityEngine;

namespace Potions.Gameplay
{
    public class AltarInteractable : ContainerInteractable
    {
        public event Action<string> ItemAdded;

        protected override bool CanHolderInteract(ItemHolder holder, InteractionType type) => holder.ItemId != null && holder.ItemId != "golem_heart";

        protected override string GetItem() => null;

        protected override void OnItemAdded(string id)
        {
            ItemAdded?.Invoke(id);
            var ghost = Instantiate(_itemGhostPrefab, _ghostOrigin);
            ghost.transform.localPosition = Vector3.zero;
            ghost.Setup(id);
        }

        [SerializeField]
        private ItemGhost _itemGhostPrefab;
        [SerializeField]
        private Transform _ghostOrigin;
    }
}