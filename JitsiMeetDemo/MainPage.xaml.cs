namespace JitsiMeetDemo;

public partial class MainPage : ContentPage
{
    private readonly IJitsiMeetService? _jitsiMeetService;

    public MainPage(IJitsiMeetService? jitsiMeetService = null)
    {
        InitializeComponent();
        _jitsiMeetService = jitsiMeetService;
    }

    private void OnJoinClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(RoomEntry.Text))
        {
            DisplayAlert("Error", "Please enter a room name", "OK");
            return;
        }

        _jitsiMeetService?.JoinMeeting(RoomEntry.Text, NameEntry.Text, "");
    }
}
