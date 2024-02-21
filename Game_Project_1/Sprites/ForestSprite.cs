using Game_Project_1.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Project_1.Sprites
{

    /// <summary>
    /// A class representing the static sky background.
    /// </summary>
    public class ForestSprite
    {
        private Texture2D texture;

        private Vector2 position = new (0, 0);

        private BoundingRectangle _bounds = new BoundingRectangle(new Vector2(0, 140), 800, 340); //not used

        public BoundingRectangle Bounds => _bounds; //not used

        public Color color = Color.White;


        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("game_background_1");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, color, 0, new Vector2(8, 0), (float)0.26f, SpriteEffects.None, 1);
        }
    }
}
