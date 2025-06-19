using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTracker.Entities
{
    public partial class GroceryItem : ObservableObject
    {
        public int Id { get; }

        [ObservableProperty]
        private int ingredientId;

        [ObservableProperty]
        private double quantity;

        [ObservableProperty]
        private UnitType unitType;

        [ObservableProperty]
        private bool isChecked;

        [ObservableProperty]
        private Ingredient? ingredient;

        // Simple constructor for basic grocery item creation
        public GroceryItem(int id, int ingredientId, double quantity, UnitType unitType)
        {
            Id = id;
            IngredientId = ingredientId;
            Quantity = quantity;
            UnitType = unitType;
            IsChecked = false; // Default to not purchased
        }
    }
}
