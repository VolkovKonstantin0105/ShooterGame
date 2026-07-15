using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace SGame.Models
{
    public class FactoryPerson
    {
        private Random _random;//для рандомного получения коррдинат вектора 
        private IGraphicsDeviceService? _graphicsDeviceManager;
        private GraphicsDevice _graphicsDevice;
        public FactoryPerson(GraphicsDevice graphicsDevice) 
        {
            _graphicsDevice = graphicsDevice;
            _random = new Random();
        }

        private Texture2D GenerateTextureEntity(Color colors,int width, int height)
        {
            Texture2D textureEntity = new Texture2D((GraphicsDevice)_graphicsDevice, width, height);
            Color[] color = new Color[width * height];
            for (int i = 0; i < color.Length; i++)
            {
                color[i] = colors;
            }
            textureEntity.SetData(color);
            return textureEntity;
        }

        public Player GeneratePlayer() => new Player(GenerateTextureEntity(Color.Pink, 32, 32), Vector2.Zero, _graphicsDevice);
        public Employee GenerateEmployee() => new Employee(GenerateTextureEntity(Color.Red, 32, 32), new Vector2(100, 100));
        //создание тексстуры пули, в качестве параметра принимает вектор-точку, которая соответствует центру тела, которое стреляет 
        //vector = new Vector(((текущийХ+32)*корень_из_2)/2,аналогично У)
        public Bullet GreateBullet(Vector2 vector) => new Bullet(GenerateTextureEntity(Color.Black, 10, 10), vector);
    }
}
