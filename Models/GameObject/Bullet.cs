using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace SGame.Models.GameObject
{
    public sealed class Bullet
    {
        private int _distance = 120; //дистанция полета пули до уничтожения ее текстуры
        private int _damage = 12; //наносимый урон
        private int _speed = 6; //скорость полета пули
       
        private Texture2D _textureBullet { get; set; } //текстура пули
        
        private Vector2 _currentPoint; //текущее положение пули
        public Vector2 Direction { get; set; } = new Vector2(1, 0);

        public Bullet(Texture2D texture, Vector2 point)
        {
            _textureBullet = texture;
            _currentPoint = point;
           
        }

        public int GetDistance() => _distance; //метод получения дистанции
        public int GetDamage() => _damage; //метод для получения количества едениц урона 
        public void Move() //метод, отвечающий за движение пули
        {
            if (_distance > 0)
            {
                _currentPoint.X += _speed;
                _distance -= 3;
            }
            if (_distance <= 0)
                _textureBullet?.Dispose();
        }
        public void DrawBullet(SpriteBatch spriteBatch) => spriteBatch.Draw(_textureBullet, _currentPoint, Color.Blue); //метод отрисовки пули
        public Rectangle RectangeBullet() => //метод получения формы пули
           new Rectangle(
               (int)_currentPoint.X,
               (int)_currentPoint.Y,
               5, 5
           );
        public Texture2D GetTextureBullet() => _textureBullet; //метод получения текстуры пули
    }
}
