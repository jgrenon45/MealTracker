using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTracker.Entities
{
    public partial class Recipe : ObservableObject
    {
        public int Id { get; }

        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public string? description;

        [ObservableProperty]
        public ObservableCollection<RecipeIngredient> ingredients = new ObservableCollection<RecipeIngredient>();

        [ObservableProperty]
        public string? instructions;

        [ObservableProperty]
        public TimeSpan preparationTime;

        [ObservableProperty]
        public TimeSpan cookingTime;

        [ObservableProperty]
        public int servings;

        //Simple constructor for basic recipe creation
        public Recipe(int id, string name)
        {
            Id = id;
            Name = name;           
        }

        //Full constructor for detailed recipe creation
        public Recipe(int id, string name, string instructions, TimeSpan preparationTime, TimeSpan cookingTime, int servings)
        {
            Id = id;
            Name = name;
            Instructions = instructions;
            PreparationTime = preparationTime;
            CookingTime = cookingTime;
            Servings = servings;
        }
    }
}
