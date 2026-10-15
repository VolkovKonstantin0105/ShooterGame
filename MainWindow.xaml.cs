using System;
using System.Windows;
using SGame.Models;

namespace SGame
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new GameWindow().Show();
                // Close(); // раскомментируй, если стартовое окно должно закрываться
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ApiButton_Click(object sender, RoutedEventArgs e)
        {
            // Заглушка: потом здесь запустим Blazor API
            MessageBox.Show("Здесь будет запуск API на Blazor");
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}