using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TankBots_Serg;

namespace TankArena.TankBot
{
    internal class informer
    {
        private static informer _in;
        private List<SergCoolBot> bots = new List<SergCoolBot>();
        private Dictionary<int, Enemy> enemyes = new Dictionary<int, Enemy>();
        private frmLog logger = new frmLog();
        private informer()
        {
            //logger.Show();
        }
        public static informer getInformer()
        {
            if (_in == null)
                _in = new informer();
            return _in;
        }
        public void AddBot(SergCoolBot b)
        {
            if (bots.Count>0 && bots[0].HakerMoves()>0)
            {
                bots.Clear();
            }
            foreach (SergCoolBot bot in bots)
                bot.HakerIsMaster = false;
            bots.Add(b);
            b.HakerIsMaster = true;
        }
        public bool isMaster(int x, int y, int movesDone)
        {
            int MaxMoveCnt=0;
            foreach (var bot in bots)
            {
                if (bot.HakerMoves() > MaxMoveCnt)
                    MaxMoveCnt = bot.HakerMoves();
            }
            foreach (var bot in bots)
            {
                if (bot.HakerMoves() < (MaxMoveCnt-1))
                {
                    bot.HakerKill();
                    bots.Remove(bot);
                    break;
                }
            }
            foreach (var bot in bots)
            {
                if (bot.HakerPos(movesDone).X == x && bot.HakerPos(movesDone).Y == y)
                {
                    return bot.HakerIsMaster;
                }
            }
            return false;
        }
        public bool isFriend(int x, int y, int moveNum)
        {
            foreach (var bot in bots)
            {
                if (bot.HakerPos(moveNum).X == x && bot.HakerPos(moveNum).Y == y)
                    return true;
            }
            return false;
        }
        public void AddToLog(string text)
        {
            //logger.AddToLog(text);
        }
        public void SetNewMaster()
        {
            foreach (var bot in bots)
            {
                if (!bot.HakerIsMaster && bot.HakerPoints() > 0)
                    bot.HakerIsMaster = true;
            }
        }

        internal void UpdateBots(List<Enemy> list1, List<Enemy> list2)
        {
            foreach (var item in list1)
            {
                if (enemyes.ContainsKey(item.ID))
                {
                    enemyes[item.ID] = item;
                }
                else
                {
                    enemyes.Add(item.ID, item);
                }
            }
            foreach (var item in list2)
            {
                if (enemyes.ContainsKey(item.ID))
                {
                    enemyes[item.ID] = item;
                }
                else
                {
                    enemyes.Add(item.ID, item);
                }
            }

        }
    }

    public class SergCoolBot: IBot
    {
        private StartGameInfo botInfo;
        private int movesDone;
        private Position currentTarget = new Position() { X = 0, Y = 0 };
        private Position prePosition = new Position();
        private Position curPosition = new Position();
        private Position rndPosition = new Position();
        private Direction curDirection;
        private enum Field
        { Empty, Tank, Bonus, None }
        private bool isMaster;
        private informer _in;

        public string GetName()
        {
            return "M1 Abrams";
        }
        public System.Drawing.Color GetColor()
        {
            return System.Drawing.Color.BlanchedAlmond;
        }

