using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop.Input;
using System.Windows.Forms;
using System.ComponentModel.DataAnnotations.Schema;
using SGame.Models.GameObject;
using SGame.Models.FactoryModels;
using SGame.Models.BoundsModel;

namespace SGame.Models.Enemy
{
    public class Player : Person
    {
        public int ID { get; set; }
        protected override int _damage { get => 15; set { } }
        protected int _countKills { get; set; }
        [NotMapped]
        protected override Texture2D _texture { get; set; }
        private Bullet _currentBullet;
        private bool _canShoot = true;
        private FactoryPerson _factory;
        private Texture2D[] _attackTextures;
        private Texture2D[] _defaultTextures;
        private Texture2D[] _damageTexture;
        private bool _isAttacking;
        private List<Obstacle> _obstacles;
        public List<Obstacle> obstacles
        {
            get => _obstacles;
            set => _obstacles = value;
        }
        private bool _isDied = false;
        [NotMapped]
        public bool IsAttacking => _isAttacking;
        public int Score { get; set; }
        public bool IsPressedB { get; set; }
        public bool IsKeyFound { get; set; }
        public bool IsInLake { get; set; }
        public Player(Texture2D[] textures, Vector2 point, GraphicsDevice graphicsDevice, Texture2D[] attackTextures, Texture2D[] damageTexture) : base(textures[0], point)
        {
            _speed = 2;
            _tag = "player";
            _currentPoint = point;
            _texture = textures[0];
            _animationTextures = textures;
            _factory = new FactoryPerson(graphicsDevice);
            _attackTextures = attackTextures;
            _defaultTextures = textures;
            _damageTexture = damageTexture;
        }
        public override void Move(WpfKeyboard keyboard)
        {
            var state = keyboard.GetState();
            var moves = new Dictionary<Keys, Action>
            {
                {
                    Keys.W, () =>
                    {
                        Vector2 newPos = new Vector2(_currentPoint.X, _currentPoint.Y - _speed);
                        if (_currentPoint.Y > 0 && CanMove(newPos))
                            _currentPoint.Y -= _speed;
                    }
                },
                {
                    Keys.S, () =>
                    {
                        Vector2 newPos = new Vector2(_currentPoint.X, _currentPoint.Y + _speed);
                        if (_currentPoint.Y <= 350 && CanMove(newPos))
                            _currentPoint.Y += _speed;
                    }
                },
                {
                    Keys.A, () =>
                    {
                         Vector2 newPos = new Vector2(_currentPoint.X - _speed, _currentPoint.Y);
                        if (_currentPoint.X > 0 && CanMove(newPos))
                            _currentPoint.X -= _speed;
                    }
                },
                {
                    Keys.D, () =>
                    {
                         Vector2 newPos = new Vector2(_currentPoint.X + _speed, _currentPoint.Y);
                        if (_currentPoint.X <= 595 && CanMove(newPos))
                            _currentPoint.X += _speed;
                    }
                },
                { Keys.Space, Shoot},

                { Keys.B, () => IsPressedB = true }
            };


            foreach (var move in moves)
            {
                if (state.IsKeyDown(
                    (Microsoft.Xna.Framework.Input.Keys)move.Key))
                {
                    move.Value();
                }
            }
            UpdateLakeEffect();
        }
        private void UpdateLakeEffect()
        {
            if (IsInLake)
            {
                _alpha -= 0.02f;

                if (_alpha < 0)
                    _alpha = 0;
            }
            else
            {
                _alpha = 1;
            }
        }
        private bool CanMove(Vector2 newPosition)
        {
            Rectangle newBounds = new Rectangle((int)newPosition.X, (int)newPosition.Y, SPRITE_SQUARE_SIZE, SPRITE_SQUARE_SIZE);
            foreach (var obstacle in _obstacles)
            {
                if (obstacle.Intersects(newBounds))
                    return false;
            }
            return true;
        }
        public override void Shoot()
        {
            if (_countAmmo <= 0 ||
                _currentBullet != null ||
                !_canShoot ||
                _isAttacking)
                return;

            _currentBullet =
                _factory.GreateBullet(
                    new Vector2(
                        _currentPoint.X + 16,
                        _currentPoint.Y + 16
                    )
                );


            _countAmmo--;
            _canShoot = false;
            StartAttackAnimation();
        }

        public void UpdateBullets()
        {
            if (_currentBullet == null)
                return;

            _currentBullet.Move();

            if (_currentBullet.GetDistance() <= 0)
            {
                _currentBullet = null;
                _canShoot = true;
            }
        }

        private void StartAttackAnimation()
        {
            if (_attackTextures.Length == 0)
                return;
            _isAttacking = true;
            _currentFrame = 0;
            _animationTimer = 0;
            _texture = _attackTextures[0];
        }

        private void UpdateAnimation(GameTime gameTime, Texture2D[] textures)
        {
            if (!_isAttacking)
                return;
            _animationTimer +=
                (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_animationTimer < _frameTime)
                return;
            _animationTimer = 0;
            _currentFrame++;
            if (_currentFrame >= textures.Length)
            {
                _isAttacking = false;
                _currentFrame = 0;
                textures = _defaultTextures;
                _texture = _defaultTextures[0];
                return;
            }
            _texture = textures[_currentFrame];
        }

        public Bullet GetCurrentBullet() => _currentBullet;
        public void UpdateAttackAnimation(GameTime gameTime) => UpdateAnimation(gameTime, _attackTextures);
        public void UpdateDamageAnimation(GameTime gameTime) => UpdateAnimation(gameTime, _damageTexture);
        public void DrawBullets()
        {
            if (_currentBullet != null)
                _currentBullet.DrawBullet(_spriteBatch);
        }
        public void GetKills(int count) => _countKills += count;
        public void TakeDamage(int damage)
        {
            if (_health > 0)
                _health -= damage;


            if (_health <= 0)
            {
                _isDied = true;
            }
                
        }
        public bool IsDiedValue() => _isDied;
    }
}