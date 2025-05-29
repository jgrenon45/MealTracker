namespace MealTracker
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(RecipesPage), typeof(RecipesPage));
        }
    }
}
