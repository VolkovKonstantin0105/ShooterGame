using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace SGame.Models
{
    public sealed class Bullet
    {
        private int _distance = 120;
        private int _damage = 12;
        private int _speed = 3;
   
        private Texture2D _textureBullet { get; set; }
        private Vector2 _currentPoint;

        public Bullet(Texture2D texture, Vector2 point)
        {
            _textureBullet = texture;
            _currentPoint = point;
        }

        public int GetDistance() => _distance;
        public int GetDamage() => _damage;
        public void Move()
        {
            if (_distance > 0)
            {
                _currentPoint.X += _speed;
                _distance -= 3;
            }
            if (_distance <= 0)
                _textureBullet?.Dispose();
        }
        public void DrawBullet(SpriteBatch spriteBatch) => spriteBatch.Draw(_textureBullet, _currentPoint, Color.Black);
        

        
    }
}
