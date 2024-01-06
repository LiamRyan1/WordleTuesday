using Microsoft.Maui.Controls;
using System.Text;

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
                    BackgroundColor = Color.FromRgb(255, 255, 255),
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
            BackgroundColor = Color.FromRgb(0, 0, 0),
            BorderColor = Color.FromRgb(255, 255,255),
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
            { "Z","X","C","V","B","N","M","delete","submit","exit"}
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
            else if(button.Text == "delete")
            {
                deleteLastLetter();
                count--;
            }
            else if(button.Text == "submit")
            {

            }
            else if(button.Text == "exit")
            {

                sendToWelcomePage();
                 
                
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
   private void deleteLastLetter()
   {
       // Find the last frame with a letter
       Frame lastFrame = null;

       foreach (Frame frame in LetterCaptureGrid.Children.OfType<Frame>().Where(f => f.Content is Label))
       {
           lastFrame = frame;
       }

       if (lastFrame != null && lastFrame.Content is Label label)
       {
          
           label.Text = string.Empty;

        
           lastFrame.Content = null;

          
           lastFrame.Focus();
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

    //get the random word from the list 
    public bool newWord = true;
    public string word;
    private string WordofTheDay()
    {
        if (newWord == true)
        {
            word = list.GenerateRandomWord();
            newWord = false;
        }
        return word;
    }
    //check if guessed word is correct
    private async void validateWord()
    {
        string wordOfTheDay = WordofTheDay().ToUpper(); //case insensitive comparison
        int LastRowIndex = LetterCaptureGrid.RowDefinitions.Count - 1;

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
            if (list.WordExists(playerWord))
            {

                //Compare the row content with the Word of the Day
                if (playerWord.Equals(wordOfTheDay, StringComparison.OrdinalIgnoreCase))
                {
                    for (int j = 0; j < playerWord.Length; j++)
                    {
                        char guessedLetter = playerWord[j];
                        //player guessed correctly
                        LetterCaptureGrid.Children
                                  .OfType<Frame>()
                                  .First(frame => Grid.GetRow(frame) == i && frame.Content is Label label && label.Text == guessedLetter.ToString())
                                  .BackgroundColor = Color.FromRgb(34, 139, 34);
                    }
                    await DisplayAlert("Congratulations!", "You guessed correctly!", "OK");
                    bool restartView = await DisplayAlert("Question?", "Would you like to play again or View your stats", "Stats", "Restart");
                    if (restartView)
                    {
                        //stats();
                    }
                    else
                    {
                        ResetGame();
                    }
                }
                //change backgorund colors of frame depending on level of correctness
                else
                {
                    //keeps track of how many times the letter occurs
                    Dictionary<char, List<Frame>> correctOccurrences = new Dictionary<char, List<Frame>>();
                    Dictionary<char, List<Frame>> incorrectOccurrences = new Dictionary<char, List<Frame>>();

                    for (int j = 0; j < playerWord.Length; j++)
                    {
                        char guessedLetter = playerWord[j];
                        char actualLetter = wordOfTheDay[j];

                        //initialize dictionaries if not already present
                        correctOccurrences.TryAdd(guessedLetter, new List<Frame>());
                        incorrectOccurrences.TryAdd(guessedLetter, new List<Frame>());

                        //check if the guessed letter is the same as the actual letter in the correct position
                        if (guessedLetter == actualLetter)
                        {
                            var frame = LetterCaptureGrid.Children
                                .OfType<Frame>()
                                .First(f => Grid.GetRow(f) == i && Grid.GetColumn(f) == j);

                            //clear the incorrect occurrences for this letter
                            foreach (var previousIncorrectFrame in incorrectOccurrences[guessedLetter])
                            {
                                previousIncorrectFrame.BackgroundColor = Color.FromRgb(100, 100, 100); // Highlight gray
                            }
                            incorrectOccurrences[guessedLetter].Clear();

                            //highlight the current letter as green
                            frame.BackgroundColor = Color.FromRgb(34, 139, 34); // Highlight green

                            //add the frame to correct occurrences
                            correctOccurrences[guessedLetter].Add(frame);
                        }
                        //check if the guessed letter is correct but in the wrong position
                        else if (wordOfTheDay.Contains(guessedLetter))
                        {
                            var frames = LetterCaptureGrid.Children
                                .OfType<Frame>()
                                .Where(f => Grid.GetRow(f) == i && f.Content is Label label && label.Text == guessedLetter.ToString())
                                .ToList();

                            if (frames.Count > 1 || !correctOccurrences[guessedLetter].Any())
                            {
                                foreach (var frame in frames)
                                {
                                    //exclude frames already marked as correct (green)
                                    if (!correctOccurrences[guessedLetter].Contains(frame))
                                    {
                                        //highlight the letter as yellow if not in incorrect occurrences
                                        if (!incorrectOccurrences[guessedLetter].Contains(frame))
                                        {
                                            frame.BackgroundColor = Color.FromRgb(204, 204, 0); //highlight yellow

                                            //add the frame to incorrect occurrences
                                            incorrectOccurrences[guessedLetter].Add(frame);
                                        }
                                        else
                                        {
                                            frame.BackgroundColor = Color.FromRgb(100, 100, 100); //highlight gray
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //if there's only one occurrence and it's correct, highlight it as gray
                                frames.First().BackgroundColor = Color.FromRgb(100, 100, 100);
                            }
                        }
                        else
                        {
                            //Guessed letter is not contained within the word of the day
                            var frame = LetterCaptureGrid.Children
                                .OfType<Frame>()
                                .First(f => Grid.GetRow(f) == i && Grid.GetColumn(f) == j);               
                            //highlight gray
                            frame.BackgroundColor = Color.FromRgb(100, 100, 100);
                        }
                    }

                    //reset the game      
                    if (i == LastRowIndex && playerWord.Length > 0 && !playerWord.Equals(wordOfTheDay, StringComparison.OrdinalIgnoreCase))
                    {
                        await DisplayAlert("Oh No!", $"You Ran out of Guesses! the correct Word was : {wordOfTheDay} ", "OK");
                        bool restartView = await DisplayAlert("Question?", "Would you like to play again or View your stats", "Stats", "Restart");
                        if (restartView)
                        {
                            //stats();
                        }
                        else
                        {
                            ResetGame();
                        }
                    }            
                }
            }
            else
            {
                foreach (View view in LetterCaptureGrid.Children.OfType<Frame>() .Where(frame => Grid.GetRow(frame) == i && frame.Content != null))
                {
                    if (view is Frame frame)
                    {
                        if (frame.Content is Label label)
                        {
                            label.Text = string.Empty;
                        }
                        frame.Content = null;
                    }
                }
                var firstFrameInRow = LetterCaptureGrid.Children
                    .OfType<Frame>()
                    .FirstOrDefault(frame => Grid.GetRow(frame) == i);

                if (firstFrameInRow != null)
                {
                    firstFrameInRow.Focus();
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
    public bool gridColor = false;
    private void OnStylingChanged(object sender, EventArgs e)
    {
        theme = false;
        UpdateButtonStyling();
        UpdateFrameStyling();
    }
    private void OnStylingChanged2(object sender, EventArgs e)
    {

        theme = true;
        UpdateButtonStyling();
        UpdateFrameStyling();

    }
    private void UpdateButtonStyling()
    {

        foreach (View view in Keys.Children)
        {
            if (view is Button button)
            {
                if (theme)
                {
                    button.BorderColor = Color.FromRgb(255, 255, 255);
                    button.BackgroundColor = Color.FromRgb(0, 0, 0);
                    button.TextColor = Color.FromRgb(255, 255, 255);
                }
                else
                {
                    button.BackgroundColor = Color.FromRgb(0, 0, 0);
                    button.TextColor = Color.FromRgb(255, 255, 255);
                    button.BorderColor = Color.FromRgb(0, 0, 0);
                }
            }
        }
    }

    private void UpdateFrameStyling()
    {
        foreach (View view in LetterCaptureGrid.Children)
        {
            if (view is Frame frame)
            {
                if (theme)
                {
                    frame.BorderColor = Color.FromRgb(255, 255, 255);
                    if (frame.BackgroundColor.Equals(Color.FromRgb(255, 255, 255)))
                    {
                        frame.BackgroundColor = Color.FromRgb(0, 0, 0);
                    }
                    if (frame.Content is Label label)
                    {
                        label.TextColor = Color.FromRgb(255, 255, 255);
                    }
                }
                else
                {
                    frame.BorderColor = Color.FromRgb(0, 0, 0);
                    if (frame.BackgroundColor.Equals(Color.FromRgb(0, 0, 0)))
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
    }
    //go to help page
    private async void Help_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HelpPage());
    }
    private async void sendToWelcomePage()
    {
        await Shell.Current.GoToAsync("//WelcomePage", true);
    }
    //reset the gane
    private void ResetGame()
    {
        //clear the letter grid
        foreach (View view in LetterCaptureGrid.Children)
        {
            if (view is Frame frame)
            {
                if (frame.Content is Label label)
                {
                    label.Text = null;
                }
                frame.Content = null;
                if (theme)
                {
                    frame.BackgroundColor = Color.FromRgb(0, 0, 0);
                }
                else
                {
                    frame.BackgroundColor = Color.FromRgb(255, 255, 255);
                }
            }
        }

        //reset the count
        count = 0;

        //generate new word
        newWord = true;
        WordofTheDay();
        //clear on screen keyboard
        Keys.Children.Clear();
        Keys.ColumnDefinitions.Clear();
        Keys.RowDefinitions.Clear();
        Keys.Clear();
        //enable begin button
        StartWordle.SetValue(Button.IsEnabledProperty, true);
    }

}


