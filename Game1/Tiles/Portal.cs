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
    public class Portal : Tile
    {
        public bool Open = false;
        public bool BlackHole = false;
        public bool SwalledGhost = false;
        

        public Portal(Texture2D texture, SpriteFont font, Tuple<int, int> rowCol, int score) : base(texture, font, rowCol, score)
        {
            _color = Color.Gray;
        }
        public override void Update()
        {
            //X-Axis Movement
            if (!(_position.X == _newPosition.X))
            {
                if (_position.X > _newPosition.X)
                    _position.X -= MovementSpeed;
                else if (_position.X < _newPosition.X)
                    _position.X += MovementSpeed;
            }

            //Y-Axis Movement
            if (!(_position.Y == _newPosition.Y))
            {
                if (_position.Y > _newPosition.Y)
                    _position.Y -= MovementSpeed;
                else if (_position.Y < _newPosition.Y)
                    _position.Y += MovementSpeed;
            }
            if (_scale < 1f)
                _scale += 0.05f;

            if (Deleted)
                _scale -= 0.1f;

            if (_scale <= 0f)
                Remove = true;

            if (Open)
            {
                if (linkCellRow == linkCellCol)
                {
                    if (BlackHole)
                        _color = Color.DeepPink;
                    else
                        _color = Color.LimeGreen;
                }
                else
                {
                    if (BlackHole)
                        _color = Color.Purple;
                    else
                        _color = Color.LightSkyBlue;
                }
            }
            else if (BlackHole)
            {
                if (SwalledGhost)
                    _color = Color.DeepPink;
                else
                    _color = Color.Purple;
            }
            else
                _color = Color.Gray;
        }

    }
}
