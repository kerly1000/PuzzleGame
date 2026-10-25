using Microsoft.Extensions.Logging;
using PuzzleGame.Data;
using PuzzleGame.Pages;
using PuzzleGame.Services;


namespace PuzzleGame
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<PuzzleDatabase>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<PuzzleService>();
            //AddTransient lubab rakenduse avamisel lisada uue lehe pildi lisamiseks
            builder.Services.AddTransient<AddPuzzlePage>();
            
            builder.Services.AddSingleton<PuzzleGame.Services.DefaultPuzzleService>();

            return builder.Build();
        }
    }
}
