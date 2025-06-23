using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        public string imagePath;

        [ObservableProperty]
        public ObservableCollection<RecipeIngredient> ingredients = new ObservableCollection<RecipeIngredient>();

        [ObservableProperty]
        public ObservableCollection<Instruction> instructions = new ObservableCollection<Instruction>();

        [ObservableProperty]
        public TimeSpan preparationTime;

        [ObservableProperty]
        public TimeSpan cookingTime;

        [ObservableProperty]
        public int servings;

        [ObservableProperty]
        public bool isFavorite;

        //Simple constructor for basic recipe creation
        public Recipe(int id, string name)
        {
            Id = id;
            Name = name;     
        }

        //Simple constructor for basic recipe creation + image
        public Recipe(int id, string name, string imagePath, bool isFavorite)
        {
            Id = id;
            Name = name;
            ImagePath = imagePath;
            IsFavorite = isFavorite;
        }

        //Full constructor for detailed recipe creation
        public Recipe(int id, string name, string imagePath, TimeSpan preparationTime, TimeSpan cookingTime, int servings)
        {
            Id = id;
            Name = name;
            ImagePath = imagePath;
            PreparationTime = preparationTime;
            CookingTime = cookingTime;
            Servings = servings;
        }

        public void DeleteOldImageFile(string oldImagePath)
        {
            if (!string.IsNullOrWhiteSpace(oldImagePath) && File.Exists(oldImagePath))
            {
                try
                {
                    File.Delete(oldImagePath);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to delete image: {ex.Message}");
                }
            }
        }
    }
}
