using Game1.Core;
using Game1.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.System
{
    public class Grid
    {
        //private int _gridSize = 4;
        public List<Tile> Tiles;
        public List<Tile> DeletedTiles;
        public List<Portal> Portals;
        private Random _random;
        public Stackman Stackman;
        private Texture2D _texture;
        private Texture2D _emptyTexture;
        private SpriteFont _font;
        private bool _gridIsFull = false;
        public bool GameOver = false;
        public bool OpenPortals = false;

       
        
        public Grid(Texture2D texture,Texture2D emptyTexture, SpriteFont font)
        {
            _texture = texture;
            _emptyTexture = emptyTexture;
            _font = font;
            _random = new Random();

            Tiles = new List<Tile>();
            DeletedTiles = new List<Tile>();
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    Tiles.Add(new Empty(_emptyTexture, _font, Tuple.Create(i,j),0));

            Portals = new List<Portal>();
            Portals.Add(new Portal(_texture, _font, Tuple.Create(-1, 0), 0));
            Portals.Add(new Portal(_texture, _font, Tuple.Create(0, 4), 0));
            Portals.Add(new Portal(_texture, _font, Tuple.Create(4, 3), 0));
            Portals.Add(new Portal(_texture, _font, Tuple.Create(3, -1), 0));
            Portals[1].linkCellRow = 0;
            Portals[1].linkCellCol = 3;
            Portals[2].linkCellRow = 3;
            Portals[2].linkCellCol = 3;
            Portals[3].linkCellRow = 3;
            Portals[3].linkCellCol = 0;

            PutStackmanInGame();
            GeneratePellet();
        }

        private void PutStackmanInGame()
        {
            var emptyCell = FindEmptyCell();
            if (_gridIsFull)
                return;

            Stackman = new Stackman(_texture, _font, Tuple.Create(emptyCell.Row, emptyCell.Col), 0);
            Tiles.Remove(emptyCell);
            Tiles.Add(Stackman);
        }

        public void GeneratePellet()
        {
            var emptyCell = FindEmptyCell();

            if (_gridIsFull)
                return;
            
            if(_random.Next(4) == 4 - 1)
            {
                if (!maxGhost())
                {
                    if (!CheckAdjacency(emptyCell))
                        GenerateGhost(emptyCell);
                    return;
                }
            }

            Pellet newPellet = new Pellet(_texture, _font, Tuple.Create(emptyCell.Row,emptyCell.Col), _random.Next(1, 3));
            Tiles.Remove(emptyCell);
            Tiles.Add(newPellet);
            
        }

        private bool CheckAdjacency(Tile emptyCell)
        {
            bool danger = false;
            if (emptyCell.Row == 0)
            {
                if (Stackman.Col == 0)
                {
                    //checking column for danger
                    if (GetTile(1, 0) is Pellet)
                        return false;
                    else if(GetTile(1,0) is Ghost)
                    {
                        if (GetTile(1, 0).Weak)
                            return false;
                        else
                        {
                            if (GetTile(2, 0) is Pellet)
                                danger = true;
                            else if (GetTile(2, 0) is Ghost) // check this doubtfull =======================
                                return false;
                            else if(GetTile(2,0) is Empty)
                            {
                                if (GetTile(3, 0) is Ghost)
                                    return false;
                                else danger = true;
                            }

                        }
                    }
                    else if(GetTile(1,0) is Empty)
                    {
                        if (GetTile(2, 0) is Pellet)
                            return false;
                        else if(GetTile(2,0) is Ghost)
                        {
                            if (GetTile(2, 0).Weak)
                                return false;
                            else if (GetTile(3, 0) is Ghost)
                                return false;
                            else danger = true;

                        }
                        else if(GetTile(2,0) is Empty)
                        {
                            if (GetTile(3, 0) is Pellet)
                                return false;
                            else if (GetTile(3, 0) is Ghost)
                                if (GetTile(3, 0).Weak)
                                    return false;
                                else danger = true;
                            else if (GetTile(3, 0) is Empty)
                                return false;
                        }
                    }
                    //if this reached means danger = true // consider opening portals here in following section
                    if (danger)
                    {
                        if (emptyCell.Col == 1)
                        {
                            if (GetTile(0, 2) is Ghost)
                                return false;
                            else if (GetTile(0, 2) is Empty)
                                if (GetTile(0, 3) is Ghost)
                                    return false;
                                else return true; // as third is either empty or pellet

                        }
                        else if(emptyCell.Col == 2)
                        {
                            if (GetTile(0, 1) is Pellet)
                                return false;
                            else if (GetTile(0, 1) is Ghost)
                                return false;
                            else // fist is empty
                            {
                                if (GetTile(0, 3) is Ghost)
                                    return false;
                                else if (GetTile(0, 3) is Pellet)
                                    return true;
                                else return true; // third is also empty
                            }
                        }
                        else if(emptyCell.Col == 3)
                        {
                            if (GetTile(0, 1) is Pellet)
                                return false;
                            else if (GetTile(0,1) is Ghost)
                            {
                                if (GetTile(0, 1).Weak)
                                    return false;
                                else // first if normal ghost
                                {
                                    if (GetTile(0, 2) is Pellet) //Player is fucked  here ===========Game Over 
                                        return true;
                                    else if (GetTile(0, 2) is Ghost) //no problemo here
                                        return false;
                                    else return false; // mid tile is empty means no problemo
                                }
                                
                            }
                            else if(GetTile(0,1) is Empty)
                            {
                                if (GetTile(0, 2) is Ghost)
                                    return false;//no problemo again 
                                if (GetTile(0, 2) is Pellet)
                                    return false;
                                if (GetTile(0, 2) is Empty)
                                    return true;
                            }
                        }
                    }
                    else return false;
       
                }
                else if(Stackman.Col == 3) { }
            }
            else if(emptyCell.Row == 3)
            {

            }

            return false;
        }

        internal bool maxGhost()
        {
            int ghostCount = 0;
            foreach (Tile tile in Tiles)
            {
                if (tile is Ghost)
                {
                    ghostCount++;
                }
            }
            if (ghostCount >= 4)
               return true;
            else  
               return false;
        }

        internal void PortalCheck(int portalNumber)
        {
            if (Portals[portalNumber].BlackHole)
            {
                Portals[portalNumber].BlackHole = false;
            }
            else
            {
                //check if row or col is full 
                for (int i = 3; i >= 0; i--)
                {
                    if(portalNumber == 2)
                        if (GetTile(i, 3) is Empty)
                            return;
                    if (portalNumber == 0)
                        if (GetTile(i , 0) is Empty)
                            return;
                    if (portalNumber == 1)
                        if (GetTile(0, i) is Empty)
                            return;
                    if (portalNumber == 3)
                        if (GetTile(3, i) is Empty)
                            return;
                }
                //row col full now check if Stackman is in correct cell
                if(portalNumber==0)
                    if(Stackman.Col == 0)
                    {
                        Portals[portalNumber].BlackHole = true;
                        Console.WriteLine("BlackHole Opened");
                    }
                if (portalNumber == 2)
                    if (Stackman.Col == 3)
                    {
                        Portals[portalNumber].BlackHole = true;
                        Console.WriteLine("BlackHole Opened");
                    }
                if (portalNumber == 1)
                    if (Stackman.Row == 0)
                    {
                        Portals[portalNumber].BlackHole = true;
                        Console.WriteLine("BlackHole Opened");
                    }
                if (portalNumber == 3)
                    if (Stackman.Row == 3)
                    {
                        Portals[portalNumber].BlackHole = true;
                        Console.WriteLine("BlackHole Opened");
                    }


            }
        }

        internal void GenerateGhost(Tile emptyCell)
        {
            Ghost newGhost = new Ghost(_texture, _font, Tuple.Create(emptyCell.Row, emptyCell.Col), 25);
            Tiles.Remove(emptyCell);
            Tiles.Add(newGhost);
        }

        private Tile FindEmptyCell()
        {
            List<Tile> emptyTiles = new List<Tile>();
            foreach(Tile tile in Tiles)
            {
                if (tile is Empty)
                    emptyTiles.Add(tile);
            }
            if (!(emptyTiles.Count == 0))
            {
                _gridIsFull = false;
                return (emptyTiles[_random.Next(emptyTiles.Count)]);
            }
            else
            {
                _gridIsFull = true;
                return null;
            }
        }
        public void MoveTile(Tile tileToMove,Tile tileAtCell)
        {
            var tileRow = tileToMove.Row; var tileCol = tileToMove.Col;
            var cellRow = tileAtCell.Row; var cellCol = tileAtCell.Col;
            if (tileAtCell is Empty)
            {
                if(tileToMove is Pellet)
                {                                        
                    tileToMove.Move(cellRow, cellCol);
                    tileAtCell.Move( tileRow,tileCol);
                }
                else if(tileToMove is Stackman)
                {
                    tileToMove.Move(cellRow, cellCol);
                    tileAtCell.Move(tileRow, tileCol);
                }
                else if(tileToMove is Ghost)
                {
                    tileToMove.Move(cellRow, cellCol);
                    tileAtCell.Move(tileRow, tileCol);
                }
            }
            else if(tileAtCell is Pellet)//---------------------------Pellet------------------------//
            {
                if (tileToMove is Pellet)
                {
                    tileToMove.Move(cellRow, cellCol);
                    tileToMove.Score += tileAtCell.Score;
                    Tiles.Remove(tileAtCell);
                    DeletedTiles.Add(tileAtCell);
                    Tiles.Add(new Empty(_emptyTexture, _font, Tuple.Create(tileRow, tileCol), 0));
                }
                else if (tileToMove is Stackman)
                {
                    tileToMove.Move(cellRow, cellCol);
                    tileToMove.Score += tileAtCell.Score;
                    Tiles.Remove(tileAtCell);
                    DeletedTiles.Add(tileAtCell);
                    Tiles.Add(new Empty(_emptyTexture, _font, Tuple.Create(tileRow, tileCol), 0));
                }
                else if (tileToMove is Ghost)
                {
                    //nothing coz ghost cant eat pellets
                }
            }
            else if(tileAtCell is Ghost)//----------------------Ghost-----------------------//
            {
                if(tileToMove is Pellet)
                {
                    //nothing
                }
                else if(tileToMove is Stackman)
                {
                    if (tileAtCell.Weak)
                    {
                        tileToMove.Score += tileAtCell.Score;
                        tileToMove.Move(cellRow, cellCol);
                        Tiles.Remove(tileAtCell);
                        DeletedTiles.Add(tileAtCell);
                        Tiles.Add(new Empty(_emptyTexture, _font, Tuple.Create(tileRow, tileCol), 0));
                    }
                    else if(!tileAtCell.Weak)
                    {
                        
                        tileToMove.Move(cellRow, cellCol);
                        Tiles.Remove(tileToMove);
                        DeletedTiles.Add(tileToMove);
                        InitiateGameOver();
                        Tiles.Add(new Empty(_emptyTexture, _font, Tuple.Create(tileRow, tileCol), 0));
                    }
                }
                else if(tileToMove is Ghost)
                {
                    tileToMove.Move(cellRow, cellCol);
                    tileToMove.Score += tileAtCell.Score;
                    Tiles.Remove(tileAtCell);
                    DeletedTiles.Add(tileAtCell);
                    Tiles.Add(new Empty(_emptyTexture, _font, Tuple.Create(tileRow, tileCol), 0));
                    tileToMove.Weak = true;
                    tileAtCell.Weak = true;
                }
            }
            else if(tileAtCell is Stackman)//-----------------------Stackman--------------------------//
            {
                if(tileToMove is Pellet)
                {
                    //nothing
                }
                else if(tileToMove is Stackman)
                {
                    InitiateGameOver();
                }
                else if(tileToMove is Ghost)
                {
                    if (tileToMove.Weak)
                    {
                        tileAtCell.Score += tileToMove.Score;
                        Tiles.Remove(tileToMove);
                        DeletedTiles.Add(tileToMove);
                        Tiles.Add(new Empty(_emptyTexture, _font, Tuple.Create(tileRow, tileCol), 0));

                    }
                    else if (!tileToMove.Weak)
                    {
                        tileToMove.Move(cellRow, cellCol);
                        Tiles.Remove(tileAtCell);
                        DeletedTiles.Add(tileAtCell);
                        InitiateGameOver();
                        Tiles.Add(new Empty(_emptyTexture, _font, Tuple.Create(tileRow, tileCol), 0));
                    }
                }
            }
            else if(tileAtCell is Portal) //-------------------Portal-----------------//
            {
                if(tileToMove is Empty)
                {
                    //error
                    InitiateGameOver();
                }
                else if(tileToMove is Pellet)
                {
                    
                    tileToMove.Move(cellRow, cellCol); //protal's cellrow cellcol
                    Stackman.Score += tileToMove.Score;
                    Tiles.Remove(tileToMove);
                    DeletedTiles.Add(tileToMove);
                    Tiles.Add(new Empty(_emptyTexture, _font, Tuple.Create(tileRow, tileCol), 0));

                }
                else if(tileToMove is Ghost)
                {
                    tileToMove.Move(cellRow, cellCol); //protal's cellrow cellcol
                    Stackman.Score += tileToMove.Score;
                    Tiles.Remove(tileToMove);
                    DeletedTiles.Add(tileToMove);
                    Tiles.Add(new Empty(_emptyTexture, _font, Tuple.Create(tileRow, tileCol), 0));

                    tileAtCell.Weak = true; // make portal blackhole weak 
                }
                else if(tileToMove is Stackman)
                {
                    if(tileAtCell.Weak)
                    {
                        Console.WriteLine("Stuck at recursion?????");
                        MoveTile(tileToMove,GetTile(tileAtCell.linkCellRow, tileAtCell.linkCellCol));
                        Console.WriteLine("Nuh uh !!!");
                    }
                    else if (!tileAtCell.Weak)
                    {
                        tileToMove.Move(cellRow, cellCol);
                        Tiles.Remove(tileToMove);
                        DeletedTiles.Add(tileToMove);
                        InitiateGameOver();
                        Tiles.Add(new Empty(_emptyTexture, _font, Tuple.Create(tileRow, tileCol), 0));
                    }
                }
            }
        }

        private void InitiateGameOver()
        {
            Console.WriteLine("GameOver");
            GameOver = true;
        }

        public Tile GetTile(int row, int col)
        {
            foreach(Tile tile in Tiles)
            {
                if (tile.Row == row && tile.Col == col)
                    return tile;
            }
            return null;
        }

        public void CloseAllBlackHoles()
        {
            foreach(Portal port in Portals)
            {
                port.BlackHole = false;
                port.SwalledGhost = false;
            }
        }

        public void Update()
        {
            foreach(Portal port in Portals)
            {
                port.Update();
            }
            foreach(Tile tile in Tiles)
            {
                tile.Update();
            }
            foreach(Tile deletedTile in DeletedTiles)
            {
                
                deletedTile.Deleted = true;
                deletedTile.Update();               
            }
            if (maxGhost())
            {
                OpenPortals = true;
                foreach (Portal port in Portals)
                {
                    port.Open = true;
                }
            }
            else
            {
                OpenPortals = false;
                foreach (Portal port in Portals)
                {
                    port.Open = false;
                }
            }

            TilesCleanup();
          
        }

        private void TilesCleanup()
        {
            Tile tileToRemove = TilesToRemove();
            if (tileToRemove is null)
                return;
            DeletedTiles.Remove(tileToRemove);
            tileToRemove.Delete();

        }

        private Tile TilesToRemove()
        {
            foreach(Tile tile in DeletedTiles)
            {
                if (tile.Remove)
                    return tile;
            }
            return null;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Portal port in Portals)
            {
                port.Draw(spriteBatch);
            }
            foreach (Tile deletedTile in DeletedTiles)
            {
                deletedTile.Draw(spriteBatch);
            }

            foreach (Tile tile in Tiles)
            {
                
                tile.Draw(spriteBatch);
                
            }
            if (GameOver)
                spriteBatch.DrawString(_font, "Game Over", new Vector2(100, 200),Color.Black, 0f, new Vector2(0, 0), 3f,SpriteEffects.None,0);
        }
    }
}
