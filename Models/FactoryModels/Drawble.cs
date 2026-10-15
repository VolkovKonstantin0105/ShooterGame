using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGame.Models.FactoryModels
{
    public abstract class Drawble
    {
        [NotMapped]//текстуры для анимации
        protected Texture2D[]? _animationTextures; //для хранения массива спрайтов
        protected int _currentFrame;//текущая картинка
        protected float _animationTimer;//время анимации
        protected float _frameTime = 0.15f;//время смены картинки
        protected float _alpha = 1f;
        [NotMapped]
        protected virtual Texture2D? _texture { get; set; }//текстура игрового персонажа
        [NotMapped]
        protected Vector2 _currentPoint; //текущая позиция игрока
        protected SpriteBatch? _spriteBatch;
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destination = new Rectangle((int)_currentPoint.X, (int)(_currentPoint.Y), 32, 32);
            spriteBatch.Draw(_texture, destination, Color.White * _alpha);
        }

        public void UpdateAnimation(GameTime gameTime)
        {
            _animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_animationTimer >= _frameTime)
            {
                _animationTimer = 0f;

                _currentFrame++;

                if (_currentFrame >= _animationTextures?.Length)
                    _currentFrame = 0;

                _texture = _animationTextures?[_currentFrame];
            }
        }
    }
}
