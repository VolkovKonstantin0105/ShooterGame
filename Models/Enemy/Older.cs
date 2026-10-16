using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop.Input;

namespace SGame.Models.Enemy
{
    public class Older : Person
    {
        public Older(Texture2D[] textures, Vector2 point, GraphicsDevice graphicsDevice)
            : base(textures[0], point)
        {
            _speed = 0;
            _damage = 0;
            _tag = "older";
            _animationTextures = textures;
            _canMove = false;
        }

        public override void Move(WpfKeyboard keyboard)
        {
            // стоит на месте
        }

        public override void Shoot()
        {
            // не атакует
        }
    }
}