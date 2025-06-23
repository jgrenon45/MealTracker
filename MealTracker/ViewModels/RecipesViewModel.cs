using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MealTracker.Database;
using MealTracker.Entities;
using MealTracker.Pages;
using SQLite;
using System.Collections.ObjectModel;

namespace MealTracker.ViewModels
{
    public partial class RecipesViewModel : ObservableObject
    {
        private readonly SqliteConnectionFactory sqliteConnectionFactory;

        #region Properties

        [ObservableProperty]
        private ObservableCollection<Recipe> recipes = new ObservableCollection<Recipe>();

        [ObservableProperty]
        private ObservableCollection<Recipe> filteredRecipes = new ObservableCollection<Recipe>();

        [ObservableProperty]
        private Recipe selectedRecipe;

        [ObservableProperty]
        private string recipeSearchText = string.Empty;
        #endregion


        public RecipesViewModel(SqliteConnectionFactory sqliteConnectionFactory)
        {
            this.sqliteConnectionFactory = sqliteConnectionFactory;  
        }

        partial void OnSelectedRecipeChanged(Recipe value)
        {
            if (value != null)
            {
                GoToRecipeDetailsCommand.Execute(value);
            }
        }

        partial void OnRecipeSearchTextChanged(string oldValue, string newValue)
        {
            FilterRecipesCommand.Execute(null);
        }

        #region Commands

        [RelayCommand]
        private async Task LoadRecipesAsync()
        {
            ISQLiteAsyncConnection database = sqliteConnectionFactory.CreateConnection();
            try
            {
                List<RecipeDTO> recipesDTO = await database.Table<RecipeDTO>().ToListAsync();

                Recipes.Clear(); // Clear existing recipes

                foreach (RecipeDTO dto in recipesDTO)
                {
                    Recipes.Add(new Recipe
                    (
                        dto.Id,
                        dto.Name,
                        dto.ImagePath,
                        dto.IsFavorite
                    ));
                }
                FilterRecipesCommand.Execute(null);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during data retrieval
                var popup = new Popup
                {
                    Content = new VerticalStackLayout
                    {
                        Children =
                        {
                            new Label
                            {
                                Text = $"Error loading recipes: {ex.Message}"
                            }
                        }
                    }
                };
                Shell.Current.CurrentPage.ShowPopup(popup);
            }
        }

        [RelayCommand]
        private async Task CreateRecipe()
        {
            ISQLiteAsyncConnection database = sqliteConnectionFactory.CreateConnection();

            RecipeDTO recipeDTO = new RecipeDTO
            {
                Name = "New Recipe"
            };

            await database.InsertAsync(recipeDTO);

            Recipes.Add(new Recipe
            (
                recipeDTO.Id,
                recipeDTO.Name
            ));
        }

        [RelayCommand]
        private async Task ShowOptions(Recipe recipe)
        {
            string action = await Shell.Current.CurrentPage.DisplayActionSheet($"Options for {recipe.Name}", "Cancel", null, "Edit", "Delete");

            switch (action)
            {
                case "Edit":
                    // Navigate to edit page or show edit UI
                    break;
                case "Delete":
                    bool confirm = await Shell.Current.CurrentPage.DisplayAlert(
                        "Delete", $"Delete {recipe.Name}?", "Yes", "No");
                    if (confirm)
                    {
                        DeleteRecipeCommand.Execute(recipe);
                    }
                    break;
            }
        }

        [RelayCommand]
        private async Task GoToRecipeDetailsAsync(Recipe recipe)
        {
            if (recipe == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(RecipeDetailsPage)}?RecipeId={recipe.Id}&IsEditMode={false}");
        }

        [RelayCommand]
        private async Task DeleteRecipeAsync(Recipe recipe)
        {
            ISQLiteAsyncConnection database = sqliteConnectionFactory.CreateConnection();

            RecipeDTO recipeDTO = new RecipeDTO
            {
                Id = recipe.Id,
                Name = recipe.Name,
            };

            await database.DeleteAsync(recipeDTO);

            recipe.DeleteOldImageFile(recipe.ImagePath); // Delete the old image file if it exists

            Recipes.Remove(recipe);
        }

        [RelayCommand]
        private void FilterRecipes()
        {
            FilteredRecipes.Clear();

            var query = RecipeSearchText?.Trim() ?? "";

            var results = string.IsNullOrWhiteSpace(query)
                ? Recipes
                : Recipes
                    .Where(i => i.Name.StartsWith(query, StringComparison.OrdinalIgnoreCase));

            foreach (Recipe item in results)
            {
                FilteredRecipes.Add(item);
            }
        }

        [RelayCommand]
        private async Task ToggleFavorite(Recipe recipe)
        {
            ISQLiteAsyncConnection database = sqliteConnectionFactory.CreateConnection();

            RecipeDTO recipeDTO = new RecipeDTO
            {
                Id = recipe.Id,
                Name = recipe.Name,
                ImagePath = recipe.ImagePath,
                PreparationTime = recipe.PreparationTime,
                CookingTime = recipe.CookingTime,
                Servings = recipe.Servings,
                IsFavorite = !recipe.IsFavorite,
            };

            await database.UpdateAsync(recipeDTO);

            recipe.IsFavorite = !recipe.IsFavorite;
        }
        #endregion
    }
}
