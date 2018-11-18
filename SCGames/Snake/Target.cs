using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Entities;

namespace SCGames.Snake
{
    public class Target : Entity
    {
        public Target( ) : base( 1, 1 )
        {
            Animation.CurrentFrame[0].Glyph = 227;
            Animation.CurrentFrame[0].Foreground = Color.Aqua;
            Animation.CurrentFrame[0].Background = Color.Transparent;
        }
    }
}
