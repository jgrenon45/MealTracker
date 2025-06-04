using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MealTracker.Database;
using MealTracker.Entities;
using SQLite;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace MealTracker.ViewModels
{
    public partial class RecipesViewModel : ObservableObject
    {
        private readonly SqliteConnectionFactory sqliteConnectionFactory;

        private ObservableCollection<Recipe> _recipes = new ObservableCollection<Recipe>();

        public ObservableCollection<Recipe> Recipes
        {
            get => _recipes;
            set => SetProperty(ref _recipes, value);
        }

        public RecipesViewModel(SqliteConnectionFactory sqliteConnectionFactory)
        {
            this.sqliteConnectionFactory = sqliteConnectionFactory;  
            
            LoadRecipesCommand.Execute(null); // Load recipes when the view model is initialized

        }

        [RelayCommand]
        private async Task LoadRecipesAsync()
        {
            ISQLiteAsyncConnection database = sqliteConnectionFactory.CreateConnection();
            try
            {
                List<RecipeDTO> recipes = await database.Table<RecipeDTO>().ToListAsync();

                foreach (RecipeDTO dto in recipes)
                {
                    _recipes.Add(new Recipe
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

            _recipes.Add(new Recipe
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

                        _recipes.Remove(recipe);
                    }
                    break;
            }
        }            

    }
}
