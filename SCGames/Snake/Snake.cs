using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Entities;
using SadConsole.Surfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = SadConsole.Console;

namespace SCGames.Snake
{

    public class Snake : Entity
    {


        // The body of the snake
        public List<Entity> Tail { get; private set; }

        // Boolean to allow only one direction change per 'Tick'
        private bool _madeTurn = false;
        // Am I alive and well?
        public bool Alive = true;

        // Movement Direction of Snake
        private Point _direction = Point.Zero;
        public Point Direction 
        {
            get { return _direction; }
            set
            {
                // No inverting of direction (DOWN <-> UP) (Left <->Right)
                if( value == new Point( -_direction.X, _direction.Y ) || value == new Point( _direction.X, -_direction.Y ) || _madeTurn )
                    return;
                _direction = value;
                _madeTurn = true; // Lock to only 1 turn per move
            }
        }



        // Grows the Snake by one 
        public void Grow( )
        {
            // Create Animated surface for a Tail piece
            var _tail = new SadConsole.Surfaces.Animated( "default", 1, 1 );
            _tail.CreateFrame();
            _tail.CurrentFrame[0].Glyph = 9;
            _tail.CurrentFrame[0].Foreground = Color.LawnGreen;
            _tail.CurrentFrame[0].Background = Color.Transparent;
            Entity tail = new Entity( _tail );
            
            
            if( Tail.Count >= 1 )
            {
                // Copy position from previous Tail piece
                tail.Position = Tail[Tail.Count-1].Position;
            }
            else
            {
                // If its the very first Tail piece copy position from Snake object
                tail.Position = Position;
            }
            // Add to Tail list and register to Entities collection 
            Tail.Add( tail );
            SnakeBoard.EntityManager.Entities.Add( tail );

        }



        public void Move( )
        {
            if( !Alive ) // Dead dont move
                return;
            if( Tail.Count > 0 )
            {
                for( int i = Tail.Count-1; i >= 0; i-- )
                {
                    if( i >= 1 )
                    {
                        
                    Tail[i].Position = Tail[i - 1].Position;
                    }
                    else // At last segment so copy position of head (this)
                    {
                        Tail[0].Position = Position;
                    }
                }
            }
            // Apply current direction to Head
            Position += Direction;
            _madeTurn = false;
        }


        public Snake( Animated anim) : base(anim)
        {
            Tail = new List<Entity>();
            SnakeBoard.EntityManager.Entities.Add( this );
        }
    }
}
