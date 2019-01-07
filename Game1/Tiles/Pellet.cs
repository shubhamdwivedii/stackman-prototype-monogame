using Game1.Core;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Tiles
{
    public class Pellet : Tile
    {
        public Pellet(Texture2D texture,SpriteFont font, Tuple<int, int> rowCol, int score) : base(texture,font, rowCol, score)
        {
            
        }
    }
}
