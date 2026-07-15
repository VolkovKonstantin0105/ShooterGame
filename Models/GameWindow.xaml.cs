using FifaFootballGame.Models.GamePresentWindowFolder.GameFolder;
using System.Windows;
namespace SGame.Models
{
    public partial class GameWindow : Window
    {
        public GameWindow()
        {
            InitializeComponent();
            var game = new Game()
            {
               
            };

            GameHost.Children.Add(game);
        }
    }
}
