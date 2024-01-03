namespace Wordle;

public partial class SettingsPage : ContentPage
{
  
    public SettingsPage()
    {
       
        InitializeComponent();
       
    }

    private async void SaveSettingsBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}