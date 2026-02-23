using Microsoft.Extensions.Logging;

namespace JitsiMeetDemo;

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

#if ANDROID
		builder.Services.AddSingleton<IJitsiMeetService, JitsiMeetDemo.Services.AndroidJitsiMeetService>();
#elif IOS
		builder.Services.AddSingleton<IJitsiMeetService, JitsiMeetDemo.Services.IOSJitsiMeetService>();
#endif

		builder.Services.AddTransient<MainPage>();

		return builder.Build();
	}
}
