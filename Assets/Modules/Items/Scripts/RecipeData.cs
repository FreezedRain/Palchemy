using System.Collections.Generic;
using UnityEngine;

namespace Potions.Gameplay
{
    [CreateAssetMenu(fileName = "NewRecipe", menuName = "Data/RecipeData")]
    public class RecipeData : ScriptableObject
    {
        public string ResultId => _resultId;
        public IReadOnlyList<string> Ingredients => _ingredients;
        public Sprite PageSprite => _pageSprite;

        public bool CanCook(List<string> usedIngredients)
        {
            if (usedIngredients.Count != _ingredients.Count)
                return false;
            return new HashSet<string>(usedIngredients).SetEquals(_ingredients);
        }

        [SerializeField]
        private string _resultId;
        [SerializeField]
        private Sprite _pageSprite;
        [SerializeField]
        public List<string> _ingredients;
    }
}