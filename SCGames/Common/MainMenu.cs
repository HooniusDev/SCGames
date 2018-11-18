using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Surfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCGames.Common
{
    public class MainMenu : Window
    {

        public MainMenu( int Width, int Height ) : base( Width, Height )
        {

            LoadTheme();


            FontMaster fontMaster = SadConsole.Global.LoadFont( "cp437_10_ext.font" );
            Font titleFont = fontMaster.GetFont( Font.FontSizes.Four );

            Basic title = new SadConsole.Surfaces.Basic( 20, 1 );
            title.Font = titleFont;
            title.Position = new Point( Width / 4 - "Main Menu".Length / 2, 2 );
            title.Print(0,0, "Main Menu" );
           
            Children.Add(title );

            var tetrisButton = new SadConsole.Controls.Button( 11, 3 );
            tetrisButton.Text = "Tetris";
            tetrisButton.TextAlignment = HorizontalAlignment.Center;
            tetrisButton.Position = new Point( Width / 2 - tetrisButton.Width / 2, 10 );
            tetrisButton.Theme = new SadConsole.Themes.ButtonLinesTheme();

            Add( tetrisButton );

            tetrisButton.Click += ( btn, args ) =>
            {
                Program.MainMenu.Hide();
                Program.TetrisWindow.Show();
            };

            var snakeButton = new SadConsole.Controls.Button( 11, 3 );
            snakeButton.Position = new Point( Width / 2 - snakeButton.Width / 2, 13 );
            snakeButton.TextAlignment = HorizontalAlignment.Center;
            snakeButton.Text = "Snake";
            snakeButton.Theme = new SadConsole.Themes.ButtonLinesTheme();

            Add( snakeButton );

            snakeButton.Click += ( btn, args ) =>
            {
                Program.MainMenu.Hide();
                Program.SnakeWindow.Show();
            };
        }

        private void LoadTheme( )
        {

            Color BackDDark = new Color( 12, 12, 24 );
            Color BackNormal = new Color( 12, 12, 24 );
            Color BackBright = new Color( 12, 12, 48 );

            Color TrimColor = new Color( 100, 60, 10 );
            Color TrimColorBright = new Color( 200, 114, 22 );

            Color TextDark = new Color( 200, 114, 22 );
            Color TextNormal = new Color( 230, 124, 14 );
            Color TextBright = new Color( 250, 134, 10 );

            Color TextSelected = BackBright;
            Color TextSelectedDark = TextDark;

            Cell Appearance_ControlMouseDown = new SadConsole.Cell( BackDDark, TextSelectedDark );
            Cell Appearance_ControlDisabled = new SadConsole.Cell( TextDark, BackDDark );

            SadConsole.Themes.Library.Default.ButtonTheme = new SadConsole.Themes.ButtonLinesTheme
            {
                BottomRightLineColors = new SadConsole.Cell( TrimColor, Color.Transparent ),
                TopLeftLineColors = new SadConsole.Cell( TrimColorBright, Color.Transparent ),
                Normal = new Cell( TextDark, Color.Transparent ),
                Disabled = Appearance_ControlDisabled,
                MouseOver = new Cell( TextBright, Color.Transparent ),
                Focused = new Cell( TextDark, Color.Transparent ),
                MouseDown = Appearance_ControlMouseDown,

            };

        }

    }
}
