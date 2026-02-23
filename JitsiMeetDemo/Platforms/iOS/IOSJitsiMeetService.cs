using System;
using Foundation;
using UIKit;
using JitsiMeet.iOS.Binding;
using Microsoft.Maui.ApplicationModel;
using JitsiMeetDemo;

namespace JitsiMeetDemo.Services
{
    public class IOSJitsiMeetService : IJitsiMeetService
    {
        private JitsiMeetView _jitsiMeetView;
        private UIViewController _viewController;

        public void JoinMeeting(string roomName, string displayName, string email)
        {
            var userInfo = new JitsiMeetUserInfo();
            if (!string.IsNullOrEmpty(displayName))
                userInfo.DisplayName = displayName;
            if (!string.IsNullOrEmpty(email))
                userInfo.Email = email;

            var options = JitsiMeetConferenceOptions.FromBuilder(builder =>
            {
                builder.ServerURL = new NSUrl("https://meet.jit.si");
                builder.Room = roomName;
                builder.UserInfo = userInfo;
                builder.SetFeatureFlag("chat.enabled", true);
                builder.SetFeatureFlag("invite.enabled", true);
                builder.SetFeatureFlag("prejoinpage.enabled", true);
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

            var currentController = Platform.GetCurrentUIViewController();
            currentController?.PresentViewController(_viewController, true, () =>
            {
                _jitsiMeetView.Join(options);
            });
        }

        public void LeaveMeeting()
        {
            _jitsiMeetView?.Leave();
            Cleanup();
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
}
