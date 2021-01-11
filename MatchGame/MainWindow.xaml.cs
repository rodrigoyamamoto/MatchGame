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
        readonly DispatcherTimer _timer = new DispatcherTimer();
        int _tenthsOfSecondsElapsed;
        int _matchesFound;

        TextBlock _lastTextBlockClicked;
        bool _findingMatch = false;

        public MainWindow()
        {
            InitializeComponent();

            _timer.Interval = TimeSpan.FromSeconds(.1);
            _timer.Tick += Timer_Tick;
            SetupGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _tenthsOfSecondsElapsed++;
            TimeTextBlock.Text = (_tenthsOfSecondsElapsed /
                                  10F).ToString("0.0s");

            if (_matchesFound == 8)
            {
                _timer.Stop();
                TimeTextBlock.Text += " - Play again?";
            }
        }

        private void SetupGame()
        {
            List<string> animalEmoji = new List<string>()
            {
                "😺", "😺",
                "😻", "😻",
                "🙈", "🙈",
                "🙉", "🙉",
                "🐶", "🐶",
                "🐗", "🐗",
                "🐰", "🐰",
                "🐔", "🐔"
            };

            Random random = new Random();
            foreach (var textBlock in MainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "TimeTextBlock")
                {
                    var index = random.Next(animalEmoji.Count);
                    var nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                }

                textBlock.Visibility = Visibility.Visible;
            }
            _timer.Start();
            _tenthsOfSecondsElapsed = 0;
            _matchesFound = 0;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            if (_findingMatch == false)
            {
                if (textBlock != null)
                {
                    textBlock.Visibility = Visibility.Hidden;
                    _lastTextBlockClicked = textBlock;
                }

                _findingMatch = true;
            }
            else if (textBlock?.Text == _lastTextBlockClicked.Text)
            {
                _matchesFound++;

                if (textBlock != null)
                    textBlock.Visibility = Visibility.Hidden;

                _findingMatch = false;
            }
            else
            {
                _lastTextBlockClicked.Visibility = Visibility.Visible;
                _findingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_matchesFound == 8)
            {
                SetupGame();
            }
        }
    }
}
