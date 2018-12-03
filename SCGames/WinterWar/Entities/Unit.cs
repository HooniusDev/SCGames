using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SadConsole;
using SadConsole.Entities;
using SadConsole.Input;
using SCGames.WinterWar.Cells;
using SCGames.WinterWar.Entities;
using System;
using System.Collections.Generic;
using Console = SadConsole.Console;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadConsole.Maps;
using SCGames.WinterWar.Maps;
using GoRogue;
using SadConsole.Effects;
using SadConsole.Surfaces;

namespace SCGames.WinterWar.Entities
{
    public class Unit : Entity
    {

        public bool PlayerControlled { get; private set; }

        /// <summary>
        /// Z value of FOV calcules from
        /// Varies by stance
        /// </summary>
        public float EyeHeight
        {
            get
            {
                float height = Z;
                switch( Stance )
                {
                    case ( UnitStance.Prone ):
                        height += .2f;
                        break;
                    case ( UnitStance.Crouch ):
                        height += 1f;
                        break;
                    case ( UnitStance.Standing ):
                        height += 1.8f;
                        break;
                }
                return height;
            }
        }
        public float ViewRadius = 50;

        public float Z;

        public UnitStance Stance;

        public Unit( Animated anim,  bool isPlayer ) : base( anim )
        {
            PlayerControlled = isPlayer;

        }
    }

    public enum UnitStance
    {
        Prone, // Lying down
        Crouch, // Kneeling height
        Standing,
    }
}
