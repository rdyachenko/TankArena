using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TankArena.TankBot
{
    // Object contains initial game setting
    public class StartGameInfo
    {
        // Your bot initial health & ammo count
        public int HitPoints { get; set; }
        public int AmmoCount { get; set; }

        // Your bot visibility, attrack, scan and splash damage range
        public int VisibilityRange { get; set; }
        public int AttrackRange { get; set; }
        public int ScanRange { get; set; }
        public int SplashRange { get; set; }

        // Your bot initial position and direction
        public Position StartPosition { get; set; }
        public Direction StartDirection { get; set; }

        // Battle field size
        public int ArenaWidth { get; set; }
        public int ArenaHeight { get; set; }

        // Special points position at battle field
        public List<Position> HPPoints { get; set; }
        public List<Position> AmmoPoints { get; set; }
    }

    // Hold x,y coordinates
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool Equals(Position position)
        {
            return ((position.X == X) && (position.Y == Y));
        }
    }

    // Bot direction
    public enum Direction : int
    {
        Up,
        Right,
        Down,
        Left
    }
}
