using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SGame.Models.BoundsModel;
using SGame.Models.Enemy;
using SGame.Models.FactoryModels;
using SGame.Models.GameObject;
using SGame.ViewModels;
using System.Windows;
using MonoGame.Framework.WpfInterop.Input;
namespace SGame.GameLogic
{
    public sealed class LogicGame
    {
        private bool _wasMousePressed = false; //была ли нажата кнопка 
        public const int SPEED_VAMPIR_BEFORE_DAMAGE = 1;
        public const int MIN_DISTANCE = 48;
        private SGame.ViewModels.PlayerEntity _playerEntity; 
        
        private Player _currentPlayer;
        private SpriteBatch _spriteBatch;
        private FactoryPerson _factoryPerson;
        private GraphicsDevice _graphicsDevice;
        private GameTime _gameTime;
        private List<Obstacle> _obstacles; //лист объектов, через которые нельзя пройти 
        public List<Obstacle> obstacles => _obstacles;
        private WaterModel _leftLake;
        private WaterModel _rightLake;

        private bool _playerInLake = false;


        //для тестовых MessageBox
        private bool _shown = true;
        public LogicGame(Player player, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            _currentPlayer = player;
            _spriteBatch = spriteBatch;
            _graphicsDevice = graphicsDevice;
            _factoryPerson = new FactoryPerson(_graphicsDevice);
            _playerEntity = new SGame.ViewModels.PlayerEntity();

            _obstacles = new()
            {
                new Obstacle(230, 208, 75, 78), //дерево
                //камень левый верхний угол
                new Obstacle(48,106,39,16),
                new Obstacle(29,123,65,12),
                //камень левый нижний угол
                new Obstacle(0,361,30,16),
                
                new Obstacle(110, 290,44,60),
                //камень праввый нижний угол
                new Obstacle(460,252,35,22),
                new Obstacle(434,276,66,22),

                //сундук 
                new Obstacle(185, 0, 32, 32),

                new Obstacle(40, 0, 120, 60),

                new Obstacle(140, 40, 83, 85)
            };

            _rightLake = new WaterModel(32, 322, 50, 50); //изменить
            _leftLake = new WaterModel(570, 230, 60, 60);

            
        }

        //метод, когда игрок пропадает в воде
        public void PlayerBoundsLake()
        {
            Vector2 position = _currentPlayer.GetPosition();

            Rectangle playerRect = new Rectangle(
                (int)position.X,
                (int)position.Y,
                32,
                32
            );

            bool isInLake =
                _rightLake.Intersects(playerRect) ||
                _leftLake.Intersects(playerRect);

            _currentPlayer.IsInLake = isInLake;
        }
        //логика сохранения в базу данных объекта
        public void SaveInDataBase()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                try
                {
                    db.Players.Add(_playerEntity);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public List<Coins> GenerateCoins()
        {
            List<Coins> coinsList = new List<Coins>();
            for (int i = 0; i < 6; i++)
            {
                Coins coins = _factoryPerson.GenerateCoins(_spriteBatch);
                coinsList.Add(coins);
            }
            return coinsList;
        }

        public bool PlayerCollideWithCoin(Player player, Coins coin)
        {
            Rectangle playerRect = new Rectangle(
                (int)player.GetPosition().X,
                (int)player.GetPosition().Y,
                32, 32
            );

            Rectangle coinRect = new Rectangle(
                (int)coin.GetPosition().X,
                (int)coin.GetPosition().Y,
                32, 32
            );

            return playerRect.Intersects(coinRect);
        }

        public void Update(GameTime gameTime, List<Coins> coinsList)
        {
            for (int i = 0; i < coinsList.Count; i++)
            {
                if (PlayerCollideWithCoin(_currentPlayer, coinsList[i]))
                {
                    coinsList.RemoveAt(i);
                    _currentPlayer.Score++;
                    _playerEntity.Score = _currentPlayer.Score;
                    break;
                }
            }
        }

        public void AttackVampir(Player player, Vampir vampir, GameTime gameTime)
        {
            Vector2 pointPlayer = player.GetPosition();
            Vector2 pointVampir = vampir.GetPosition();
            float distance = (float)Math.Sqrt((pointPlayer.X - pointVampir.X) * (pointPlayer.X - pointVampir.X) + (pointPlayer.Y - pointVampir.Y) * (pointPlayer.Y - pointVampir.Y));
            if (distance <= vampir.AttackDistance)
            { 
                vampir.IsSeePlayer = true;
                if (distance <= MIN_DISTANCE - 12)
                    if (!vampir.IsBite)
                        vampir.Shoot();
                
                if (vampir.poison.IsActive)
                    vampir.poison.Update(gameTime, player, vampir.GetDamage());

                if (vampir.IsBite && !vampir.poison.IsActive)
                    vampir.isBite = false;
            }
            if (distance > vampir.AttackDistance)
                vampir.IsSeePlayer = false;
        }

        public bool CanMove(Vector2 newPosition) //вспомогательный метод чтобы определять могу ли проходить сквозь препятствия (вынести в GameLogic)
        {
            Rectangle newBounds = new Rectangle((int)newPosition.X, (int)newPosition.Y, 32, 32);

            foreach (var obstacle in _obstacles)
            {
                if (obstacle.Intersects(newBounds))
                    return false;
            }
            return true;
        }

        public void OpenChest(Chest chest, GameTime gameTime, Key key)
        {
            if (chest != null && _currentPlayer != null)
            {
                var pointChest = chest.GetPosition();
                var _currentPoint = _currentPlayer.GetPosition();
                float distance = (float)Math.Sqrt((_currentPoint.X - pointChest.X) * (_currentPoint.X - pointChest.X) + (_currentPoint.Y - pointChest.Y) * (_currentPoint.Y - pointChest.Y));
                if (distance < MIN_DISTANCE)//вынести в константу!!!
                {
                    if (_currentPlayer.IsPressedB) //&& (key.IsFound == true)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            chest.UpdateAnimation(gameTime);
                        }
                        chest.IsOpen = true;
                        _currentPlayer.IsPressedB = false;
                    }
                }
            }
        }

        public void BulletDamage(Vampir vampir)
        {
            Bullet bullet = _currentPlayer.GetCurrentBullet();
            if (bullet != null)
            {
                if (bullet.RectangeBullet().Intersects(vampir.RectangeEntity()))
                {
                    bullet.GetTextureBullet()?.Dispose();
                    vampir.Speed = SPEED_VAMPIR_BEFORE_DAMAGE;
                }
            } 
        }

        public void MouseClick(WpfMouse mouse, Key key)
        {
            var mouseState = mouse.GetState();
            bool isPressed = (int)mouseState.LeftButton == 1;

            if (isPressed && !_wasMousePressed)
            {
                if ((mouseState.X <= 300) && (mouseState.X >= 250) && (mouseState.Y <= 300) && (mouseState.Y >= 250))
                {
                    MessageBox.Show("Ключ найден!!!");
                    key.IsFound = true;
                }

            }
            _wasMousePressed = isPressed;
        }
        
    }
}
