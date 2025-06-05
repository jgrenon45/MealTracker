using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MealTracker.Database;
using MealTracker.Entities;
using SQLite;

namespace MealTracker.ViewModels
{
    [QueryProperty(nameof(RecipeId), "RecipeId")]
    [QueryProperty(nameof(IsEditMode), "IsEditMode")]
    public partial class RecipeDetailsViewModel : ObservableObject
    {
        private readonly SqliteConnectionFactory sqliteConnectionFactory;

        #region Properties

        [ObservableProperty]
        private int recipeId;

        [ObservableProperty]
        private Recipe recipe;

        [ObservableProperty]
        private bool isIngredientsVisible = true;

        [ObservableProperty]
        private bool isInstructionsVisible = false;

        [ObservableProperty]
        private bool isEditMode = false;
        #endregion

        public RecipeDetailsViewModel(SqliteConnectionFactory sqliteConnectionFactory)
        {
            this.sqliteConnectionFactory = sqliteConnectionFactory;
        }

        partial void OnRecipeIdChanged(int value)
        {
            LoadRecipeDetailsCommand.Execute(null); // Load recipe when RecipeId changes
        }

        #region Commands
        [RelayCommand]
        private async Task LoadRecipeDetailsAsync()
        {
            ISQLiteAsyncConnection database = sqliteConnectionFactory.CreateConnection();
            try
            {
                RecipeDTO recipeDTO = await database.Table<RecipeDTO>().FirstOrDefaultAsync(r => r.Id == RecipeId);

                if (recipeDTO != null)
                {
                    Recipe = new Recipe
                    (
                        recipeDTO.Id,
                        recipeDTO.Name,
                        recipeDTO.Description,
                        recipeDTO.Ingredients,
                        recipeDTO.Instructions,
                        recipeDTO.PreparationTime,
                        recipeDTO.CookingTime,
                        recipeDTO.Servings
                    );
                }
                else
                {
                    var popup = new Popup
                    {
                        Content = new VerticalStackLayout
                        {
                            Children =
                        {
                            new Label
                            {
                                Text = "// No recipe found with the given ID"
                            }
                        }
                        }
                    };
                    Shell.Current.CurrentPage.ShowPopup(popup);
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
                                Text = $"Error loading selected recipe: {ex.Message}"
                            }
                        }
                    }
                };
                Shell.Current.CurrentPage.ShowPopup(popup);
            }
        }

        [RelayCommand]
        private async Task SaveRecipeAsync()
        {
            ISQLiteAsyncConnection database = sqliteConnectionFactory.CreateConnection();

            RecipeDTO recipeDTO = new RecipeDTO
            {
                Id = Recipe.Id,
                Name = Recipe.Name,
                Description = Recipe.Description,
                Ingredients = Recipe.Ingredients,
                Instructions = Recipe.Instructions,
                PreparationTime = Recipe.PreparationTime,
                CookingTime = Recipe.CookingTime,
                Servings = Recipe.Servings
            };

            try
            {
                await database.UpdateAsync(recipeDTO);

            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during data saving
                var popup = new Popup
                {
                    Content = new VerticalStackLayout
                    {
                        Children =
                        {
                            new Label
                            {
                                Text = $"Error saving recipe: {ex.Message}"
                            }
                        }
                    }
                };
                Shell.Current.CurrentPage.ShowPopup(popup);
            }

        }

        [RelayCommand]
        private void ShowIngredients()
        {
            IsIngredientsVisible = true;
            IsInstructionsVisible = false;
        }

        [RelayCommand]
        private void ShowInstructions()
        {
            IsIngredientsVisible = false;
            IsInstructionsVisible = true;
        }

        [RelayCommand]
        private void ToggleEditMode()
        {
            IsEditMode = !IsEditMode;
        }
        #endregion
    }
}
