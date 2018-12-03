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


namespace SCGames.WinterWar
{
    class MapView : Window
    {

        public BattleMap BattleMap;

        int ZoomLevel = 2;

        bool _targetting = false;
        bool _height = false;

        TargettingLine _targetLine;

        public void Zoom( int level )
        {
            ZoomLevel = level;
            if( ZoomLevel < 1 )
                ZoomLevel = 1;
            if( ZoomLevel > 3 )
                ZoomLevel = 3;
            System.Console.WriteLine( "Zooming " + ZoomLevel.ToString() );
            Point center = BattleMap.ViewPort.Center;
            switch( ZoomLevel )
            {
                case 1:
                    BattleMap.Font = BattleMap.Font.Master.GetFont( Font.FontSizes.One );
                    BattleMap.Position = new Point( 4, 4 );
                    BattleMap.ViewPort = new Microsoft.Xna.Framework.Rectangle( 0, 0, ( Width - 2 ) * 4, ( Height - 2 ) * 4 );
                    break;
                case 2:
                    BattleMap.Font = BattleMap.Font.Master.GetFont( Font.FontSizes.Two );
                    BattleMap.Position = new Point( 2, 2 );
                    BattleMap.ViewPort = new Microsoft.Xna.Framework.Rectangle( 0, 0, ( Width - 2 ) * ZoomLevel, ( Height - 2 ) * ZoomLevel );
                    break;
                case 3:
                    BattleMap.Font = BattleMap.Font.Master.GetFont( Font.FontSizes.Four );
                    BattleMap.Position = new Point( 1, 1 );
                    BattleMap.ViewPort = new Microsoft.Xna.Framework.Rectangle( 0, 0, ( Width - 2 ), ( Height - 2 ) );
                    break;
                default:
                    BattleMap.Font = BattleMap.Font.Master.GetFont( Font.FontSizes.Two );
                    BattleMap.Position = new Point( 2, 2 );
                    BattleMap.ViewPort = new Microsoft.Xna.Framework.Rectangle( 0, 0, ( Width - 2 ) * ZoomLevel, ( Height - 2 ) * ZoomLevel );
                    break;
            }

            BattleMap.CenterViewPortOnPoint( center );
            foreach( Entity e in BattleMap.EntityManager.Entities )
            {
                e.Animation.Font = BattleMap.Font;
            }
            //BattleMap.ViewPort = new Microsoft.Xna.Framework.Rectangle( 0, 0, ( Width - 2 ) *  ZoomLevel, ( Height - 2 ) * ZoomLevel );

            //BattleMap.CenterViewPortOnPoint( TestTrooper.Position );
        }

        public MapView(  ) : base (59, 39)
        { 
            
            Title = "Map";

            FontMaster fontMaster = SadConsole.Global.LoadFont( "Kein5x5.font" );

            BattleMap = MapLoader.Load(  "WinterWar/Maps/0/" );
            Children.Add( BattleMap );
            Children.Add( BattleMap.TileInfo );
            BattleMap.Position = new Point( 2, 2 );

            BattleMap.ViewPort = new Microsoft.Xna.Framework.Rectangle( 0, 0, ( Width - 2 ) * ZoomLevel, ( Height - 2 ) * ZoomLevel );
            BattleMap.MouseOverNewCell += HandleMouseCellChanged;
            _targetLine = new TargettingLine( BattleMap );


            BattleMap.UpdateFov();

        }

        public void HandleMouseCellChanged( object obj, EventArgs e )
        {
            if( !_targetting )
            {
                return;
            }
            else
            {
                var start = BattleMap.EntityManager.Entities[0].Position;
                var end = BattleMap.OldMousePos;
                _targetLine.Show( BattleMap.GetCurrentUnit().Position, end );
            }

        }

