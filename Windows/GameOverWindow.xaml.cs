using System.Windows;

namespace SGame
{
    public partial class GameOverWindow : Window
    {
        public GameOverWindow(string title, string message)
        {
            InitializeComponent();
            TitleText.Text = title;
            MessageText.Text = message;
        }

        private void Ok_Click(object sender, RoutedEventArgs e) => Close();
    }
}