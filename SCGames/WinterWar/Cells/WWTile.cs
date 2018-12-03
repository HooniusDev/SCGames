using Microsoft.Xna.Framework;
using SCGames.WinterWar.Actions;
using SCGames.WinterWar.Cells;
using System;

namespace SadConsole.Maps
{
    /// <summary>
    /// A map tile.
    /// </summary>
    public partial class WWTile : Cell
    {
        protected int tileState;
        protected int tileType;
        protected int flags;

        /// <summary>
        /// Color to tint when in shadow
        /// </summary>
        public Color ShadowColor;
        /// <summary>
        /// Display Height info
        /// </summary>
        protected Cell AppearanceHeight;
        /// <summary>
        /// Appearance when in full FOV
        /// </summary>
        protected Cell AppearanceFOV;
        /// <summary>
        /// Appearance when not in FOV
        /// </summary>
        protected Cell AppearanceUnSeen;
        /// <summary>
        /// Amount of light coming to tile
        /// </summary>
        public float Light { get; set; }
        /// <summary>
        /// Height of tile floor
        /// </summary>
        public float Z { get; set; }

        public static Cell AppearanceNeverSeen = new Cell( Color.Black, Color.Black, '.' );

        public event EventHandler TileChanged;

        public string Title { get; set; }
        public string Description { get; set; }

        public Action<WWTile, int> OnTileStateChanged { get; set; }
        public Action<WWTile, int> OnTileFlagsChanged { get; set; }
        public Action<WWTile, ActionBase> OnProcessAction { get; set; }

        public string DefinitionId { get; protected set; }

        /// <summary>
        /// The type of tile represented.
        /// </summary>
        public int Type => tileType;

        /// <summary>
        /// Flags for the tile such as blocks LOS.
        /// </summary>
        public int Flags
        {
            get => flags;
            set
            {
                if( flags == value )
                    return;

                var oldFlags = flags;
                flags = value;
                OnTileFlagsChanged?.Invoke( this, oldFlags );
                UpdateAppearance();
                TileChanged?.Invoke( this, EventArgs.Empty );
            }
        }

        /// <summary>
        /// The state of the tile.
        /// </summary>
        public int TileState
        {
            get => tileState;
            set
            {
                if( tileState == value )
                    return;

                var oldState = tileState;
                tileState = value;
                OnTileStateChanged?.Invoke( this, oldState );
                UpdateAppearance();
                TileChanged?.Invoke( this, EventArgs.Empty );
            }
        }

        /// <summary>
        /// Where this tile is located on the map.
        /// </summary>
        public Point Position { get; set; }

        public WWTile( Cell fov, Cell unseen, Color shadow, float lightAmount, float z, int glyph ) : base( unseen.Foreground, unseen.Background, glyph )
        {

            Light = lightAmount; // no light
            Z = z; // height of cell floor

            ShadowColor = shadow;

            // Lerp tiles current color based on light level
            Color fg = Color.Lerp( ShadowColor, fov.Foreground, lightAmount ).FillAlpha();
            Color bg = Color.Lerp( ShadowColor, fov.Background, lightAmount ).FillAlpha();
            
            AppearanceFOV = new Cell( fg, bg, glyph );
            AppearanceUnSeen = new Cell( (fg * .8f ).FillAlpha(), ( bg * .8f ).FillAlpha(), glyph);

            //AppearanceNeverSeen = new Cell( ( unseen.Foreground * .8f ).FillAlpha(), ( unseen.Background * .8f ).FillAlpha(), glyph );
            AppearanceNeverSeen = AppearanceUnSeen;
            AppearanceNeverSeen.CopyAppearanceTo( this );

            SetFlag( TileFlags.PermaLight );

            Color heightShade = Color.Lerp( Color.Black, Color.White,  z );
            AppearanceHeight = new Cell( heightShade , heightShade );
            UpdateAppearance();
        }

        public WWTile( ) : base( Color.Wheat, Color.Turquoise, 3 )
        {
        }

        public void RenderHeight( bool on )
        {
            if( on )
            {
                AppearanceHeight.CopyAppearanceTo( this );
                TileChanged?.Invoke( this, EventArgs.Empty );
            }
            else
            {
                UpdateAppearance();
            }
        }

        //public void ChangeAppearance( Cell normal )
        //{
        //    Color dimFore = normal.Foreground * DimAmount;
        //    Color dimBack = normal.Background * DimAmount;
        //    dimFore.A = 255;
        //    dimBack.A = 255;

        //    ChangeAppearance( normal, new Cell( dimFore, dimBack, normal.Glyph ) );
        //}

        //public void ChangeAppearance( Cell normal, Cell dim )
        //{
        //    AppearanceLight = normal;
        //    AppearanceShadow = dim;

        //    UpdateAppearance();
        //}

        //public void ChangeGlyph( int glyph )
        //{
        //    AppearanceLight.Glyph = glyph;
        //    AppearanceShadow.Glyph = glyph;

        //    UpdateAppearance();
        //}

        /// <summary>
        /// Adds the specified flags to the <see cref="flags"/> property.
        /// </summary>
        /// <param name="flags">The flags to set.</param>
        public void SetFlag( params TileFlags[] flags )
        {
            var total = 0;

            foreach( var flag in flags )
                total = total | ( int ) flag;

            Flags = Helpers.SetFlag( this.flags, total );
        }
        /// <summary>
        /// Removes the specified flags to the <see cref="flags"/> property.
        /// </summary>
        /// <param name="flags">The flags to remove.</param>
        public void UnsetFlag( params TileFlags[] flags )
        {
            var total = 0;

            foreach( var flag in flags )
                total = total | ( int ) flag;

            Flags = Helpers.UnsetFlag( this.flags, total );
        }

        public void ProcessAction( SCGames.WinterWar.Actions.ActionBase action ) => OnProcessAction?.Invoke( this, action );

        protected virtual void UpdateAppearance( )
        {
            // not seen
            if( !Helpers.HasFlag( in flags, ( int ) TileFlags.InLOS ) )
            {
                AppearanceUnSeen.CopyAppearanceTo( this );
            }
            // seen
            else if( Helpers.HasFlag( in flags, ( int ) TileFlags.InLOS ) )
            {
                AppearanceFOV.CopyAppearanceTo( this );
            }
            //else if( !Helpers.HasFlag( in flags, ( int ) TileFlags.Seen ) )
            //{
            //    AppearanceNeverSeen.CopyAppearanceTo( this );
            //}

            TileChanged?.Invoke( this, EventArgs.Empty );
        }
    }
}