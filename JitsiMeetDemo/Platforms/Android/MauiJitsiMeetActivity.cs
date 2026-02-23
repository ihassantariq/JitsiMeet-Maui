using Android.App;
using Android.Content;
using Android.OS;
using Org.Jitsi.Meet.Sdk;
using System.Linq;
using Com.Facebook.React.Modules.Core;
using AndroidX.Core.App;
using AndroidX.AppCompat.App;

namespace JitsiMeetDemo.Platforms.Android
{
    [Activity(Theme = "@style/Maui.SplashTheme", Exported = true)]
    public class MauiJitsiMeetActivity : AppCompatActivity, Org.Jitsi.Meet.Sdk.IJitsiMeetActivityInterface
    {
        private JitsiMeetView _view;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

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
                .SetRoom(roomName)
                .SetUserInfo(userInfo)
                .SetFeatureFlag("chat.enabled", true)
                .SetFeatureFlag("invite.enabled", false)
                .Build();

            _view.Join(options);
            SetContentView(_view);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _view?.Dispose();
            JitsiMeetActivityDelegate.OnHostDestroy(this);
        }

        protected override void OnResume()
        {
            base.OnResume();
            JitsiMeetActivityDelegate.OnHostResume(this);
        }

        protected override void OnStop()
        {
            base.OnStop();
            JitsiMeetActivityDelegate.OnHostPause(this);
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
