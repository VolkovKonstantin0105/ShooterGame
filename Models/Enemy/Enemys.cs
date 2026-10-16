using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SGame.GameLogic;

namespace SGame.Models.Enemy
{
    public abstract class Enemys : Person
    {
        private const float ArriveDistance = 2f; // на каком расстоянии точка считается достигнутой
        private const float FadeSpeed = 0.02f;   // скорость исчезновения в конце маршрута
        protected Vector2[] _patrolPath = Array.Empty<Vector2>(); 
        private int _currentTarget;                              
        private bool _isHiding;                                   
        public bool IsSeePlayer { get; set; }
        public bool IsBite { get; set; }                          
        public float AttackDistance { get; protected set; } = 100f;
        public Poison poison { get; } = new Poison();

        public int Speed
        {
            get => _speed;
            set => _speed = value;
        }

        // Старое имя, оставлено, чтобы проект собирался. Замени на IsBite там, где подсветит компилятор.
        [Obsolete("Используй IsBite")]
        public bool isBite
        {
            get => IsBite;
            set => IsBite = value;
        }

        protected Enemys(Texture2D[] textures, Vector2 point) : base(textures[0], point)
        {
            _animationTextures = textures;
        }
        public override void Shoot()
        {
            if (IsBite)
                return;

            IsBite = true;
            poison.Start();
        }

        public Rectangle RectangeEntity() =>
            new Rectangle((int)_currentPoint.X, (int)_currentPoint.Y, SPRITE_SQUARE_SIZE, SPRITE_SQUARE_SIZE);

        protected void Patrol()
        {
            if (_patrolPath.Length == 0)
                return;

            if (_isHiding)
                FadeOut();

            Vector2 target = _patrolPath[_currentTarget];
            Vector2 toTarget = target - _currentPoint;

            if (toTarget.Length() < ArriveDistance)
            {
                _currentPoint = target;

                if (_currentTarget == _patrolPath.Length - 1)
                {
                    _isHiding = true; // дошли до конца маршрута
                    return;
                }

                _currentTarget++;
                return;
            }

            toTarget.Normalize();
            _currentPoint += toTarget * _speed;
        }
        protected void ChasePlayer(Vector2 playerPosition)
        {
            Vector2 toPlayer = playerPosition - _currentPoint;
            if (toPlayer == Vector2.Zero)
                return; 

            toPlayer.Normalize();
            _currentPoint += toPlayer * _speed;
        }

        private void FadeOut()
        {
            _alpha -= FadeSpeed;
            if (_alpha > 0)
                return;
            _currentPoint = _patrolPath[0];
            _currentTarget = _patrolPath.Length > 1 ? 1 : 0;
            _alpha = 1f;
            _isHiding = false;
        }
    }
}