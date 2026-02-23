using System;
using ObjCRuntime;

namespace JitsiMeet.iOS.Binding
{
    [Native]
    public enum WebRTCLoggingSeverity : long
    {
        Verbose,
        Info,
        Warning,
        Error,
        None
    }

    [Native]
    public enum RecordingMode : long
    {
        File,
        Stream
    }
}
