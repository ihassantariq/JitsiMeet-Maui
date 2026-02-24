namespace JitsiMeetDemo;

public partial class MainPage : ContentPage
{
    private readonly IJitsiMeetService? _jitsiMeetService;

    public MainPage(IJitsiMeetService? jitsiMeetService = null)
    {
        InitializeComponent();
        _jitsiMeetService = jitsiMeetService;
    }

    private async void OnJoinClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(RoomEntry.Text))
        {
            await DisplayAlert("Error", "Please enter a room name", "OK");
            return;
        }

        var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();
        if (cameraStatus != PermissionStatus.Granted)
        {
            cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
        }

        var micStatus = await Permissions.CheckStatusAsync<Permissions.Microphone>();
        if (micStatus != PermissionStatus.Granted)
        {
            micStatus = await Permissions.RequestAsync<Permissions.Microphone>();
        }

        if (cameraStatus != PermissionStatus.Granted || micStatus != PermissionStatus.Granted)
        {
            await DisplayAlert("Permissions Denied", "Camera and Microphone permissions are required to join the meeting.", "OK");
            return;
        }

        _jitsiMeetService?.JoinMeeting(RoomEntry.Text, NameEntry.Text, "");
    }
}
