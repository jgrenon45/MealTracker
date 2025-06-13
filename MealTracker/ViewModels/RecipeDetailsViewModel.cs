using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MealTracker.Database;
using MealTracker.Entities;
using MealTracker.Pages;
using Microsoft.VisualBasic.FileIO;
using SQLite;
using System.Collections.ObjectModel;

namespace MealTracker.ViewModels
{
    [QueryProperty(nameof(CurrentRecipeId), "RecipeId")]
    [QueryProperty(nameof(IsEditMode), "IsEditMode")]
    public partial class RecipeDetailsViewModel : ObservableObject
    {
        private readonly SqliteConnectionFactory sqliteConnectionFactory;

        #region Properties

        [ObservableProperty]
        private int currentRecipeId;

        [ObservableProperty]
        private Recipe recipe;

        [ObservableProperty]
        private bool isIngredientsVisible = true;

        [ObservableProperty]
        private bool isInstructionsVisible = false;

        [ObservableProperty]
        private bool isEditMode = false;

        public List<UnitType> UnitTypes { get; } = Enum.GetValues(typeof(UnitType)).Cast<UnitType>().ToList();
        #endregion

        public RecipeDetailsViewModel(SqliteConnectionFactory sqliteConnectionFactory)
        {
            this.sqliteConnectionFactory = sqliteConnectionFactory;
        }

        partial void OnCurrentRecipeIdChanged(int value)
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
                RecipeDTO recipeDTO = await database.Table<RecipeDTO>().FirstOrDefaultAsync(r => r.Id == CurrentRecipeId);
                recipeDTO.Ingredients = await database.Table<RecipeIngredientDTO>().Where(ri => ri.RecipeId == recipeDTO.Id).ToListAsync();
                recipeDTO.Instructions = await database.Table<InstructionDTO>().Where(i => i.RecipeId == recipeDTO.Id).ToListAsync();

                if (recipeDTO != null)
                {
                    Recipe = new Recipe
                    (
                        recipeDTO.Id,
                        recipeDTO.Name,
                        recipeDTO.PreparationTime,
                        recipeDTO.CookingTime,
                        recipeDTO.Servings
                    );

                    foreach (RecipeIngredientDTO riDTO in recipeDTO.Ingredients)
                    {
                        riDTO.Ingredient = await database.Table<IngredientDTO>().FirstOrDefaultAsync(i => i.Id == riDTO.IngredientId);
                        Recipe.Ingredients.Add(new RecipeIngredient
                        (
                            riDTO.Id,
                            riDTO.RecipeId,
                            riDTO.IngredientId,
                            new Ingredient
                            (
                                riDTO.Ingredient.Id,
                                riDTO.Ingredient.Name
                            ),
                            riDTO.Quantity,
                            riDTO.Unit
                        ));
                    }

                    foreach (InstructionDTO ingredientDTO in recipeDTO.Instructions)
                    {                       
                        Recipe.Instructions.Add(new Instruction
                        (
                            ingredientDTO.Id,
                            ingredientDTO.Description,
                            ingredientDTO.Order,
                            ingredientDTO.EstimatedTime,
                            ingredientDTO.IsCompleted
                        ));
                        Recipe.Instructions.ToList().Sort((x, y) => x.Order.CompareTo(y.Order)); // Sort instructions by order
                    }

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

            //Save recipe
            RecipeDTO recipeDTO = new RecipeDTO
            {
                Id = Recipe.Id,
                Name = Recipe.Name,
                Description = Recipe.Description,
                PreparationTime = Recipe.PreparationTime,
                CookingTime = Recipe.CookingTime,
                Servings = Recipe.Servings
            };

            try
            {
                var result = await database.UpdateAsync(recipeDTO);

                //Delete old RecipeIngredientDTOs
                var oldIngredients = await database.Table<RecipeIngredientDTO>()
                    .Where(ri => ri.RecipeId == Recipe.Id)
                    .ToListAsync();

                //Delete old InstructionsDTOs
                var oldInstructions = await database.Table<InstructionDTO>()
                    .Where(i => i.RecipeId == Recipe.Id)
                    .ToListAsync();

                foreach (var old in oldIngredients)
                {
                    await database.DeleteAsync(old);
                }

                foreach (var old in oldInstructions)
                {
                    await database.DeleteAsync(old);
                }

                //Insert updated RecipeIngredientDTOs
                foreach (var ri in Recipe.Ingredients)
                {
                    var riDTO = new RecipeIngredientDTO
                    {
                        RecipeId = Recipe.Id,
                        IngredientId = ri.IngredientId,
                        Quantity = ri.Quantity,
                        Unit = ri.UnitType
                    };
                    await database.InsertAsync(riDTO);
                }

                foreach (var instruction in Recipe.Instructions)
                {
                    var instructionDTO = new InstructionDTO
                    {                     
                        RecipeId = Recipe.Id,
                        Description = instruction.Description,
                        Order = instruction.Order,
                        EstimatedTime = instruction.EstimatedTime,
                        IsCompleted = instruction.IsCompleted
                    };
                    await database.InsertAsync(instructionDTO);
                }

                await Shell.Current.CurrentPage.DisplayAlert("Recipe saved successfully!", recipeDTO.Name + " informations have been updated", "Back to recipes");
                await Shell.Current.GoToAsync(nameof(RecipesPage));
                
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

        [RelayCommand]
        private async Task ShowIngredientsPopupAsync()
        {            
            var popup = new IngredientPicker(new IngredientPickerViewModel(sqliteConnectionFactory, Recipe));

            var result = await Shell.Current.CurrentPage.ShowPopupAsync(popup);

            if (result is IEnumerable<object> objectList)
            {
                //Cast the object list to ingredient list
                List<Ingredient> selectedIngredients = objectList.OfType<Ingredient>().ToList();
            
                Recipe.Ingredients.Clear(); // Clear existing ingredients so only the ones selected in the popup are added

                foreach (Ingredient ingredient in selectedIngredients)
                {
                    RecipeIngredient ri = new RecipeIngredient
                    (
                        Recipe.Id,
                        ingredient.Id,
                        ingredient
                    );
                    Recipe.Ingredients.Add(ri);
                }               
            }
        }

        [RelayCommand]
        private void AddInstruction()
        {
            Instruction newInstruction = new Instruction
            (
                String.Empty, // Default empty instruction text
                Recipe.Instructions.Count() + 1
            );
            Recipe.Instructions.Add(newInstruction);
        }

        [RelayCommand]
        private void DeleteInstruction(Instruction instruction)
        {
            if (Recipe.Instructions.Contains(instruction))
            {
                Recipe.Instructions.Remove(instruction);

                // Reorder remaining instructions
                for (int i = 0; i < Recipe.Instructions.Count; i++)
                {
                    Recipe.Instructions[i].Order = i + 1;
                }
            }
        }
        #endregion
    }
}
