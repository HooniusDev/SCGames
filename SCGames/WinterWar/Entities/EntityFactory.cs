using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = SadConsole.Console;

namespace SCGames.WinterWar.Entities
{

    static class EntityFactory
    {

        public static Cell Trunk = new Cell( new Color( 71, 15, 0 ), Color.LightGray, 2 );
        public static Point[] TreeShadowShape = { new Point(1,1), new Point(2,2), new Point(3,3), new Point(3,2), new Point( 3,4), new Point(2,3), new Point(4,3), new Point(4,4) };

        public static void PlaceTree( this Console map, Point position )
        {
            // Place trunk
            map.SetCellAppearance( position.X, position.Y, Trunk );
            // Add Shadows
            // TODO: Check if height differs etc...
            for( int i = 0; i < TreeShadowShape.Length; i++ )
            {
                int x = position.X + TreeShadowShape[i].X;
                int y = position.Y + TreeShadowShape[i].Y;
                Cell old = map.GetCellAppearance( x, y );
                map.SetCellAppearance( x, y, new Cell( ( old.Foreground ).FillAlpha(), ( old.Background * .8f ).FillAlpha(), old.Glyph ) );
            }
        }


        public static Font EntityFont;

        public static Entity Birch;
        public static Entity Spruce;

        public static Entity GetRandomTree( )
        {
            var anim = new SadConsole.Surfaces.Animated( "default", 2, 2, EntityFont );

            anim.DefaultBackground = Color.Transparent;
            anim.DefaultForeground = Color.White;

            var frame = anim.CreateFrame();
            frame.Cells[0].Glyph = 2;
            frame.Cells[1].Glyph = 255;
            frame.Cells[2].Glyph = 255;
            if( Program.Random.Next( 10 ) > 3 )
            {
                frame.Cells[0].Glyph = 2;
                frame.Cells[1].Glyph = 3;
                frame.Cells[2].Glyph = 18;
                frame.Cells[3].Glyph = 19;
            }
            else
            {
                frame.Cells[0].Glyph = 4;
                frame.Cells[1].Glyph = 5;
                frame.Cells[2].Glyph = 20;
                frame.Cells[3].Glyph = 21;
            }

            return new SadConsole.Entities.Entity( anim );
            //tree.Position = MapConsole.GetPointFromIndex( i );
        }

    }
}
