using Microsoft.Xna.Framework;
using SadConsole;
using SCGames.Common.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCGames.Common.Extentions;
using SadConsole.Controls;
using SCGames.ThemeEditor;
using SadConsole.Themes;

namespace SCGames.Editor
{
    public class ThemeEditorWindow : Window
    {

        SampleConsole SampleConsole;

        ListBox SampleThemesList;

        int PropertyX = 53;

        ColorScheme Dark = ColorScheme.Dark;
        ColorScheme Light;

        ColorScheme _current;

        public ThemeEditorWindow( int width, int height ) : base( width, height )
        {

            Light = new ColorScheme( new Color( 140, 140, 160 ), new Color( 20, 20, 30 ), new Color( 255, 180, 80 ) );
            _current = Light;
            

            Title = "Theme Editor";

            SampleConsole = new SampleConsole();
            SampleConsole.Position = new Point( 2, 2 );
            Children.Add( SampleConsole );
            SampleConsole.IsVisible = true;

            SampleThemesList = new SadConsole.Controls.ListBox( 10, 4 );
            SampleThemesList.Position = new Point( PropertyX, 3 );
            SampleThemesList.HideBorder = false;
            SampleThemesList.Items.Add( "Dark" );
            SampleThemesList.Items.Add( "Light" );
            Add( SampleThemesList );

            SampleThemesList.SelectedItemChanged += ( s, a ) =>
            {
                if( SampleThemesList.SelectedIndex == 0 )
                {
                    _current = Dark;
                    _current.Library.Apply( SampleConsole );
                    //WriteColorProperties();
                }
                else
                {
                    _current = Light;
                    _current.Library.Apply( SampleConsole );
                    //WriteColorProperties();
                }
            };

            #region radios
            /*
            var radioButton = new RadioButton( 20, 1 );
            radioButton.Text = "Normal";
            radioButton.Position = new Point( PropertyX, 12 );
            Add( radioButton );
            radioButton.IsSelectedChanged += ( btn, args ) =>
            {
            };

            radioButton = new RadioButton( 20, 1 );
            radioButton.Text = "Normal With Ends";
            radioButton.Position = new Point( PropertyX, 13 );
            Add( radioButton );
            radioButton.IsSelectedChanged += ( btn, args ) =>
            {
                //Current.ButtonStyle = LibraryExtended.ButtonStyles.NormalWithEnds;
                //Current.Apply( SampleConsole );
            };

            radioButton = new RadioButton( 20, 1 );
            radioButton.Text = "Thin Lines";
            radioButton.Position = new Point( PropertyX, 14 );
            Add( radioButton );
            radioButton.IsSelected = true;
            radioButton.IsSelectedChanged += ( btn, args ) =>
            {
                //Current.ButtonStyle = LibraryExtended.ButtonStyles.Thinlines;
                //Current.Apply( SampleConsole );
            };

            Current.Apply( this );
            */
            #endregion

            _current.Library.Apply( SampleConsole );
            //WriteColorProperties();

        }

        //private void WriteColorProperties()
        //{
        //    int y = 10;
        //    int fg = Width - 6;
        //    DefaultForeground = Color.White;
        //    Print( fg, y, "Fg", Color.White );
        //    Print( PropertyX, y++, "Color", Color.White );

        //    Print( PropertyX, y, "Accent", Color.White );
        //    Cells[GetIndexFromPoint( new Point( fg, y++ ) )].Foreground = _current.Accent;

        //    Print( PropertyX, y, "ControlBack", Color.White );
        //    Cells[GetIndexFromPoint( new Point( fg, y++ ) )].Foreground = _current.ControlBack;

        //    Print( PropertyX, y, "ControlBackLight", Color.White );
        //    Cells[GetIndexFromPoint( new Point( fg, y++ ) )].Foreground = _current.ControlBackLight;

        //    Print( PropertyX, y, "ControlBackSelected", Color.White );
        //    Cells[GetIndexFromPoint( new Point( fg, y++ ) )].Foreground = _current.ControlBackSelected;

        //    Print( PropertyX, y, "ControlHostBack", Color.White );
        //    Cells[GetIndexFromPoint( new Point( fg, y++ ) )].Foreground = _current.ControlHostBack;

        //    Print( PropertyX, y, "ControlHostFore", Color.White );
        //    Cells[GetIndexFromPoint( new Point( fg, y++ ) )].Foreground = _current.ControlHostFore;

        //    Print( PropertyX, y, "Text", Color.White );
        //    Cells[GetIndexFromPoint( new Point( fg, y++ ) )].Foreground = _current.Text;

        //    Print( PropertyX, y, "TextBright", Color.White );
        //    Cells[GetIndexFromPoint( new Point( fg, y++ ) )].Foreground = _current.TextBright;

        //    Print( PropertyX, y, "TextDark", Color.White );
        //    Cells[GetIndexFromPoint( new Point( fg, y++ ) )].Foreground = _current.TextDark;

        //    Print( PropertyX, y, "TextLight", Color.White );
        //    Cells[GetIndexFromPoint( new Point( fg, y++ ) )].Foreground = _current.TextLight;

        //    Print( PropertyX, y, "TextSelected", Color.White );
        //    Cells[GetIndexFromPoint( new Point( fg, y++ ) )].Foreground = _current.TextSelected;

        //    Print( PropertyX, y, "TextSelectedDark", Color.White );
        //    Cells[GetIndexFromPoint( new Point( fg, y++ ) )].Foreground = _current.TextSelectedDark;

        //    Print( PropertyX, y, "MenuBack", Color.White );
        //    Cells[GetIndexFromPoint( new Point( fg, y++ ) )].Foreground = _current.MenuBack;

        //    Print( PropertyX, y, "MenuLines", Color.White );
        //    Cells[GetIndexFromPoint( new Point( fg, y++ ) )].Foreground = _current.MenuLines;

        //    Print( PropertyX, y, "TitleText", Color.White );
        //    Cells[GetIndexFromPoint( new Point( fg, y++ ) )].Foreground = _current.TitleText;

        //    Print( PropertyX, y, "ModalBackground", Color.White );
        //    Cells[GetIndexFromPoint( new Point( fg, y++ ) )].Foreground = _current.ModalBackground;
        //}

        private void SampleThemesList_SelectedItemChanged( object sender, ListBox.SelectedItemEventArgs e )
        {
            throw new NotImplementedException();
        }
    }
}
