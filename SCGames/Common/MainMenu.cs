using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Surfaces;
using SadConsole.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCGames.Common.Extentions;
using SadConsole.Controls;
using SCGames.Common.Themes;
using SCGames.Common.Controls;

namespace SCGames.Common
{
    public class MainMenu : Window
    {

        public MainMenu( int Width, int Height ) : base( Width, Height )
        {

            FontMaster fontMaster = SadConsole.Global.LoadFont( "cp437_10_ext.font" );
            Font titleFont = fontMaster.GetFont( Font.FontSizes.Four );

            Basic title = new SadConsole.Surfaces.Basic( 20, 1 );
            title.Font = titleFont;
            title.Position = new Point( Width / 4 - "Main Menu".Length / 2, 2 );
            title.Print( 0, 0, "Main Menu" );

            Children.Add( title );

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
            snakeButton.Position = new Point( Width / 2 - snakeButton.Width / 2, 14 );
            snakeButton.TextAlignment = HorizontalAlignment.Center;
            snakeButton.Text = "Snake";
            snakeButton.Theme = new SadConsole.Themes.ButtonLinesTheme();

            Add( snakeButton );

            snakeButton.Click += ( btn, args ) =>
            {
                Program.MainMenu.Hide();
                Program.SnakeWindow.Show();
            };

            var themeButton = new SadConsole.Controls.Button( 11, 3 );
            themeButton.Position = new Point( Width / 2 - snakeButton.Width / 2, 18 );
            themeButton.TextAlignment = HorizontalAlignment.Center;
            themeButton.Text = "Theme";
            themeButton.Theme = new SadConsole.Themes.ButtonLinesTheme();

            Add( themeButton );

            themeButton.Click += ( btn, args ) =>
            {
                Program.MainMenu.Hide();
                Program.ThemeEditor.Show();
            };

            var viewerButton = new SadConsole.Controls.Button( 11, 3 );
            viewerButton.Position = new Point( Width / 2 - snakeButton.Width / 2, 22 );
            viewerButton.TextAlignment = HorizontalAlignment.Center;
            viewerButton.Text = "Viewer";
            viewerButton.Theme = new SadConsole.Themes.ButtonLinesTheme();

            Add( viewerButton );

            viewerButton.Click += ( btn, args ) =>
            {
                Program.MainMenu.Hide();
                Program.TilesetViewer.Show();
            };

            var mapViewButton = new SadConsole.Controls.Button( 11, 3 );
            mapViewButton.Position = new Point( Width / 2 - snakeButton.Width / 2, 26 );
            mapViewButton.TextAlignment = HorizontalAlignment.Center;
            mapViewButton.Text = "MapView";
            mapViewButton.Theme = new SadConsole.Themes.ButtonLinesTheme();

            Add( mapViewButton );

            mapViewButton.Click += ( btn, args ) =>
            {
                Program.MainMenu.Hide();
                //Children.Add( Program.MapView );
                //Global.CurrentScreen = Program.MapView;
                Program.MapView.Show();
                Global.FocusedConsoles.Set( Program.MapView );
            };

            var themeRadioButton = new RadioButton( 10, 1 );
            themeRadioButton.Text = "Light";
            themeRadioButton.Position = new Point( 60, 2 );
            themeRadioButton.GroupName = "Theme";
            Add( themeRadioButton );
            themeRadioButton.IsSelectedChanged += OnThemeRadioChanged;

            themeRadioButton = new RadioButton( 10, 1 );
            themeRadioButton.Text = "Dark";
            themeRadioButton.Position = new Point( 60, 4 );
            themeRadioButton.GroupName = "Theme";
            Add( themeRadioButton );
            themeRadioButton.IsSelectedChanged += OnThemeRadioChanged;

        }

        private void OnThemeRadioChanged( object btn, EventArgs args )
        {
            RadioButton b = ( RadioButton ) btn;
            if( b.Text == "Light" && b.IsSelected == true )
            {
                Program.LightTheme.Apply( this );
                System.Console.WriteLine( "TO LIGHT" );
            }
            if( b.Text == "Dark" && b.IsSelected == true )
            {
                Program.DarkTheme.Apply( this );
                System.Console.WriteLine( "TO DARK" );

            }
        }

    }
}
