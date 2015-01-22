using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TankArena.TankBot;

namespace TankArena.GameArena
{
    public class BotDetailsHandler
    {
        private OutGameInfo lastMove = new OutGameInfo();
        private List<Enemy> lastAtackers = new List<Enemy>();
        private string botName = null;
        private string botTypeName = null;

        public IBot Bot { get; set; }
        public Position BotPosition { get; set; }
        public int HitPoints { get; set; }
        public int AmmoCount { get; set; }
        public Direction BotDirection { get; set; }
        public int ArenaWidth { get; set; }
        public int ArenaHeight { get; set; }
        public System.Drawing.Color botColor { get; set; }
        public OutGameInfo LastMove
        {
            get { return lastMove; }
            set
            {
                if (value == null)
                {
                    lastMove.Move = BotMove.Skip;
                }
                else
                {
                    lastMove.Move = value.Move;
                    lastMove.TargetPosition = value.TargetPosition;
                }
            }
        }
        public string Name
        {
            get
            {
                if (botName == null)
                    botName = Bot.GetName();
                return botName;
            }
        }
        public string TypeName
        {
            get
            {
                if (botTypeName == null)
                    botTypeName = Bot.GetType().ToString();
                return botTypeName;
            }
        }

        public void AddAtacker(Enemy enemy)
        {
            lastAtackers.Add(enemy);
        }

        public OutGameInfo ProcessMove(InGameInfo inInfo)
        {
            OutGameInfo outInfo;
            inInfo.Attackers = lastAtackers;

            try
            {
                outInfo = Bot.ProcessMove(inInfo);
            }
            catch
            {
                outInfo = null;
            }

            lastAtackers = new List<Enemy>();
            return outInfo;
        }

        public void UpdateDirection(BotMove botMove)
        {
            if (botMove == BotMove.TurnRight)
            {
                BotDirection++;
                if ((int)BotDirection > 3)
                {
                    BotDirection = (Direction)0;
                }
            }
            else if (botMove == BotMove.TurnLeft)
            {
                BotDirection--;
                if ((int)BotDirection < 0)
                {
                    BotDirection = (Direction)3;
                }
            }
        }

        public void MoveAhead()
        {
            // Note: 2 bots can stand on same position
            switch (BotDirection)
            {
                case Direction.Right:
                    if (BotPosition.X < ArenaWidth - 1)
                        BotPosition.X++;
                    break;
                case Direction.Left:
                    if (BotPosition.X > 0)
                        BotPosition.X--;
                    break;
                case Direction.Down:
                    if (BotPosition.Y < ArenaHeight - 1)
                        BotPosition.Y++;
                    break;
                case Direction.Up:
                    if (BotPosition.Y > 0)
                        BotPosition.Y--;
                    break;
            }
        }
    }

    public class ShotDescription
    {
        public Position FromPosition { get; set; }
        public Position ToPosition { get; set; }
    }
}
