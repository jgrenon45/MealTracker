using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTracker.Entities
{
    public partial class RecipeIngredient : ObservableObject
    {
        public int Id { get; }

        [ObservableProperty]
        private int recipeId;

        [ObservableProperty]
        private int ingredientId;

        [ObservableProperty]
        private double quantity;

        [ObservableProperty]
        private UnitType unitType;

        // Simple constructor for basic recipe ingredient creation
        public RecipeIngredient(int id, int recipeId, int ingredientId, double quantity, UnitType unitType)
        {
            Id = id;
            RecipeId = recipeId;
            IngredientId = ingredientId;
            Quantity = quantity;
            UnitType = unitType;
        }
        
    }
}
