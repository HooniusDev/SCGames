using Microsoft.Xna.Framework;
using SadConsole.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCGames.WinterWar.Cells
{
    public class MapTile : Tile
    {

        public static MapTile Snow { get { return new MapTile( new Color( 240, 240, 245 ), new Color( 220, 220, 235 ), '.' ); } }
        public static MapTile Trench { get { return new MapTile( new Color( 130, 100, 76 ), new Color( 100, 80, 55 ), ',' ); } }
        public static MapTile TrenchWall { get { return new MapTile( new Color( 139, 110, 89 ), new Color( 130, 100, 80 ), '#' ); } }
        public static MapTile Bridge { get { return new MapTile( new Color( 64, 55, 34 ), new Color( 108, 84, 62 ), '-' ); } }
        public static MapTile BridgeRail { get { return new MapTile( new Color( 64, 55, 57 ), new Color( 110, 98, 89 ), '#' ); } }

        public float Height { get; set; }

        public MapTile( Color fg, Color bg, int glyph ) : base( fg, bg, glyph )
        {
            // Snow
            // PackedSnow
            // ShallowSnow
            // Ice
            // Dirt
            // Trench
            // Tree

        }
        
    }
}
