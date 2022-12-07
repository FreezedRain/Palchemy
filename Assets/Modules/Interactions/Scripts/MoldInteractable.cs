using System;
using System.Collections.Generic;
using UnityEngine;

namespace Potions.Gameplay
{
    public class MoldInteractable : ContainerInteractable
    {
        protected override bool CanHolderInteract(ItemHolder holder) => holder.ItemId == "golem_heart";

        protected override string GetItem() => null;

        protected override void OnItemAdded(string id)
        {
            Instantiate(_golemPrefab, _golemSpawnPoint.transform.position, Quaternion.identity);
        }

        public override bool CanSkip(Interactor interactor)
        {
            return !CanInteract(interactor);
        }

        [SerializeField]
        private CharacterLogic _golemPrefab;
        [SerializeField]
        private Transform _golemSpawnPoint;
    }
}