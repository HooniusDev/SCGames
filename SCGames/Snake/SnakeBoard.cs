using Microsoft.Xna.Framework;
using SadConsole.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = SadConsole.Console;

namespace SCGames.Snake
{
    public class SnakeBoard : Console
    {

        public event EventHandler DeathHandler;
        public event EventHandler EatHandler;

        // Manager for snake and targets
        public static EntityManager EntityManager { get; private set; }

        // The Snake: Is an Head(Entity) with a list of Body(Entities) 
        public Snake Snake;

        // Random generator for Target placement
        private Random _random;

        private float _startSpeed = 300f;
        // Speed (delay) of Snake movement
        private float _speed = 300f;
        // Helper timer
        private float _timer;

        public override void Update( TimeSpan time )
        {

            //Update timer and call OnTick() when below zero
            _timer -= time.Milliseconds;
            if( _timer < 0 )
            {
                OnTick();
                CheckCollisision();
                _timer = _speed;
            }

            // Direction controls
            if( SadConsole.Global.KeyboardState.IsKeyReleased( Microsoft.Xna.Framework.Input.Keys.Down ) )
            {
                Snake.Direction = new Point( 0, 1 );
            }

            if( SadConsole.Global.KeyboardState.IsKeyReleased( Microsoft.Xna.Framework.Input.Keys.Up ) )
            {
                Snake.Direction = new Point( 0, -1 );
            }

            if( SadConsole.Global.KeyboardState.IsKeyReleased( Microsoft.Xna.Framework.Input.Keys.Left ) )
            {
                Snake.Direction = new Point( -1, 0 );
            }

            if( SadConsole.Global.KeyboardState.IsKeyReleased( Microsoft.Xna.Framework.Input.Keys.Right ) )
            {
                Snake.Direction = new Point( 1, 0 );
            }

            //TODO: Add pause state

            base.Update( time );
        }

        public void CheckCollisision( )
        {
            if( !Snake.Alive )
                return;
            // Check if collided with walls and if so, DIE!
            if( Snake.Position.X < 0 || Snake.Position.X >= Width || Snake.Position.Y < 0 || Snake.Position.Y >= Height  )
                OnSnakeDeath();

            // did we crash into anything?
            foreach( Entity e in EntityManager.Entities.ToList() )
            {
                // Snake head cannot collide into itself so skip it
                if( e == Snake )
                    continue;
                if( e.Position == Snake.Position )
                {
                    // Snake got to target so let's eat
                    if( e is Target )
                    {
                        
                        OnSnakeEat( e );
                    }
                    // Oh noes Snake bites itself, thats not healthy
                    else
                    {
                        OnSnakeDeath();
                    }
                }

            }
        }

        // SnakeBoard constructor
        public SnakeBoard( int width, int height ) : base( width, height )
        {
            _random = new Random();
            OnStart();
        }

        // Called ever time _timer gets under 0
        public void OnTick( )
        {
            Snake.Move();
        }

        public void PlaceTarget( )
        {
            // get random x and y values
            int x = _random.Next( Width );
            int y = _random.Next( Height );
            // TODO: Check if position is occupied by Snake
            var target = new Target( );
            target.Position = new Point( x, y );
            EntityManager.Entities.Add( target );
        }

        // Snake took a bite
        public void OnSnakeEat( Entity target )
        {
            Snake.Grow(); // Grow snake
            EntityManager.Entities.Remove( target ); // Remove Target
            PlaceTarget(); // Place new Target
            EatHandler?.Invoke( this, new EventArgs() ); // Fire an event (SnakeWindow will care )
            _speed -= 25; // Make things go faster
        }

        // Poor Snake died in a horrible way!
        public void OnSnakeDeath( )
        {
            Snake.Alive = false;
            DeathHandler?.Invoke( this, new EventArgs() ); // Fire an event (SnakeWindow will care )
        }


        public void OnStart( )
        {
            // Delete all Children. Clears old EntityManager on a Restart
            Children.Clear();

            EntityManager = new EntityManager();
            Children.Add( EntityManager );

            // Redraw the board itself 
            DefaultBackground = new Color( 10, 10, 10 );
            DefaultForeground = new Color( 30, 30, 30 );
            int DefaultGlyph = 260;
            Fill( DefaultForeground, DefaultBackground, DefaultGlyph );

            // Animated surface for Snake Head
            var _head = new SadConsole.Surfaces.Animated( "default", 1, 1 );
            _head.CreateFrame();
            _head.Frames[0].SetGlyph( 0, 0, 1 );
            _head.Frames[0].SetForeground( 0, 0, Color.LawnGreen );
            _head.Frames[0].SetBackground( 0, 0, Color.TransparentBlack );
            // Create the Snake and Position to center of board
            Snake = new Snake( _head )
            {
                Position = new Point( Width / 2, Height / 2 )
            };
            PlaceTarget();
            // Reset speed to default and Reset Timer
            _speed = _startSpeed;
            _timer = _speed;

        }
    }
}
