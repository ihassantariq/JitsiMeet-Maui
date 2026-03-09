using Foundation;
using UIKit;
using JitsiMeet.Maui;
using JitsiMeet.Maui.Platforms.iOS;

namespace JitsiMeetDemo;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	// Handle the auth redirect from the Jitsi sign-in page in Safari
	// The URL looks like: org.jitsi.meet://meet.jit.si/Room123#jwt=TOKEN
	[Export("application:openURL:options:")]
	public override bool OpenUrl(UIApplication application, NSUrl url, NSDictionary options)
	{
		if (url.Scheme == "org.jitsi.meet")
		{
			// Get the iOS Jitsi service and re-join with the authenticated URL
			var service = IPlatformApplication.Current?.Services.GetService<IJitsiMeetService>();
			if (service is IOSJitsiMeetService iosService)
			{
				iosService.JoinWithAuthUrl(url);
				return true;
			}
		}

		return base.OpenUrl(application, url, options);
	}
}
