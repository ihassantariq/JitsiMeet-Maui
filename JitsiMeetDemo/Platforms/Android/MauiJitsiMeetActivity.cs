using Android.App;
using Android.Content;
using Android.OS;
using Org.Jitsi.Meet.Sdk;
using System.Linq;
using Com.Facebook.React.Modules.Core;
using AndroidX.Core.App;
using AndroidX.AppCompat.App;
using AndroidX.LocalBroadcastManager.Content;

namespace JitsiMeetDemo.Platforms.Android
{
    [Activity(
        Theme = "@style/Maui.SplashTheme", 
        Exported = true, 
        LaunchMode = global::Android.Content.PM.LaunchMode.SingleTask, 
        ConfigurationChanges = global::Android.Content.PM.ConfigChanges.Keyboard | 
            global::Android.Content.PM.ConfigChanges.KeyboardHidden | 
            global::Android.Content.PM.ConfigChanges.Mnc | 
            global::Android.Content.PM.ConfigChanges.Mcc | 
            global::Android.Content.PM.ConfigChanges.Navigation | 
            global::Android.Content.PM.ConfigChanges.Orientation | 
            global::Android.Content.PM.ConfigChanges.ScreenLayout | 
            global::Android.Content.PM.ConfigChanges.ScreenSize | 
            global::Android.Content.PM.ConfigChanges.SmallestScreenSize | 
            global::Android.Content.PM.ConfigChanges.Touchscreen | 
            global::Android.Content.PM.ConfigChanges.UiMode,
        SupportsPictureInPicture = true)]
    public class MauiJitsiMeetActivity : AppCompatActivity, Org.Jitsi.Meet.Sdk.IJitsiMeetActivityInterface
    {
        private JitsiMeetView _view;

        private JitsiBroadcastReceiver _broadcastReceiver;

        private class JitsiBroadcastReceiver : global::Android.Content.BroadcastReceiver
        {
            private readonly MauiJitsiMeetActivity _activity;

            public JitsiBroadcastReceiver(MauiJitsiMeetActivity activity)
            {
                _activity = activity;
            }

            public override void OnReceive(Context? context, Intent? intent)
            {
                var action = intent?.Action;
                if (action == "org.jitsi.meet.CONFERENCE_TERMINATED" || action == "org.jitsi.meet.READY_TO_CLOSE")
                {
                    _activity.Finish();
                }
            }
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _broadcastReceiver = new JitsiBroadcastReceiver(this);
            var intentFilter = new IntentFilter();
            intentFilter.AddAction("org.jitsi.meet.CONFERENCE_TERMINATED");
            intentFilter.AddAction("org.jitsi.meet.READY_TO_CLOSE");
            LocalBroadcastManager.GetInstance(this).RegisterReceiver(_broadcastReceiver, intentFilter);

            _view = new JitsiMeetView(this);
            global::Android.Content.Intent intent = Intent;

            string roomName = intent?.GetStringExtra("JitsiRoom") ?? "";
            string displayName = intent?.GetStringExtra("JitsiName") ?? "";
            string email = intent?.GetStringExtra("JitsiEmail") ?? "";

            var userInfo = new JitsiMeetUserInfo();
            if (!string.IsNullOrEmpty(displayName))
                userInfo.DisplayName = displayName;
            if (!string.IsNullOrEmpty(email))
                userInfo.Email = email;

            var options = new JitsiMeetConferenceOptions.Builder()
                .SetServerURL(new Java.Net.URL("https://meet.jit.si"))
                .SetRoom(roomName)
                .SetUserInfo(userInfo)
                .SetFeatureFlag("chat.enabled", true)
                .SetFeatureFlag("invite.enabled", true)
                .SetFeatureFlag("prejoinpage.enabled", true)
                .SetFeatureFlag("welcomepage.enabled", false)
                .SetConfigOverride("disableDeepLinking", "true")
                .Build();

            _view.Join(options);
            SetContentView(_view);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            LocalBroadcastManager.GetInstance(this).UnregisterReceiver(_broadcastReceiver);
            _view?.Dispose();
            JitsiMeetActivityDelegate.OnHostDestroy(this);
        }

        protected override void OnResume()
        {
            base.OnResume();
            JitsiMeetActivityDelegate.OnHostResume(this);
        }

        protected override void OnPause()
        {
            base.OnPause();
            JitsiMeetActivityDelegate.OnHostPause(this);
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            JitsiMeetActivityDelegate.OnActivityResult(this, requestCode, (int)resultCode, data);
        }

        protected override void OnNewIntent(Intent? intent)
        {
            base.OnNewIntent(intent);
            JitsiMeetActivityDelegate.OnNewIntent(intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, global::Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            JitsiMeetActivityDelegate.OnRequestPermissionsResult(requestCode, permissions, grantResults.Select(g => (int)g).ToArray());
        }

        public override void OnBackPressed()
        {
            JitsiMeetActivityDelegate.OnBackPressed();
            base.OnBackPressed();
        }

        public int CheckPermission(string permission, int pid, int uid)
        {
            return (int)global::Android.Content.PM.Permission.Granted;
        }

        public int CheckSelfPermission(string permission)
        {
            return (int)global::Android.Content.PM.Permission.Granted;
        }

        public void RequestPermissions(string[] permissions, int requestCode, Com.Facebook.React.Modules.Core.IPermissionListener listener)
        {
            ActivityCompat.RequestPermissions(this, permissions, requestCode);
        }

        public new bool ShouldShowRequestPermissionRationale(string permission)
        {
            return false;
        }

        void ActivityCompat.IOnRequestPermissionsResultCallback.OnRequestPermissionsResult(int requestCode, string[] permissions, global::Android.Content.PM.Permission[] grantResults)
        {
        }
    }
}
