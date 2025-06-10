using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MealTracker.Database;
using MealTracker.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTracker.ViewModels
{
    public partial class IngredientPickerViewModel : ObservableObject
    {
        private readonly SqliteConnectionFactory sqliteConnectionFactory;

        #region Properties

        [ObservableProperty]
        private ObservableCollection<Ingredient> ingredients = new ObservableCollection<Ingredient>();

        [ObservableProperty]
        private ObservableCollection<object> selectedIngredients = new ObservableCollection<object>();
        #endregion

        public IngredientPickerViewModel(SqliteConnectionFactory sqliteConnectionFactory)
        {
            this.sqliteConnectionFactory = sqliteConnectionFactory;
            LoadIngredientsCommand.Execute(null); // Load ingredients when the ViewModel is initialized 
        }


        #region Commands
        [RelayCommand]
        private async Task LoadIngredientsAsync()
        {
            ISQLiteAsyncConnection database = sqliteConnectionFactory.CreateConnection();

            try
            {
                List<IngredientDTO> ingredientDTOs = await database.Table<IngredientDTO>().ToListAsync();
                Ingredients.Clear(); // Clear existing ingredients
                foreach (var dto in ingredientDTOs)
                {
                    Ingredients.Add(new Ingredient
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
                                Text = $"Error loading ingredients : {ex.Message}"
                            }
                        }
                    }
                };
                Shell.Current.CurrentPage.ShowPopup(popup);
            }
        }

        [RelayCommand]
        private async Task CreateIngredientAsync()
        {
            ISQLiteAsyncConnection database = sqliteConnectionFactory.CreateConnection();

            string result = await Shell.Current.CurrentPage.DisplayPromptAsync("Create new ingredient", "Enter the name of the ingredient:", "Add", "Cancel", "Ingredient Name");

            //If popup cancelled or string is empty, we do not create a new ingredient
            if (!string.IsNullOrEmpty(result))
            {
                IngredientDTO ingredientDTO = new IngredientDTO
                {
                    Name = result
                };

                await database.InsertAsync(ingredientDTO);

                Ingredients.Add(new Ingredient
                (
                    ingredientDTO.Id,
                    ingredientDTO.Name
                ));
            }
        }

        #endregion


    }
}
