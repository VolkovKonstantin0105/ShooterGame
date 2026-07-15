using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;
using SGame.GameLogic;
using SGame.Models;
using System.Windows;
namespace FifaFootballGame.Models.GamePresentWindowFolder.GameFolder
{
    public sealed class Game : WpfGame
    {

        private IGraphicsDeviceService _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;
      

        private WpfKeyboard _keyboard;
        private WpfMouse _mouse;

        FactoryPerson _factory;
        private Player _player;

        //тестовый
        Employee _employee;

        private LogicGame _logic;

        public Game() {}

        protected override void Initialize()
        {
            _graphicsDeviceManager = new WpfGraphicsDeviceService(this);

            _keyboard = new WpfKeyboard(this);
            _mouse = new WpfMouse(this);
            _factory = new FactoryPerson(GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _player = _factory.GeneratePlayer();
            _player.SetSpriteBatch(_spriteBatch);
            _employee = _factory.GenerateEmployee();
            ExceptionDataBase();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (_spriteBatch != null)
            {
                _spriteBatch.Begin();
                _player.Draw(_spriteBatch);
                _player.DrawBullets();
                _employee.Draw(_spriteBatch);
                _spriteBatch.End();
            }


            base.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _player.Move(_keyboard);
            _player.UpdateBullets();



        }
        private void ExceptionDataBase()
        {
            if (_player != null)
            {
                try
                {
                    _logic = new LogicGame(_player);//логика игры
                    _logic.SaveInDataBase();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);

                }
            }
        }
    }
}