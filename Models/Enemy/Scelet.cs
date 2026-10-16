using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop.Input;

namespace SGame.Models.Enemy
{
    /// <summary>
    /// Скелет. Из Enemys он уже получил: IsSeePlayer, IsBite, poison, AttackDistance, Speed,
    /// Shoot() (укус), Patrol() и ChasePlayer(). Осталось написать только Move().
    /// </summary>
    public class Scelet : Enemys
    {
        public Scelet(Texture2D[] textures, Vector2 point) : base(textures, point)
        {
            _speed = 1;
            _tag = "scelet";
        }

        public override void Move(WpfKeyboard keyboard)
        {
            // TODO: логика скелета (можно взять Patrol() и ChasePlayer(позиция игрока))
            throw new NotImplementedException();
        }
    }
}