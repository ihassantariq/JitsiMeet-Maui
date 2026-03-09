namespace JitsiMeet.Maui;

/// <summary>
/// Configuration options for joining a Jitsi Meet conference.
/// </summary>
public sealed class JitsiMeetOptions
{
    /// <summary>
    /// The name of the room to join.
    /// </summary>
    public required string RoomName { get; init; }

    /// <summary>
    /// The Jitsi Meet server URL. Defaults to the public Jitsi Meet instance.
    /// </summary>
    public string ServerUrl { get; init; } = "https://meet.jit.si";

    /// <summary>
    /// The display name of the local participant.
    /// </summary>
    public string? DisplayName { get; init; }

    /// <summary>
    /// The email address of the local participant.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// The URL of the avatar of the local participant.
    /// </summary>
    public string? AvatarUrl { get; init; }

    /// <summary>
    /// A JWT token used for authentication.
    /// </summary>
    public string? Jwt { get; init; }

    /// <summary>
    /// Whether the audio should be muted on join. Defaults to false.
    /// </summary>
    public bool AudioMuted { get; init; } = false;

    /// <summary>
    /// Whether the video should be muted on join. Defaults to true.
    /// </summary>
    public bool VideoMuted { get; init; } = true;

    /// <summary>
    /// Feature flags to set on the conference.
    /// </summary>
    public IDictionary<string, object>? FeatureFlags { get; init; }

    /// <summary>
    /// Config overrides to set on the conference.
    /// </summary>
    public IDictionary<string, object>? ConfigOverrides { get; init; }
}
