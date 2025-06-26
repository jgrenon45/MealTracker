using CommunityToolkit.Maui;
using MealTracker.Database;
using MealTracker.Pages;
using MealTracker.ViewModels;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using Syncfusion.Maui.Toolkit.Hosting;

namespace MealTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                // Initialize the Syncfusion .NET MAUI Toolkit by adding the below line of code
                .ConfigureSyncfusionToolkit()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Logging.AddDebug();

            builder.Services.AddSingleton<SqliteConnectionFactory>();

            builder.Services.AddSingleton<RecipesPage>();
            builder.Services.AddSingleton<RecipesViewModel>();

            builder.Services.AddTransient<RecipeDetailsPage>();
            builder.Services.AddTransient<RecipeDetailsViewModel>();

            builder.Services.AddTransient<IngredientPicker>();
            builder.Services.AddTransient<IngredientPickerViewModel>();

            builder.Services.AddTransient<GroceriesPage>();
            builder.Services.AddTransient<GroceriesViewModel>();

#if DEBUG
#endif

            return builder.Build();
        }
    }
}
