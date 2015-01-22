using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TankArena.TankBot
{
    // Object contains enemy bot information
    public class Enemy
    {
        public int ID { get; set; }
        public Position Position { get; set; }

        // HP of enemy bot in case of usage in InGameInfo.Targets
        // Damage obtained from enemy bot in case of usage in InGameInfo.Attackers
        public int HitPoints { get; set; }
    }
}
