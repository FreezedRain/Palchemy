using UnityEngine;

namespace Potions.Gameplay
{
    public class BookInteractable : BaseInteractable
    {
        protected override void OnInteract(Interactor interactor)
        {
            if (!_levelSelectUI.IsActive)
                _levelSelectUI.Show();
        }

        [SerializeField] private LevelSelectUI _levelSelectUI;
    }
}