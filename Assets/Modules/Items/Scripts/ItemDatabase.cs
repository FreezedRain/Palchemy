using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Potions.Gameplay
{
    public class ItemDatabase : MonoSingleton<ItemDatabase>
    {
        public static ItemData GetItem(string id) => id == null ? null : Instance._items[id];

        public RecipeData FindRecipeByIngredients(List<string> ingredients) =>
            _recipes.FirstOrDefault(r => r.CanCook(ingredients));

        public RecipeData FindRecipeByOutcome(string id) =>
            _recipes.FirstOrDefault(r => r.ResultId == id);

        protected override void Awake()
        {
            base.Awake();
            LoadItems();
            LoadRecipes();
        }

        private void LoadItems()
        {
            _items = new();
            var loadedItems = Resources.LoadAll<ItemData>(_itemsFolder);
            foreach (var item in loadedItems)
            {
                _items.Add(item.Id, item);
            }

            Debug.Log($"Loaded {_items.Count} items.");
        }

        private void LoadRecipes()
        {
            _recipes = Resources.LoadAll<RecipeData>(_recipesFolder).ToList();

            Debug.Log($"Loaded {_recipes.Count} recipes.");
        }

        [SerializeField] private string _itemsFolder;
        [SerializeField] private string _recipesFolder;

        private Dictionary<string, ItemData> _items;
        private List<RecipeData> _recipes;
    }
}