using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Entities;

namespace SCGames.Snake
{
    public class Target : Entity
    {
        /// <summary>
        /// Class to represent edible things
        /// </summary>
        public Target( ) : base( 1, 1 )
        {
            // TODO: Add a bit variety to glyph and color
            Animation.CurrentFrame[0].Glyph = 20;
            Animation.CurrentFrame[0].Foreground = Color.Orange;
            Animation.CurrentFrame[0].Background = Color.Transparent;
        }
    }
}
