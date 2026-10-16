using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop.Input;

namespace SGame.Models.Enemy
{
    /// <summary>
    /// Вампир: ходит по маршруту, а заметив игрока, преследует его.
    /// Всё общее (яд, укус, погоня, маршрут) лежит в Enemys.
    /// </summary>
    public class Vampir : Enemys
    {
        // TODO: подправить точки маршрута под карту
        private static readonly Vector2[] Route =
        {
            new Vector2(30, 250),
            new Vector2(0, 350),
            new Vector2(0, 348),
            new Vector2(80, 348),
            new Vector2(380, 348),
            new Vector2(520, 285),
            new Vector2(590, 315),
            new Vector2(590, 354)
        };

        private readonly Player _player;

        // graphicsDevice не используется, оставлен, чтобы не менять вызов в FactoryPerson
        public Vampir(Texture2D[] textures, Vector2 point, GraphicsDevice graphicsDevice, Player player)
            : base(textures, point)
        {
            _speed = 2;
            _damage = 12; // самый высокий урон
            _tag = "vampir";
            _patrolPath = Route;
            _player = player;
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