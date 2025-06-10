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
        private Recipe selectedRecipe;
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
                        dto.Name
                    ));
                }
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
                        ISQLiteAsyncConnection database = sqliteConnectionFactory.CreateConnection();

                        RecipeDTO recipeDTO = new RecipeDTO
                        {
                            Id = recipe.Id,
                            Name = recipe.Name
                        };

                        await database.DeleteAsync(recipeDTO);

                        Recipes.Remove(recipe);
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
        #endregion
    }
}
