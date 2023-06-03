using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Model;
using StubLib;
using View.AppVM;
using ViewModel;

namespace View
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
                })
                .Services.AddSingleton<IDataManager, StubData>()
                .AddSingleton<ChampionsMgrVM>()
                .AddSingleton<MainAppVM>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }

}
