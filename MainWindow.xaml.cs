using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace MatchGame
{
    public partial class MainWindow : Window
    {
        // DispatcherTimer to track time
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed; // Track elapsed time in tenths of a second
        int matchesFound; // Track the number of matches found
        float bestTime = float.MaxValue; // Initialize best time as a very large value
        bool isFirstGame = true; // Flag to track if it's the first game

        public MainWindow()
        {
            InitializeComponent();

            // Set up timer properties
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;

            // Start the game
            SetUpGame();
        }

        // Event handler for timer tick
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update elapsed time and display
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.00");

            // Check if all matches are found
            if (matchesFound == 8)
            {
                timer.Stop(); // Stop the timer
                // Check if it's the player's first game
                if (!isFirstGame)
                {
                    // Display message only for subsequent games
                    if (tenthsOfSecondsElapsed / 10F < bestTime)
                    {
                        bestTime = tenthsOfSecondsElapsed / 10F; // Update best time
                        MessageBox.Show($"Congratulations! You beat your best time: {bestTime} seconds.");
                    }
                    else if (tenthsOfSecondsElapsed / 10F == bestTime)
                    {
                        MessageBox.Show($"Hmm, seems like your decision speed is the same as your best time: {bestTime} seconds.");
                    }
                    else
                    {
                        MessageBox.Show($"Come on, you can do much better! Your best time: {bestTime} seconds.");
                    }
                }
                // Update flag for subsequent games
                isFirstGame = false;
                timeTextBlock.Text += " - Play again?";
            }
        }

        // List of animal emojis for the game
        List<string> animalEmoji = new List<string>()
        {
            "🐙","🐙", "🐟","🐟", "🐘","🐘", "🐺","🐺", "🐫","🐫", "🐱‍🐉","🐱‍🐉",
            "🐯","🐯", "🦔","🦔", "🐼", "🐼", "🦁", "🦁", "🐍", "🐍", "🐨", "🐨"
        };

        // Function to set up the game board
        private void SetUpGame()
        {
            Random random = new Random();
            List<string> availableEmojiPairs = animalEmoji.ToList();
            List<string> currentEmojiPairs = new List<string>();

            // Randomly select pairs of emojis for the game
            for (int i = 0; i < 8; i++)
            {
                int index = random.Next(availableEmojiPairs.Count / 2) * 2;
                currentEmojiPairs.Add(availableEmojiPairs[index]);
                currentEmojiPairs.Add(availableEmojiPairs[index + 1]);
                availableEmojiPairs.RemoveAt(index);
                availableEmojiPairs.RemoveAt(index);
            }

            // Shuffle the selected emoji pairs
            currentEmojiPairs = currentEmojiPairs.OrderBy(x => random.Next()).ToList();

            // Assign shuffled emoji pairs to the game board
            int textBlockIndex = 0;
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    textBlock.Text = currentEmojiPairs[textBlockIndex++];
                }
            }

            // Start the timer and reset game variables
            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }

        // Event handler for clicking on a text block (representing a card)
        TextBlock lastTextBlockClicked;
        bool findingMatch = false;
        private void Text_Block_Mouse_down(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            // Check if another card is already being flipped
            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            // Check if the clicked card matches the previously clicked card
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            // If no match, show the previously clicked card again
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        // Event handler for clicking on the time text block
        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Check if all matches are found, and prompt to play again
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}