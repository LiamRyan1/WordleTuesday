namespace Wordle;

public partial class HelpPage : ContentPage
{
	public HelpPage()
	{
		InitializeComponent();
	}
    private async void OnLinkTapped(object sender, EventArgs e)
    {
        await Launcher.OpenAsync(new Uri("https://www.nytimes.com/games/wordle/index.html"));
    }
}