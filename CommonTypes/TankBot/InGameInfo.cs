using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TankArena.TankBot
{
    // Specifies current move game situation
    public class InGameInfo
    {
        // Your bot health
        public int HitPoints { get; set; }
        // List of bots found while scan
        public List<Enemy> Targets { get; set; }
        // List of bots attacked you by prev step
        public List<Enemy> Attackers { get; set; }

        // Lists above cannot be null
        public InGameInfo()
        {
            HitPoints = 0;
            Targets = new List<Enemy>();
            Attackers = new List<Enemy>();
        }
    }
}
