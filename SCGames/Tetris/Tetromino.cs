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

    public enum TetrominoType
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

    // TODO Z piece rotates in a funky way..
    public class Tetromino : Entity
    {
        // Array of the actual 1x1 blocks
        public Entity[] Shape { get; private set; }
        // Appearance of the blocks
        public Cell Appearance;

        public static Random Random = new Random();
        private static TetrominoType previous;
        private TetrominoType _type;

        // Affects Z and ZInvert(They have only 2 rotations)
        bool _rotated = false;

        public static Tetromino GetRandom( )
        {

            int r = Random.Next( ( int ) TetrominoType.Count );

            // Re roll if the previous == new
            if( r == ( int ) previous )
            {
                r = Random.Next( ( int ) TetrominoType.Count );
            }
            // TODO: Cast r to TetrominoShape?
            switch (r)
            {
                case 0:
                    previous = TetrominoType.ILong;
                    return new Tetromino( Tetris.TetrominoType.ILong );
                case 1:
                    previous = TetrominoType.IShort;
                    return new Tetromino( Tetris.TetrominoType.IShort );
                case 2:
                    previous = TetrominoType.L;
                    return new Tetromino( Tetris.TetrominoType.L );
                case 3:
                    previous = TetrominoType.LInvert;
                    return new Tetromino( Tetris.TetrominoType.LInvert );
                case 4:
                    previous = TetrominoType.Z;
                    return new Tetromino( Tetris.TetrominoType.Z );
                case 5:
                    previous = TetrominoType.ZInvert;
                    return new Tetromino( Tetris.TetrominoType.ZInvert );
                default:
                    break;
            }
            return new Tetromino( TetrominoType.IShort );
        }

        public Tetromino( TetrominoType type ): base(1,1)
        {
            _type = type;
            switch( type )
            {
                case Tetris.TetrominoType.ILong:
                    Appearance = new Cell( Color.HotPink, Color.DeepPink, 260 );
                    Shape = new Entity[]
                    {
                        CreateBlock(new Point(0,-2)),
                        CreateBlock(new Point(0,-1)),
                        CreateBlock(new Point(0,0)),
                        CreateBlock(new Point(0,1))
                    };
                    break;
                case Tetris.TetrominoType.IShort:
                    Appearance = new Cell( Color.ForestGreen, Color.DarkGreen, 260 );
                    Shape = new Entity[]
                    {
                        CreateBlock(new Point(0,-1)),
                        CreateBlock(new Point(0,0)),
                    };
                    break;
                case Tetris.TetrominoType.L:
                    Appearance = new Cell( Color.Red, Color.DarkRed, 260 );
                    Shape = new Entity[]
                    {
                        CreateBlock(new Point(0,-1)),
                        CreateBlock(new Point(0,0)),
                        CreateBlock(new Point(0,1)),
                        CreateBlock(new Point(1,1))
                    };
                    break;
                case Tetris.TetrominoType.LInvert:
                    Appearance = new Cell( Color.Blue, Color.SteelBlue, 260 );
                    Shape = new Entity[]
                    {
                        CreateBlock(new Point(0,-1)),
                        CreateBlock(new Point(0,0)),
                        CreateBlock(new Point(0,1)),
                        CreateBlock(new Point(-1,1))
                    };
                    break;
                case Tetris.TetrominoType.Z:
                    Appearance = new Cell( Color.RosyBrown, Color.SandyBrown, 260 );
                    Shape = new Entity[]
                    {
                        CreateBlock(new Point(0,-1)),
                        CreateBlock(new Point(0,0)),
                        CreateBlock(new Point(-1,-1)),
                        CreateBlock(new Point(1,0))
                    };
                    _rotated = false;
                    break;
                case Tetris.TetrominoType.ZInvert:
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

        public bool CanRotate( TetrisBoard board )
        {
            // Check if empty for each child
            foreach( Point p in RotatedPoints() )
            {
                if( !board.IsEmpty( Position + p ) )
                    return false;
            }
            return true;
        }

        public Point PositionToGlobal( Entity e )
        {
            return Position + e.Position;
        }

        // Get Shape Entity positions after a rotation
        // TODO: Lots of same code as in Rotate, find a way to merge them
        private List<Point> RotatedPoints(  )
        {
            List<Point> points = new List<Point>();

            if( _type == TetrominoType.Z )
            {
                if( _rotated )
                {
                    points.Add( new Point( 1, 0 ));
                    points.Add( new Point( 0, 0 ));
                    points.Add( new Point( 1, -1 ));
                    points.Add( new Point( 0, 1 ));
                }
                else
                {
                    points.Add( new Point( 0, -1 ));
                    points.Add( new Point( 0, 0 ));
                    points.Add( new Point( -1, -1 ));
                    points.Add( new Point( 1, 0 ));
                }
            }
            else if( _type == TetrominoType.ZInvert )
            {
                if( !_rotated )
                {
                    points.Add(  new Point( -1, 0 ));
                    points.Add( new Point( 0, 0 ));
                    points.Add( new Point( -1, -1 ));
                    points.Add( new Point( 0, 1 ));
                }
                else
                {
                    points.Add( new Point( 0, -1 ));
                    points.Add( new Point( 0, 0 ));
                    points.Add( new Point( 1, -1 ));
                    points.Add( new Point( -1, 0 ));
                }
            }
            else
            {
                foreach( Entity e in Shape )
                {
                    points.Add( new Point( e.Position.Y, e.Position.X * -1 )); 
                }
            }

            return points;
        }

        public void Rotate( )
        {
            // Hacky way of rotating Z shapes
            if( _type == TetrominoType.Z )
            {
                if( !_rotated )
                {
                    Point[] points = RotatedPoints().ToArray();
                    Shape[0].Position = points[0];
                    Shape[1].Position = points[1];
                    Shape[2].Position = points[2];
                    Shape[3].Position = points[3];
                    _rotated = true;
                }
                else
                {
                    Point[] points = RotatedPoints().ToArray();
                    Shape[0].Position = points[0];
                    Shape[1].Position = points[1];
                    Shape[2].Position = points[2];
                    Shape[3].Position = points[3];
                    _rotated = false;
                }
            }
            else if( _type == TetrominoType.ZInvert )
            {
                if( !_rotated )
                {
                    Point[] points = RotatedPoints().ToArray();
                    Shape[0].Position = points[0];
                    Shape[1].Position = points[1];
                    Shape[2].Position = points[2];
                    Shape[3].Position = points[3];
                    _rotated = true;
                }
                else
                {
                    Point[] points = RotatedPoints().ToArray();
                    Shape[0].Position = points[0];
                    Shape[1].Position = points[1];
                    Shape[2].Position = points[2];
                    Shape[3].Position = points[3];
                    _rotated = false;
                }
            }
            else
            {
                foreach( Entity e in Shape )
                {
                    e.Position = new Point( e.Position.Y, e.Position.X * -1 );
                }
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
