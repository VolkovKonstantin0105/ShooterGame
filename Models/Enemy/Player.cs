using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop.Input;
using SGame.Models.BoundsModel;
using SGame.Models.FactoryModels;
using SGame.Models.GameObject;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace SGame.Models.Enemy
{
    public class Player : Person
    {
        private const int MoveSpeed = 2;
        private const int AttackDamage = 15;
        private const int BulletSpawnOffset = 16;

        private const int MinX = 0;
        private const int MinY = 0;
        private const int MaxX = 596;
        private const int MaxY = 352;

        private enum Facing
        {
            Left,
            Right
        }

        private readonly Texture2D[] _idleRightTextures;
        private readonly Texture2D[] _idleLeftTextures;
        private readonly Texture2D[] _attackTextures;
        private readonly Texture2D[] _damageTextures;

        private readonly FactoryPerson _factory;

        private Bullet? _currentBullet;
        private Facing _facing = Facing.Right;
        private bool _canShoot = true;
        private bool _isAttacking;
        private bool _isDied;

        public int ID { get; set; }
        public int Score { get; set; }
        public bool IsPressedB { get; set; }
        public bool IsKeyFound { get; set; }
        public bool IsInLake { get; set; }
        public bool IsAttacking => _isAttacking;
        public List<Obstacle> obstacles { get; set; } = new();

        protected int _countKills { get; set; }

        protected override int _damage { get => AttackDamage; set { } }

        private Texture2D[] IdleTextures => _facing == Facing.Left ? _idleLeftTextures : _idleRightTextures;

        public Player(
            Texture2D[] texturesR,
            Vector2 point,
            GraphicsDevice graphicsDevice,
            Texture2D[] attackTextures,
            Texture2D[] damageTexture,
            Texture2D[] texturesL)
            : base(texturesR[0], point)
        {
            _speed = MoveSpeed;
            _tag = "player";
            _animationTextures = texturesR;

            _idleRightTextures = texturesR;
            _idleLeftTextures = texturesL;
            _attackTextures = attackTextures;
            _damageTextures = damageTexture;

            _factory = new FactoryPerson(graphicsDevice);
        }

        public override void Move(WpfKeyboard keyboard)
        {
            var state = keyboard.GetState();

            if (state.IsKeyDown(Keys.W))
                TryMove(new Vector2(0, -_speed));

            if (state.IsKeyDown(Keys.S))
                TryMove(new Vector2(0, _speed));

            if (state.IsKeyDown(Keys.A))
            {
                SetFacing(Facing.Left);
                TryMove(new Vector2(-_speed, 0));
            }

            if (state.IsKeyDown(Keys.D))
            {
                SetFacing(Facing.Right);
                TryMove(new Vector2(_speed, 0));
            }

            if (state.IsKeyDown(Keys.Space))
                Shoot();

            if (state.IsKeyDown(Keys.B))
                IsPressedB = true;

            UpdateLakeEffect();
        }

        private void TryMove(Vector2 offset)
        {
            Vector2 newPosition = _currentPoint + offset;

            if (IsInsideField(newPosition) && CanMove(newPosition))
                _currentPoint = newPosition;
        }

        private static bool IsInsideField(Vector2 position) =>
            position.X >= MinX && position.X <= MaxX &&
            position.Y >= MinY && position.Y <= MaxY;

        private bool CanMove(Vector2 newPosition)
        {
            var newBounds = new Rectangle((int)newPosition.X, (int)newPosition.Y, SPRITE_SQUARE_SIZE, SPRITE_SQUARE_SIZE);

            foreach (var obstacle in obstacles)
            {
                if (obstacle.Intersects(newBounds))
                    return false;
            }
            return true;
        }

        private void SetFacing(Facing facing)
        {
            if (_facing == facing)
                return;

            _facing = facing;
            _animationTextures = IdleTextures;

            if (_isAttacking)
                return;

            _currentFrame = 0;
            _texture = IdleTextures[0];
        }

        private void UpdateLakeEffect()
        {
            if (IsInLake)
                _alpha = Math.Max(0, _alpha - 0.02f);
            else
                _alpha = 1;
        }

        public override void Shoot()
        {
            if (_countAmmo <= 0 || _currentBullet != null || !_canShoot || _isAttacking)
                return;

            float dirX = _facing == Facing.Right ? 1f : -1f;

            var spawnPoint = new Vector2(
                _currentPoint.X + BulletSpawnOffset * dirX,
                _currentPoint.Y + BulletSpawnOffset
            );

            _currentBullet = _factory.GreateBullet(spawnPoint);
            _currentBullet.Direction = new Vector2(dirX, 0);

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

        public void DrawBullets()
        {
            if (_currentBullet == null || _spriteBatch == null)
                return;

            _currentBullet.DrawBullet(_spriteBatch);
        }

        public Bullet? GetCurrentBullet() => _currentBullet;

        public void UpdateAttackAnimation(GameTime gameTime) => PlayOneShotAnimation(gameTime, _attackTextures);
        public void UpdateDamageAnimation(GameTime gameTime) => PlayOneShotAnimation(gameTime, _damageTextures);

        private void StartAttackAnimation()
        {
            if (_attackTextures.Length == 0)
                return;

            _isAttacking = true;
            _currentFrame = 0;
            _animationTimer = 0;
            _texture = _attackTextures[0];
        }

        private void PlayOneShotAnimation(GameTime gameTime, Texture2D[] frames)
        {
            if (!_isAttacking)
                return;

            _animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_animationTimer < _frameTime)
                return;

            _animationTimer = 0;
            _currentFrame++;

            if (_currentFrame >= frames.Length)
            {
                FinishOneShotAnimation();
                return;
            }

            _texture = frames[_currentFrame];
        }

        private void FinishOneShotAnimation()
        {
            _isAttacking = false;
            _currentFrame = 0;
            _texture = IdleTextures[0];
        }

        public void GetKills(int count) => _countKills += count;

        public void TakeDamage(int damage)
        {
            if (_isDied)
                return;

            _health -= damage;

            if (_health <= 0)
                _isDied = true;
        }

        public bool IsDiedValue() => _isDied;
    }
}