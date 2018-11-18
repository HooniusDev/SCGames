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

        public EntityManager EntityManager { get; private set; }

        public Snake Snake;
        public Target Target;

        private Random _random;

        public SnakeBoard( int width, int height ) : base( width, height )
        {
            EntityManager = new EntityManager();
            Children.Add( EntityManager );

            _random = new Random();

            OnStart();
        }

        public void OnTick( )
        {
            Snake.Position += Snake.Direction;
        }

        public void OnStart( )
        {
            DefaultBackground = new Color( 10, 10, 10 );
            DefaultForeground = new Color( 30, 30, 30 );
            int DefaultGlyph = 260;

            Fill( DefaultForeground, DefaultBackground, DefaultGlyph );

            Snake = new Snake();
            Snake.Position = new Point( Width / 2, Height / 2 );
            EntityManager.Entities.Add( Snake );

            

        }
    }
}
