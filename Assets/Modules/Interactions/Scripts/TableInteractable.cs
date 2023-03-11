using System;
using Potions.Global;
using UnityEngine;

namespace Potions.Gameplay
{
    /// <summary>
    /// Hold items that can be dropped and picked up later.
    /// Interaction type is important for golem AI.
    /// </summary>
    public class TableInteractable : ContainerInteractable
    {
        protected override bool CanHolderInteract(ItemHolder holder, InteractionType type)
        {
            switch (type)
            {
                case InteractionType.Any:
                    return _itemId != null ^ holder.ItemId != null;
                case InteractionType.Pickup:
                    return _itemId != null && holder.ItemId == null;
                case InteractionType.Drop:
                    return _itemId == null && holder.ItemId != null;
            }

            return false;
        }

        protected override string GetItem() => _itemId;

        protected override void OnItemAdded(string id)
        {
            _itemId = id;
            _itemHolder.SetItem(id);
        }

        protected override void PlayInteractAudio(Interactor interactor)
        {
            var clip = interactor.Character.ItemHolder.Item == null ? _dropClip : _pickupClip;
            clip.Play(transform.position);
        }

        private void Start()
        {
            if (!String.IsNullOrEmpty(_overrideItemId))
                OnItemAdded(_overrideItemId);
        }

        [SerializeField] private ItemHolder _itemHolder;
        [SerializeField] private string _overrideItemId;
        [SerializeField] private AudioClipData _pickupClip;
        [SerializeField] private AudioClipData _dropClip;
        private string _itemId;
    }
}