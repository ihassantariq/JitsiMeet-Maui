using System;

namespace JitsiMeetDemo
{
    public interface IJitsiMeetService
    {
        void JoinMeeting(string roomName, string displayName, string email);
        void LeaveMeeting();
    }
}
