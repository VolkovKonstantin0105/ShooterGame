using Microsoft.Xna.Framework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media.Animation;
namespace SGame.Models.BoundsModel
{
    [NotMapped]
    public class Obstacle
    {
        public Rectangle Bounds { get; private set; }
        
        public Obstacle(int x, int y, int width, int height) 
        {
            Bounds = new Rectangle(x, y, width, height);
        }
        public bool Intersects(Rectangle other) => Bounds.Intersects(other); //проверка столкновения с объектом 

    }
}
