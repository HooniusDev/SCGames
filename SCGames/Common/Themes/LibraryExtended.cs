using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Surfaces;
using SadConsole.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCGames.Common.Themes
{
    public class LibraryExtended : Library
    {

        public enum ButtonStyles
        {
            Normal,
            NormalWithEnds,
            Thinlines,
        }
        private ButtonStyles _buttonStyle = ButtonStyles.Thinlines;
        public ButtonStyles ButtonStyle
        {
            get { return _buttonStyle; }
            set
            {
                if( _buttonStyle != value )
                {
                    _buttonStyle = value;
                    CreateButtonTheme();
                }
            }
        }

        public Color ControlBack = new Color( 12, 12, 36 );
        public Color ControlBackDark = new Color( 12, 12, 24 );
        public Color ControlBackLight = new Color( 12, 12, 48 );
        public Color ControlBackSelected = new Color( 22, 22, 68 );
        public Color ControlHostBack = new Color( 12, 12, 36 );
        public Color ControlHostFore = new Color( 100, 60, 10 );

        public Color Text = new Color( 230, 124, 14 );
        public Color TextBright = new Color( 250, 134, 10 );
        public Color TextDark = new Color( 200, 114, 22 );
        public Color TextLight = new Color( 250, 134, 10 );
        public Color TextSelected = new Color( 12, 12, 48 );
        public Color TextSelectedDark = new Color( 200, 114, 22 );

        public Color TitleText = new Color( 250, 134, 10 );

        public LibraryExtended( ) : base()
        {

        }

        public void Init( )
        {
            Appearance_ControlNormal = new Cell( Text, ControlBack );
            Appearance_ControlDisabled = new Cell( TextDark, ControlBackDark );
            Appearance_ControlOver = new Cell( TextBright, ControlBackSelected );
            Appearance_ControlSelected = new Cell( TextSelected, ControlBackSelected );
            Appearance_ControlMouseDown = new Cell( TextSelected, ControlBackDark );
            Appearance_ControlFocused = new Cell( TextBright, ControlBackLight );

            ControlsConsoleTheme = new ControlsConsoleTheme();

            CreateWindowTheme();
            CreateButtonTheme();
            CreateRadioButtonTheme();
            CreateCheckBoxTheme();
            CreateListBoxTheme();
        }

        protected void CreateListBoxTheme( )
        {
            ListBoxTheme = new ListBoxTheme()
            {
                Normal = new Cell( TextDark, ( ControlBackLight * 1.3f ).FillAlpha() ),
                BorderLineStyle = ( int[] ) SurfaceBase.ConnectedLineThick.Clone(),
                BorderTheme = new ThemeStates
                {
                    Normal = Appearance_ControlNormal,
                }
            };
        }

        protected void CreateCheckBoxTheme( )
        {
            CheckBoxTheme = new CheckBoxTheme();
        }

        protected void CreateRadioButtonTheme( )
        {
            RadioButtonTheme = new RadioButtonTheme();
            RadioButtonTheme.SetBackground( Color.Transparent );
            RadioButtonTheme.SetForeground( Text );
            RadioButtonTheme.CheckedIcon = new ThemeStates();
            RadioButtonTheme.CheckedIcon.SetGlyph( '*' );
            RadioButtonTheme.CheckedIcon.SetBackground( Color.Transparent );
            RadioButtonTheme.CheckedIcon.SetForeground( Text );

            RadioButtonTheme.LeftBracket.SetBackground( Color.Transparent );
            RadioButtonTheme.LeftBracket.SetForeground( Text );

            RadioButtonTheme.RightBracket.SetBackground( Color.Transparent );
            RadioButtonTheme.RightBracket.SetForeground( Text );
            RadioButtonTheme.UncheckedIcon.SetBackground( Color.Transparent );
        }
        protected void CreateButtonTheme( )
        {

            switch( ButtonStyle )
            {
                case ButtonStyles.Thinlines:
                    ButtonTheme = new ButtonLinesTheme
                    {
                        BottomRightLineColors = Appearance_ControlDisabled,
                        TopLeftLineColors = Appearance_ControlFocused,
                        Normal = new Cell( TextDark, ( ControlBackLight * 1.3f ).FillAlpha() ),
                        Disabled = Appearance_ControlDisabled,
                        MouseOver = new Cell( TextBright, ( ControlBackLight * 1.6f ).FillAlpha() ),
                        Focused = new Cell( TextDark, ( ControlBackLight * 1.3f ).FillAlpha() ),
                        MouseDown = Appearance_ControlMouseDown,
                    };
                    break;
                case ButtonStyles.Normal:
                    ButtonTheme = new ButtonTheme
                    {
                        Normal = new Cell( TextDark, (ControlBackLight * 1.3f).FillAlpha() ),
                        Disabled = Appearance_ControlDisabled,
                        MouseOver = new Cell( TextBright, ( ControlBackLight * 1.6f ).FillAlpha() ),
                        Focused = new Cell( TextDark, ( ControlBackLight * 1.3f ).FillAlpha() ),
                        MouseDown = Appearance_ControlMouseDown,
                        ShowEnds = false,
                    };
                    break;
                default:
                    ButtonTheme = new ButtonTheme
                    {
                        Normal = new Cell( TextDark, ( ControlBackLight * 1.3f ).FillAlpha() ),
                        Disabled = Appearance_ControlDisabled,
                        MouseOver = new Cell( TextBright, ( ControlBackLight * 1.6f ).FillAlpha() ),
                        Focused = new Cell( TextDark, ( ControlBackLight * 1.3f ).FillAlpha() ),
                        MouseDown = Appearance_ControlMouseDown,
                        ShowEnds = true,
                    };
                    break;

            }

        }

        protected void CreateWindowTheme( )
        {
            WindowTheme = new WindowTheme();
            WindowTheme.BorderStyle.Background = ControlBack;
            WindowTheme.BorderStyle.Foreground = TextDark;
            WindowTheme.TitleStyle = new Cell( TitleText );
            WindowTheme.FillStyle = Appearance_ControlNormal;
        }
    }
}
