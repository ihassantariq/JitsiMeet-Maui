namespace JitsiMeet.Maui;

/// <summary>
/// Cross-platform service for interacting with Jitsi Meet conferences.
/// </summary>
public interface IJitsiMeetService
{
    /// <summary>
    /// Joins a Jitsi Meet conference with the specified options.
    /// </summary>
    /// <param name="options">The conference options.</param>
    Task JoinMeetingAsync(JitsiMeetOptions options);

    /// <summary>
    /// Leaves the current Jitsi Meet conference.
    /// </summary>
    Task LeaveMeetingAsync();
}
