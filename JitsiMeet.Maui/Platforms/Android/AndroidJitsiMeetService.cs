using Android.Content;
using Org.Jitsi.Meet.Sdk;
using Microsoft.Maui.ApplicationModel;

namespace JitsiMeet.Maui.Platforms.Android;

public class AndroidJitsiMeetService : IJitsiMeetService
{
    public Task JoinMeetingAsync(JitsiMeetOptions options)
    {
        var activity = Platform.CurrentActivity;
        if (activity == null) return Task.CompletedTask;

        var serverUrl = new Java.Net.URL(options.ServerUrl);

        if (Org.Jitsi.Meet.Sdk.JitsiMeet.DefaultConferenceOptions == null)
        {
            var defaultOptions = new JitsiMeetConferenceOptions.Builder()
                .SetServerURL(serverUrl)
                .SetFeatureFlag("welcomepage.enabled", false)
                .Build();

            Org.Jitsi.Meet.Sdk.JitsiMeet.DefaultConferenceOptions = defaultOptions;
        }

        var intent = new Intent(activity, typeof(MauiJitsiMeetActivity));
        intent.PutExtra("JitsiRoom", options.RoomName);
        intent.PutExtra("JitsiServerUrl", options.ServerUrl);
        intent.PutExtra("JitsiName", options.DisplayName ?? "");
        intent.PutExtra("JitsiEmail", options.Email ?? "");
        intent.PutExtra("JitsiAvatarUrl", options.AvatarUrl ?? "");
        intent.PutExtra("JitsiJwt", options.Jwt ?? "");
        intent.PutExtra("JitsiAudioMuted", options.AudioMuted);
        intent.PutExtra("JitsiVideoMuted", options.VideoMuted);

        if (options.FeatureFlags != null)
        {
            foreach (var flag in options.FeatureFlags)
            {
                intent.PutExtra($"JitsiFeatureFlag_{flag.Key}", flag.Value?.ToString() ?? "");
            }
        }

        if (options.ConfigOverrides != null)
        {
            foreach (var config in options.ConfigOverrides)
            {
                intent.PutExtra($"JitsiConfig_{config.Key}", config.Value?.ToString() ?? "");
            }
        }

        activity.StartActivity(intent);
        return Task.CompletedTask;
    }

    public Task LeaveMeetingAsync()
    {
        var activity = Platform.CurrentActivity;
        if (activity is MauiJitsiMeetActivity jitsiActivity)
        {
            jitsiActivity.Finish();
        }
        return Task.CompletedTask;
    }
}
