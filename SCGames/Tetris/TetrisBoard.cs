using Microsoft.Xna.Framework;
using SadConsole.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = SadConsole.Console;

namespace SCGames.Tetris
{
    public class TetrisBoard : Console
    {

        public delegate void ScoreChanged( int score );

        public event ScoreChanged OnScoreChangeEvent;

        /// <summary>
        /// Default glyph for blocks
        /// </summary>
        public int DefaultGlyph = 260;

        public EntityManager EntityManager;

        public Tetromino Current { get; private set; }    
        public Tetromino Next { get; private set; }

        public int Score { get; set; }


        public TetrisBoard( int width, int height ) : base( width, height )
        {

            ViewPort = new Rectangle( 0, 0, width, height );
            EntityManager = new EntityManager();
            Children.Add( EntityManager );

            OnStart();
        }

        private void newBlock( )
        {
            // Create next if it is null
            if( Next == null )
                Next = Tetromino.GetRandom();
            // Remove Current from EntityManager
            if( Current != null )
            {
                EntityManager.Entities.Remove( Current );
            }

            Current = Next;

            EntityManager.Entities.Add( Current );
            Next = Tetromino.GetRandom();

            // Check for full rows and clear them!
            for( int y = Height-1; y > 0; y-- )
            {
                // If row was full, recheck it (upper row was moved down)
                RemoveFullRow( y );
            }
        }

        public void RemoveFullRow( int row )
        {
            for( int x = 0; x < Width; x++ )
            {
                if( IsEmpty( new Point( x, row ) ) )
                    return;
                if( x == Width - 1 )
                {
                    // Go through each column
                    for( int col = 0; col < Width; col++ )
                    {
                        // move each cell one down above the emptied row
                        for( int y = row; y > 0; y-- )
                        {
                            SetCellAppearance( col, y, GetCellAppearance( col, y - 1 ));
                        }
                    }
                }
            }
            Score++;
            OnScoreChangeEvent?.Invoke( Score );
            System.Console.WriteLine( "Score " + Score.ToString() );
            RemoveFullRow(row);
            //return;
        }

        public bool MoveDown( )
        {
            // Check if empty for each child
            foreach( Entity e in Current.shape )
            {
                Point target = new Point(0,1) + Current.Position + e.Position;

                if( !IsEmpty( target ) )
                {
                    Plant();
                    return false;
                }
            }
            Current.Position += new Point( 0, 1 );

            return true;
        }

        public bool MoveLeft( )
        {
            // Check if empty for each child
            foreach( Entity e in Current.shape )
            {
                Point target = new Point( -1, 0 ) + Current.Position + e.Position;

                if( !IsEmpty( target ) )
                    return false;
            }
            Current.Position += new Point( -1, 0 );
            return true;
        }

        public bool MoveRight( )
        {
            // Check if empty for each child
            foreach( Entity e in Current.shape )
            {
                Point target = new Point( 1, 0 ) + Current.Position + e.Position;

                if( !IsEmpty( target ) )
                    return false;
            }
            Current.Position += new Point( 1, 0 );
            return true;
        }

        public void Rotate( )
        {
            Current.Rotate();
        }

        public void OnStart( )
        {
            initBoard();
            Current = null;
            Next = null;
            newBlock();
        }

        private void initBoard( )
        {
            DefaultBackground = new Color( 10, 10, 10 );
            DefaultForeground = new Color( 30, 30, 30 );

            Fill( DefaultForeground, DefaultBackground, 260 );

            if( Current != null )
                EntityManager.Entities.Remove( Current);
        }

        public void Plant(  )
        {
            System.Console.WriteLine( "Plant" );
            foreach( Entity e in Current.shape )
            {
                Point position = Current.PositionToGlobal( e );
                if( position.Y == 0 )
                {
                    OnStart();
                    return;
                }

                SetCellAppearance( position.X, position.Y, Current.Appearance );
            }
            newBlock();
        }

        public bool IsEmpty( Point position )
        {
            //System.Console.WriteLine( "IsEmpty: " + position.ToString() );
            // Cant move out of the sides
            if( position.X < 0 || position.X >= Width )
            {
                return false;
            }
            // Check if at bottom
            if( position.Y >= Height )
            {
                return false;
            }
            // Hack to get positions over the top to be accessible
            if( position.Y < 0 )
            {
                return true;
            }
            // If Foreground color is not Default it is not empty
            if( GetCellAppearance( position.X, position.Y ).Foreground == DefaultForeground )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public class ScoreChangedEventArgs : EventArgs
        {
            public int Score;
        }

    }
}
