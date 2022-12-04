using UnityEngine;

namespace Potions.Level
{
    public class LevelUI : MonoBehaviour
    {
        public RecipeCard CreateRecipeCard()
        {
            return Instantiate(_recipeCardPrefab, _cardHolder);
        }
        
        [SerializeField]
        private RecipeCard _recipeCardPrefab;
        [SerializeField]
        private RectTransform _cardHolder;
    }
}