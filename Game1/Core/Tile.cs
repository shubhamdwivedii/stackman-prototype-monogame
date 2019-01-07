using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Game1.Core
{
    public class Tile
    {
        public static int OriginPoint = 100;
        public static int TileWidth = 80;  
        public static int MovementSpeed = 16;  //should completely divide 80 // 2 ,4 ,8 , 10, 16

        protected bool _animate = false;
        public int Row, Col;
        public int NewRow, NewCol;
        public int Score;
        protected Texture2D _texture;
        protected SpriteFont _font;
        protected Vector2 _position;
        protected Vector2 _newPosition;
        protected Vector2 _origin;
        protected float _scale;
        protected float _rotation;
        protected Color _color;
        protected float _depth;

        public bool Deleted = false;
        public bool Remove = false;

        public bool Weak = false; //used only by ghosts //also used by portals
        public int linkCellRow = 0, linkCellCol = 0; //only used by portals


        public Tile(Texture2D texture , SpriteFont font, Tuple<int,int> rowCol , int score)
        {
            _texture = texture;
            _font = font;
            _color = Color.White;
            Score = score;
            Row = rowCol.Item1;
            //NewRow = Row;
            Col = rowCol.Item2;
            //NewCol = Col;
            _position = new Vector2(OriginPoint + (TileWidth * Col), (OriginPoint) + (TileWidth * Row));
            _scale = 0.2f;
            _rotation = 0f;
            _origin = new Vector2(40, 40);
            _depth = 0.5f;

            Move(rowCol.Item1,rowCol.Item2);
        }

        public void Move(int row, int col)
        {
            Row = row;
            Col = col;
            _newPosition = new Vector2(OriginPoint + (TileWidth * Col), (OriginPoint) + (TileWidth * Row));
        }

        public virtual void Update()
        {
            //X-Axis Movement
            if(!(_position.X == _newPosition.X))
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

            if (Weak)
                _color = Color.Green;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position + _origin , null,_color,_rotation,_origin,_scale,SpriteEffects.None,_depth);
            if(!Deleted)
                spriteBatch.DrawString(_font, "  "+Score, new Vector2(_position.X + 10, _position.Y + 14), Color.Black);
        }

        internal void Delete()
        {
            //yet to implement

        }
    }

}
