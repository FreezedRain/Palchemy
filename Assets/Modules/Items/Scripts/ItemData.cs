using UnityEngine;

namespace Potions.Gameplay
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Data/ItemData")]
    public class ItemData : ScriptableObject
    {
        public string Id => _id;
        public Sprite Sprite => _sprite;

        [SerializeField]
        private string _id;
        [SerializeField]
        public Sprite _sprite;
    }
}