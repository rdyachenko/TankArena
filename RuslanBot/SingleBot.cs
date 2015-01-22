using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TankArena.TankBot
{
    public class SingleBot : IBot
    {
        private StartGameInfo _startGameInfo;
        int _moves;
        int _amor;
        Position _curPos;
        Direction _dir;
        bool _HasTarget;
        state curState;
        Enemy targEnemy;
        Position targPos;
        Position amoPos;
        Position hitPos;

        private enum state
        {
            gotoHitPoints,
            gotoAmor,
            gotoTarget,
            run,
            findTarget
        }

        #region IBot Members

        public void Initialize(StartGameInfo startGameInfo)
        {
            _startGameInfo = startGameInfo;
            _moves = 0;
            _amor = startGameInfo.AmmoCount;
            _dir = startGameInfo.StartDirection;
            _curPos = startGameInfo.StartPosition;
            curState = state.run;
            amoPos = null;
            hitPos = null;
            targPos = null;
            _HasTarget = false;
        }

        public OutGameInfo ProcessMove(InGameInfo inGameInfo)
        {
            OutGameInfo outInfo = new OutGameInfo();

            IfNeedChangeTargPos(inGameInfo);

            if (_curPos.X == 0 || _curPos.Y == 0 || _curPos.X == _startGameInfo.ArenaWidth || _curPos.Y == _startGameInfo.ArenaHeight)
                curState = state.run;

            if (_amor <= 2)
                curState = state.gotoAmor;

            if (inGameInfo.Targets.Count > 0 && GetValEnemy(inGameInfo.Targets) != null)
                curState = state.gotoTarget;

            if (_amor <= 0)
                curState = state.gotoAmor;

            if (inGameInfo.HitPoints < 40)
                curState = state.gotoHitPoints;

            if (_curPos.Equals(_startGameInfo.HPPoints[0]) || _curPos.Equals(_startGameInfo.HPPoints[1]) ||
                _curPos.Equals(_startGameInfo.AmmoPoints[0]) || _curPos.Equals(_startGameInfo.AmmoPoints[1]))
            {
                outInfo.Move = BotMove.MoveAhead;
                switch (_dir)
                {
                    case Direction.Down:
                        _curPos.Y++;
                        break;
                    case Direction.Up:
                        _curPos.Y--;
                        break;
                    case Direction.Right:
                        _curPos.X++;
                        break;
                    case Direction.Left:
                        _curPos.X--;
                        break;
                }
                return outInfo;
            }

            switch (curState)
            {
                case state.gotoHitPoints:
                    outInfo = gotoHitPoints(inGameInfo);
                    break;
                case state.gotoAmor:
                    outInfo = gotoAmor(inGameInfo);
                    break;
                case state.gotoTarget:
                    outInfo = gotoTarget(inGameInfo);
                    break;
                case state.run:
                    outInfo = run(inGameInfo);
                    break;
                case state.findTarget:
                    outInfo = findTarget(inGameInfo);
                    break;
            }

            _moves++;
            return outInfo;
        }

        public string GetName()
        {
            return "RuslanBot";
        }

        #endregion

        private void IfNeedChangeTargPos(InGameInfo inGameInfo)
        {
            Position pos = null;
            switch (curState)
            {
                case state.gotoAmor: pos = amoPos; break;
                case state.gotoHitPoints: pos = hitPos; break;
                case state.run: pos = targPos; break;
            }

            if (pos != null)
            {
                foreach (Enemy en in inGameInfo.Attackers)
                {
                    if (GetDistance(en.Position, pos) < GetDistance(_curPos, pos))
                    {
                        switch (curState)
                        {
                            case state.gotoAmor: amoPos = GetMaxPath(_startGameInfo.AmmoPoints); break;
                            case state.gotoHitPoints: hitPos = GetMaxPath(_startGameInfo.HPPoints); break;
                            case state.run: targPos = GetRandPos(); break;
                        }
                    }
                }
            }
        }

        private int GetDistance(Position p1, Position p2)
        {
            int dx;
            int dy;
            int distance;

            dx = p1.X - p2.X;
            dy = p1.Y - p2.Y;

            distance = dx * dx + dy * dy;
            if (distance != 0)
                distance = (int)Math.Sqrt(distance);

            return distance;
        }

        private Enemy GetValEnemy(List<Enemy> Targs)
        {
            Enemy enret = null;
            int dist = -1;
            int mindist = GetDistance(new Position() { X = 0, Y = 0 }, new Position() { X = _startGameInfo.ArenaWidth, Y = _startGameInfo.ArenaHeight });

            foreach (Enemy en in Targs)
            {
                dist = GetDistance(_curPos, en.Position);
                if (en.HitPoints > 0 && dist < mindist)
                {
                    enret = en;
                    mindist = dist;
                }
            }
            return enret;
        }

        private OutGameInfo gotoTarget(InGameInfo inGameInfo)
        {
            OutGameInfo outInfo = new OutGameInfo();

            if (inGameInfo.Targets.Count > 0)
                targEnemy = GetValEnemy(inGameInfo.Targets);
            if (targEnemy != null)
            {
                int dist = GetDistance(_curPos, targEnemy.Position);
                if (dist <= 12)
                {
                    outInfo.Move = BotMove.Shoot;
                    outInfo.TargetPosition = targEnemy.Position;
                    _amor--;
                }
                else
                {
                    outInfo.Move = GetNextMove(targEnemy.Position);
                }
            }
            else
            {
                curState = state.findTarget;
            }

            return outInfo;
        }

        private OutGameInfo gotoHitPoints(InGameInfo inGameInfo)
        {
            OutGameInfo outInfo = new OutGameInfo();

            if (hitPos == null)
                hitPos = GetMinPath(_startGameInfo.HPPoints);

            outInfo.Move = GetNextMove(hitPos);

            if (_curPos.Equals(hitPos))
            {
                curState = state.findTarget;
                hitPos = null;
            }

            return outInfo;
        }

        private OutGameInfo gotoAmor(InGameInfo inGameInfo)
        {
            OutGameInfo outInfo = new OutGameInfo();

            //             if (inGameInfo.Attackers.Count > 0)
            //                 amoPos = GetMaxPath(_startGameInfo.AmmoPoints[0], _startGameInfo.AmmoPoints[1]);
            //             else
            if (amoPos == null)
                amoPos = GetMinPath(_startGameInfo.AmmoPoints);


            outInfo.Move = GetNextMove(amoPos);

            if (_curPos.Equals(amoPos))
            {
                curState = state.findTarget;
                _amor = 5;
                amoPos = null;
            }

            return outInfo;
        }

        private OutGameInfo run(InGameInfo inGameInfo)
        {
            OutGameInfo outInfo = new OutGameInfo();

            if (targPos == null)
            {
                targPos = GetRandPos();
            }

            outInfo.Move = GetNextMove(targPos);

            if (_curPos.Equals(targPos))
            {
                curState = state.findTarget;
                targPos = null;
            }

            return outInfo;
        }

        private OutGameInfo findTarget(InGameInfo inGameInfo)
        {
            OutGameInfo outInfo = new OutGameInfo();

            outInfo.Move = BotMove.Scan;
            curState = state.run;

            return outInfo;
        }

        private Position GetRandPos()
        {
            Position pos = new Position();
            Random rnd = new Random();
            pos.X = rnd.Next(0, _startGameInfo.ArenaWidth);
            pos.Y = rnd.Next(0, _startGameInfo.ArenaHeight);
            return pos;
        }

        private Position GetMinPath(List<Position> poss)
        {
            Position retPos = null;
            int dist = 0;
            int mindist = GetDistance(new Position() { X = 0, Y = 0 }, new Position() { X = _startGameInfo.ArenaWidth, Y = _startGameInfo.ArenaHeight });

            foreach (Position pos in poss)
            {
                dist = GetDistance(_curPos, pos);
                if (dist < mindist)
                {
                    mindist = dist;
                    retPos = pos;
                }
            }

            return retPos;
        }

        private Position GetMaxPath(List<Position> poss)
        {
            Position retPos = null;
            int dist = 0;
            int maxdist = 0;

            foreach (Position pos in poss)
            {
                dist = GetDistance(_curPos, pos);
                if (dist > maxdist)
                {
                    maxdist = dist;
                    retPos = pos;
                }
            }

            return retPos;
        }

        private BotMove GetNextMove(Position targetPos)
        {
            BotMove nextMove = BotMove.MoveAhead;

            if (_curPos.X < targetPos.X)
            {
                switch (_dir)
                {
                    case Direction.Down:
                        nextMove = BotMove.TurnLeft;
                        _dir = Direction.Right;
                        break;
                    case Direction.Up:
                        nextMove = BotMove.TurnRight;
                        _dir = Direction.Right;
                        break;
                    case Direction.Right:
                        nextMove = BotMove.MoveAhead;
                        _dir = Direction.Right;
                        break;
                    case Direction.Left:
                        nextMove = BotMove.TurnRight;
                        _dir = Direction.Up;
                        break;
                }
            }
            if (_curPos.X > targetPos.X)
            {
                switch (_dir)
                {
                    case Direction.Down:
                        nextMove = BotMove.TurnRight;
                        _dir = Direction.Left;
                        break;
                    case Direction.Up:
                        nextMove = BotMove.TurnLeft;
                        _dir = Direction.Left;
                        break;
                    case Direction.Right:
                        nextMove = BotMove.TurnLeft;
                        _dir = Direction.Up;
                        break;
                    case Direction.Left:
                        nextMove = BotMove.MoveAhead;
                        _dir = Direction.Left;
                        break;
                }
            }

            if (_curPos.X == targetPos.X)
            {
                if (_curPos.Y < targetPos.Y)
                {
                    switch (_dir)
                    {
                        case Direction.Down:
                            nextMove = BotMove.MoveAhead;
                            _dir = Direction.Down;
                            break;
                        case Direction.Up:
                            nextMove = BotMove.TurnLeft;
                            _dir = Direction.Left;
                            break;
                        case Direction.Right:
                            nextMove = BotMove.TurnRight;
                            _dir = Direction.Down;
                            break;
                        case Direction.Left:
                            nextMove = BotMove.TurnLeft;
                            _dir = Direction.Down;
                            break;
                    }
                }
                if (_curPos.Y > targetPos.Y)
                {
                    switch (_dir)
                    {
                        case Direction.Down:
                            nextMove = BotMove.TurnRight;
                            _dir = Direction.Left;
                            break;
                        case Direction.Up:
                            nextMove = BotMove.MoveAhead;
                            _dir = Direction.Up;
                            break;
                        case Direction.Right:
                            nextMove = BotMove.TurnLeft;
                            _dir = Direction.Up;
                            break;
                        case Direction.Left:
                            nextMove = BotMove.TurnRight;
                            _dir = Direction.Up;
                            break;
                    }
                }
            }

            if (nextMove == BotMove.MoveAhead)
            {
                switch (_dir)
                {
                    case Direction.Down:
                        _curPos.Y++;
                        break;
                    case Direction.Up:
                        _curPos.Y--;
                        break;
                    case Direction.Right:
                        _curPos.X++;
                        break;
                    case Direction.Left:
                        _curPos.X--;
                        break;
                }
            }

            return nextMove;
        }

    }
}
