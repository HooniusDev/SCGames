using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Surfaces;

namespace SCGames.Common.Controls
{
    public class Panel : Basic
    {
        /// <summary>
        /// Color for Top and Left lines of the panel 
        /// (Supposed to be dark color)
        /// </summary>
        public Color EdgeHighlight;
        /// <summary>
        /// Color for Bottom and Right lines of the panel 
        /// (Supposed to be light color)
        /// </summary>
        public Color EdgeShadow;
        /// <summary>
        /// Color of the Background 
        /// </summary>
        public Color FillColor;

        private bool _fake3d;
        /// <summary>
        /// Use shadow inside Panel
        /// </summary>
        public bool Fake3D
        {
            get => _fake3d;
            set
            {
                _fake3d = value;
            }
        }

        /// <summary>
        /// Surface that is used for printing 
        /// (Printing directly to the Panel would override graphics, atleast currently...)
        /// </summary>
        Basic _printSurface;
        /// <summary>
        /// Cursor that prints to the _printSurface
        /// </summary>
        Cursor _printCursor;
        /// <summary>
        /// Manages use of AddLine()'s _printCursor.NewLine(). 
        /// If new Line would be added after each print, 
        /// first line of text would scroll out and an 
        /// empty line would show at bottom
        /// </summary>
        bool _firstLine = true;


        /// <summary>
        /// Adds a new line to the Panel
        /// </summary>
        /// <param name="text">Text to be added</param>
        public void AddLine( string text )
        {
            if( _firstLine )
                _firstLine = false;
            else
                _printCursor.NewLine();
            _printCursor.Print( new ColoredString( text, new Cell(Color.Black, Color.Transparent) ));
        }
        /// <summary>
        /// Clears the text
        /// </summary>
        public new void Clear( )
        {
            _printSurface.Clear();
            // reset cursor and act like i'm brand new
            _firstLine = true;
            _printCursor.Position = Point.Zero;
        }

        /// <summary>
        /// Font must use IsSadExtended and I have added these two items to GlyphDefinitions
        ///     "shadow_left": {
        ///  "Glyph": 221,
        ///  "Mirror": 0
        ///  },
        ///"shadow_top": {
        ///  "Glyph": 223,
        ///  "Mirror": 0
        /// }
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
public Panel( int width, int height ) : base( width, height )
        {
            FillColor = Color.DarkSlateGray;
            EdgeShadow = new Color(12,12, 24);
            EdgeHighlight = new Color( 200, 140, 55 );
            Fake3D = true;
            DrawPanel();

            _printSurface = new Basic( Width, Height )
            {
                DefaultBackground = Color.Transparent,
                DefaultForeground = Color.White,
            };
            _printCursor = new Cursor( _printSurface );
            Children.Add( _printSurface );
        }

        /// <summary>
        /// Redraw the panel
        /// </summary>
        public void DrawPanel( )
        {

            Fill( null, FillColor, null, null );

            if( Font.Master.IsSadExtended )
            {

                // gets Decorators from Font file  
                var edgeLeft = Font.Master.GetDecorator( "box-edge-left", EdgeShadow );
                var edgeBottom = Font.Master.GetDecorator( "box-edge-bottom", EdgeHighlight );
                var edgeRight = Font.Master.GetDecorator( "box-edge-right", EdgeHighlight );
                var edgeTop = Font.Master.GetDecorator( "box-edge-top", EdgeShadow );
                var edgeRightBottom = Font.Master.GetDecorator( "box-edge-right-bottom", EdgeHighlight );
                var edgeleftTop = Font.Master.GetDecorator( "box-edge-left-top", EdgeShadow );
                var shadowLeft = Font.Master.GetDecorator( "shadow_left", ( FillColor * .9f ).FillAlpha() );
                var shadowTop = Font.Master.GetDecorator( "shadow_top", ( FillColor * .9f ).FillAlpha() );

                // Draw the shadow?
                Color? shadow = null;
                if( _fake3d )
                    shadow = ( FillColor * .9f ).FillAlpha();

                // top egde
                AddDecorator( 1, 0, Width - 2, new[] { shadowTop, edgeTop } );
                // bottom edge
                AddDecorator( 1, Height - 1, Width - 2, new[] { edgeBottom } );

                for( int y = 1; y < Height - 1; y++ )
                {
                    // Left edge
                    AddDecorator( 0, y, 1, new[] { shadowLeft, edgeLeft } );
                    // Right edge
                    AddDecorator( Width - 1, y, 1, new[] { edgeRight } );
                }


                // Bottom Left Corner
                SetDecorator( 0, Height - 1, 1, new[]
                    {
                        shadowLeft,
                        edgeLeft,
                        edgeBottom
                    } );

                // Top Left
                AddDecorator( 0, 0, 1, new[]
                     {
                        shadowLeft,
                        shadowTop,
                        edgeleftTop
                    } );

                //Top Right Corner
                AddDecorator(
                     Width - 1, 0, 1, new[]
                    {
                        shadowTop,
                        edgeTop,
                        edgeRight
                    } );

                //Bottom Right (Doesnt Work!)
                AddDecorator(
                     Width - 1, Height - 1, 1, new[]
                    {
                        edgeRightBottom
                    } );
                // Hack to get Bottom Right corner to render
                SetGlyph( Width - 1, Height - 1, edgeRightBottom.Glyph, this.EdgeHighlight );

            }
        }
    }
}


