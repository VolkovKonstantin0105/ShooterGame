using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop.Input;
using System.ComponentModel.DataAnnotations.Schema;
namespace SGame.Models
{
    public abstract class Person
    {
        protected SpriteBatch _spriteBatch;//для отрисовки объектов
        protected int _speed { get; set; } //переменная для хранения скорости объекта
        protected string? _tag { get; set; } = "unknown"; //по умолчанию тэг будет "не известен"
        protected int _health = 100; //количество здоровья 
        protected virtual int _damage { get; set; } //виртуальное свойство и переменная для стрельбы по противнику
        [NotMapped]
        protected virtual Texture2D? _texture { get; set; }//текстура игрового персонажа
        [NotMapped]
        protected Vector2 _currentPoint; //текущая позиция игрока
        protected int _height = 640;
        protected int _width = 420;
        protected int _countAmmo = 20; //еоличество пуль
        public Person(Texture2D texture, Vector2 point) { }
        public abstract void Move(WpfKeyboard keyboard);//метод передвижения объекта
        public abstract void Shoot();//метод для стрельбы
        public void Draw(SpriteBatch spriteBatch) //метод отрисовки объекта
        {
            Rectangle destination = new Rectangle((int)_currentPoint.X, (int)(_currentPoint.Y), 32, 32);
            spriteBatch.Draw(_texture, destination, Color.White);
        }
        public void SetSpriteBatch(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

    }
}
