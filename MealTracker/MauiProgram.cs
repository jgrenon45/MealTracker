using CommunityToolkit.Maui;
using MealTracker.Database;
using MealTracker.Pages;
using MealTracker.ViewModels;
using Microsoft.Extensions.Logging;

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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Logging.AddDebug();

            builder.Services.AddSingleton<SqliteConnectionFactory>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddTransient<RecipesPage>();
            builder.Services.AddTransient<RecipesViewModel>();

            builder.Services.AddTransient<RecipeDetailsPage>();
            builder.Services.AddTransient<RecipeDetailsViewModel>();

#if DEBUG
#endif

            return builder.Build();
        }
    }
}
