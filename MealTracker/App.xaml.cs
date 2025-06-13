using CommunityToolkit.Maui.Views;
using MealTracker.Database;
using MealTracker.Entities;
using SQLite;

namespace MealTracker
{
    public partial class App : Application
    {
        private readonly SqliteConnectionFactory _sqliteConnectionFactory;

        public App(SqliteConnectionFactory sqliteConnectionFactory)
        {
            InitializeComponent();
            _sqliteConnectionFactory = sqliteConnectionFactory;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override async void OnStart()
        {
            ISQLiteAsyncConnection database = _sqliteConnectionFactory.CreateConnection();

            try
            {
                await database.CreateTableAsync<RecipeDTO>(); // Ensure the Meal table is created on startup
                await database.CreateTableAsync<IngredientDTO>(); // Ensure the Ingredient table is created on startup
                await database.CreateTableAsync<RecipeIngredientDTO>(); // Ensure the RecipeIngredient table is created on startup
                await database.CreateTableAsync<InstructionDTO>(); // Ensure the Instruction table is created on startup
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during table creation
                var popup = new Popup
                {
                    Content = new VerticalStackLayout
                    {
                        Children =
                        {
                            new Label
                            {
                                Text = $"Error creating database table: {ex.Message}"
                            }
                        }
                    }
                };
                Shell.Current.CurrentPage.ShowPopup(popup);
            }

            base.OnStart();
        }
    }
}