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


namespace SCGames.Snake
{
    public class SnakeWindow : Window
    {

        public SnakeBoard Board { get; private set; }
        int boardWidth = 30;
        int boardHeight = 20;

        float timer;
        float speed = 500f;

        public override void Update( TimeSpan time )
        {
            // TODO these should be in TetrisBoard
            timer -= time.Milliseconds;
            if( timer < 0 )
            {
                Board.OnTick();
                timer = speed;
            }

            // Drop Current
            if( SadConsole.Global.KeyboardState.IsKeyReleased( Microsoft.Xna.Framework.Input.Keys.Down ) )
            {
                Board.Snake.Direction = new Point( 0, 1 );
            }

            if( SadConsole.Global.KeyboardState.IsKeyReleased( Microsoft.Xna.Framework.Input.Keys.Up ) )
            {
                Board.Snake.Direction = new Point( 0, -1 );
            }

            if( SadConsole.Global.KeyboardState.IsKeyReleased( Microsoft.Xna.Framework.Input.Keys.Left ) )
            {
                Board.Snake.Direction = new Point( -1, 0 );
            }

            if( SadConsole.Global.KeyboardState.IsKeyReleased( Microsoft.Xna.Framework.Input.Keys.Right ) )
            {
                Board.Snake.Direction = new Point( 1, 0 );
            }

            //TODO: Add pause state

            base.Update( time );
        }

        public SnakeWindow( ) : base( 60, 40 )
        {
            InitializeView();
            //Board.OnScoreChangeEvent += OnScoreChanged;
            //Print( 40, 5, "Lines: 0" );
            //timer = dropDelay;
        }

        private void InitializeView( )
        {
            // Centers this window to parent
            Center();

            Title = "Snake";

            Board = new SnakeBoard( boardWidth, boardHeight );
            Board.Position = new Point( Width / 2 - boardWidth / 2, 5 );

            // Add Border around the board
            var border = new SadConsole.Surfaces.Basic( boardWidth + 2, boardHeight + 2 );

            border.DrawBox( new Rectangle( 0, 0, border.Width, border.Height ), new Cell( DefaultForeground, DefaultBackground ), connectedLineStyle: SurfaceBase.ConnectedLineThin );

            border.Draw( TimeSpan.Zero );
            border.Position = new Point( -1, -1 );

            // Add border to Board children so it draws later
            Board.Children.Add( border );
            Children.Add( Board );
        }
    }
}
