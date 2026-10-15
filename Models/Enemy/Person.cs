using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop.Input;
using SGame.Models.FactoryModels;
namespace SGame.Models.Enemy
{
    public abstract class Person : Drawble
    {
        public const int SPRITE_SQUARE_SIZE = 32;
        protected SpriteBatch _spriteBatch ;//для отрисовки объектов
        protected int _speed { get; set; } //переменная для хранения скорости объекта
        protected string? _tag { get; set; } = "unknown"; //по умолчанию тэг будет "не известен"
        protected int _health = 100; //количество здоровья 
        protected virtual int _damage { get; set; } //виртуальное свойство и переменная для стрельбы по противнику
        protected int _height = 640;
        protected int _width = 420;
        protected int _countAmmo = 20; //количество пуль
        protected bool _canMove = true; //могу ли двигаться
        public string? Tag => _tag;
        public Person(Texture2D texture, Vector2 point)
        {
            _texture = texture;        
            _currentPoint = point;     
        }
        public abstract void Move(WpfKeyboard keyboard);//метод передвижения объекта
        public abstract void Shoot();//метод для стрельбы
        public void SetSpriteBatch(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }
        public Texture2D? GetTexture() => _texture; 
        public Vector2 GetPosition() => _currentPoint;
        public int GetDamage() => _damage;

        


    }
}
