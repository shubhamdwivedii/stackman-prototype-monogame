using Game1.Core;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Tiles
{
    public class Empty : Tile
    {
        public Empty(Texture2D texture, SpriteFont font, Tuple<int, int> rowCol, int score) : base(texture, font, rowCol, score)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, _color);
            
        }
    }
}
