using Android.App;
using Android.Content;
using Android.OS;
using Org.Jitsi.Meet.Sdk;
using System.Linq;
using Com.Facebook.React.Modules.Core;
using AndroidX.Core.App;
using AndroidX.AppCompat.App;
using AndroidX.LocalBroadcastManager.Content;

namespace JitsiMeet.Maui.Platforms.Android;

[Activity(
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
[IntentFilter(
    new[] { global::Android.Content.Intent.ActionView },
    Categories = new[] { global::Android.Content.Intent.CategoryDefault, global::Android.Content.Intent.CategoryBrowsable },
    DataScheme = "org.jitsi.meet")]
[IntentFilter(
    new[] { global::Android.Content.Intent.ActionView },
    Categories = new[] { global::Android.Content.Intent.CategoryDefault, global::Android.Content.Intent.CategoryBrowsable },
    DataScheme = "https",
    DataHost = "meet.jit.si")]
public class MauiJitsiMeetActivity : AppCompatActivity, Org.Jitsi.Meet.Sdk.IJitsiMeetActivityInterface
{
    private JitsiMeetView? _view;
    private JitsiBroadcastReceiver? _broadcastReceiver;

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

        JitsiMeetConferenceOptions options;

        if (intent?.Action == global::Android.Content.Intent.ActionView && intent.Data != null)
        {
            options = new JitsiMeetConferenceOptions.Builder()
                .SetRoom(intent.Data.ToString())
                .SetConfigOverride("disableDeepLinking", true)
                .Build();
        }
        else
        {
            string roomName = intent?.GetStringExtra("JitsiRoom") ?? "";
            string serverUrl = intent?.GetStringExtra("JitsiServerUrl") ?? "https://meet.jit.si";
            string displayName = intent?.GetStringExtra("JitsiName") ?? "";
            string email = intent?.GetStringExtra("JitsiEmail") ?? "";
            string avatarUrl = intent?.GetStringExtra("JitsiAvatarUrl") ?? "";
            string jwt = intent?.GetStringExtra("JitsiJwt") ?? "";
            bool audioMuted = intent?.GetBooleanExtra("JitsiAudioMuted", false) ?? false;
            bool videoMuted = intent?.GetBooleanExtra("JitsiVideoMuted", true) ?? true;

            var userInfo = new JitsiMeetUserInfo();
            if (!string.IsNullOrEmpty(displayName))
                userInfo.DisplayName = displayName;
            if (!string.IsNullOrEmpty(email))
                userInfo.Email = email;

            var builder = new JitsiMeetConferenceOptions.Builder()
                .SetServerURL(new Java.Net.URL(serverUrl))
                .SetRoom(roomName)
                .SetUserInfo(userInfo)
                .SetAudioMuted(audioMuted)
                .SetVideoMuted(videoMuted)
                .SetConfigOverride("disableDeepLinking", true);

            if (!string.IsNullOrEmpty(jwt))
                builder.SetToken(jwt);

            // Apply feature flags and config overrides from intent extras
            var extras = intent?.Extras;
            if (extras != null)
            {
                foreach (var key in extras.KeySet()!)
                {
                    if (key.StartsWith("JitsiFeatureFlag_bool_"))
                    {
                        var flagName = key.Substring("JitsiFeatureFlag_bool_".Length);
                        builder.SetFeatureFlag(flagName, extras.GetBoolean(key));
                    }
                    else if (key.StartsWith("JitsiFeatureFlag_int_"))
                    {
                        var flagName = key.Substring("JitsiFeatureFlag_int_".Length);
                        builder.SetFeatureFlag(flagName, extras.GetInt(key));
                    }
                    else if (key.StartsWith("JitsiFeatureFlag_str_"))
                    {
                        var flagName = key.Substring("JitsiFeatureFlag_str_".Length);
                        builder.SetFeatureFlag(flagName, extras.GetString(key));
                    }
                    else if (key.StartsWith("JitsiConfig_bool_"))
                    {
                        var configName = key.Substring("JitsiConfig_bool_".Length);
                        builder.SetConfigOverride(configName, extras.GetBoolean(key));
                    }
                    else if (key.StartsWith("JitsiConfig_int_"))
                    {
                        var configName = key.Substring("JitsiConfig_int_".Length);
                        builder.SetConfigOverride(configName, extras.GetInt(key));
                    }
                    else if (key.StartsWith("JitsiConfig_str_"))
                    {
                        var configName = key.Substring("JitsiConfig_str_".Length);
                        builder.SetConfigOverride(configName, extras.GetString(key));
                    }
                }
            }

            options = builder.Build();
        }

        _view?.Join(options);
        SetContentView(_view);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (_broadcastReceiver != null)
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

        if (intent?.Action == global::Android.Content.Intent.ActionView && intent.Data != null)
        {
            var newOptions = new JitsiMeetConferenceOptions.Builder()
                .SetRoom(intent.Data.ToString())
                .Build();
            _view?.Join(newOptions);
            return;
        }

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
