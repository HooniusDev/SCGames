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

namespace SCGames.WinterWar.Maps
{
    class TargettingLine
    {
        private BattleMap _map;

        List<Cell> _line;
        Recolor2 _effect;

        private bool isHidden;

        public TargettingLine( BattleMap map )
        {
            _map = map;
            _line = new List<Cell>();

            _effect = new Recolor2()
            {
                Background = Color.Blue,
                Permanent = false,
                DoForeground = false
            };
        }

        public void Show( Point start, Point end )
        {
            System.Console.WriteLine( "Line Show" );
            if( isHidden )
                isHidden = false;

            Coord startC = start.ToCoord();

            Direction dir = Direction.GetCardinalDirection( startC , end.ToCoord() );

            if( dir == Direction.UP )
                startC += Coord.Get( 1, -1 );
            else if( dir == Direction.DOWN )
                startC += Coord.Get( 0, 2 );
            else if( dir == Direction.RIGHT )
                startC += Coord.Get( 2, 1 );
            else
                startC += Coord.Get( -1, 0 );

            var coords = Lines.Get( startC, end.ToCoord(), Lines.Algorithm.DDA );

            Clear();

            foreach( Coord c in coords )
            {
                Cell cell = _map[c.X, c.Y];
                _line.Add( cell );
                _map.SetEffect( cell, _effect.Clone() );
            }
        }

        public void Hide( )
        {
            isHidden = true;
            Clear();
        }

        private void Clear( )
        {
            System.Console.WriteLine( "Line Clear" );
            if( _line.Count > 0 )
            {
                foreach( Cell c in _line )
                {
                    _map.SetEffect( c, null );
                }
                _line.Clear();
            }
            //_map.Effects.RemoveAll();
        }
    }
}
