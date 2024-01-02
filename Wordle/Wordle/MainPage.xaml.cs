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
        for (int i = 0; i < 6; i++)
        {
            LetterCaptureGrid.AddRowDefinition(new RowDefinition());
        }

        for (int i = 0; i < 5; i++)
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
            Keys.AddRowDefinition(new RowDefinition());

            for (int j = 0; j < 10; j++)
            {
                Keys.AddColumnDefinition(new ColumnDefinition());
                Frame styledFrame1 = new Frame
                {
                    CornerRadius = 10,
                    BorderColor = Color.FromRgb(0, 0, 0),
                    HasShadow = true,
                    Padding= new Thickness(10),
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
            Padding = new Thickness(10),
            HorizontalOptions = LayoutOptions.FillAndExpand,
        };
        buttons.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Button)) * 0.8; // Adjust the multiplier as needed

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
    public static int count = 0;
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
                count++;
                if (count == 5)
                {
                    validateWord();
                    count = 0;
                }
                
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
        string wordOfTheDay = WordofTheDay().ToUpper(); // Convert Word of the Day to uppercase for case-insensitive comparison

        for (int i = 0; i < LetterCaptureGrid.RowDefinitions.Count; i++)
        {
            StringBuilder guessedWord = new StringBuilder();

            foreach (View view in LetterCaptureGrid.Children)
            {
                if (view is Frame frame && Grid.GetRow(frame) == i && frame.Content != null)
                {
                    if (frame.Content is Label label)
                    {
                        string letter = label.Text;
                        guessedWord.Append(letter);
                    }
                }
            }

            string playerWord = guessedWord.ToString().ToUpper(); // Convert player's word to uppercase for case-insensitive comparison

            // Compare the row content with the Word of the Day
            if (playerWord.Equals(wordOfTheDay, StringComparison.OrdinalIgnoreCase))
            {
                for (int j = 0; j < playerWord.Length; j++)
                {
                    char guessedLetter = playerWord[j];
                    // Player guessed correctly
                    LetterCaptureGrid.Children
                              .OfType<Frame>()
                              .First(frame => Grid.GetRow(frame) == i && frame.Content is Label label && label.Text == guessedLetter.ToString())
                              .BackgroundColor = Color.FromRgb(34, 139, 34);
                }
                DisplayAlert("Congratulations!", "You guessed correctly!", "OK");
               
            }
            else
            {
                for (int j = 0; j < playerWord.Length; j++)
                {
                    char guessedLetter = playerWord[j];
                    char actualLetter = wordOfTheDay[j];

                    // Correct letter in the correct position: change the grid cell of the guessed letters background to green
                    if (guessedLetter == actualLetter)
                    {
                        LetterCaptureGrid.Children
                            .OfType<Frame>()
                            .First(frame => Grid.GetRow(frame) == i && Grid.GetColumn(frame) == j)
                            .BackgroundColor = Color.FromRgb(34, 139, 34);
                    }
                    // Correct letter in the wrong position: change the grid cell of the guessed letters background to yellow
                    else if (wordOfTheDay.Contains(guessedLetter))
                    {
                        LetterCaptureGrid.Children
                            .OfType<Frame>()
                            .First(frame => Grid.GetRow(frame) == i && frame.Content is Label label && label.Text == guessedLetter.ToString())
                            .BackgroundColor = Color.FromRgb(255,255,153);
                    }
                }
            }
        }
    }

    private void Settings_Clicked_1(object sender, EventArgs e)
    {
        DisplayAlert("Congratulations!", "You entered the settings!", "OK");
    }

    
}


