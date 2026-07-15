using Microsoft.Xna.Framework.Graphics;
using SGame.Models;
using SGame.ViewModels;
using SharpDX.Direct2D1;
using SharpDX.Direct3D11;
using System.Drawing;
using System.Windows;
using Microsoft.Xna.Framework;
using MonoGame.Framework.WpfInterop.Input;

namespace SGame.GameLogic
{
    public sealed class LogicGame
    {
        private Player _currentPlayer;
        public LogicGame(Player player)
        {
            _currentPlayer = player;
        }
        //логика сохранения в базу данных объекта
        public void SaveInDataBase()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                try
                {
                    db.Players.Add(_currentPlayer);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

       
    }
}
