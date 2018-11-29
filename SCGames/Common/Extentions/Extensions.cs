using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Surfaces;
using SadConsole.Themes;
using SCGames.Common.Themes;

namespace SCGames.Common.Extentions
{
    public static class Extensions
    {
        public static void Apply( this Library library, ControlsConsole controlsHost )
        {
            controlsHost.Theme = library.ControlsConsoleTheme;

            if( controlsHost is Window window )
                window.Theme = library.WindowTheme;

            foreach( var c in controlsHost.Controls )
            {
                switch( c )
                {
                    case SelectionButton control:
                        control.Theme = library.SelectionButtonTheme;
                        break;

                    case Button control:
                        control.Theme = library.ButtonTheme;
                        break;

                    case ScrollBar control:
                        control.Theme = library.ScrollBarTheme;
                        break;

                    case RadioButton control:
                        control.Theme = library.RadioButtonTheme;
                        break;

                    case ListBox control:
                        control.Theme = library.ListBoxTheme;
                        control.Slider.Theme = library.ScrollBarTheme;
                        break;

                    case CheckBox control:
                        control.Theme = library.CheckBoxTheme;
                        break;

                    case TextBox control:
                        control.Theme = library.TextBoxTheme;
                        break;

                    case ProgressBar control:
                        control.Theme = library.ProgressBarTheme;
                        break;
                }
            }
        }
    }
}