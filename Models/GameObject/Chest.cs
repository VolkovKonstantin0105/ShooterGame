using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SGame.Models.FactoryModels;
using System.ComponentModel.DataAnnotations.Schema;
namespace SGame.Models.GameObject
{
    public sealed class Chest : Drawble
    {
        private bool _isOpen = false;
        public bool IsOpen
        {
            get => _isOpen;
            set => _isOpen = value;
        }
        public Chest(Microsoft.Xna.Framework.Vector2 currentPoint, Texture2D[] textures)
        {
            _animationTextures = textures;
            _currentPoint = currentPoint;
            _texture = textures[0];
        }

        public Vector2 GetPosition() => _currentPoint;
    }
}
