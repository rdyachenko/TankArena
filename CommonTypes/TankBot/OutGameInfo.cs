using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TankArena.TankBot
{
    // Hold your bot move setting
    public class OutGameInfo
    {
        public BotMove Move { get; set; }

        // Required in case of BotMove.Shoot
        public Position TargetPosition { get; set; }
    }

    // Available moves for your bot
    public enum BotMove : int
    { 
        TurnLeft,
        TurnRight,
        MoveAhead,
        Scan,
        Shoot,
        Skip
    }
}
