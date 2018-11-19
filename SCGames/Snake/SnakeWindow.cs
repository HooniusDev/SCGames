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
using SadConsole.Effects;

namespace SCGames.Snake
{
    public class SnakeWindow : Window
    {

        public SnakeBoard Board { get; private set; }
        int boardWidth = 30;
        int boardHeight = 20;

        private Cell _defaultAppearance = new Cell();

        private Rectangle _scoreRect;
        private Basic _scoreLabel;

        //TODO: Add a visible score
        private int _score = 0;
        public int Score
        {
            get { return _score; }
            set
            {
                
                _score = value;
                
                _scoreLabel.Print( 0, 0, _score.ToString() );
                // Make the score blink for some time
                // SOme time being ETERNITY due to a bug in SadConsole! Points finger @Thraka ;)
                _scoreLabel.SetEffect( _scoreLabel.GetCells( _scoreRect ), new Blink() { RemoveOnFinished = true, BlinkCount = 10, BlinkSpeed = 0.2d, } );
            }
        }

        public SnakeWindow( ) : base( 60, 40 )
        {

            _scoreRect = new Rectangle( 0, 0, 5, 1 );
            _scoreLabel = new Basic( 5, 1 );
            _scoreLabel.Position = new Point( Width / 2, 3 );
            Children.Add( _scoreLabel );

            InitializeView();
        
        }

        public void OnSnakeEat( object sender, EventArgs args )
        {
            Score += 25;
        }

        public void OnSnakeDeath( object sender, EventArgs args )
        {
            // Show a Death message
            // This doesnt properly show the button. Caused by thinline button!
            Message( "You Died! Score: " + Score.ToString(), "Okay", () =>
            {
                //Call Restart when button pressed
                Restart();
            } );
        }

        // Restarts game;
        public void Restart( )
        {
            Score = 0;
            Board.OnStart();
        }

        private void InitializeView( )
        {
            // Centers this window to parent
            Center();

            Title = "Snake";

            Board = new SnakeBoard( boardWidth, boardHeight );
            Board.Position = new Point( Width / 2 - boardWidth / 2, 5 );

            Board.DeathHandler += OnSnakeDeath;
            Board.EatHandler += OnSnakeEat;

            Print( Board.Position.X, 3, "SCORE:" );

            // Add Border around the board
            var border = new SadConsole.Surfaces.Basic( boardWidth + 2, boardHeight + 2 );

            border.DrawBox( new Rectangle( 0, 0, border.Width, border.Height ), new Cell( DefaultForeground, DefaultBackground ), connectedLineStyle: SurfaceBase.ConnectedLineThin );

            border.Position = new Point( -1, -1 );

            // Add border to Board children so it draws later
            Board.Children.Add( border );
            Children.Add( Board );

            Score = 0;
        }

    }
}
