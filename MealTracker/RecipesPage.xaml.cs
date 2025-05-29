using MealTracker.ViewModels;

namespace MealTracker;

public partial class RecipesPage : ContentPage
{
	public RecipesPage(RecipesViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}