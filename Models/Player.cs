using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop.Input;
using System.Windows.Forms;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGame.Models
{
    public class Player : Person
    {
        public int ID { get; set; } //id для базы данных
        protected override int _damage { get => 15; set { } }//сила удара определена как 15 единиц
        protected int _countKills { get; set; } = 0; //по умолчанию 0

        [NotMapped] //аттрибут, чтобы данное поле не сохранять в базу данных
        protected override Texture2D _texture { get; set; }
        private GameTime _gameTime;
        [NotMapped]
        private GraphicsDevice _graphicsDevice;
        [NotMapped]
       
        private List<Bullet> _bullets = new List<Bullet>(); //список пуль

        private Bullet _currentBullet = null;
        private bool _canShoot = true;
        private FactoryPerson _factory;
        public Player(Texture2D texture, Vector2 point, GraphicsDevice graphicsDevice) : base (texture, point)
        {
            _speed = 4;
            _tag = "player";
            _currentPoint = point; //определяем появление игрока пока что вначале координат
            _texture = texture;
            _factory = new FactoryPerson(graphicsDevice);
            
           
            
        }
        public Player() : base(null, Vector2.Zero) //конструктор для базы данных
        {
            _speed = 4;
            _tag = "player";
            _currentPoint = Vector2.Zero;
            _texture = null;
            
           
        }

        public override void Move(WpfKeyboard keyboard)
        {
            var state = keyboard.GetState(); //получаем состояние клавиатуры

            var moves = new Dictionary<Keys, Action>
            {
                { Keys.W, () => 
                    {
                        if(_currentPoint.Y > 0)//заменить на паременную потом 
                        _currentPoint.Y -= _speed;
                    }
                },
                { Keys.S, () =>
                    {
                        if(_currentPoint.Y <= 350)
                        _currentPoint.Y += _speed;
                    } 
                },
                { Keys.A, () =>
                    {
                        if(_currentPoint.X > 0)
                        _currentPoint.X -= _speed; 
                    }
                },
                { Keys.D, () =>
                    {
                        if(_currentPoint.X <= 595)
                        _currentPoint.X += _speed;
                    } 
                },
                
                { 
                    Keys.Space, () =>
                        Shoot() 
                },

            };

            //вызываем действия
            foreach (var move in moves)
                if (state.IsKeyDown((Microsoft.Xna.Framework.Input.Keys)move.Key))
                    move.Value();
        }

        public override void Shoot()
        {
            try
            {
                if (_countAmmo > 0 && _currentBullet == null && _canShoot)
                {
                    _currentBullet = _factory.GreateBullet(new Vector2(_currentPoint.X + 16, _currentPoint.Y + 16));
                    _countAmmo--;
                    _canShoot = false; 
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
             
        }
        public void UpdateBullets()
        {
            if (_currentBullet != null)
            {
                _currentBullet.Move(); 
                if (_currentBullet.GetDistance() <= 0)
                {
                    _currentBullet = null; 
                    _canShoot = true; 
                }
            }
        }

        public void DrawBullets()
        {
            if (_currentBullet != null)
            {
                _currentBullet.DrawBullet(_spriteBatch);
            }
        }
        

        public void GetKills(int count) => _countKills+= count; //увеличиваю количество убитых врагов 
    }
}
