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
    public partial class GroceriesViewModel : ObservableObject
    {
        private readonly SqliteConnectionFactory sqliteConnectionFactory;

        public GroceriesViewModel(SqliteConnectionFactory sqliteConnectionFactory)
        {
            this.sqliteConnectionFactory = sqliteConnectionFactory;
        }

        #region Properties
        [ObservableProperty]
        public ObservableCollection<GroceryItem> groceryItems = new ObservableCollection<GroceryItem>();

        public List<UnitType> UnitTypes { get; } = Enum.GetValues(typeof(UnitType)).Cast<UnitType>().ToList();

        #endregion

        #region Commands

        [RelayCommand]
        public async Task LoadGroceryItemsAsync()
        {
            ISQLiteAsyncConnection database = sqliteConnectionFactory.CreateConnection();
            try
            {
                List<GroceryItemDTO> groceryItemsDTO = await database.Table<GroceryItemDTO>().ToListAsync();
                GroceryItems.Clear(); // Clear existing grocery items
                foreach (GroceryItemDTO dto in groceryItemsDTO)
                {
                    dto.Ingredient = await database.Table<IngredientDTO>().FirstOrDefaultAsync(i => i.Id == dto.IngredientId);
                    GroceryItem newItem = new GroceryItem
                    (
                        dto.Id,
                        dto.IngredientId,
                        dto.Quantity,
                        dto.UnitType
                    );
                    newItem.Ingredient = new Ingredient
                    (
                        dto.Ingredient.Id,
                        dto.Ingredient.Name
                    );
                    GroceryItems.Add(newItem);
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
                                Text = $"Error loading grocery items: {ex.Message}"
                            }
                        }
                    }
                };
                Shell.Current.CurrentPage.ShowPopup(popup);
            }
        }

        [RelayCommand]
        public async Task AddGroceryItemAsync()
        {
            var popup = new IngredientPicker(new IngredientPickerViewModel(sqliteConnectionFactory));

            var result = await Shell.Current.CurrentPage.ShowPopupAsync(popup);

            if (result is IEnumerable<object> objectList)
            {
                //Cast the object list to ingredient list
                List<Ingredient> selectedIngredients = objectList.OfType<Ingredient>().ToList();

                foreach (Ingredient ingredient in selectedIngredients)
                {
                    GroceryItem gi = new GroceryItem
                    (
                        0,
                        ingredient.Id,
                        0,
                        UnitType.Grams
                    );
                    gi.Ingredient = ingredient;
                    GroceryItems.Add(gi);
                }
            }
        }
        #endregion
    }
}
