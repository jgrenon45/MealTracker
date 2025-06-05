using MealTracker.Pages;

namespace MealTracker
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(RecipesPage), typeof(RecipesPage));
            Routing.RegisterRoute(nameof(RecipeDetailsPage), typeof(RecipeDetailsPage));
        }
    }
}
