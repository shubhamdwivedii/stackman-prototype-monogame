using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.System
{
    public class Activity
    {
        private Texture2D _texture;
        private Texture2D _emptyTexture;
        private SpriteFont _font;
        private KeyboardState _keyStateNow, _keyStateLast;

        private Grid _grid;

        public bool GenerateTwoTiles = true;

        public Activity(Texture2D texture, Texture2D emptyTexture, SpriteFont spriteFont)
        {
            _texture = texture;
            _emptyTexture = emptyTexture;
            _font = spriteFont;
            _grid = new Grid(_texture, _emptyTexture, _font);
        }

        //private Grid _grid;




        public void Update()
        {
            
            _keyStateNow = Keyboard.GetState();
            //
            if (_grid.GameOver)
            {
                if (_keyStateLast.IsKeyDown(Keys.Space) && _keyStateNow.IsKeyUp(Keys.Space))
                {
                    _grid = new Grid(_texture, _emptyTexture, _font);
                    Console.WriteLine("************"+"\n*****************");
                }
            }
            else
            {
                if (_keyStateLast.IsKeyDown(Keys.Up) && _keyStateNow.IsKeyUp(Keys.Up))
                    UpMove();
                if (_keyStateLast.IsKeyDown(Keys.Down) && _keyStateNow.IsKeyUp(Keys.Down))
                    DownMove();
                if (_keyStateLast.IsKeyDown(Keys.Left) && _keyStateNow.IsKeyUp(Keys.Left))
                    LeftMove();
                if (_keyStateLast.IsKeyDown(Keys.Right) && _keyStateNow.IsKeyUp(Keys.Right))
                    RightMove();
                if (_keyStateLast.IsKeyDown(Keys.W) && _keyStateNow.IsKeyUp(Keys.W))
                {
                    if(_grid.OpenPortals)
                        if (_grid.Stackman.Row == _grid.Portals[0].linkCellRow && _grid.Stackman.Col == _grid.Portals[0].linkCellCol)
                            _grid.MoveTile(_grid.Stackman, _grid.GetTile(_grid.Portals[2].linkCellRow, _grid.Portals[2].linkCellCol));
                }
                if (_keyStateLast.IsKeyDown(Keys.S) && _keyStateNow.IsKeyUp(Keys.S))
                {
                    if (_grid.OpenPortals)
                        if (_grid.Stackman.Row == _grid.Portals[2].linkCellRow && _grid.Stackman.Col == _grid.Portals[2].linkCellCol)
                            _grid.MoveTile(_grid.Stackman, _grid.GetTile(_grid.Portals[0].linkCellRow, _grid.Portals[0].linkCellCol));
                }
                if (_keyStateLast.IsKeyDown(Keys.A) && _keyStateNow.IsKeyUp(Keys.A))
                {
                    if (_grid.OpenPortals)
                        if (_grid.Stackman.Row == _grid.Portals[3].linkCellRow && _grid.Stackman.Col == _grid.Portals[3].linkCellCol)
                            _grid.MoveTile(_grid.Stackman, _grid.GetTile(_grid.Portals[1].linkCellRow, _grid.Portals[1].linkCellCol));
                }
                if (_keyStateLast.IsKeyDown(Keys.D) && _keyStateNow.IsKeyUp(Keys.D))
                {
                    if (_grid.OpenPortals)
                        if (_grid.Stackman.Row == _grid.Portals[1].linkCellRow && _grid.Stackman.Col == _grid.Portals[1].linkCellCol)
                            _grid.MoveTile(_grid.Stackman, _grid.GetTile(_grid.Portals[3].linkCellRow, _grid.Portals[3].linkCellCol));
                }
            }
            //
            _keyStateLast = _keyStateNow;
            _grid.Update();
        }
        //Game Controlls

        public void UpMove()
        {
            if (_grid.Portals[0].BlackHole)
            {
                Console.WriteLine("Moving into Blackhole");
                for (int i = 0; i < 4; i++)
                {
                    Console.WriteLine("Moving into Blackhole");
                    _grid.MoveTile(_grid.GetTile(i,0),_grid.Portals[0]);
                }
                _grid.Portals[0].BlackHole = false;
                _grid.Portals[0].SwalledGhost = false;
                Console.WriteLine("Portal Closed");                
            }
            
            for(int i = 3; i >= 1; i--)
            {
                for(int j =0; j < 4; j++)
                {
                    _grid.MoveTile(_grid.GetTile(i, j), _grid.GetTile(i - 1, j));
                }
            }
            _grid.PortalCheck(2);
            _grid.GeneratePellet();
            if (GenerateTwoTiles)
                _grid.GeneratePellet();
        }
        public void DownMove()
        {
            if (_grid.Portals[2].BlackHole)
            {
                Console.WriteLine("Moving into Blackhole");
                for (int i = 3; i >= 0; i--)
                {
                    Console.WriteLine("Moving into Blackhole");
                    _grid.MoveTile(_grid.GetTile(i, 3), _grid.Portals[2]);
                }
                _grid.Portals[2].BlackHole = false;
                _grid.Portals[2].SwalledGhost = false;
                Console.WriteLine("Portal Closed");
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    _grid.MoveTile(_grid.GetTile(i, j), _grid.GetTile(i + 1, j));
                }
            }
            _grid.PortalCheck(0);
            _grid.GeneratePellet();
            if (GenerateTwoTiles)
                _grid.GeneratePellet();
        }
        public void LeftMove()
        {
            if (_grid.Portals[3].BlackHole)
            {
                Console.WriteLine("Moving into Blackhole");
                for (int j = 0; j < 4; j++)
                {
                    Console.WriteLine("Moving into Blackhole");
                    _grid.MoveTile(_grid.GetTile(3,j), _grid.Portals[3]);
                }
                _grid.Portals[3].BlackHole = false;
                _grid.Portals[3].SwalledGhost = false;
                Console.WriteLine("Portal Closed");
            }
            for (int j = 3; j >=1 ; j--) 
            {
                for (int i = 0; i < 4 ; i++)
                {               
                    _grid.MoveTile(_grid.GetTile(i, j), _grid.GetTile(i , j - 1));                
                }
            }
            _grid.PortalCheck(1);
            _grid.GeneratePellet();
            if (GenerateTwoTiles)
                _grid.GeneratePellet();
        }
        public void RightMove()
        {
            if (_grid.Portals[1].BlackHole)
            {
                Console.WriteLine("Moving into Blackhole");
                for (int j = 3; j >=0 ; j--)
                {
                    Console.WriteLine("Moving into Blackhole");
                    _grid.MoveTile(_grid.GetTile(0,j), _grid.Portals[1]);
                }
                _grid.Portals[1].BlackHole = false;
                _grid.Portals[1].SwalledGhost = false;
                Console.WriteLine("Portal Closed");
            }
            for (int j = 0; j <  3; j++)
            {
                for (int i = 0; i < 4; i++)
                {                   
                    _grid.MoveTile(_grid.GetTile(i, j), _grid.GetTile(i, j + 1));                 
                }
            }
            _grid.PortalCheck(3);
            _grid.GeneratePellet();
            if (GenerateTwoTiles)
                _grid.GeneratePellet();
        }

        //Game Controlsss

        public void Draw(SpriteBatch spriteBatch)
        {
            _grid.Draw(spriteBatch);
        }
    }
}
