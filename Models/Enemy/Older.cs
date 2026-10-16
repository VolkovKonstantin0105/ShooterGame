using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop.Input;
namespace SGame.Models.Enemy
{
    //класс старца, который просто стоит на месте и разговаривает с персонажем
    public class Older : Person
    {
        private GraphicsDevice _graphicDevice;
        private Vector2 _olderPoint;
        public Older(Texture2D[] textures, Vector2 point, GraphicsDevice graphicsDevice) : base(textures[0], point)
        {
            _speed = 1;
            _damage = 0; 
            _tag = "older";
            _olderPoint = point; 
            _texture = textures[0];
            _animationTextures = textures;
            _graphicDevice = graphicsDevice;
        }

        public override void Move(WpfKeyboard keyboard)
        {
            throw new NotImplementedException();
        }

        public override void Shoot()
        {
            throw new NotImplementedException();
        }
    }
}
