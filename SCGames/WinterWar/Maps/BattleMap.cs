using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SadConsole;
using SadConsole.Entities;
using SadConsole.Input;
using SCGames.WinterWar.Cells;
using SCGames.WinterWar.Entities;
using Console = SadConsole.Console;
using System.IO;
using SadConsole.Maps;
using System;
using GoRogue.MapViews;
using System.Collections.Generic;
using GoRogue;
using System.Collections;
using SadConsole.Surfaces;

namespace SCGames.WinterWar.Maps
{
    public class BattleMap : Console, ISettableMapView<MapTile>, IEnumerable<MapTile>
    {

        public event EventHandler MouseOverNewCell;

        public EntityManager EntityManager { get; private set; }

        protected MapTile[] Tiles;

        private void BattleMap_TileChanged( object sender, EventArgs e ) => IsDirty = true;

        public Point OldMousePos;
        public Cell OldCell;

        public Console TileInfo;

        public Entity TestTrooper;

        public BattleMap( int width, int height ) : base( width, height )
        {

            EntityManager = new EntityManager();
            Children.Add( EntityManager );

            FontMaster fontMaster = SadConsole.Global.LoadFont( "Kein5x5.font" );

            EntityFactory.EntityFont = fontMaster.GetFont( Font.FontSizes.Two );
            Font = fontMaster.GetFont( Font.FontSizes.Two );

            Tiles = new MapTile[width * height];

            for( int i = 0; i < Tiles.Length; i++ )
            {
                Tiles[i] = new MapTile( Color.Wheat, Color.Turquoise, 3 );
                Tiles[i].Position = new Point( i % Width, i / Width );
                Tiles[i].TileChanged += BattleMap_TileChanged;
            }

            TileInfo = new Console( 40, 1 )
            {
                Font = SadConsole.Global.FontDefault.Master.GetFont( Font.FontSizes.One ),
                Position = new Point( 2, 2 ),
                DefaultForeground = Color.Black,
                DefaultBackground = Color.Transparent,
            };
            

            Global.FocusedConsoles.Push( this );

            CreateTrooper(31,13);
            CreateTrooper( 42, 28 );
            CreateTrooper( 54, 48 );
            CreateTrooper( 64, 69 );
            CreateTrooper( 83, 91 );

            //Print( 10, 10, new ColoredString( "@", Color.Black, Color.Yellow ));

            //Surface = new SadConsole.Surfaces.Basic( width, height, fontMaster.GetFont( Font.FontSizes.Two ) );

            //Children.Add( Surface );

        }

        public new MapTile this[int index]
        {
            get => Tiles[index];
            set => SetupTile( value, new Point( index % Width, index / Width ) );
        }

        public new MapTile this[int x, int y]
        {
            get => Tiles[GetIndexFromPoint( x, y )];
            set => SetupTile( value, new Point( x, y ) );
        }

        private void SetupTile( MapTile tile, Point position )
        {
            // Dehook
            this[position].TileChanged -= BattleMap_TileChanged;
            tile.Position = position;
            tile.TileChanged += BattleMap_TileChanged;
            Tiles[position.ToIndex( Width )] = tile;
            Cells[position.ToIndex( Width )] = tile;
        }


        public MapTile this[Coord position]
        {
            get => Tiles[position.ToIndex( Width )];
            set => SetupTile( value, position.ToPoint() );
        }

        public MapTile this[Point position]
        {
            get => Tiles[position.ToIndex( Width )];
            set => SetupTile( value, position );
        }

        public Entity GetEntity( Point position )
        {
            foreach( Entity e in EntityManager.Entities )
            {
                if( e.Position == position )
                    return e;  
            }
            return null;
        }

        public override void Update( TimeSpan timeElapsed )
        {
            var conMouseState = new SadConsole.Input.MouseConsoleState( this, SadConsole.Global.MouseState );
            if( isMouseOver )
            {
                if( OldMousePos != conMouseState.CellPosition )
                {
                    TileInfo.Clear();
                    MapTile tile = GetTile( conMouseState.CellPosition );
                    string cellPos = $"{conMouseState.CellPosition.X}, {conMouseState.CellPosition.Y}";
                    string height = Math.Round( (30f*tile.Height), 1 ).ToString();
                    TileInfo.Print( 0, 0, new ColoredString( cellPos  + "(" + height +"): " + tile.Description, Color.Black, Color.DarkGray));

                    OldMousePos = conMouseState.CellPosition;
                    System.Console.WriteLine( OldMousePos.ToString() + " Cell changed");
                    MouseOverNewCell?.Invoke( this, new EventArgs() );
                }


            }
            base.Update( timeElapsed );
        }


