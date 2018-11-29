﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoRogue;
using Microsoft.Xna.Framework;

namespace SCGames.WinterWar.Maps
{
    public static class GoRogueConversion
    {

        public static Point ToPoint( this Coord position ) => new Point( position.X, position.Y );

        public static Coord ToCoord( this Point position ) => Coord.Get( position.X, position.Y );

    }
}
