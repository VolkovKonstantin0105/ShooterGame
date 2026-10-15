using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SGame.Models.FactoryModels;

namespace SGame.Models.GameObject
{
    public sealed class Coins : Drawble
    {
        private string _tag { get; init; } =  "coin"; //тег
        public string Tag => _tag;
        public Coins(Texture2D[] textures, Vector2 vector, SpriteBatch spriteBatch)
        {
            _currentPoint = vector;        
            _animationTextures = textures;
            _texture = textures[0];
            _spriteBatch = spriteBatch;
            _currentFrame = 0;
            _animationTimer = 0f;
            _frameTime = 0.15f;
        }

        public Vector2 GetPosition() => _currentPoint; //получение позиции монеты 

    }
}