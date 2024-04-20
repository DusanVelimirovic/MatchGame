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

namespace MatchGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetUpGame();
        }
          
        private void SetUpGame()
        {
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
            };

            // Create a new random number generator
            Random random = new Random();


            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                int index = random.Next(animalEmoji.Count); // Pick a random number between 0 and number of the elements in the list
                string nextEmoji = animalEmoji[index]; // Retrieve emoji with given index from the emoji List
                textBlock.Text = nextEmoji; // Update textBlock text with random emoji
                animalEmoji.RemoveAt(index); // Remove randomly pick emoji
            }
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
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }
    }
}
