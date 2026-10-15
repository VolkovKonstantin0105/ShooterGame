using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;
using SGame;
using SGame.GameLogic;
using SGame.Models.Enemy;
using SGame.Models.FactoryModels;
using SGame.Models.GameObject;
using System.IO;
using System.Windows;
namespace FifaFootballGame.Models.GamePresentWindowFolder.GameFolder
{
    public static class TextureLoader
    {
        public static Texture2D Load(GraphicsDevice device, string path)
        {
            using var stream = File.OpenRead(path);
            return Texture2D.FromStream(device, stream);
        }
    }
    public sealed class Game : WpfGame
    {
        private bool _gameOverHandled;
        private IGraphicsDeviceService? _graphicsDeviceManager;
        private SpriteBatch? _spriteBatch;

        //элементы доя работы с обработкой нажатия мыши
        private WpfKeyboard _keyboard;
        private WpfMouse? _mouse;

        //игровые персонажи (игрок, голова, вампир, скелет) 
        private Player _player { get; set; }
        private Employee _employee { get; set; }
        private Vampir _vampir { get; set; }
        
        //лист монет
        List<Coins> _listCoins;

        //объекты игровой логики и графики
        private FactoryPerson _factory { get; set; }
        private LogicGame _logic;
        private Texture2D _background;
        private Chest _chest;

        private Key _key;

       
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
            try
            {
                _spriteBatch = new SpriteBatch(GraphicsDevice);
                _background = TextureLoader.Load(GraphicsDevice, "Content/Texture/background.png"); //загрузка фона
                
                _player = _factory.GeneratePlayer(); //создание персонажа
                _chest = _factory.GenerateChest();
                _logic = new LogicGame(_player, _spriteBatch, GraphicsDevice);
                _player.obstacles = _logic.obstacles; //передаем список объектов 
                _player.SetSpriteBatch(_spriteBatch);
                

                _employee = _factory.GenerateEmployee();//генерация синего врага 
                _vampir = _factory.CreateVampir(_spriteBatch, _player); //генерация вампира

                ExceptionDataBase(); //база данных
                _listCoins = _logic.GenerateCoins();//метод для генерации монет в рандомном месте

                _key = new Key();

                
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            if (_spriteBatch != null)
            {
                _spriteBatch.Begin();

                if (_background != null)
                {
                    _spriteBatch.Draw(_background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                }

                _player.Draw(_spriteBatch);
                _player.DrawBullets();
                _employee.Draw(_spriteBatch);
                _vampir.Draw(_spriteBatch);
                foreach (var coin in _listCoins)
                {
                    coin.Draw(_spriteBatch);
                }

                _chest.Draw(_spriteBatch);

                _spriteBatch.End();
            }


            base.Draw(gameTime);
        }
        protected override void Update(GameTime gameTime)
        {
            if (_player.IsDiedValue())
            {
                CloseWindow();
                return;
            }
            base.Update(gameTime);
            _player.Move(_keyboard);
            _logic.PlayerBoundsLake();
            _player.UpdateAttackAnimation(gameTime);
            if (!_player.IsAttacking)
            {
                _player.UpdateAnimation(gameTime);
            }
            _employee.UpdateAnimation(gameTime);//анимация
            _employee.Move(null);
            _player.UpdateBullets();
            _vampir.UpdateAnimation(gameTime); //анимация
            _vampir.Move(null); 
            foreach (var coin in _listCoins)
            {
                coin.UpdateAnimation(gameTime);
            }

            _logic.Update(gameTime, _listCoins);
            _logic.AttackVampir(_player, _vampir, gameTime);
            _logic.BulletDamage(_vampir);
            _logic.MouseClick(_mouse, _key);
            try
            {
                _logic.OpenChest(_chest, gameTime, _key);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }    
        }
        private void ExceptionDataBase()
        {
            if (_player != null)
            {
                try
                {
                    _logic.SaveInDataBase(); //метод сохранения статистки в бд
                } 
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);

                }
            }
        }
        private void CloseWindow()
        {
            
                if (!_gameOverHandled)
                {
                    _gameOverHandled = true;

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        var owner = Window.GetWindow(this);
                        new GameOverWindow("ВЫ ПОГИБЛИ", "Попробуй еще раз") { Owner = owner }.ShowDialog();
                        owner?.Close();
                    }));
                }
           
        }
    }
}