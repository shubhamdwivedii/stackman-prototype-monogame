using Game1.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Tiles
{
    public class Ghost : Tile
    {
        
        public Ghost(Texture2D texture, SpriteFont font, Tuple<int, int> rowCol, int score) : base(texture, font, rowCol, score)
        {
            _color = Color.Blue;
        }
        public void Weaken()
        {
            Weak = true;
        }

    }
}
