using System;
using SadConsole;
using Console = SadConsole.Console;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SCGames.Tetris;
using SCGames.Common;
using SCGames.Snake;

namespace SCGames
{
    class Program
    {

        public const int Width = 60;
        public const int Height = 40;

        public static MainMenu MainMenu;
        public static TetrisWindow TetrisWindow;
        public static SnakeWindow SnakeWindow;

        static void Main(string[] args)
        {

            
            // Setup the engine and creat the main window.
            SadConsole.Game.Create("cp437_10_ext.font", Width*2, Height*2);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;

            // Hook the update event that happens each frame so we can trap keys and respond.
            SadConsole.Game.OnUpdate = Update;
                        
            // Start the game.
            SadConsole.Game.Instance.Run();

            //
            // Code here will not run until the game window closes.
            //
            
            SadConsole.Game.Instance.Dispose();
        }
        
        private static void Update(GameTime time)
        {
            // Called each logic update.

            // As an example, we'll use the F5 key to make the game full screen
            if (SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.F5))
            {
                SadConsole.Settings.ToggleFullScreen();
            }
        }

        private static void Init()
        {
            // Load a double sized font
            FontMaster fontMaster = SadConsole.Global.LoadFont( "cp437_10_ext.font" );
            Global.FontDefault = fontMaster.GetFont( Font.FontSizes.Two );

            MainMenu = new MainMenu( 60, 40 );
            MainMenu.Show();

            // Create
            TetrisWindow = new TetrisWindow();
            SnakeWindow = new SnakeWindow();


        }
    }
}
