using System;
using Android.Content;
using Org.Jitsi.Meet.Sdk;
using Microsoft.Maui.ApplicationModel;
using JitsiMeetDemo;
using JitsiMeetDemo.Platforms.Android;

namespace JitsiMeetDemo.Services
{
    public class AndroidJitsiMeetService : IJitsiMeetService
    {
        public void JoinMeeting(string roomName, string displayName, string email)
        {
            var activity = Platform.CurrentActivity;
            if (activity == null) return;

            var serverUrl = new Java.Net.URL("https://meet.jit.si");

            // Initialize global configuration if not already done
            if (Org.Jitsi.Meet.Sdk.JitsiMeet.DefaultConferenceOptions == null)
            {
                var defaultOptions = new JitsiMeetConferenceOptions.Builder()
                    .SetServerURL(serverUrl)
                    .SetFeatureFlag("welcomepage.enabled", false)
                    .Build();

                Org.Jitsi.Meet.Sdk.JitsiMeet.DefaultConferenceOptions = defaultOptions;
            }

            var intent = new Intent(activity, typeof(MauiJitsiMeetActivity));
            intent.PutExtra("JitsiRoom", roomName);
            intent.PutExtra("JitsiName", displayName);
            intent.PutExtra("JitsiEmail", email);
            activity.StartActivity(intent);
        }

        public void LeaveMeeting()
        {
            var activity = Platform.CurrentActivity;
            if (activity != null && activity is MauiJitsiMeetActivity jitsiActivity)
            {
                jitsiActivity.Finish();
            }
        }
    }
}