        /// <summary>
        /// Gets a tile from the map.
        /// </summary>
        /// <param name="position">The map position of the tile.</param>
        /// <returns>The tile if it exists, otherwise null.</returns>
        public MapTile GetTile( Point position ) => IsTileValid( position.X, position.Y ) ? this[position] : null;

        /// <summary>
        /// Test if a tile blocks movement.
        /// </summary>
        /// <param name="x">Tile coordinate x.</param>
        /// <param name="y">Tile coordinate y.</param>
        /// <returns>True if the tile blocks movement.</returns>
        public bool IsTileWalkable( int x, int y )
        {
            if( x < 0 || y < 0 || x >= Width || y >= Height )
                return false;

            return !Helpers.HasFlag( Tiles[y * Width + x].Flags, ( int ) TileFlags.BlockMove );
        }

        /// <summary>
        /// Test if a tile blocks line of sight.
        /// </summary>
        /// <param name="x">Tile coordinate x.</param>
        /// <param name="y">Tile coordinate y.</param>
        /// <returns>True if the tile blocks light of sight.</returns>
        public bool IsTileOpaque( int x, int y )
        {
            if( x < 0 || y < 0 || x >= Width || y >= Height )
                return true;

            return Helpers.HasFlag( Tiles[y * Width + x].Flags, ( int ) TileFlags.BlockLOS );
        }

        /// <summary>
        /// Test if a tile coordinate is within the map bounds.
        /// </summary>
        /// <param name="x">Tile coordinate x.</param>
        /// <param name="y">Tile coordinate y.</param>
        /// <returns>True if the tile exists.</returns>
        public bool IsTileValid( int x, int y ) => x >= 0 || y >= 0 || x < Width || y > Height;

        /// <summary>
        /// Returns a random floor tile.
        /// </summary>
        /// <returns>A tile that is not a wall.</returns>
        public Tile FindEmptyTile( )
        {
            while( true )
            {
                var foundTile = this[this.RandomPosition( GoRogue.Random.SingletonRandom.DefaultRNG )];

                if( !Helpers.HasFlag( foundTile.Flags, ( int ) TileFlags.BlockMove ) )
                    return foundTile;
            }
        }

        //public override void Draw( TimeSpan timeElapsed )
        //{
        //    base.Draw( timeElapsed );

        //    foreach( var gameObject in GameObjects )
        //        gameObject.Item.Draw( timeElapsed );
        //}

        //public override void Update( TimeSpan timeElapsed )
        //{
        //    base.Update( timeElapsed );

        //    foreach( var gameObject in GameObjects )
        //        gameObject.Item.Update( timeElapsed );

        //    GameObjects.SyncView();
        //}

        /// <summary>
        /// Translates FOV values between GoRogue and our game tiles.
        /// </summary>
        public class TranslationFOV : GoRogue.MapViews.TranslationMap<Tile, double>
        {
            public TranslationFOV( GoRogue.MapViews.IMapView<Tile> map ) : base( map ) { }

            protected override double TranslateGet( Tile value )
            {
                // 1 = blocked; 0 = see thru
                return Helpers.HasFlag( value.Flags, ( int ) TileFlags.BlockLOS ) ? 1.0 : 0.0;
            }
        }

        public new IEnumerator<MapTile> GetEnumerator( )
        {
            return new List<MapTile>( Tiles ).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator( )
        {
            return GetEnumerator();
        }

        public void CreateTrooper( int x, int y )
        {
            var anim = new SadConsole.Surfaces.Animated( "default", 2, 2, Font );

            anim.DefaultBackground = new Color( 240, 240, 250, 50);
            anim.DefaultForeground = Color.Blue;

            var frame = anim.CreateFrame();
            frame.Cells[0].Glyph = 0;
            frame.Cells[1].Glyph = 1;
            frame.Cells[2].Glyph = 16;
            frame.Cells[3].Glyph = 17;

            TestTrooper = new Entity( anim );
            TestTrooper.Position = new Point( x,y );
            this.EntityManager.Entities.Add( TestTrooper );
            this.CenterViewPortOnPoint(TestTrooper.Position);
            //CenterViewPortOnPoint( TestTrooper.Position );
        }

    }
}
