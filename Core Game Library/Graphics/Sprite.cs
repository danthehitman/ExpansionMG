using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HML.Expansion.Graphics
{
    public class Sprite
    {
        public Vector2 Position { get; set; }

        private Texture2D texture;

        public Sprite(Vector2 position, Texture2D texture)
        {
            Position = position;
            this.texture = texture;
        }

        public Sprite(Texture2D texture)
        {
            this.texture = texture;
        }

        public void Draw(SpriteBatch sp)
        {
            sp.Draw(texture, Position, null, Color.White);
        }
    }
}
