using MealTracker.ViewModels;

namespace MealTracker.Pages;

public partial class GroceriesPage : ContentPage
{
	public GroceriesPage(GroceriesViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is GroceriesViewModel vm)
        {
            vm.LoadGroceryItemsCommand.Execute(null); // Load recipes when the page appears
        }
    }
}