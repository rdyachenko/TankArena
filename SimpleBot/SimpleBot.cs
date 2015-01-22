using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TankArena.TankBot
{
    public class SimpleBot : IBot
    {
        private StartGameInfo botInfo;
        private int movesDone;


        public string GetName()
        {
            return "Simple Bot";
        }

        public void Initialize(StartGameInfo startGameInfo)
        {
            botInfo = startGameInfo;
            movesDone = 0;
        }

        public OutGameInfo ProcessMove(InGameInfo inGameInfo)
        {
            // Move ahead
            OutGameInfo myMove = new OutGameInfo() { Move = BotMove.MoveAhead };

            // Each 20 moves- scan
            if (movesDone%20 == 0)
                myMove.Move = BotMove.Scan;

            if (botInfo.AmmoCount > 0)
            {
                // In case if target found- shoot
                if (inGameInfo.Targets.Count > 0)
                {
                    myMove.Move = BotMove.Shoot;
                    myMove.TargetPosition = inGameInfo.Targets[0].Position;
                    botInfo.AmmoCount--;
                }
                else
                {
                    // In case if some bot attack me- shoot in attacker
                    if (inGameInfo.Attackers.Count > 0)
                    {
                        myMove.Move = BotMove.Shoot;
                        myMove.TargetPosition = inGameInfo.Attackers[0].Position;
                        botInfo.AmmoCount--;
                    }
                }
            }

            movesDone++;
            return myMove;
        }
    }
}
