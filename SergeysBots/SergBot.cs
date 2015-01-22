using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TankArena.TankBot
{
    public class SergBot: IBot
    {
        private StartGameInfo botInfo;
        private int movesDone;
        private Position currentTarget = new Position() { X = 0, Y = 0 };

        public string GetName()
        {
            return "Serg Bot";
        }

        public void Initialize(StartGameInfo startGameInfo)
        {
            botInfo = startGameInfo;
            movesDone = 0;
        }

        public OutGameInfo ProcessMove(InGameInfo inGameInfo)
        {
            botInfo.HitPoints = inGameInfo.HitPoints;
            // Move ahead
            OutGameInfo myMove = new OutGameInfo() { Move = BotMove.MoveAhead };

            // Each 10 moves- scan
            if (movesDone > 0 && movesDone % 10 == 0)
                myMove.Move = BotMove.Scan;

            if (botInfo.AmmoCount > 2 && botInfo.HitPoints > 9)
            {
                // In case if some bot attack me- shoot in attacker
                if (inGameInfo.Attackers.Count > 0)
                {
                    myMove.Move = BotMove.Shoot;
                    myMove.TargetPosition = inGameInfo.Attackers[0].Position;
                    botInfo.AmmoCount--;
                }
                else
                {
                    // In case if target found- shoot
                    if (inGameInfo.Targets.Count > 0)
                    {
                        myMove.Move = BotMove.Shoot;
                        myMove.TargetPosition = inGameInfo.Targets[0].Position;
                        botInfo.AmmoCount--;
                    }
                }
            }
            else
            {

                if (botInfo.HitPoints < 10)  /*Move to HitPoint*/
                {
                    if (GetDistance(botInfo.StartPosition, botInfo.HPPoints[0]) < GetDistance(botInfo.StartPosition, ((Position)botInfo.HPPoints[1])))
                        currentTarget = botInfo.HPPoints[0];
                    else
                        currentTarget = botInfo.HPPoints[1];
                }
                if (botInfo.AmmoCount < 3)  /*Move to AmmoPoint*/
                {
                    if (GetDistance(botInfo.StartPosition, botInfo.AmmoPoints[0]) < GetDistance(botInfo.StartPosition, ((Position)botInfo.AmmoPoints[1])))
                        currentTarget = botInfo.AmmoPoints[0];
                    else
                        currentTarget = botInfo.AmmoPoints[1];
                }
                /* Calculate direction */
                Direction needDirection = botInfo.StartDirection;
                if (currentTarget.X > botInfo.StartPosition.X)
                {
                    needDirection = Direction.Right;
                }
                if (currentTarget.X < botInfo.StartPosition.X)
                {
                    needDirection = Direction.Left;
                }
                if (currentTarget.X == botInfo.StartPosition.X)
                {
                    if (currentTarget.Y > botInfo.StartPosition.Y)
                        needDirection = Direction.Down;
                    else
                        needDirection = Direction.Up;
                }
                if (needDirection == botInfo.StartDirection)
                {
                    /* Move to selected target */
                    myMove.Move = BotMove.MoveAhead;
                }
                else
                {
                    /* Turn to selected target */
                    myMove.Move = BotMove.TurnRight;
                }
            }

            movesDone++;
            ApplyMove(myMove);
            return myMove;

        }


        private void ApplyMove(OutGameInfo move)
        {
            switch (move.Move)
            {
                case BotMove.MoveAhead:
                    #region BotMove.MoveAhead
                    switch (botInfo.StartDirection)
                    {
                        case Direction.Up:
                            botInfo.StartPosition.Y--;
                            break;
                        case Direction.Down:
                            botInfo.StartPosition.Y++;
                            break;
                        case Direction.Right:
                            botInfo.StartPosition.X++;
                            break;
                        case Direction.Left:
                            botInfo.StartPosition.X--;
                            break;
                    }
                    break;
                    #endregion
                case BotMove.TurnRight:
                    #region BotMove.TurnRight
                    switch (botInfo.StartDirection)
                    {
                        case Direction.Up:
                            botInfo.StartDirection = Direction.Right;
                            break;
                        case Direction.Down:
                            botInfo.StartDirection = Direction.Left;
                            break;
                        case Direction.Right:
                            botInfo.StartDirection = Direction.Down;
                            break;
                        case Direction.Left:
                            botInfo.StartDirection = Direction.Up;
                            break;
                    }
                    break;
                    #endregion
            }
        }
        private int GetDistance(Position pos1, Position pos2)
        {
            int dx = 0;
            int dy = 0;
            int distance;

            dx = pos1.X - pos2.X;
            dy = pos1.Y - pos2.Y;

            distance = dx * dx + dy * dy;
            if (distance != 0)
                distance = (int)Math.Sqrt(distance);

            return distance;
        }

        public System.Drawing.Color GetColor()
        {
            return System.Drawing.Color.Red;
        }

    }

}
