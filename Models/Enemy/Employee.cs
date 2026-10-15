using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop.Input;
namespace SGame.Models.Enemy
{
    public class Employee : Person
    {
        private new SpriteBatch _spriteBatch;
        private GraphicsDevice _graphicDevice;

        protected override int _damage { get => 10; set { } }
        protected override Texture2D? _texture { get; set; }
        private Vector2 _employeePoint;  
        private Vector2[] _points;
        private int _currentTarget = 0;
        private bool _isHiding;
        private float _hideTimer;
        public Employee(Texture2D[] textures, Vector2 point, GraphicsDevice graphicsDevice) : base(textures[0], point)
        {
            _speed = 1;
            _tag = "blue_fireball";
            _employeePoint = point; //спавн вначале коордирнат 
            _texture = textures[0];
            _animationTextures = textures;
            _graphicDevice = graphicsDevice;
            _points = new Vector2[] 
            {
                new Vector2(0,0),
                new Vector2(0, 78),    // A
                new Vector2(100, 78),  // B
                new Vector2(100,164),  // C
                new Vector2(275,164),  // D
                new Vector2(380,110),  // E
                new Vector2(520,180),  // F
                new Vector2(570,50),   // G       
            };
            
        }
        public override void Move(WpfKeyboard keyboard)
        {
            RepresentationEntity();
            MovingAroundTheMap();
        }
        public override void Shoot()
        {
            //логику позже добавлена будет   
        }

        protected void RepresentationEntity()
        {
            if (_isHiding)
            {
                _alpha -= 0.02f;

                if (_alpha <= 0)
                {
                    _employeePoint = _points[0];
                    _currentPoint = _employeePoint;
                    _currentTarget = 1;
                    _alpha = 1f;
                    _isHiding = false;
                }
                return;
            }
        }

        private void MovingAroundTheMap()
        {
            Vector2 target = _points[_currentTarget];
            Vector2 direction = target - _employeePoint;
            if (direction.Length() < 2f)
            {
                _employeePoint = target;
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
            _employeePoint += direction * _speed;
            _currentPoint = _employeePoint;
        }
    }
}
