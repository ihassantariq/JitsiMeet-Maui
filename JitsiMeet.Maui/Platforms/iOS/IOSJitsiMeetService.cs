using Foundation;
using UIKit;
using JitsiMeet.iOS.Binding;
using Microsoft.Maui.ApplicationModel;

namespace JitsiMeet.Maui.Platforms.iOS;

public class IOSJitsiMeetService : IJitsiMeetService
{
    private JitsiMeetView? _jitsiMeetView;
    private UIViewController? _viewController;

    public Task JoinMeetingAsync(JitsiMeetOptions options)
    {
        var userInfo = new JitsiMeetUserInfo();
        if (!string.IsNullOrEmpty(options.DisplayName))
            userInfo.DisplayName = options.DisplayName;
        if (!string.IsNullOrEmpty(options.Email))
            userInfo.Email = options.Email;
        if (!string.IsNullOrEmpty(options.AvatarUrl))
            userInfo.Avatar = new NSUrl(options.AvatarUrl);

        var conferenceOptions = JitsiMeetConferenceOptions.FromBuilder(builder =>
        {
            builder.ServerURL = new NSUrl(options.ServerUrl);
            builder.Room = options.RoomName;
            builder.UserInfo = userInfo;

            if (!string.IsNullOrEmpty(options.Jwt))
                builder.Token = options.Jwt;

            builder.SetAudioMuted(options.AudioMuted);
            builder.SetVideoMuted(options.VideoMuted);

            builder.SetConfigOverride("disableDeepLinking", true);

            if (options.FeatureFlags != null)
            {
                foreach (var flag in options.FeatureFlags)
                {
                    if (flag.Value is bool boolVal)
                        builder.SetFeatureFlag(flag.Key, boolVal);
                    else
                        builder.SetFeatureFlag(flag.Key, flag.Value);
                }
            }

            if (options.ConfigOverrides != null)
            {
                foreach (var config in options.ConfigOverrides)
                {
                    if (config.Value is bool boolVal)
                        builder.SetConfigOverride(config.Key, boolVal);
                    else
                        builder.SetConfigOverride(config.Key, config.Value);
                }
            }
        });

        _jitsiMeetView = new JitsiMeetView
        {
            Frame = UIScreen.MainScreen.Bounds,
            Delegate = new CustomJitsiDelegate(this)
        };

        _viewController = new UIViewController
        {
            View = _jitsiMeetView,
            ModalPresentationStyle = UIModalPresentationStyle.FullScreen
        };

        var tcs = new TaskCompletionSource();
        var currentController = Platform.GetCurrentUIViewController();
        currentController?.PresentViewController(_viewController, true, () =>
        {
            _jitsiMeetView.Join(conferenceOptions);
            tcs.TrySetResult();
        });

        if (currentController == null)
            tcs.TrySetResult();

        return tcs.Task;
    }

    public Task LeaveMeetingAsync()
    {
        _jitsiMeetView?.Leave();
        Cleanup();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Called from AppDelegate when a deep link or auth redirect URL arrives.
    /// URL looks like: org.jitsi.meet://meet.jit.si/Room123#jwt=TOKEN
    /// </summary>
    public void JoinWithAuthUrl(NSUrl url)
    {
        var options = JitsiMeetConferenceOptions.FromBuilder(builder =>
        {
            builder.Room = url.AbsoluteString;
            builder.SetConfigOverride("disableDeepLinking", true);
        });

        if (_jitsiMeetView != null)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _jitsiMeetView.Join(options);
            });
            return;
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            _jitsiMeetView = new JitsiMeetView
            {
                Frame = UIScreen.MainScreen.Bounds,
                Delegate = new CustomJitsiDelegate(this)
            };

            _viewController = new UIViewController
            {
                View = _jitsiMeetView,
                ModalPresentationStyle = UIModalPresentationStyle.FullScreen
            };

            var currentController = Platform.GetCurrentUIViewController();
            currentController?.PresentViewController(_viewController, true, () =>
            {
                _jitsiMeetView.Join(options);
            });
        });
    }

    internal void Cleanup()
    {
        _jitsiMeetView?.RemoveFromSuperview();
        _jitsiMeetView = null;

        _viewController?.DismissViewController(true, null);
        _viewController = null;
    }

    private class CustomJitsiDelegate : JitsiMeetViewDelegate
    {
        private readonly IOSJitsiMeetService _parent;

        public CustomJitsiDelegate(IOSJitsiMeetService parent)
        {
            _parent = parent;
        }

        public override void ConferenceTerminated(NSDictionary data)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _parent.Cleanup();
            });
        }

        public override void ReadyToClose(NSDictionary data)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _parent.Cleanup();
            });
        }
    }
}
