using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TankArena.TankBot;

namespace TankArena.GameArena.LoggerXML
{
    public class GameHeaderLog
    {
        public int GameFieldWidth { get; set; }
        public int GameFieldHeight { get; set; }
        public List<Position> HPPoints { get; set; }
        public List<Position> AmmoPoints { get; set; }
        public int BotsCount { get; set; }
        public int RoundsCount { get; set; }
        public bool HasWinner { get; set; }
        public string WinnerBotName { get; set; }
        public string WinnerBotTypeName { get; set; }

        public GameHeaderLog()
        { 
        }

        public GameHeaderLog(int gameFieldWidth, int gameFieldHeight, List<Position> hpPoints, List<Position> ammoPoints)
        {
            GameFieldWidth = gameFieldWidth;
            GameFieldHeight = gameFieldHeight;
            HPPoints = hpPoints;
            AmmoPoints = ammoPoints;
        }
    }
}
