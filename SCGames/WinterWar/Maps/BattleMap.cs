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
    public class BattleMap : Console, ISettableMapView<WWTile>, IEnumerable<WWTile>
    {

        public event EventHandler MouseOverNewCell;

        public EntityManager EntityManager { get; private set; }
        public int CurrentUnitID = -1;

        protected WWTile[] Tiles;

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

            Tiles = new WWTile[width * height];

            for( int i = 0; i < Tiles.Length; i++ )
            {
                Tiles[i] = new WWTile(  );
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

            CreateTrooper(31,13, true);
            CreateTrooper( 42, 28, true );
            CreateTrooper( 54, 48, true );
            CreateTrooper( 80, 45, false );

            CreateTrooper( 85, 50, false );
            CreateTrooper( 83, 91, true );

            CreateTrooper( 80, 40, false );



            GetNextPlayerUnit();

        }

        public new WWTile this[int index]
        {
            get => Tiles[index];
            set => SetupTile( value, new Point( index % Width, index / Width ) );
        }

        public new WWTile this[int x, int y]
        {
            get => Tiles[GetIndexFromPoint( x, y )];
            set => SetupTile( value, new Point( x, y ) );
        }

        private void SetupTile( WWTile tile, Point position )
        {
            // Dehook
            this[position].TileChanged -= BattleMap_TileChanged;
            tile.Position = position;
            tile.TileChanged += BattleMap_TileChanged;
            Tiles[position.ToIndex( Width )] = tile;
            Cells[position.ToIndex( Width )] = tile;
        }


        public WWTile this[Coord position]
        {
            get => Tiles[position.ToIndex( Width )];
            set => SetupTile( value, position.ToPoint() );
        }

        public WWTile this[Point position]
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
                    WWTile tile = GetTile( conMouseState.CellPosition );
                    string cellPos = $"{conMouseState.CellPosition.X}, {conMouseState.CellPosition.Y}";
                    string height = Math.Round( (tile.Z), 1 ).ToString();
                    TileInfo.Print( 0, 0, new ColoredString( cellPos  + "(" + height +"): " + tile.Description, Color.Black, Color.DarkGray));

                    OldMousePos = conMouseState.CellPosition;
                    //System.Console.WriteLine( OldMousePos.ToString() + " Cell changed");
                    MouseOverNewCell?.Invoke( this, new EventArgs() );
                }


            }
            base.Update( timeElapsed );
        }

        public void UpdateFov( )
        {

            foreach( Entity e in EntityManager.Entities )
            {
                if( ( e is Unit) && !( e as Unit ).PlayerControlled )
                    continue;
                RadiusAreaProvider r = new RadiusAreaProvider( e.Position.ToCoord(), 10, Radius.CIRCLE );
                foreach( Coord c in r.CalculatePositions() )
                {
                    this[c.X, c.Y].SetFlag( TileFlags.InLOS | TileFlags.Seen );
                }
            }
        }

        public bool IsPlayerUnit( Entity e )
        {
            if( ( e is Unit ) && ( e as Unit ).PlayerControlled )
                return true;
            else
                return false;
        }

        public bool IsCurrentPlayerUnit(  )
        {
            if( ( EntityManager.Entities[CurrentUnitID] is Unit ) && ( EntityManager.Entities[CurrentUnitID] as Unit ).PlayerControlled )
                return true;
            else
                return false;
        }

        public Unit GetNextPlayerUnit( )
        {
            System.Console.WriteLine( "GetNextPlayerUnit" );

            if( CurrentUnitID > -1 )
            {
                Entity e = EntityManager.Entities[CurrentUnitID];
                e.Animation.CurrentFrameIndex = 0;
            }

            CurrentUnitID = ( CurrentUnitID + 1 ) % EntityManager.Entities.Count;

            while( !IsCurrentPlayerUnit() )
            {
                System.Console.WriteLine( "This no Player Constrolled " + CurrentUnitID.ToString() );
                CurrentUnitID = ( CurrentUnitID + 1 ) % EntityManager.Entities.Count;
            }

            EntityManager.Entities[CurrentUnitID].Animation.CurrentFrameIndex = 1;
            System.Console.WriteLine( "Current is " + CurrentUnitID.ToString() );
            return EntityManager.Entities[CurrentUnitID] as Unit;
        }

        public Unit GetCurrentUnit( )
        {
            return EntityManager.Entities[CurrentUnitID] as Unit;
        }
        

        /// <summary>
        /// Gets a tile from the map.
        /// </summary>
        /// <param name="position">The map position of the tile.</param>
        /// <returns>The tile if it exists, otherwise null.</returns>
        public WWTile GetTile( Point position ) => IsTileValid( position.X, position.Y ) ? this[position] : null;

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
        public Cell FindEmptyTile( )
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
        public class TranslationFOV : GoRogue.MapViews.TranslationMap<WWTile, double>
        {
            public TranslationFOV( GoRogue.MapViews.IMapView<WWTile> map ) : base( map ) { }

            protected override double TranslateGet( WWTile value )
            {
                // 1 = blocked; 0 = see thru
                return Helpers.HasFlag( value.Flags, ( int ) TileFlags.BlockLOS ) ? 1.0 : 0.0;
            }
        }

        public new IEnumerator<WWTile> GetEnumerator( )
        {
            return new List<WWTile>( Tiles ).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator( )
        {
            return GetEnumerator();
        }

        public void CreateTrooper( int x, int y, bool playerControl )
        {
            var anim = new SadConsole.Surfaces.Animated( "default", 2, 2, Font );

            anim.DefaultBackground = new Color( 240, 240, 250, 50 );
            if( playerControl )
                anim.DefaultForeground = Color.Blue;
            else
                anim.DefaultForeground = Color.Red;

            var frame = anim.CreateFrame();
            frame.Cells[0].Glyph = 0;
            frame.Cells[1].Glyph = 1;
            frame.Cells[2].Glyph = 16;
            frame.Cells[3].Glyph = 17;

            frame = anim.CreateFrame();
            frame.Cells[0].Glyph = 0;
            frame.Cells[0].Foreground = Color.Yellow;
            frame.Cells[1].Glyph = 1;
            frame.Cells[1].Foreground = Color.Yellow;
            frame.Cells[2].Glyph = 16;
            frame.Cells[2].Foreground = Color.Yellow;
            frame.Cells[3].Glyph = 17;
            frame.Cells[3].Foreground = Color.Yellow;


            TestTrooper = new Unit( anim, playerControl )
            {
                Position = new Point( x, y ),
                Z = this[x, y].Z,
            };

            this.EntityManager.Entities.Add( TestTrooper );
            this.CenterViewPortOnPoint(TestTrooper.Position);
        }

    }
}
