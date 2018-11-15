using SadConsole;
using System;
using Console = SadConsole.Console;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SadConsole.Surfaces;
using SadConsole.Entities;
using SadConsole.Input;

namespace SCGames.Tetris
{
    class TetrisWindow : Window
    {

        float timer;
        float dropDelay = 500f;
        float dropDelayFast = 5;
        bool dropped = false;

        public TetrisBoard Board;
        int playWidth = 10;
        int playHeight = 16;

        public TetrisWindow( ) : base( 60, 20 )
        {
            InitializeView();
            Board.OnScoreChangeEvent += OnScoreChanged;
            Print( 40, 5, "Score: 0" );
            timer = dropDelay;
        }

        public void OnScoreChanged( int score )
        {
            Print( 40, 5, "Score: " + score.ToString());
        }

        public override void Update( TimeSpan time )
        {
            // TODO these should be in TetrisBoard
            timer -= time.Milliseconds;
            if( timer < 0 )
            {
                if( Board.TryMoveDown() )
                {
                    // It moves automatically!
                }
                else
                {
                    dropped = false;
                }

                if( dropped )
                    timer = dropDelayFast;
                else
                    timer = dropDelay;
            }

            // Drop Current
            if( SadConsole.Global.KeyboardState.IsKeyReleased( Microsoft.Xna.Framework.Input.Keys.Down ) )
            {
                timer = 0;
                dropped = true;
            }

            if( SadConsole.Global.KeyboardState.IsKeyReleased( Microsoft.Xna.Framework.Input.Keys.Up ) )
            {
                Board.TryRotate();
            }

            if( SadConsole.Global.KeyboardState.IsKeyReleased( Microsoft.Xna.Framework.Input.Keys.Left ) )
            {
                Board.TryMoveLeft();
            }

            if( SadConsole.Global.KeyboardState.IsKeyReleased( Microsoft.Xna.Framework.Input.Keys.Right ) )
            {
                Board.TryMoveRight();
            }

            //TODO: Add pause state

            base.Update( time );
        }

        

        private void InitializeView( )
        {
            // Centers this window to parent
            Center();

            Title = "Tetris";

            // Calculate PlayArea position so its centered on X 
            int playAreaX = Width / 2 - playWidth / 2;
            int playAreaY = 2;

            // Create the board
            Board = new TetrisBoard( playWidth, playHeight );
            Board.Position = new Point( playAreaX, playAreaY );
            
            // Add Border around the board
            var border = new SadConsole.Surfaces.Basic( playWidth + 2, playHeight + 2 );

            border.DrawBox( new Rectangle( 0, 0, border.Width, border.Height ),new Cell(DefaultForeground, DefaultBackground ),connectedLineStyle:SurfaceBase.ConnectedLineThin );

            border.Draw( TimeSpan.Zero );
            border.Position = new Point( - 1,  - 1 );

            // Add border to Board children so it draws later
            Board.Children.Add( border );
            Children.Add( Board );
        }
    }
}
