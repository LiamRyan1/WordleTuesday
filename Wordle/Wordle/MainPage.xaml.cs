using Microsoft.Maui.Controls;
using System.Runtime.InteropServices;
using System.Text;

namespace Wordle;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
    private void CreateTheGrid()
    {
        for (int i = 0; i < 5; i++)
        {
            LetterCaptureGrid.AddRowDefinition(new RowDefinition());
        }

        for (int i = 0; i < 6; i++)
        {
            LetterCaptureGrid.AddColumnDefinition(new ColumnDefinition());
        }

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Frame styledFrame = new Frame
                {
                    BorderColor = Color.FromRgb(50, 50, 50),
                    CornerRadius = 10,
                    HasShadow = true,
                    Padding = new Thickness(10),
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
        WordofTheDay();
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
            // Get the clicked letter from the button
            string letter = button.Text;
            if (button.Text == "")
            {
                DisplayAlert("Button Clicked", $"You clicked button is empty", "OK");
            }
            // Update the content of the first empty frame in the letter grid
            else
            {
                addLetter(letter);
                validateWord();
            }
        }
    }
    private void addLetter(string letter)
    {
        foreach (View view in LetterCaptureGrid.Children)
        {
            if (view is Frame frame && frame.Content == null)
            {
                frame.Content = new Label
                {
                    Text = letter,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };
                break;
            }
        }
    }
    //download and read from json
    private string WordofTheDay()
    {
        return "donny";
    }

    private void validateWord()
    {

        for (int i = 0; i < LetterCaptureGrid.RowDefinitions.Count; i++)
        {

            StringBuilder GuessedWord = new StringBuilder();

            foreach (View view in LetterCaptureGrid.Children)
            {
                if (view is Frame frame && Grid.GetRow(frame) == i && frame.Content != null)
                {
                    if (frame.Content is Label label)
                    {
                        string letter = label.Text;
                        GuessedWord.Append(letter);
                    }
                }
            }
            // Compare the row content with the Word of the Day
            string playersword = GuessedWord.ToString();
            if (playersword.Equals(WordofTheDay(), StringComparison.OrdinalIgnoreCase))
            {

                // Player guessed correctly
                DisplayAlert("Congratulations!", "You guessed correctly!", "OK");
                break;
            }
            else
            {
                string WordOfTheDay = WordofTheDay();
               for(int j = 0; j < playersword.Length; j++)
                {
                    char guessedLetter = playersword[j];
                    char actualLetter = WordOfTheDay[j];
                    if (char.ToUpper(guessedLetter).Equals(char.ToUpper(actualLetter)))
                    {
                   
                    }
              
                }
            }
        }
    }
}
 

