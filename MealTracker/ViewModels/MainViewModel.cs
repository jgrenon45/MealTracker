using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MealTracker.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            // Initialize any properties or commands here if needed
        }

        [RelayCommand]
        private async Task GetStarted()
        {
            await Shell.Current.GoToAsync(nameof(RecipesPage));
        }
    }
}
