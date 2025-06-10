using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        //Only for Display purposes, not stored in the database
        [ObservableProperty]
        private Ingredient? ingredient;

        public RecipeIngredient()
        {
            // Default constructor for serialization/deserialization
        }

        // Base constructor for basic recipe ingredient creation
        public RecipeIngredient(int recipeId, int ingredientId, Ingredient ingredient)
        {
            RecipeId = recipeId;
            IngredientId = ingredientId;
            Ingredient = ingredient;
        }

        public RecipeIngredient(int id, int recipeId, int ingredientId, Ingredient ingredient, double quantity, UnitType unitType)
        {
            Id = id;
            RecipeId = recipeId;
            IngredientId = ingredientId;
            Ingredient = ingredient;
            Quantity = quantity;
            UnitType = unitType;
        }

    }
}
