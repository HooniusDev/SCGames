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
        public readonly int DefaultGlyph = 260;

        public EntityManager EntityManager { get; private set; }

        public Tetromino Current { get; private set; }    
        public Tetromino Next { get; private set; }

        public int Score { get; set; }

        public TetrisBoard( int width, int height ) : base( width, height )
        {
            EntityManager = new EntityManager();
            Children.Add( EntityManager );

            OnStart();
        }

        private void GetNewTetromino( )
        {
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
            RemoveFullRow(row);
        }

        public bool TryMoveDown( )
        {
            // Check if empty for each child
            foreach( Entity e in Current.Shape )
            {
                Point target = new Point(0,1) + Current.Position + e.Position;

                if( !IsEmpty( target ) )
                {
                    Plant();
                    return false;
                }
            }
            Current.MoveDown();

            return true;
        }

        public bool TryMoveLeft( )
        {
            // Check if empty for each child
            foreach( Entity e in Current.Shape )
            {
                Point target = new Point( -1, 0 ) + Current.Position + e.Position;

                if( !IsEmpty( target ) )
                    return false;
            }
            Current.MoveLeft();
            return true;
        }

        public bool TryMoveRight( )
        {
            // Check if empty for each child
            foreach( Entity e in Current.Shape )
            {
                Point target = new Point( 1, 0 ) + Current.Position + e.Position;

                if( !IsEmpty( target ) )
                    return false;
            }
            Current.MoveRight();
            return true;
        }

        public void TryRotate( )
        {
            if (Current.CanRotate(this))
                Current.Rotate();
        }

        public void OnStart( )
        {
            initBoard();
            Current = Tetromino.GetRandom();
            Next = Tetromino.GetRandom();
            GetNewTetromino();
        }

        private void initBoard( )
        {
            DefaultBackground = new Color( 10, 10, 10 );
            DefaultForeground = new Color( 30, 30, 30 );

            Fill( DefaultForeground, DefaultBackground, DefaultGlyph );

            if( Current != null )
                EntityManager.Entities.Remove( Current);
        }

        /// <summary>
        /// Plants Current Tetromino to the board
        /// </summary>
        public void Plant(  )
        {
            foreach( Entity e in Current.Shape )
            {
                Point position = Current.PositionToGlobal( e );
                if( position.Y == 0 )
                {
                    OnStart();
                    return;
                }

                SetCellAppearance( position.X, position.Y, Current.Appearance );
            }
            GetNewTetromino();
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
