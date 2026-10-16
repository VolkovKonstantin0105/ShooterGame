using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop.Input;
using SGame.GameLogic;
namespace SGame.Models.Enemy
{
    public class Vampir : Person
    {
        private Vector2 _vampirPoint;
        private Vector2[] _points;//массив точек по которм будет двигаться вампир
        private GraphicsDevice _graphicDevice;
        private bool _isSeePlayer = false;
        private bool _isBite = false; //переменная отслеживающая укус вампира
        private Poison _poison; //класс яда
        public Poison poison => _poison;
        public bool IsBite => _isBite; //укусил ли 
        public bool isBite 
        { 
            get => _isBite;
            set => _isBite = value;
        }

        //тест
        public int Speed
        {
            get => _speed;
            set => _speed = value;
        }

        private float _attackDistance = 100f;
        public float AttackDistance => _attackDistance;
        public bool IsSeePlayer
        {
            get => _isSeePlayer;
            set => _isSeePlayer = value;
        }

        //поля касающиеся отрисовки объекта
        private bool _isHiding;//видимость
        private float _hideTimer;
        private int _currentTarget = 0;


        //игрок
        Player _player;
        public Vampir(Texture2D[] textures, Vector2 point, GraphicsDevice graphicsDevice, Player player) : base(textures[0], point)
        {
            _speed = 2;
            _damage = 12; //самый высокий
            _tag = "vampir";
            _vampirPoint = point; //определяем появление игрока пока что вначале координат
            _texture = textures[0];
            _animationTextures = textures;
            _graphicDevice = graphicsDevice;

            _points = new Vector2[]
            {
                new Vector2(30, 250),//подправить
                new Vector2(0, 350), //подправить
                new Vector2(0, 348),//360 - продправить (коряво)
                new Vector2(80, 348),
                new Vector2(380, 348),
                new Vector2(520, 285),
                new Vector2(520, 285),
                new Vector2(520, 285),
                new Vector2(590, 315),
                new Vector2(590, 354)
            };

            _poison = new Poison();//эффект получения урона

            _player = player;
        }

        public override void Move(WpfKeyboard keyboard) //сделать как общий метод для объектов-противников через abstract class Movement
        {
            if (!_isSeePlayer)
            {
                if (_isHiding)
                {
                    _alpha -= 0.02f;
                    if (_alpha <= 0)
                    {
                        _vampirPoint = _points[0];
                        _currentPoint = _vampirPoint;
                        _currentTarget = 1;
                        _alpha = 1f;
                        _isHiding = false;
                    }
                }

                Vector2 target = _points[_currentTarget];
                Vector2 direction = target - _vampirPoint;

                if (direction.Length() < 2f)
                {
                    _vampirPoint = target;
                    if (_currentTarget == _points.Length - 1)//момент когда я дошел до конца самого
                    {
                        _isHiding = true;
                        _hideTimer = 1.5f;
                        return;
                    }
                    _currentTarget++;//тут я увеличиваю направление(переход к другим точкам)
                    return;
                }


                direction.Normalize();
                _vampirPoint += direction * _speed;
                _currentPoint = _vampirPoint;

            }
            if(_isSeePlayer)
            {
                //вижу игрока - начинаю преследовать
                Vector2 direction = Vector2.Normalize(_player.GetPosition() - _currentPoint);
                _currentPoint += direction * _speed;

                _vampirPoint = _currentPoint;//если разнрица в дистанции больше 100, то чтобы не телепортировался гандон

            }
        }

        public override void Shoot() //метод укуса игрока
        {
            if (!_isBite)
            {
                _isBite = true;
                _poison.Start();
            }  
        }

        //вынести потом метод
        public Rectangle RectangeEntity() =>
            new Rectangle(
                (int)_vampirPoint.X,
                (int)_vampirPoint.Y,
                32, 32
            );
    }
}
