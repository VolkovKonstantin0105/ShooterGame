using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop.Input;
using SGame.Models.FactoryModels;

namespace SGame.Models.Enemy
{
    public abstract class Person : Drawble
    {
        public const int SPRITE_SQUARE_SIZE = 32;

        protected int _speed { get; set; }                    // скорость движения
        protected string? _tag { get; set; } = "unknown";     // метка объекта ("player", "vampir")
        protected int _health = 100;                          // здоровье
        protected virtual int _damage { get; set; }           // урон, который наносит объект
        protected int _countAmmo = 20;                        // количество пуль
        protected bool _canMove = true;                       // может ли двигаться

        // TODO: значения перепутаны (640 - это ширина, 420 - высота), переименуй через F2
        protected int _height = 640;
        protected int _width = 420;

        public string? Tag => _tag;

        protected Person(Texture2D texture, Vector2 point)
        {
            _texture = texture;
            _currentPoint = point;
        }

        public abstract void Move(WpfKeyboard keyboard); // как объект двигается
        public abstract void Shoot();                    // как объект атакует

        public void SetSpriteBatch(SpriteBatch spriteBatch) => _spriteBatch = spriteBatch;
        public Texture2D? GetTexture() => _texture;
        public Vector2 GetPosition() => _currentPoint;
        public int GetDamage() => _damage;
    }
}