        public void Initialize(StartGameInfo startGameInfo)
        {
            botInfo = startGameInfo;
            curPosition = startGameInfo.StartPosition;
            curDirection = startGameInfo.StartDirection;
            movesDone = 0;

            _in = informer.getInformer();
            _in.AddBot(this);
        }
        public OutGameInfo ProcessMove(InGameInfo inGameInfo)
        {
            // Apply health
            botInfo.HitPoints = inGameInfo.HitPoints;

            // Move ahead by default
            OutGameInfo myMove = new OutGameInfo() { Move = BotMove.MoveAhead };

            Direction needDirection = curDirection;
            
            _in.UpdateBots(inGameInfo.Attackers,inGameInfo.Targets);

            if (onBonusPoint())
            {
                if (botInfo.StartPosition.Equals(botInfo.HPPoints[0]) || botInfo.StartPosition.Equals(botInfo.HPPoints[1]))
                    botInfo.HitPoints = 10;
                if (botInfo.StartPosition.Equals(botInfo.AmmoPoints[0]) || botInfo.StartPosition.Equals(botInfo.AmmoPoints[1]))
                    botInfo.AmmoCount = 5;
                myMove.Move = BotMove.MoveAhead;
            }
            else
            {

                if (botInfo.AmmoCount > 0 && botInfo.HitPoints > 1)
                {
                    if (NeedRunOff(inGameInfo)) // if need make legs
                    {
                        myMove.Move = BotMove.MoveAhead;
                        //currentTarget = SelectTargetForRunOff(curPosition, inGameInfo);
                        //needDirection = CalculateDirection(curPosition, currentTarget);
                        //myMove.TargetPosition = currentTarget;
                    }
                    else
                    {
                        currentTarget = SelectTargetForAttack(inGameInfo);
                        if (currentTarget != null)
                        {
                            myMove.Move = BotMove.Shoot;
                            myMove.TargetPosition = currentTarget;
                            if (!isMaster)
                            {
                                _in.AddToLog(string.Format("Master?: x={0}, y={1}", myMove.TargetPosition.X, myMove.TargetPosition.Y));
                                if (_in.isMaster(myMove.TargetPosition.X, myMove.TargetPosition.Y,movesDone))
                                    myMove.Move = BotMove.MoveAhead;
                            }
                        }
                        else
                        {
                            myMove.Move = BotMove.MoveAhead;
                            currentTarget = SelectTargetForMove();
                            if (currentTarget != null)
                            {
                                myMove.TargetPosition = currentTarget;
                                needDirection = CalculateDirection(curPosition, myMove.TargetPosition);
                            }
                            else
                            {
                                if (curPosition.Equals(rndPosition))
                                {
                                    Random rnd = new Random();
                                    rndPosition.X = rnd.Next(botInfo.ArenaWidth - 1);
                                    rndPosition.Y = rnd.Next(botInfo.ArenaHeight - 1);
                                    myMove.Move = BotMove.Scan;
                                }
                                else
                                {
                                    if (!isMaster)myMove.TargetPosition = rndPosition;
                                    else myMove.TargetPosition = GetNextCorn(curPosition, curDirection);
                                    needDirection = CalculateDirection(curPosition, myMove.TargetPosition);
                                }
                            }
                        }
                    }
                }
                else
                {
                    myMove.Move = BotMove.MoveAhead;
                    currentTarget = SelectTargetForMove();
                    needDirection = CalculateDirection(curPosition, currentTarget);

                }
                // Result analise before return
                if (myMove.Move == BotMove.Shoot)
                {
                    int dist = GetDistance(myMove.TargetPosition, curPosition);

                    if (dist < 2 || dist > 12)
                    {
                        if (dist > 12)
                            rndPosition = myMove.TargetPosition;
                        myMove.Move = BotMove.MoveAhead;
                    }
                }
                if (myMove.Move == BotMove.MoveAhead)
                {

                    if (needDirection == curDirection)
                    {
                        /* Move to selected target */
                        if (NextField(curDirection, curPosition) != Field.None)
                            myMove.Move = BotMove.MoveAhead;
                        else
                            myMove.Move = BotMove.TurnRight;

                    }
                    else
                    {
                        /* Turn to selected target */
                        if (needDirection > curDirection || (needDirection == Direction.Up && curDirection == Direction.Left))
                            myMove.Move = BotMove.TurnRight;
                        else
                            myMove.Move = BotMove.TurnLeft;
                    }
                }
                
            }

            ApplyMove(myMove);

            movesDone++;
            return myMove;
        }

        private bool NeedRunOff(InGameInfo inGameInfo)
        {
            Dictionary<int,Enemy> others = new Dictionary<int,Enemy>();
            foreach (var item in inGameInfo.Attackers)
            {
                if (!others.ContainsKey(item.ID) && item.HitPoints > 0 && !_in.isFriend(item.Position.X, item.Position.Y,movesDone))
                    others.Add(item.ID, item);
            }
            foreach (var item in inGameInfo.Targets)
            {
                if (!others.ContainsKey(item.ID) && item.HitPoints > 0 && !_in.isFriend(item.Position.X, item.Position.Y, movesDone))
                    others.Add(item.ID, item);
            }
            if (others.Count > 1)
                return true;
            return false;
        }
        
