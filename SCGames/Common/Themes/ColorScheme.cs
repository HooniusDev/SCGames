using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Themes;
using SCGames.Common.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCGames.Common.Themes
{
    class ColorScheme
    {

        public static ColorScheme Dark 
        { get
            {
                return new ColorScheme( new Color( 36, 36, 48 ), new Color( 223, 228, 234 ), new Color( 255, 180, 80 ) );
            }
        }

        public Color Accent { get; set; }
        public Color Primary { get; set; }
        public Color Neutrals { get; set; }

        public Color PanelLight { get; set; }
        public Color PanelNormal { get; set; }
        public Color PanelDark { get; set; }

        public Color TextAlternative { get; set; }
        public Color TextNormal { get; set; }

        public Color TrimLight { get; set; }
        public Color TrimNormal { get; set; }
        public Color TrimDark { get; set; }

        public Library Library { get; private set; }

       /// <summary>
       /// Creates a new ColorScheme
       /// </summary>
       /// <param name="primary">Color of Background for Consoles and Controls</param>
       /// <param name="neutrals">Color of Text and Corders</param>
        public ColorScheme( Color primary, Color neutrals, Color? accents = null )
        {

            Primary = primary;
            Neutrals = neutrals;
            Accent = accents ?? new Color( 200, 114, 22 );

            PanelNormal = primary;
            System.Console.WriteLine( "Brightness of Panel: " + PanelNormal.GetBrightness().ToString() );
            if( PanelNormal.GetBrightness() > .5f )
            {
                PanelLight = LightenColor( PanelNormal, .1f );
                PanelDark = DarkenColor( PanelNormal, .2f );
            }
            else
            {
                PanelLight = LightenColor( PanelNormal, .4f );
                PanelDark = DarkenColor( PanelNormal, .9f );
            }
            PanelDark = DarkenColor( PanelNormal );

            TextNormal = neutrals;
 
            TextAlternative = Color.Lerp( TextNormal, Accent, .5f );


            ToLibrary();

            //ModalBackground = new Color( 20, 20, 20 );
        }

        public void ToLibrary( )
        {

            Library.Default.Appearance_ControlNormal = new Cell( TextNormal, Color.Transparent );
            Library.Default.Appearance_ControlDisabled = new Cell( TextAlternative, PanelDark );
            Library.Default.Appearance_ControlOver = new Cell( TextAlternative, Color.Transparent );
            Library.Default.Appearance_ControlSelected = new Cell( Accent, PanelLight );
            Library.Default.Appearance_ControlMouseDown = new Cell( TextAlternative, PanelDark );
            Library.Default.Appearance_ControlFocused = new Cell( TextAlternative, Color.Transparent );

            Colors.ControlHostBack = PanelNormal;
            Colors.ControlHostFore = TextNormal;

            Library = new Library();

            Library.ControlsConsoleTheme = new ControlsConsoleTheme();
            Library.WindowTheme = new WindowTheme();

            Library.ScrollBarTheme = new ScrollBarTheme();
            //Library.Default.ScrollBarTheme = Library.ScrollBarTheme;
            Library.ButtonTheme = new ButtonLinesTheme();
            Library.CheckBoxTheme = new CheckBoxTheme();
            Library.ListBoxTheme = new ListBoxTheme();
            //Library.ListBoxTheme.ScrollBarTheme = Library.Default.ScrollBarTheme;
            Library.ProgressBarTheme = new ProgressBarTheme();
            Library.RadioButtonTheme = new RadioButtonTheme();
            Library.TextBoxTheme = new TextBoxTheme();
            Library.SelectionButtonTheme = new ButtonTheme();

            //Library.ControlsConsoleTheme = new ControlsConsoleTheme();
            //Library.ControlsConsoleTheme.FillStyle = new Cell( TextNormal, PanelNormal );

            //Library.WindowTheme = new WindowTheme();
            //Library.WindowTheme.FillStyle = new Cell( TextNormal, PanelNormal );
            //Library.WindowTheme.BorderStyle = new Cell( TextAlternative, PanelNormal );
            //Library.WindowTheme.TitleStyle = new Cell( Accent, PanelNormal );

            //Library.ScrollBarTheme = new ScrollBarTheme();
            //Library.ButtonTheme = new ButtonLinesTheme();

            //Library.CheckBoxTheme = new CheckBoxTheme();
            //Library.CheckBoxTheme.SetForeground( TextNormal );
            //Library.CheckBoxTheme.CheckedIcon.SetForeground( Accent );
            //Library.CheckBoxTheme.LeftBracket.SetForeground( TextNormal );
            //Library.CheckBoxTheme.RightBracket.SetForeground( TextNormal );

            //Library.ListBoxTheme = new ListBoxTheme();
            //Library.ListBoxTheme.ItemTheme.SetBackground( PanelDark );
            //Library.ListBoxTheme.BorderTheme.SetForeground( TextAlternative );
            //Library.ListBoxTheme.ItemTheme.Selected = new Cell( Accent, PanelLight );
            //Library.ListBoxTheme.ItemTheme.MouseOver = new Cell( TextAlternative, Color.Transparent );

            //Library.ProgressBarTheme = new ProgressBarTheme();

            //Library.RadioButtonTheme = new RadioButtonTheme(); 
            //Library.RadioButtonTheme.CheckedIcon.SetForeground( Accent );
            //Library.RadioButtonTheme.MouseOver = new Cell( TextAlternative, Color.Transparent );

            //Library.TextBoxTheme = new TextBoxTheme();
            //Library.TextBoxTheme.SetForeground( TextNormal );

            //Library.SelectionButtonTheme = new ButtonTheme();
            //Library.SelectionButtonTheme.SetForeground( TextNormal );
            //Library.SelectionButtonTheme.ShowEnds = false;
            //Library.SelectionButtonTheme.Normal = new Cell( TextNormal, Color.Transparent );
            //Library.SelectionButtonTheme.MouseOver = new Cell( TextNormal, PanelLight );
            //Library.SelectionButtonTheme.Selected = new Cell( Accent, Color.Transparent );

        }

        public void Apply( ControlsConsole console )
        {
            #region Colors OVERRIDES
            /* Colors overrides
                //Colors.MenuBack = MenuBack;
                //Colors.MenuLines = MenuLines;
                //Colors.TitleText = TitleText;
                //Colors.ModalBackground = ModalBackground;

                //Colors.ControlBack = Color.Transparent;
                //Colors.ControlBackDark = Color.Transparent;
                //Colors.ControlBackLight = Color.Transparent;
                //Colors.ControlBackSelected = ControlBackSelected;
                //Colors.ControlHostBack = ControlHostBack;
                //Colors.ControlHostFore = ControlHostFore;

                //Colors.Text = Text;
                //Colors.TextBright = TextBright;
                //Colors.TextDark = TextDark;
                //Colors.TextLight = TextLight;
                //Colors.TextSelected = TextSelected;
                //Colors.TextSelectedDark = TextSelectedDark;
            */
            #endregion

            //Library.Default.Appearance_ControlNormal = new Cell( Text, Color.Transparent );
            //Library.Default.Appearance_ControlDisabled = new Cell( TextDark, ControlBackDark );
            //Library.Default.Appearance_ControlOver = new Cell( TextBright, ControlBackSelected );
            //Library.Default.Appearance_ControlSelected = new Cell( TextSelected, ControlBackSelected );
            //Library.Default.Appearance_ControlMouseDown = new Cell( TextSelected, ControlBackDark );
            //Library.Default.Appearance_ControlFocused = new Cell( TextBright, ControlBackLight );

            Library.Default.ControlsConsoleTheme = new ControlsConsoleTheme();
            Library.Default.WindowTheme = new WindowTheme();

            Library.Default.ScrollBarTheme = new ScrollBarTheme();
            Library.Default.ButtonTheme = new ButtonTheme();
            Library.Default.CheckBoxTheme = new CheckBoxTheme();
            Library.Default.ListBoxTheme = new ListBoxTheme();
            Library.Default.ProgressBarTheme = new ProgressBarTheme();
            Library.Default.RadioButtonTheme = new RadioButtonTheme();
            Library.Default.TextBoxTheme = new TextBoxTheme();
            Library.Default.SelectionButtonTheme = new ButtonTheme();

            //Library.Default.ListBoxTheme.ItemTheme.Selected = new Cell( ControlBackSelected, TextLight );

            //Library.Default.WindowTheme.FillStyle.Background = MenuBack;


            //Library.Default.Apply( console );
        }

        private Color LightenColor( Color color, float amount = .2f )
        {
            return (color * (1+amount)).FillAlpha();
        }

        private Color DarkenColor( Color color, float amount = .2f )
        {
            return ( color * (1-amount) ).FillAlpha();
        }
    }
}
