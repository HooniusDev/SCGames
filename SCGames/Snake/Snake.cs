using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Entities;
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

        //private SadConsole.Surfaces.Animated _idle;
        //private SadConsole.Surfaces.Animated _moving;

        private Point _direction;
        public Point Direction 
        {
            get { return _direction; }
            set
            {
                // TODO: Validate direction
                // No inverting of direction (DOWN -> UP)
                if( value == new Point( -_direction.X, _direction.Y ) || value == new Point( _direction.X, -_direction.Y ) )
                    return;
                Animation = Animations["moving"];
                Animation.Start();
                _direction = value;
            }
        }


        public Snake( ) : base( 1, 1 )
        {
            var _idle = new SadConsole.Surfaces.Animated( "default", 1, 1 );

            _idle.CreateFrame();
            _idle.Frames[0].SetGlyph( 0, 0, 1 );
            _idle.Frames[0].SetForeground( 0, 0, Color.Aqua );
            _idle.Frames[0].SetBackground( 0, 0, Color.Transparent );

            _idle.CreateFrame();
            _idle.Frames[1].SetGlyph(0,0,1);
            _idle.Frames[1].SetForeground( 0, 0, Color.DarkBlue );
            _idle.Frames[1].SetBackground( 0, 0, Color.Transparent );

            _idle.AnimationDuration = 1;
            _idle.Repeat = true;
            _idle.Start();

            var _moving = new SadConsole.Surfaces.Animated( "moving", 1, 1 );

            _moving.CreateFrame();
            _moving.Frames[0].SetGlyph( 0, 0, 1 );
            _moving.Frames[0].SetForeground( 0, 0, Color.GreenYellow );
            _moving.Frames[0].SetBackground( 0, 0, Color.Transparent );

            _moving.CreateFrame();
            _moving.Frames[1].SetGlyph( 0, 0, 1 );
            _moving.Frames[1].SetForeground( 0, 0, Color.DarkOliveGreen );
            _moving.Frames[1].SetBackground( 0, 0, Color.Transparent );

            _moving.AnimationDuration = 1;
            _moving.Repeat = true;

            Animations.Remove( "default" );
            Animations.Add( "default", _idle );
            Animations.Add( "moving", _moving );
            

            Animation = _idle;
            Animation.Start();


        }
    }
}
