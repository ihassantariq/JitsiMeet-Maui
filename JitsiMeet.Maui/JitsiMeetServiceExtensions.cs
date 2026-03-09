using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;

namespace JitsiMeet.Maui;

/// <summary>
/// Extension methods for registering Jitsi Meet services in a MAUI application.
/// </summary>
public static class JitsiMeetServiceExtensions
{
    /// <summary>
    /// Registers the Jitsi Meet service for the current platform.
    /// </summary>
    /// <param name="builder">The MAUI app builder.</param>
    /// <returns>The builder instance for chaining.</returns>
    public static MauiAppBuilder UseJitsiMeet(this MauiAppBuilder builder)
    {
#if ANDROID
        builder.Services.AddSingleton<IJitsiMeetService, Platforms.Android.AndroidJitsiMeetService>();
#elif IOS
        builder.Services.AddSingleton<IJitsiMeetService, Platforms.iOS.IOSJitsiMeetService>();
#endif
        return builder;
    }
}
