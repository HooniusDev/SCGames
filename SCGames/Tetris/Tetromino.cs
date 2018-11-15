using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = SadConsole.Console;

namespace SCGames.Tetris
{

    public enum TetrominoShape
    {
        ILong,
        IShort,
        L,
        LInvert,
        Z,
        ZInvert,
        Count, // Not an actual shape, but acts like Count in lists so it's easy to get random item. 
    }

    // TODO add (Shape,ColorFG,ColorBG) Tuples or something  

    public class Tetromino : Entity
    {
        // Array of the actual 1x1 blocks
        public Entity[] Shape { get; private set; }
        // Appearance of the blocks
        public Cell Appearance;

        public static Random Random = new Random();
        private static TetrominoShape previous;

        public static Tetromino GetRandom( )
        {

            int r = Random.Next( ( int ) TetrominoShape.Count );

            // Re roll if the previous == new
            if( r == ( int ) previous )
            {
                r = Random.Next( ( int ) TetrominoShape.Count );
            }
            // TODO: Cast r to TetrominoShape?
            switch (r)
            {
                case 0:
                    previous = TetrominoShape.ILong;
                    return new Tetromino( Tetris.TetrominoShape.ILong );
                case 1:
                    previous = TetrominoShape.IShort;
                    return new Tetromino( Tetris.TetrominoShape.IShort );
                case 2:
                    previous = TetrominoShape.L;
                    return new Tetromino( Tetris.TetrominoShape.L );
                case 3:
                    previous = TetrominoShape.LInvert;
                    return new Tetromino( Tetris.TetrominoShape.LInvert );
                case 4:
                    previous = TetrominoShape.Z;
                    return new Tetromino( Tetris.TetrominoShape.Z );
                case 5:
                    previous = TetrominoShape.ZInvert;
                    return new Tetromino( Tetris.TetrominoShape.Z );
                default:
                    break;
            }
            return new Tetromino( TetrominoShape.IShort );
        }


        public Tetromino( TetrominoShape type ): base(1,1)
        {
            switch( type )
            {
                case Tetris.TetrominoShape.ILong:
                    Appearance = new Cell( Color.HotPink, Color.DeepPink, 260 );
                    Shape = new Entity[]
                    {
                        CreateBlock(new Point(0,-2)),
                        CreateBlock(new Point(0,-1)),
                        CreateBlock(new Point(0,0)),
                        CreateBlock(new Point(0,1))
                    };
                    break;
                case Tetris.TetrominoShape.IShort:
                    Appearance = new Cell( Color.ForestGreen, Color.DarkGreen, 260 );
                    Shape = new Entity[]
                    {
                        CreateBlock(new Point(0,-1)),
                        CreateBlock(new Point(0,0)),
                    };
                    break;
                case Tetris.TetrominoShape.L:
                    Appearance = new Cell( Color.Red, Color.DarkRed, 260 );
                    Shape = new Entity[]
                    {
                        CreateBlock(new Point(0,-1)),
                        CreateBlock(new Point(0,0)),
                        CreateBlock(new Point(0,1)),
                        CreateBlock(new Point(1,1))
                    };
                    break;
                case Tetris.TetrominoShape.LInvert:
                    Appearance = new Cell( Color.Blue, Color.SteelBlue, 260 );
                    Shape = new Entity[]
                    {
                        CreateBlock(new Point(0,-1)),
                        CreateBlock(new Point(0,0)),
                        CreateBlock(new Point(0,1)),
                        CreateBlock(new Point(-1,1))
                    };
                    break;
                case Tetris.TetrominoShape.Z:
                    Appearance = new Cell( Color.RosyBrown, Color.SandyBrown, 260 );
                    Shape = new Entity[]
                    {
                        CreateBlock(new Point(0,-1)),
                        CreateBlock(new Point(0,0)),
                        CreateBlock(new Point(-1,-1)),
                        CreateBlock(new Point(1,0))
                    };
                    break;
                case Tetris.TetrominoShape.ZInvert:
                    Appearance = new Cell( Color.Violet, Color.MediumVioletRed, 260 );
                    Shape = new Entity[]
                    {
                        CreateBlock(new Point(0,-1)),
                        CreateBlock(new Point(0,0)),
                        CreateBlock(new Point(1,-1)),
                        CreateBlock(new Point(-1,0))
                    };
                    break;
            }
            foreach( Entity e in Shape )
            {
                e.IsVisible = false;
            }
            Position = new Point( 4, -1 );
        }

        public bool CanMove( TetrisBoard board, Point direction )
        {
            // Check if empty for each child
            foreach( Entity e in Shape )
            {
                Point target = direction + Position + e.Position;
                
                if( !board.IsEmpty( direction + Position + e.Position ) )
                    return false;
            }

            return true;
        }

        public Point PositionToGlobal( Entity e )
        {
            return Position + e.Position;
        }

        public void Rotate( )
        {
            
            foreach( Entity e in Shape )
            {
                e.Position = new Point( e.Position.Y, e.Position.X * -1 );
            }
        }

        public void MoveLeft()
        {
            Position += new Point( -1, 0 );
        }

        public void MoveRight( )
        {
            Position += new Point( 1, 0 );
        }

        private Entity CreateBlock(  Point pos )
        {

          
            Entity block = new Entity( 1, 1 );
            block.Animation.CurrentFrame[0].Glyph = 260;
            block.Animation.CurrentFrame[0].Foreground = Appearance.Foreground;
            block.Animation.CurrentFrame[0].Background = Appearance.Background;
            block.Position = Position + pos;
            Children.Add( block );
            return block;
        }

        public void MoveDown( )
        {
            foreach( Entity e in Shape )
            {
                if( PositionToGlobal( e ).Y >= -1 )
                {
                    e.IsVisible = true;
                }
            }
            Position += new Point( 0, 1 );
           
        }
    }





}
