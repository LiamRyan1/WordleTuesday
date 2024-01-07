namespace Wordle;

public partial class WelcomePage : ContentPage
{
    public WelcomePage()
    {
        InitializeComponent();
    }
    private async void PlayGame_Clicked(object sender, EventArgs e)
    {

        if (UsernameEntry.Text == null)
        {
           await DisplayAlert("Error","You have not entered a username","ok");
        }
        else
        {
            await Shell.Current.GoToAsync("//MainPage", true);
            Username.username = UsernameEntry.Text;
        }
    }
    public static class Username
    {
        public static string username { get; set; }
    }
}