        private Position SelectTargetForRunOff(Position curPos, InGameInfo inGameInfo)
        {
            Direction d;
            Dictionary<int, Enemy> others = new Dictionary<int, Enemy>();
            int UpCount = 0, DownCount = 0, RightCount = 0, LeftCount = 0;
            foreach (var item in inGameInfo.Attackers)
            {
                if (!others.ContainsKey(item.ID) && item.HitPoints > 0)
                    others.Add(item.ID, item);
            }
            foreach (var item in inGameInfo.Targets)
            {
                if (!others.ContainsKey(item.ID) && item.HitPoints > 0)
                    others.Add(item.ID, item);
            }
            foreach (var item in others.Values)
            {
                #region Vectors
                switch (CalculateDirection(curPos, item.Position))
                {
                    case Direction.Down:
                        UpCount++;
                        break;
                    case Direction.Up:
                        DownCount++;
                        break;
                    case Direction.Right:
                        LeftCount++;
                        break;
                    case Direction.Left:
                        RightCount++;
                        break;
                }
                #endregion
            }
            #region Vectors+Turns
            switch (curDirection)
            {
                case Direction.Down:
                    DownCount += 2;
                    RightCount++;
                    LeftCount++;
                    break;
                case Direction.Left:
                    LeftCount += 2;
                    UpCount++;
                    DownCount++;
                    break;
                case Direction.Right:
                    RightCount += 2;
                    UpCount++;
                    DownCount++;
                    break;
                case Direction.Up:
                    UpCount += 2;
                    RightCount++;
                    LeftCount++;
                    break;
                default:
                    break;
            }
            #endregion
            Direction resultDirection = Direction.Up;
            if (UpCount < LeftCount) resultDirection = Direction.Left;
            if (LeftCount < RightCount) resultDirection = Direction.Right;
            if (RightCount < DownCount) resultDirection = Direction.Down;
            switch (resultDirection)
            {
                case Direction.Down:
                    return new Position() { X = curPos.X, Y = curPos.Y + 1 };
                case Direction.Left:
                    return new Position() { X = curPos.X - 1, Y = curPos.Y };
                case Direction.Right:
                    return new Position() { X = curPos.X + 1, Y = curPos.Y };
                case Direction.Up:
                    return new Position() { X = curPos.X, Y = curPos.Y - 1 };
                default:
                    return curPos;
            }

        }
        private Position SelectTargetForAttack(InGameInfo inGameInfo)
        {
            Enemy forRet = null;
            // In case if some bot attack me - shoot in attacker
            if (inGameInfo.Attackers.Count > 0)
            {
                foreach (var item in inGameInfo.Attackers)
                {
                    if (item.HitPoints > 0 && !_in.isFriend(item.Position.X, item.Position.Y, movesDone))
                        if (forRet==null || forRet.HitPoints>item.HitPoints)
                            forRet = item;    
                }
            }
            if (forRet == null)
            {
                // In case if target found - shoot
                if (inGameInfo.Targets.Count > 0)
                {
                    foreach (var item in inGameInfo.Targets)
                    {
                        if (item.HitPoints > 0 && !_in.isFriend(item.Position.X, item.Position.Y, movesDone))
                            if (forRet == null || forRet.HitPoints > item.HitPoints)
                                forRet = item;
                    }
                }
            }
            if (forRet!=null)
                return forRet.Position;
            return null;
        }
        private Position SelectTargetForMove()
        {
            if (botInfo.HitPoints < 2)  /*Move to HitPoint*/
            {
                if (GetDistance(curPosition, botInfo.HPPoints[0]) < GetDistance(curPosition, ((Position)botInfo.HPPoints[1])))
                    return botInfo.HPPoints[0];
                else
                    return botInfo.HPPoints[1];
            }
            if (botInfo.AmmoCount == 0)  /*Move to AmmoPoint*/
            {
                if (GetDistance(curPosition, botInfo.AmmoPoints[0]) < GetDistance(curPosition, ((Position)botInfo.AmmoPoints[1])))
                    return botInfo.AmmoPoints[0];
                else
                    return botInfo.AmmoPoints[1];
            }
            if (botInfo.HitPoints < 100)  /*Move to HitPoint*/
            {
                if (GetDistance(curPosition, botInfo.HPPoints[0]) < GetDistance(curPosition, ((Position)botInfo.HPPoints[1])))
                    return botInfo.HPPoints[0];
                else
                    return botInfo.HPPoints[1];
            }
            if (botInfo.AmmoCount < 5) /*Move to AmmoPoint*/
            {
                if (GetDistance(curPosition, botInfo.AmmoPoints[0]) < GetDistance(curPosition, ((Position)botInfo.AmmoPoints[1])))
                    return botInfo.AmmoPoints[0];
                else
                    return botInfo.AmmoPoints[1];
            }
            return null;
        }

