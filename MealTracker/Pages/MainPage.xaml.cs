using MealTracker.ViewModels;

namespace MealTracker
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
       
    }
}
