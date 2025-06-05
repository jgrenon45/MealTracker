using MealTracker.ViewModels;

namespace MealTracker.Pages;

public partial class RecipeDetailsPage : ContentPage
{
	public RecipeDetailsPage(RecipeDetailsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}