using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SGame.Models.FactoryModels
{
    public abstract class Drawble
    {
        private const int DrawSize = 32; 
        protected Texture2D[]? _animationTextures; // кадры текущей анимации
        protected int _currentFrame;               // номер текущего кадра
        protected float _animationTimer;           // сколько времени показывается кадр
        protected float _frameTime = 0.15f;        // через сколько секунд менять кадр
        protected float _alpha = 1f;               // прозрачность: 1 - виден, 0 - невидим
        protected virtual Texture2D? _texture { get; set; } // текущий кадр
        protected Vector2 _currentPoint;           // позиция на экране
        protected SpriteBatch? _spriteBatch;

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_texture == null)
                return;

            var destination = new Rectangle((int)_currentPoint.X, (int)_currentPoint.Y, DrawSize, DrawSize);
            spriteBatch.Draw(_texture, destination, Color.White * _alpha);
        }

        public void UpdateAnimation(GameTime gameTime)
        {
            if (_animationTextures is not { Length: > 0 })
                return;

            _animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_animationTimer < _frameTime)
                return;

            _animationTimer = 0f;
            _currentFrame = (_currentFrame + 1) % _animationTextures.Length;
            _texture = _animationTextures[_currentFrame];
        }
    }
}