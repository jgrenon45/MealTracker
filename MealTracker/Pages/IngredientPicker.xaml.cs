using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using MealTracker.Database;
using MealTracker.ViewModels;
using System.Collections.ObjectModel;

namespace MealTracker.Pages;

public partial class IngredientPicker : Popup
{   
    private readonly IngredientPickerViewModel viewModel;

    public IngredientPicker(IngredientPickerViewModel vm)
	{
		InitializeComponent();
        viewModel = vm;
		BindingContext = viewModel;
    }

    private void AddIngredientsButton_Clicked(object sender, EventArgs e)
    {
        Close(viewModel.SelectedIngredients);
    }

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}