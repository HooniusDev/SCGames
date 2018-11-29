using SadConsole;
using System;
using Console = SadConsole.Console;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SadConsole.Surfaces;
using SadConsole.Entities;
using SadConsole.Input;

namespace SCGames
{
    /// <summary>
    /// Just playing out with idea for window to view and edit Tileset and Font data
    /// Heavily VIP... 
    /// Look at SadConsole/DemoProjects/Windows/TileView or something like that for better
    /// </summary>
    public class TilesetViewer : Window
    {

        Console TilesetConsole;
        Console PropertyConsole;

        public TilesetViewer( ) : base( Program.Width, Program.Height )
        {

            //TilesetConsole.DrawBox( new Rectangle( 0, 0, TilesetConsole.Width, TilesetConsole.Height ), new Cell( Color.Wheat, Color.Black, '%' ) );

            PropertyConsole = new Console( Width - 64, Height );
            PropertyConsole.Position = new Point( 64, 0 );
            //TilesetConsole.FillWithRandomGarbage();
            PropertyConsole.DrawBox( new Rectangle( 0, 0, PropertyConsole.Width, PropertyConsole.Height ), new Cell( Color.Wheat, Color.Black, '#' ));
            Children.Add( PropertyConsole );

            LoadTileset("unscii_16_ext.font");
        }

        public void LoadTileset( string path, int tileWidth = 16, int tileHeight = 8 )
        {
            FontMaster fontMaster = SadConsole.Global.LoadFont( "unscii_16_ext.font" );
            fontMaster.Columns = 64;
            //fontMaster.Rows = 35;

            TilesetConsole = new Console( 64, 70 );
            TilesetConsole.Font = fontMaster.GetFont( Font.FontSizes.Two );
            TilesetConsole.ViewPort = new Rectangle( 0, 0, Width, Height );
            Children.Add( TilesetConsole );

            for( int x = 0; x < fontMaster.Columns; x++ )
            {
                for( int y = 0; y < fontMaster.Rows; y++ )
                {
                    int index = GetIndexFromPoint( x, y );
                    TilesetConsole.Cells[index].Glyph = index;
                }
            }
        }
        

    }
}
