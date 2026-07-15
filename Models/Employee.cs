using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop.Input;
namespace SGame.Models
{
    public class Employee : Person
    {
        private SpriteBatch _spriteBatch;
        protected override int _damage { get => 10; set { } }
        protected override Texture2D? _texture { get; set; }
        private Vector2 _employeePoint { get; set; }
        public Employee(Texture2D texture, Vector2 point) : base(texture, point)
        {
            _speed = 4;
            _tag = "zombie";
            _employeePoint = point; //определяем появление игрока пока что вначале координат
            _texture = texture;
        }

        public override void Move(WpfKeyboard keyboard)
        {
            
        }

        public override void Shoot()
        {
            
        }
    }
}
