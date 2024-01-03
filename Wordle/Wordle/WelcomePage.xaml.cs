namespace Wordle;

public partial class WelcomePage : ContentPage
{
	public WelcomePage()
	{
		InitializeComponent();
	}

    private async void PlayGame_Clicked(object sender, EventArgs e)
    {
        
        await Shell.Current.GoToAsync("//MainPage", true);
    }
}