using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using SGame.Models.Enemy;
using SGame.Models.GameObject;
namespace SGame.Models.FactoryModels
{
    public static class TextureLoader
    {
        public static Texture2D Load(GraphicsDevice graphicsDevice, string path)
        {
            using var stream = File.OpenRead(path);
            return Texture2D.FromStream(graphicsDevice, stream);
        }
    }
    public class FactoryPerson
    {
        public const int HEIGHT = 640;
        public const int WIDTH = 420;
        public const int MIN_VALUE_GENERATE_COIN = 16;
        public const int BULLET_SPRITE_SQUARE_SIZE = 5;
        public const int SPRITE_SQUARE_SIZE = 32;
        public const int SPRITE_COUNT_FRAME = 4;
        public const int CHEST_X_POSITION = 185;
        public const int CHEST_Y_POSITION = 0;
        private GraphicsDevice _graphicsDevice;
        public FactoryPerson(GraphicsDevice graphicsDevice) => _graphicsDevice = graphicsDevice;
        
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
        public Player GeneratePlayer()
        {
            Texture2D[] playerTextures =
                GenerateArrayTextures("Content/Texture/priest1_v1_", SPRITE_COUNT_FRAME);

            Texture2D[] attackTextures =
                GenerateArrayTextures("Content/Texture/PlayerAttack/priest3_v1_", SPRITE_COUNT_FRAME);

            Texture2D[] damageTextures =
                GenerateArrayTextures("Content/Texture/PlayerDamage/priest2_v2_", SPRITE_COUNT_FRAME);

            return new Player(playerTextures, Vector2.Zero, _graphicsDevice, attackTextures, damageTextures);
        }
        public Employee GenerateEmployee()
        {
            string path = "Content/Texture/Enemy/skull_v2_";
            Texture2D[] enemyTextures = GenerateArrayTextures(path, SPRITE_COUNT_FRAME);
            return new Employee(enemyTextures,Vector2.Zero, _graphicsDevice);
        }
        public Coins GenerateCoins(SpriteBatch spriteBatch)
        {
            Random random = new();
            int x = random.Next(MIN_VALUE_GENERATE_COIN, HEIGHT - 2 * SPRITE_SQUARE_SIZE);
            int y = random.Next(MIN_VALUE_GENERATE_COIN, WIDTH - 2 * SPRITE_SQUARE_SIZE);
            string path = "Content/Texture/Coins/coin_";
            Texture2D[] coinsTexture = GenerateArrayTextures(path, 4);
            
            return new Coins(coinsTexture, new Vector2(x, y), spriteBatch);
        }
        public Bullet GreateBullet(Vector2 vector) => new Bullet(GenerateTextureEntity(Color.Blue, BULLET_SPRITE_SQUARE_SIZE, BULLET_SPRITE_SQUARE_SIZE), vector);
        public Vampir CreateVampir(SpriteBatch spriteBatch, Player player)
        {
            string path = "Content/Texture/Vampir/vampire_v2_";
            Texture2D[] vampirTexture = GenerateArrayTextures(path, SPRITE_COUNT_FRAME);
            return new Vampir(vampirTexture, new Vector2(30, 250), _graphicsDevice, player);
        }

        public Chest GenerateChest()
        {
            string path = "Content/Texture/ChestTextures/chest_open_";
            Texture2D[] chestTextures = GenerateArrayTextures(path, SPRITE_COUNT_FRAME);
            return new Chest(new Vector2(CHEST_X_POSITION, CHEST_Y_POSITION), chestTextures);
        }

        private Texture2D[] GenerateArrayTextures(string path, int countPictures)
        {
            var textures = new Texture2D[countPictures];
            for (int i = 0; i < countPictures; i++)
            {
                string fullPath = $"{path}{i+1}.png";
                textures[i] = TextureLoader.Load(_graphicsDevice, fullPath);
            }
            return textures;
        }
    }
}
