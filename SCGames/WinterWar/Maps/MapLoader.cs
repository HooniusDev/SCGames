using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SadConsole;
using SadConsole.Maps;
using SCGames.WinterWar.Cells;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCGames.WinterWar.Maps
{
    public static class MapLoader
    {

        static readonly Color SnowTile = Color.Transparent;
        static readonly Color TrenchTile = new Color( 143, 86, 59 );
        static readonly Color TrenchWallTile = new Color( 251, 242, 54 );
        static readonly Color Bridge = new Color( 138, 111, 48 );
        static readonly Color BridgeRail = new Color( 143, 151, 74 );

        static readonly Cell SnowFOV = new Cell( new Color( 240, 240, 245 ), new Color( 220, 220, 235 ) );
        static readonly Color SnowShadow = new Color( 100, 100, 125 );
        static readonly Cell SnowUnSeen = new Cell( new Color( 100, 100, 125 ), new Color( 110, 110, 130 ));
        static readonly Cell TrenchFOV = new Cell( new Color( 143, 86, 59 ), new Color( 120, 66, 39 ) );
        static readonly Color TrenchShadow =  new Color( 40, 30, 20 );

        static readonly Cell TrenchUnSeen = new Cell( new Color( 70, 43, 30 ), new Color(75, 48, 36 ));
        static readonly Cell TrenchWallFOV = new Cell( new Color( 163, 106, 79 ), new Color( 143, 86, 59 ));
        static readonly Cell TrenchWallUnSeen = new Cell( new Color( 81, 52, 40 ), new Color( 90, 60, 50 ) );

        public static BattleMap Load( string path )
        {

            // Load Albedo data
            FileStream stream = new FileStream( path + "albedo.png", FileMode.Open );
            var albedo = Texture2D.FromStream( Global.GraphicsDevice, stream );
            int width = albedo.Width;
            int height = albedo.Height;
            var albedoArray = new Color[width * height];
            albedo.GetData<Color>( albedoArray );
            stream.Dispose();

            stream = new FileStream( path + "shadow.png", FileMode.Open );
            var shadow = Texture2D.FromStream( Global.GraphicsDevice, stream );
            var shadowArray = new Color[width * height];
            shadow.GetData<Color>( shadowArray );
            stream.Dispose();

            stream = new FileStream( path + "materials.png", FileMode.Open );
            var materials = Texture2D.FromStream( Global.GraphicsDevice, stream );
            var materialsArray = new Color[width * height];
            materials.GetData<Color>( materialsArray );
            stream.Dispose();

            stream = new FileStream( path + "heightmap.png", FileMode.Open );
            var heightmap = Texture2D.FromStream( Global.GraphicsDevice, stream );
            var heightmapArray = new Color[width * height];
            heightmap.GetData<Color>( heightmapArray );
            stream.Dispose();

            BattleMap map = new BattleMap( albedo.Width, albedo.Height );

            // Write Albedo data to MapConsole ( Make Cell types )
            for( int i = 0; i < ( width * height ); i++ )
            {
                if( materialsArray[i] == TrenchTile )
                {
                    map[i] = new WWTile( TrenchFOV, TrenchUnSeen, TrenchShadow, shadowArray[i].GetBrightness(), heightmapArray[i].GetBrightness(), ',' );

                    map[i].Description = "Trench";
                }
                else if( materialsArray[i] == TrenchWallTile )
                {
                    //map[i] = MapTile.TrenchWall;
                    map[i] = new WWTile( TrenchWallFOV, TrenchWallUnSeen, TrenchShadow, shadowArray[i].GetBrightness(), heightmapArray[i].GetBrightness(), '#' );
                    map[i].Description = "Trench wall.";

                }
                else if( materialsArray[i] == Bridge )
                {
                    //map[i] = MapTile.Bridge;
                    map[i] = new WWTile( SnowFOV, SnowUnSeen, TrenchShadow, shadowArray[i].GetBrightness(), heightmapArray[i].GetBrightness(), ',' );
                    map[i].Description = "Bridge deck";

                }
                else if( materialsArray[i] == BridgeRail )
                {
                    //map[i] = MapTile.BridgeRail;
                    map[i] = new WWTile( SnowFOV, SnowUnSeen, TrenchShadow, shadowArray[i].GetBrightness(), heightmapArray[i].GetBrightness(), '#' );
                    map[i].Description = "Bridge railing";

                }
                else
                {
                    map[i] = new WWTile( SnowFOV, SnowUnSeen, SnowShadow, shadowArray[i].GetBrightness(), heightmapArray[i].GetBrightness(), '.' );
                    map[i].Description = "Snow";
                }
                //WWTile tile = new WWTile( SnowFOV, SnowUnSeen, shadowArray[i].GetBrightness(), heightmapArray[i].GetBrightness(), '.' );
                //map[i].SetFlag( TileFlags.Seen | TileFlags.Lighted );
                //map[i].Height = heightmapArray[i].GetBrightness();
                //map[i].Background = Color.Lerp( map[i].Background, map[i].ShadowColor, 1f - shadowArray[i].GetBrightness() );
                //map[i].Z = heightmapArray[i].GetBrightness();

            }
            return map;
        }
    }
}
