using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MealTracker.Database;
using MealTracker.Entities;
using SQLite;
using System.Collections.ObjectModel;

namespace MealTracker.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly SqliteConnectionFactory sqliteConnectionFactory;

        public MainViewModel(SqliteConnectionFactory sqliteConnectionFactory)
        {
            this.sqliteConnectionFactory = sqliteConnectionFactory;
        }

        [RelayCommand]
        private async Task GetStarted()
        {
            await Shell.Current.GoToAsync(nameof(RecipesPage));
        }

        [RelayCommand]
        private async Task ResetIngredients()
        {
            ISQLiteAsyncConnection database = sqliteConnectionFactory.CreateConnection();

            // Clear the Ingredients table in the database
            await database.DeleteAllAsync<IngredientDTO>();

        }
    }
}
