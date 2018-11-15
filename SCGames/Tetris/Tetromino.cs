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

    public enum Shapes
    {
        ILong,
        IShort,
        L,
    }

    public class Tetromino : Entity
    {

        //public static Tetromino ILong = new Tetromino( Shapes.ILong );
        //public static Tetromino IShort = new Tetromino( Shapes.IShort );
        //public static Tetromino L = new Tetromino( Shapes.L );

        public Entity[] shape { get; private set; }

        public TetrisBoard Board;

        public Cell Appearance;

        public static Tetromino GetRandom( )
        {
            Random rand = new Random();
            int r = rand.Next( 3 );
            switch (r)
            {
                case 0:
                    return new Tetromino( Shapes.ILong );
                case 1:
                    return new Tetromino( Shapes.IShort );
                case 2:
                    return new Tetromino( Shapes.L );
                default:
                    return new Tetromino( Shapes.L );
            }
        }


        public Tetromino( Shapes type ): base(1,1)
        {
            switch( type )
            {
                case Shapes.ILong:
                    System.Console.WriteLine( "ILong" );
                    Appearance = new Cell( Color.HotPink, Color.DeepPink, 260 );
                    shape = new Entity[]
                    {
                        CreateBlock(new Point(0,-2)),
                        CreateBlock(new Point(0,-1)),
                        CreateBlock(new Point(0,0)),
                        CreateBlock(new Point(0,1))
                    };
                    break;

                case Shapes.IShort:
                    System.Console.WriteLine( "IShort" );
                    Appearance = new Cell( Color.ForestGreen, Color.DarkGreen, 260 );
                    shape = new Entity[]
                    {
                        CreateBlock(new Point(0,-1)),
                        CreateBlock(new Point(0,0)),
                    };
                    break;
                case Shapes.L:
                    System.Console.WriteLine( "L" );
                    Appearance = new Cell( Color.Red, Color.DarkRed, 260 );
                    shape = new Entity[]
                    {
                        CreateBlock(new Point(0,-1)),
                        CreateBlock(new Point(0,0)),
                        CreateBlock(new Point(0,1)),
                        CreateBlock(new Point(1,1))
                    };
                    break;
            }
            Position = new Point( 4, 0 );
        }

        public bool CanMove( TetrisBoard board, Point direction )
        {
            // Check if empty for each child
            foreach( Entity e in shape )
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
            
            foreach( Entity e in shape )
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
           Position += new Point( 0, 1 );
        }
    }





}
