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
        float delay = 500f;
        float fastDelay = 5;
        bool dropped = false;

        public TetrisBoard Board;
        int playWidth = 10;
        int playHeight = 16;

        public TetrisWindow( ) : base( 50, 20 )
        {
            InitializeView();
            Board.OnScoreChangeEvent += OnScoreChanged;
            Print( 35, 5, "Score: 0" );
            timer = delay;
        }

        public void OnScoreChanged( int score )
        {
            System.Console.WriteLine( "new Score: " + score.ToString() );
            Print( 35, 5, "Score: " + score.ToString());
        }

        public override void Update( TimeSpan time )
        {

            timer -= time.Milliseconds;
            if( timer < 0 )
            {
                if( Board.MoveDown() )
                {
                    //current.MoveDown();
                }
                else
                {
                    dropped = false;
                    //timer = delay;
                }

                if( dropped )
                    timer = fastDelay;
                else
                    timer = delay;
            }

            if( SadConsole.Global.KeyboardState.IsKeyReleased( Microsoft.Xna.Framework.Input.Keys.Down ) )
            {
                timer = 0;
                dropped = true;
            }

            if( SadConsole.Global.KeyboardState.IsKeyReleased( Microsoft.Xna.Framework.Input.Keys.Up ) )
            {
                Board.Rotate();
            }

            if( SadConsole.Global.KeyboardState.IsKeyReleased( Microsoft.Xna.Framework.Input.Keys.Left ) )
            {
                Board.MoveLeft();
            }

            if( SadConsole.Global.KeyboardState.IsKeyReleased( Microsoft.Xna.Framework.Input.Keys.Right ) )
            {
                Board.MoveRight();
            }

            base.Update( time );
        }

        

        private void InitializeView( )
        {

            Center();

            Title = "Tetris";

            // Calculate PlayArea position so its centered on X 
            int playAreaX = Width / 2 - playWidth / 2;
            int playAreaY = 2;

            // Create the board
            Board = new TetrisBoard( playWidth, playHeight );
            Board.Position = new Point( playAreaX, playAreaY );
            

            var border = new SadConsole.Surfaces.Basic( playWidth + 2, playHeight + 2 );

            border.DrawBox( new Rectangle( 0, 0, border.Width, border.Height ),new Cell(DefaultForeground, DefaultBackground ),connectedLineStyle:SurfaceBase.ConnectedLineThin );

            border.Draw( TimeSpan.Zero );
            border.Position = new Point( - 1,  - 1 );

            // Add PlayArea as a child to border so it renders on top
            Board.Children.Add( border );
            Children.Add( Board );
        }
    }
}
