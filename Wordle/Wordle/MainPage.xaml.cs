using Microsoft.Maui.Controls;
using System.Runtime.InteropServices;

namespace Wordle;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void CreateTheGrid()
    {

        for (int j = 0; j < 5; j++)
        {
            LetterCaptureGrid.AddRowDefinition(new RowDefinition());


        }
        for (int j = 0; j < 6; j++)
        {
            LetterCaptureGrid.AddColumnDefinition(new ColumnDefinition());
        }
        for (int j = 0; j < 5; j++)
        {
            for (int i = 0; i < 6; i++)
            {
                Frame styledFrame = new Frame
                {
                    BorderColor = Color.FromRgb(50, 50, 50), //Set the background color
                    CornerRadius = 10, //set rounded corners
                    HasShadow = true, //add a shadow effect
                    Padding = new Thickness(10), //Add padding to the frame
                };
                LetterCaptureGrid.Add(styledFrame, j, i);
            }

        }
        StartWordle.SetValue(Button.IsEnabledProperty, false);
    }
    private void StartWordle_Clicked(object sender, EventArgs e)
    {
        CreateTheGrid();
        CreateKeyboard();
    }
    private void CreateKeyboard()
    {



        for (int i = 0; i < 3; i++)
        {
            Keys.AddRowDefinition(new RowDefinition { Height = GridLength.Star });

            for (int j = 0; j < 10; j++)
            {
                Keys.AddColumnDefinition(new ColumnDefinition { Width = GridLength.Star });
                Frame styledFrame1 = new Frame
                {
                    CornerRadius = 10,
                    BorderColor = Color.FromRgb(0, 0, 0),
                    HasShadow = true,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                };
                Keys.Add(styledFrame1, i, j);
            }
          
        }
        //creat the buttons
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Button button = createbuttons(i, j);
                Grid.SetRow(button, i);
                Grid.SetColumn(button, j);
                Keys.Children.Add(button);
            }
        }
    }
    private Button createbuttons(int row, int column)
    {
        Button buttons = new Button
        {
            Text = GetButtonText(row, column),
            BackgroundColor = Color.FromRgb(50, 50, 50),
        };
        buttons.Clicked += OnButtonClicked;
        return buttons;
    }
    private string GetButtonText(int row, int column)
    {
        string[,] Letter = new string[,]
        {
            {"Q","W","E","R","T","Y","U","I","O","P"},
            {"A","S","D","F","G","H","J","K","L",""},
            { "Z","X","C","V","B","N","M","","",""}
        };

        return Letter[row, column];
    }
    private void OnButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            // Handle button click action

            if (button.Text == "")
            {
                DisplayAlert("Button Clicked", $"You clicked button is empty", "OK");
            }
            else
            {
                DisplayAlert("Button Clicked", $"You clicked button {button.Text}", "OK");
            }
        }
    }
}
 
