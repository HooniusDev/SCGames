using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SCGames.Common.Themes;
using SadConsole.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCGames.Common.Controls;

namespace SCGames.ThemeEditor
{
    public class SampleConsole : Window
    {

        public Button SampleButton { get; private set; }

        public SampleConsole( ) : base( 50, 36 )
        {

            Title = "Preview";
            Global.FocusedConsoles.Push( this );

            var panel = new Panel( 20, 2 )
            {
                Fake3D = false
            };
            panel.DrawPanel();
            panel.Position = new Point( 1, 24 );
            panel.AddLine( "This is a Panel." );

            Children.Add( panel );

            var button = new SadConsole.Controls.Button( 11, 3 )
            {
                Text = "Click",
                Position = new Point( 1, 6 ),
                Theme = new Button3dTheme()
            };
            Add( button );

            button = new SadConsole.Controls.Button( 11, 3 )
            {
                Text = "Click",
                Position = new Point( 1, 10 ),
                Theme = new ButtonLinesTheme()
            };
            Add( button );

            var prog1 = new ProgressBar( 10, 1, HorizontalAlignment.Left );
            prog1.Position = new Point( 16, 5 );
            Add( prog1 );

            var prog2 = new ProgressBar( 1, 6, VerticalAlignment.Bottom );
            prog2.Position = new Point( 18, 7 );
            Add( prog2 );

            var slider = SadConsole.Controls.ScrollBar.Create( Orientation.Horizontal, 10 );
            slider.Position = new Point( 16, 3 );
            slider.Maximum = 18;
            Add( slider );

            slider = SadConsole.Controls.ScrollBar.Create( Orientation.Vertical, 6 );
            slider.Position = new Point( 16, 7 );
            slider.Maximum = 6;
            Add( slider );

            var listbox = new SadConsole.Controls.ListBox( 20, 6 );
            listbox.Position = new Point( 28, 3 );
            listbox.HideBorder = false;
            listbox.Items.Add( "item 1" );
            listbox.Items.Add( "item 2" );
            listbox.Items.Add( "item 3" );
            listbox.Items.Add( "item 4" );
            listbox.Items.Add( "item 5" );
            listbox.Items.Add( "item 6" );
            listbox.Items.Add( "item 7" );
            listbox.Items.Add( "item 8" );
            Add( listbox );

            var radioButton = new RadioButton( 20, 1 );
            radioButton.Text = "Group 1 Option 1";
            radioButton.Position = new Point( 28, 12 );
            Add( radioButton );

            radioButton = new RadioButton( 20, 1 );
            radioButton.Text = "Group 1 Option 2";
            radioButton.Position = new Point( 28, 13 );
            Add( radioButton );

            var selButton = new SadConsole.Controls.SelectionButton( 24, 1 );
            selButton.Text = "Selection Button 1";
            selButton.Position = new Point( 1, 15 );
            Add( selButton );

            var selButton1 = new SadConsole.Controls.SelectionButton( 24, 1 );
            selButton1.Text = "Selection Button 2";
            selButton1.Position = new Point( 1, 16 );
            Add( selButton1 );

            var selButton2 = new SadConsole.Controls.SelectionButton( 24, 1 );
            selButton2.Text = "Selection Button 3";
            selButton2.Position = new Point( 1, 17 );
            Add( selButton2 );

            selButton.PreviousSelection = selButton2;
            selButton.NextSelection = selButton1;
            selButton1.PreviousSelection = selButton;
            selButton1.NextSelection = selButton2;
            selButton2.PreviousSelection = selButton1;
            selButton2.NextSelection = selButton;

            var input = new TextBox( 20 );
            input.Position = new Point( 1, 20 );
            Add( input );

            var checkbox = new SadConsole.Controls.CheckBox( 13, 1 )
            {
                Text = "Check box",
                Position = new Point( 24, 20 )
            };
            Add( checkbox );

        }


    }
}
