namespace Wordle;

public partial class SettingsPage : ContentPage
{
    Settings set;
    public SettingsPage(Settings s)
    {
        set = s;
        InitializeComponent();
        BindingContext = set;
    }

    private async void SaveSettingsBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void LightMode_Clicked(object sender, EventArgs e)
    {
        set.lightmode();
    }

    private void Dark_Clicked(object sender, EventArgs e)
    {
        set.darkmode();
    }

 
}