        private Field NextField(Direction direction, Position position)
        {

            Position next = new Position();
            next.X = position.X;
            next.Y = position.Y;
            switch (direction)
            {
                case Direction.Up:
                    next.Y--;
                    break;
                case Direction.Down:
                    next.Y++;
                    break;
                case Direction.Right:
                    next.X++;
                    break;
                case Direction.Left:
                    next.X--;
                    break;
            }
            if (next.Y < 0 | next.Y > botInfo.ArenaHeight | next.X < 0 | next.X > botInfo.ArenaWidth)
                return Field.None;
            if (next.Equals(botInfo.HPPoints[0]) | next.Equals(botInfo.HPPoints[1]) | next.Equals(botInfo.AmmoPoints[0]) | next.Equals(botInfo.AmmoPoints[1]))
                return Field.Bonus;
            return Field.Empty;
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
        private Direction CalculateDirection(Position posFrom, Position posTo)
        {
            /* Calculate direction */
            if (posTo.X > posFrom.X)
            {
                return Direction.Right;
            }
            if (posTo.X < posFrom.X)
            {
                return Direction.Left;
            }
            if (posTo.X == posFrom.X)
            {
                if (posTo.Y > posFrom.Y)
                    return Direction.Down;
                else
                    return Direction.Up;
            }

            return Direction.Down;
        }
        private Position GetNextCorn(Position curPos, Direction curDir)
        {
            switch (curDir)
            {
                case Direction.Up:
                    if (curPos.Y > 0)
                        return new Position() { Y = 0, X = curPos.X };
                    else
                        return new Position() { Y = 0, X = botInfo.ArenaWidth - 1 };
                case Direction.Down:
                    if (curPos.Y < (botInfo.ArenaHeight - 1))
                        return new Position() { Y = botInfo.ArenaHeight - 1, X = curPos.X };
                    else
                        return new Position() { Y = botInfo.ArenaHeight - 1, X = 0 };
                case Direction.Right:
                    if (curPos.X < (botInfo.ArenaWidth - 1))
                        return new Position() { X = botInfo.ArenaWidth - 1, Y = curPos.Y };
                    else
                        return new Position() { X = botInfo.ArenaWidth - 1, Y = botInfo.ArenaHeight - 1 };
                default: // Left
                    if (curPos.X > 0)
                        return new Position() { X = 0, Y = curPos.Y };
                    else
                        return new Position() { X = 0, Y = 0 };
            }
        }
        private bool onBonusPoint()
        {
            if (botInfo.StartPosition.Equals(botInfo.HPPoints[0]))
                return true;
            if (botInfo.StartPosition.Equals(botInfo.HPPoints[1]))
                return true;
            if (botInfo.StartPosition.Equals(botInfo.AmmoPoints[0]))
                return true;
            if (botInfo.StartPosition.Equals(botInfo.AmmoPoints[1]))
                return true;
            return false;
        }
        
        private void ApplyMove(OutGameInfo move)
        {
            switch (move.Move)
            {
                case BotMove.MoveAhead:
                    #region BotMove.MoveAhead
                    prePosition = new Position() { X = curPosition.X, Y = curPosition.Y };
                    switch (curDirection)
                    {
                        case Direction.Up:
                            curPosition.Y--;
                            break;
                        case Direction.Down:
                            curPosition.Y++;
                            break;
                        case Direction.Right:
                            curPosition.X++;
                            break;
                        case Direction.Left:
                            curPosition.X--;
                            break;
                    }
                    break;
                    #endregion
                case BotMove.TurnRight:
                    #region BotMove.TurnRight
                    curDirection++;
                    if (curDirection > Direction.Left)
                        curDirection = Direction.Up;
                    break;
                    #endregion
                case BotMove.TurnLeft:
                    #region BotMove.TurnLeft
                    curDirection--;
                    if (curDirection < Direction.Up)
                        curDirection = Direction.Left;
                    break;
                    #endregion
                case BotMove.Shoot:
                    #region BotMove.Shoot
                    botInfo.AmmoCount--;
                    break;
                    #endregion BotMove.Shoot
            }
        }
        
        #region Communications
        internal Position HakerPos(int MoveNum)
        {
            if (MoveNum == movesDone)
                return curPosition;
            return prePosition;
        }
        internal bool HakerIsMaster
        {
            get
            {
                return isMaster;
            }
            set
            {
                isMaster = value;
            }
        }
        internal int HakerPoints()
        { 
            return botInfo.HitPoints;
        }
        internal void HakerKill()
        {
            botInfo.HitPoints = 0;
            if (isMaster)
            {
                isMaster = false;
                _in.SetNewMaster();
            }
        }
        internal int HakerMoves()
        {
            return movesDone;
        }
        #endregion
    }


}