        public override bool ProcessKeyboard( Keyboard info )
        {
            if( info.IsKeyReleased( Microsoft.Xna.Framework.Input.Keys.Z ) )
            {
                ZoomLevel -= 1;
                Zoom( ZoomLevel );
            }
            if( info.IsKeyReleased( Microsoft.Xna.Framework.Input.Keys.X ) )
            {
                ZoomLevel += 1;
                Zoom( ZoomLevel );
            }

            if( info.IsKeyDown( Microsoft.Xna.Framework.Input.Keys.Down ) )
            {
                BattleMap.CenterViewPortOnPoint( BattleMap.ViewPort.Center + new Point( 0, 1 ) );
            }

            if( info.IsKeyDown( Microsoft.Xna.Framework.Input.Keys.Up ) )
            {
                BattleMap.CenterViewPortOnPoint( BattleMap.ViewPort.Center + new Point( 0, -1 ) );
            }

            if( info.IsKeyDown( Microsoft.Xna.Framework.Input.Keys.Left ) )
            {
                BattleMap.CenterViewPortOnPoint( BattleMap.ViewPort.Center + new Point( -1, 0 ) );
            }

            if( info.IsKeyDown( Microsoft.Xna.Framework.Input.Keys.Right ) )
            {
                BattleMap.CenterViewPortOnPoint( BattleMap.ViewPort.Center + new Point( 1, 0 ) );
            }

            if( info.IsKeyPressed( Microsoft.Xna.Framework.Input.Keys.A ) )
            {
                BattleMap.TestTrooper.Position += new Point( 1, 0 );
            }

            if( info.IsKeyPressed( Microsoft.Xna.Framework.Input.Keys.N ) )
            {
                BattleMap.GetNextPlayerUnit();
            }

            if( info.IsKeyReleased( Microsoft.Xna.Framework.Input.Keys.T ) )
            {
                if( _targetting ) // -> off
                {
                    _targetLine.Hide();
                    _targetting = false;
                }
                else
                {
                    _targetting = true;
                }
            }

            if( info.IsKeyReleased( Microsoft.Xna.Framework.Input.Keys.H ) )
            {
                _height = !_height;

                foreach( WWTile t in BattleMap )
                {
                    t.RenderHeight( _height );
                }
            }

            if( info.IsKeyReleased( Microsoft.Xna.Framework.Input.Keys.C ) )
            {
                _targetLine.Hide();
            }

            return base.ProcessKeyboard( info );
        }



        //public void LoadMapData( string path )
        //{
        //    // Load Heightmap data
        //    // TODO: Store heightmap data!
        //    FileStream fileStream = new FileStream( path + "heightmap256.png", FileMode.Open );
        //    HeightMap = Texture2D.FromStream( Global.GraphicsDevice, fileStream );
        //    fileStream.Dispose();

        //    MapWidht = HeightMap.Width;
        //    MapHeight = HeightMap.Height;

        //    Color[] mapheightmapArray = new Color[MapWidht * MapHeight];
        //    HeightMap.GetData<Color>( mapheightmapArray );

        //    // Create console now that we know the map dimensions
        //    BattleMap = new Console( MapWidht, MapHeight );
        //    BattleMap.Position = new Point( ZoomLevel, ZoomLevel );
        //    Children.Add( BattleMap );

        //    // Load Albedo data
        //    fileStream = new FileStream( path + "albedo.png", FileMode.Open );
        //    var albedo = Texture2D.FromStream( Global.GraphicsDevice, fileStream );
        //    var albedoArray = new Color[MapWidht * MapHeight];
        //    albedo.GetData<Color>( albedoArray );
        //    fileStream.Dispose();

        //    // Write Albedo data to BattleMap ( Make Cell types )
        //    for( int i = 0; i < ( MapWidht * MapHeight ); i++ )
        //    {
        //        //BattleMap.Cells[i] = new Cell( (albedoArray[i]*1.3f).FillAlpha(), albedoArray[i].FillAlpha(), 32 );
        //        BattleMap.Cells[i] = TileFactory.Snow;
        //    }

        //    // Load Entities data
        //    fileStream = new FileStream( path + "entity256.png", FileMode.Open );
        //    var EntityData = Texture2D.FromStream( Global.GraphicsDevice, fileStream );
        //    Color[] entityArray = new Color[MapWidht * MapHeight];
        //    EntityData.GetData<Color>( entityArray );
        //    fileStream.Dispose();

        //    // Create Entities (Just trees for now)
        //    for( int i = 0; i < ( MapWidht * MapHeight ); i++ )
        //    {
        //        Color color = entityArray[i];
        //        if( color == Color.Red )
        //        {
        //            //var tree = EntityFactory.GetRandomTree();
        //            //tree.Position = BattleMap.GetPointFromIndex( i );
        //            //EntityManager.Entities.Add( tree );
        //            BattleMap.PlaceTree( BattleMap.GetPointFromIndex( i ) );
        //        }
        //        if( color == new Color( 80, 60, 50 ))
        //        {
        //            //BattleMap.Cells[i] = TileFactory.Trench;
        //        }

        //    }

        //}

        //public void CreateTrooper( )
        //{
        //    var anim = new SadConsole.Surfaces.Animated( "default", 2, 2, BattleMap.Font );

        //    anim.DefaultBackground = Color.Transparent;
        //    anim.DefaultForeground = Color.Blue;

        //    var frame = anim.CreateFrame();
        //    frame.Cells[0].Glyph = 0;
        //    frame.Cells[1].Glyph = 1;
        //    frame.Cells[2].Glyph = 16;
        //    frame.Cells[3].Glyph = 17;

        //    TestTrooper = new Entity( anim );
        //    TestTrooper.Position = new Point( 76,58 );
        //    EntityManager.Entities.Add( TestTrooper );
        //    BattleMap.CenterViewPortOnPoint( TestTrooper.Position );
        //}


    }
}
