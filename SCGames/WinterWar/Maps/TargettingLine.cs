using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SadConsole;
using SadConsole.Entities;
using SadConsole.Input;
using SCGames.WinterWar.Cells;
using SCGames.WinterWar.Entities;
using System;
using System.Collections.Generic;
using Console = SadConsole.Console;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadConsole.Maps;
using SCGames.WinterWar.Maps;
using GoRogue;
using SadConsole.Effects;
using SadConsole.Surfaces;

namespace SCGames.WinterWar.Maps
{
    class TargettingLine
    {
        private BattleMap _map;

        List<Entity> _entitities;

        private bool isHidden;

        private Cell AppearanceLine = new Cell( Color.White, Color.Blue);

        public TargettingLine( BattleMap map )
        {
            _map = map;
            _entitities = new List<Entity>();
            


        }

        public void Show( Point lineStart, Point lineEnd )
        {
            System.Console.WriteLine( $"Line Show[{lineStart},{lineEnd}]" );
            if( isHidden )
                isHidden = false;

            Clear();
        
            Direction dir = Direction.GetCardinalDirection( lineStart.ToCoord(), lineEnd.ToCoord() );

            if( dir == Direction.UP )
            {
                System.Console.WriteLine( "UP" );
                lineStart.X += 1;
            }
            else if( dir == Direction.RIGHT )
            {
                System.Console.WriteLine( "RIGHT" );
                lineStart.X += 1;
                lineStart.Y += 1;
            }
            else if( dir == Direction.DOWN )
            {
                System.Console.WriteLine( "DOWN" );
                lineStart.Y += 1;
            }
            else if( dir == Direction.LEFT )
            {
                System.Console.WriteLine( "LEFT" );
            }

            IEnumerable<Coord> line = Lines.Get( lineStart.ToCoord(), lineEnd.ToCoord(), Lines.Algorithm.DDA );


            int count = 0;
            foreach( Coord p in line )
            {
                Animated anim = new Animated( "default", 1, 1, _map.Font );
                var frame = anim.CreateFrame();
                frame[0].CopyAppearanceFrom( AppearanceLine );
                frame[0].Glyph = '0'  + count;
                Entity e = new Entity( anim );
                _entitities.Add( e );
                e.Position = p.ToPoint();
                _map.EntityManager.Entities.Add( e );
                count++;
            }

            //System.Console.WriteLine( "length: " + line.Count<Coord>() );
            //frame.SetCellAppearance( 0, 0, new Cell( Color.White, Color.Blue ));

            //Animation = anim;
            
        }

        public bool IsValidCell( int x, int y )
        {
            return x >= 0 && x < _map.Width && y >= 0 && y < _map.Height;
        }

        public void Hide( )
        {
            isHidden = true;
            Clear();
        }

        private void Clear( )
        {
            System.Console.WriteLine( "Line Clear" );
            foreach( Entity e in _entitities )
            {
                _map.EntityManager.Entities.Remove( e );
            }
            _entitities.Clear();
        }

        private static List<Point> Line( int x0, int y0, int x1, int y1 )
        {
            List<Point> line = new List<Point>();
            var len = Math.Max( Math.Abs( x1 - x0 ), Math.Abs( y1 - y0 ) );

            for( int i = 0; i < len; i++ )

            {

                var t = ( float ) i / len;

                var x = Math.Round( x0 * ( 1.0 - t ) + x1 * t );

                var y = Math.Round( y0 * ( 1.0 - t ) + y1 * t );

                line.Add( new Point( ( int ) x, ( int ) y ));

            }
            return line;
        }

    }
}
