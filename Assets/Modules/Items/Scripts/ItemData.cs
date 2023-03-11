using UnityEngine;

namespace Potions.Gameplay
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Data/ItemData")]
    public class ItemData : ScriptableObject
    {
        public string Id => _id;
        public Sprite Sprite => _sprite;
        public bool IsIngredient => _isIngredient;
        public bool CanDestroy => _canDestroy;

        [SerializeField]
        private string _id;
        [SerializeField]
        public Sprite _sprite;
        [SerializeField]
        private bool _isIngredient;
        [SerializeField]
        private bool _canDestroy;
    }
}