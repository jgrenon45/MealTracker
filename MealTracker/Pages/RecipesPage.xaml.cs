using MealTracker.ViewModels;

namespace MealTracker.Pages;

public partial class RecipesPage : ContentPage
{
	public RecipesPage(RecipesViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

	protected override void OnAppearing()
	{
		base.OnAppearing();

		if (BindingContext is RecipesViewModel vm)
		{
			vm.LoadRecipesCommand.Execute(null); // Load recipes when the page appears
		}
    }
}