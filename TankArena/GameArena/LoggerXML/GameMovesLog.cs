using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TankArena.TankBot;

namespace TankArena.GameArena.LoggerXML
{
    public class GameMovesLog
    {
        public List<BotInfoLog> BotInfoList;

        public GameMovesLog()
        { 
        }

        public GameMovesLog(List<BotDetailsHandler> botsInGameDetails)
        {
            BotInfoList = new List<BotInfoLog>();
            BotInfoLog botInfo;
            int botID = 0;

            foreach (BotDetailsHandler botDetils in botsInGameDetails)
            {
                botInfo = new BotInfoLog();
                botInfo.ID = botID++;
                botInfo.Name = botDetils.Name;
                botInfo.TypeName = botDetils.TypeName;
                botInfo.Move = botDetils.LastMove.Move;
                botInfo.HitPoints = botDetils.HitPoints;
                botInfo.AmmoCount = botDetils.AmmoCount;
                botInfo.BotDirection = botDetils.BotDirection;
                botInfo.X = botDetils.BotPosition.X;
                botInfo.Y = botDetils.BotPosition.Y;
                if (botDetils.LastMove.TargetPosition != null)
                {
                    botInfo.TargetX = botDetils.LastMove.TargetPosition.X;
                    botInfo.TargetY = botDetils.LastMove.TargetPosition.Y;
                }
                BotInfoList.Add(botInfo);
            }
        }
    }
    
    public class BotInfoLog
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public BotMove Move { get; set; }
        public int HitPoints { get; set; }
        public int AmmoCount { get; set; }
        public Direction BotDirection { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int TargetX { get; set; }
        public int TargetY { get; set; }
    }
}
