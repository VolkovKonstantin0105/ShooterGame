using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop.Input;
using SharpDX.Direct2D1;

namespace SGame.Models.Enemy
{
    /// <summary>
    /// Скелет. Из Enemys он уже получил: IsSeePlayer, IsBite, poison, AttackDistance, Speed,
    /// Shoot() (укус), Patrol() и ChasePlayer(). Осталось написать только Move().
    /// </summary>
    public class Scelet : Enemys
    {
        private Player _player;
        
        public Scelet(Texture2D[] textures, Vector2 point, Player player) : base(textures, point)
        {
            _speed = 1;
            _tag = "scelet";
            _player = player;
            _animationTextures = textures;
        }

        public override void Move(WpfKeyboard keyboard)
        {
            if (IsSeePlayer)
                ChasePlayer(_player.GetPosition());
            else
                Patrol();
        }



        
    }
}