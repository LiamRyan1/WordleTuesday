
using Microsoft.Maui.Controls;
using System.Runtime.InteropServices;
using System.Text;
using System.Net;
namespace Wordle;

public partial class MainPage : ContentPage
{

    private Settings set;
    ListWords list;
    public MainPage()
    {
        InitializeComponent();
        CreateTheGrid();
        list = new ListWords();
        set = new Settings();
        set.StylingChanged += OnStylingChanged;
        set.StylingChanged2 += OnStylingChanged2;
        InitializeAsync();
    }
    private async void InitializeAsync()
    {
        await list.getWordList();
        
    }
//Create the grid for the letters to be captured in
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
    }
    //Start game button - creates an on screen keyboard and  generates the word of the day
    private void StartWordle_Clicked(object sender, EventArgs e)
    {
        CreateKeyboard();
        string word = list.GenerateRandomWord();
        WordofTheDay();
    }
    //on screen keyboard
    private void CreateKeyboard()
    {
        for (int i = 0; i < 3; i++)
        {
            Keys.AddRowDefinition(new RowDefinition());
        }
        for (int j = 0; j < 10; j++)
        {
            Keys.AddColumnDefinition(new ColumnDefinition());
        }
        //add buttons to thethe keyboard grid
          for (int i = 0; i < 3; i++)
          {
             for (int j = 0; j < 10; j++)
             {
                Button button = createbuttons(i, j);
                 Grid.SetRow(button, i);
                     Grid.SetColumn(button, j);
                     Keys.Add(button);
                 }
          }
        StartWordle.SetValue(Button.IsEnabledProperty, false);
    }
    //create the keyboard buttons
    private Button createbuttons(int row, int column)
    {
        Button buttons = new Button
        {
            Text = GetButtonText(row, column),
            BackgroundColor = Color.FromRgb(50, 50, 50),
        };
        buttons.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Button)) * 0.8; 

        buttons.Clicked += OnButtonClicked;
        return buttons;
    }
    
    //text for the buttons
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
    //counter
    public static int count = 0;
    private async void OnButtonClicked(object sender, EventArgs e)
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
                //keep count of how many letters have been added to the grid
                count++;
                if (count == 5)
                {
                    //vlaidates the word in the row once number of letters equals 5
                    validateWord();
                    count = 0;
                }
                
            }
        }
    }
    private void addLetter(string letter)
    {
        //goes through all Views in the lettercapture grid
        foreach (View view in LetterCaptureGrid.Children)
        {
            //if the view is a frame and the frame is empty
            if (view is Frame frame && frame.Content == null)
            {
                //adds a label with the letter from the button
                frame.Content = new Label
                {
                    Text = letter,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                  
                };
                //check if the app is in dark or light mode and change text of label
                if (theme == true && frame.Content is Label label)
                {
                    label.TextColor = Color.FromRgb(255, 255, 255);
                }
         
                
               break;
            }
        }
    }

    
    private string WordofTheDay()
    {
        return "donny";
    }

    private void validateWord()
    {
        string wordOfTheDay = WordofTheDay().ToUpper(); //case insensitive comparison

        //build a string each row made up of the guessed letters
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

            string playerWord = guessedWord.ToString().ToUpper(); 

            //Compare the row content with the Word of the Day
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
            //change backgorund colors of frame depending on level of correctness
            else
            {
                for (int j = 0; j < playerWord.Length; j++)
                {
                    char guessedLetter = playerWord[j];
                    char actualLetter = wordOfTheDay[j];

                    //Correct letter in the correct position
                    if (guessedLetter == actualLetter)
                    {
                        LetterCaptureGrid.Children
                            .OfType<Frame>()
                            .First(frame => Grid.GetRow(frame) == i && Grid.GetColumn(frame) == j)
                            .BackgroundColor = Color.FromRgb(34, 139, 34);
                    }
                    //Correct letter in the wrong position
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
    //send to settings page
    private async void Settings_Clicked(object sender, EventArgs e)
    {
      
        await Navigation.PushAsync(new SettingsPage(set));
    }




    //Light and darkMode
    public bool theme = false;
    private void OnStylingChanged(object sender, EventArgs e)
    {
      
        UpdateButtonStyling();
        UpdateFrameStyling();
    }
    private void OnStylingChanged2(object sender, EventArgs e)
    {

        UpdateButtonStyling2();
        UpdateFrameStyling2();
    }
    private void UpdateButtonStyling()
    {
        theme = false;
        foreach (View view in Keys.Children)
        {
            if (view is Button button)
            {
                button.BackgroundColor = Color.FromRgb(255, 255, 255);
                button.TextColor = Color.FromRgb(0, 0, 0);
                button.BorderColor = Color.FromRgb(0, 0, 0);
            }
        }
    }
    private void UpdateButtonStyling2()
    {
        theme = true;
        foreach (View view in Keys.Children)
        {
            if (view is Button button)
            {
                button.BorderColor = Color.FromRgb(255, 255, 255);
                button.BackgroundColor = Color.FromRgb(0, 0, 0);
                button.TextColor = Color.FromRgb(255, 255, 255);
            }
        }
    }
    private void UpdateFrameStyling()
    {
       
        foreach (View view in LetterCaptureGrid.Children)
        {
            if (view is Frame frame)
            {
                frame.BorderColor = Color.FromRgb(0, 0, 0);
                if (frame.BackgroundColor != Color.FromRgb(34, 139, 34) || frame.BackgroundColor != Color.FromRgb(255, 255, 153))
                {
                    frame.BackgroundColor = Color.FromRgb(255, 255, 255);
                
                }
               if (frame.Content is Label label)
                {
                    label.TextColor = Color.FromRgb(0, 0, 0);
                }
            }
        }

    }
    private void UpdateFrameStyling2()
    {
        
        foreach (View view in LetterCaptureGrid.Children)
        {
            if (view is Frame frame)
            {
                
                frame.BorderColor = Color.FromRgb(255, 255, 255);
                if (frame.BackgroundColor != Color.FromRgb(34, 139, 34) || frame.BackgroundColor != Color.FromRgb(255, 255, 153))
                {
                    frame.BackgroundColor = Color.FromRgb(0, 0, 0);
                }

                if (frame.Content is Label label)
                {
                    label.TextColor = Color.FromRgb(255, 255, 255);
                }
            }
        }
    }
}


