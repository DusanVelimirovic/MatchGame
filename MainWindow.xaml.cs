using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

/*
 * This can be a Version 1 in witch player from the begining see all card.
 * Version 2 can be a little more complex in a sence that player can't see all cards except only card witch click.
 * 
 * 
 * 
 */

namespace MatchGame
{
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Dispatcher class - timer
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;

            SetUpGame();
        }

        // Timer logic
        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.00");

            // Logic for stoping timer
            if(matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " - Play again?";

            }
        }

        // Create a List of animals emoji
        List<string> animalEmoji = new List<string>()
            {
                "🐙","🐙",
                "🐟","🐟",
                "🐘","🐘",
                "🐺","🐺",
                "🐫","🐫",
                "🐱‍🐉","🐱‍🐉",
                "🐯","🐯",
                "🦔","🦔",
                "🐼", "🐼",  // New pair
                "🦁", "🦁",  // New pair
                "🐍", "🐍",  // New pair
                "🐨", "🐨",  // New pair

            };

        // Function to Set up the Game
        private void SetUpGame()
        {
            // Create a new random number generator
            Random random = new Random();

            // Create a list to hold all available emoji pairs
            List<string> availableEmojiPairs = animalEmoji.ToList();

            // Create a list to hold the emoji pairs for the current game
            List<string> selectedEmojiPairs = new List<string>();

            // Randomly select 8 pairs of emojis from the available list
            for (int i = 0; i < 8; i++)
            {
                // Select a random emoji pair index
                int index = random.Next(availableEmojiPairs.Count / 2) * 2; // Ensure we select pairs
                                                                            // Add the selected emoji pair to the game list
                selectedEmojiPairs.Add(availableEmojiPairs[index]);
                selectedEmojiPairs.Add(availableEmojiPairs[index + 1]);
                // Remove the selected emoji pair from the available list to avoid duplicates
                availableEmojiPairs.RemoveAt(index);
                availableEmojiPairs.RemoveAt(index);
            }

            // Shuffle the selected emoji pairs
            selectedEmojiPairs = selectedEmojiPairs.OrderBy(x => random.Next()).ToList();

            // Assign the shuffled emoji pairs to the TextBlocks in the main grid
            int textBlockIndex = 0;
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    textBlock.Text = selectedEmojiPairs[textBlockIndex++];
                }
            }

            // Start the timer and reset game variables
            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }

        // Make images response to mouse click
        TextBlock lastTextBlockClicked;
        bool findingMatch = false;
        private void Text_Block_Mouse_down(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if(findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }


        // Event handler for timer
        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(matchesFound == 8)
            {
                SetUpGame(); // Reset game when all images have been found
            }

        }
    }
}
