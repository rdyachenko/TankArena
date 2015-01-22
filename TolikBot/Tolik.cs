using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TankArena.TankBot
{
    class TolikBot : IBot
    {
        #region IBot implementation
        public void Initialize(StartGameInfo startGameInfo)
        {
            _startGameInfo = startGameInfo;


            _dir = startGameInfo.StartDirection;
            _pos = startGameInfo.StartPosition;

            _hpLeft = startGameInfo.HitPoints;
            _ammoLeft = startGameInfo.AmmoCount;

            if (_startGameInfo.AmmoPoints == null)
                _startGameInfo.AmmoPoints = new List<Position>();

            if (_startGameInfo.HPPoints == null)
                _startGameInfo.HPPoints = new List<Position>();
        }

        public OutGameInfo ProcessMove(InGameInfo inGameInfo)
        {
            calcStateBeforeMove(inGameInfo);

            OutGameInfo result = getNextMove();

            calcStateAfterMove(result);
            _prevMove = result;
            return result;
        }

        public string GetName()
        {
            return "Tolik MonsterKill";
        }
        #endregion

        OutGameInfo _prevMove = new OutGameInfo { Move = BotMove.Skip };
        StartGameInfo _startGameInfo;
        Direction _dir;
        Position _pos;
        int _hpLeft;
        int _ammoLeft;

        List<Enemy> _attakers;
        List<Enemy> _targets;

        List<Enemy> _enemiesCanShoot;

        private OutGameInfo getNextMove()
        {
            if (mustMove())
                return moveAheadFromPoint();

            if (_enemiesCanShoot.Count > 0 && _ammoLeft > 0)
                return fireEnemyMove();

            if (_ammoLeft == 0)
                return nearestAmmoMove();

            if (_hpLeft < _startGameInfo.HitPoints)
            {
                return nearestHealthMove();
            }

            if (_ammoLeft < _startGameInfo.AmmoCount)
            {
                return nearestAmmoMove();
            }

            if (_prevMove.Move == BotMove.Scan)
            {
                return searchModeMove();
            }

            return scanMove();
        }

        private bool mustMove()
        {
            foreach (Position pos in _startGameInfo.AmmoPoints)
            {
                if (pos.X == _pos.X && pos.Y == _pos.Y)
                    return true;
            }

            foreach (Position pos in _startGameInfo.HPPoints)
            {
                if (pos.X == _pos.X && pos.Y == _pos.Y)
                    return true;
            }

            return false;
        }

        private OutGameInfo nearestHealthMove()
        {
            return new OutGameInfo
            {
                Move = nearestPointMove(_startGameInfo.HPPoints)
            };
        }
        private OutGameInfo nearestAmmoMove()
        {
            return new OutGameInfo
            {
                Move = nearestPointMove(_startGameInfo.AmmoPoints)
            };
        }
        private OutGameInfo searchModeMove()
        {
            return nearestHealthMove();
        }

        private OutGameInfo fireEnemyMove()
        {
            return new OutGameInfo
            {
                Move = BotMove.Shoot,
                TargetPosition = findTargetPosition()
            };
        }
        private OutGameInfo moveAheadFromPoint()
        {
            return new OutGameInfo
            {
                Move = BotMove.MoveAhead
            };
        }
        private OutGameInfo skipMove()
        {
            return new OutGameInfo
            {
                Move = BotMove.Skip
            };
        }
        private OutGameInfo scanMove()
        {
            return new OutGameInfo
            {
                Move = BotMove.Scan
            };
        }


        private BotMove nearestPointMove(List<Position> positions)
        {
            int i = 2 + _startGameInfo.ArenaHeight + _startGameInfo.ArenaWidth;
            Position posNearest = null;
            foreach (Position pos in positions)
            {
                if (numberMoves(pos) < i)
                {
                    i = numberMoves(pos);
                    posNearest = pos;
                }
            }
            return (posNearest == null) ? BotMove.MoveAhead : getMoveToPoint(posNearest);
        }
        private int numberMoves(Position pos)
        {
            int absDx = Math.Abs(_pos.X - pos.X);
            int absDy = Math.Abs(_pos.Y - pos.Y);
            int moves = absDx + absDy;

            // turns count

            if (_pos.X > pos.X && _dir == Direction.Right)
                return moves + 2;

            if (_pos.X < pos.X && _dir == Direction.Left)
                return moves + 2;

            if (_pos.Y < pos.Y && _dir == Direction.Up)
                return moves + 2;

            if (_pos.Y > pos.Y && _dir == Direction.Down)
                return moves + 2;

            if (absDx == 0)
            {
                if (_pos.Y > pos.Y && _dir == Direction.Up)
                    return moves;

                if (_pos.Y < pos.Y && _dir == Direction.Down)
                    return moves;

                return moves + 1;
            }

            if (absDy == 0)
            {
                if (_pos.X > pos.X && _dir == Direction.Left)
                    return moves;

                if (_pos.X < pos.X && _dir == Direction.Right)
                    return moves;

                return moves + 1;
            }

            return moves + 1;
        }
        private Position findTargetPosition()
        {
            if (_enemiesCanShoot.Count > 0)
                return _enemiesCanShoot[0].Position;
            else
                return new Position { X = -1, Y = -1 };
        }
        private bool canFirePosition(Position pos)
        {
            return true;
        }
        private BotMove getMoveToPoint(Position pos)
        {
            if (pos.X > _pos.X && _dir == Direction.Right)
                return BotMove.MoveAhead;

            if (pos.X < _pos.X && _dir == Direction.Left)
                return BotMove.MoveAhead;

            if (pos.Y < _pos.Y && _dir == Direction.Up)
                return BotMove.MoveAhead;

            if (pos.Y > _pos.Y && _dir == Direction.Down)
                return BotMove.MoveAhead;

            switch (_dir)
            {
                case Direction.Up:
                    if (pos.X > _pos.X)
                        return BotMove.TurnRight;
                    else
                        return BotMove.TurnLeft;
                    break;
                case Direction.Right:
                    if (pos.Y > _pos.Y)
                        return BotMove.TurnRight;
                    else
                        return BotMove.TurnLeft;
                    break;
                case Direction.Down:
                    if (pos.X > _pos.X)
                        return BotMove.TurnLeft;
                    else
                        return BotMove.TurnRight;
                    break;
                case Direction.Left:
                    if (pos.Y > _pos.Y)
                        return BotMove.TurnLeft;
                    else
                        return BotMove.TurnRight;
                    break;
            }
            return BotMove.MoveAhead;
        }

        private bool onAmmoPoint()
        {
            foreach (var point in _startGameInfo.AmmoPoints)
            {
                if (point.X == _pos.X && point.Y == _pos.Y)
                    return true;
            }
            return false;
        }

        private void calcStateBeforeMove(InGameInfo inGameInfo)
        {
            _hpLeft = inGameInfo.HitPoints;
            _attakers = inGameInfo.Attackers;
            _targets = inGameInfo.Targets;

            _enemiesCanShoot = _attakers;
            if (_enemiesCanShoot == null)
                _enemiesCanShoot = new List<Enemy>();
            if (_targets != null)
            {
                foreach (var target in _targets)
                {
                    if (canFirePosition(target.Position))
                    {
                        _enemiesCanShoot.Add(target);
                    }
                }
            }

            if (onAmmoPoint())
            {
                _ammoLeft = _startGameInfo.AmmoCount;
            }
        }


        private void calcStateAfterMove(OutGameInfo result)
        {
            switch (result.Move)
            {
                case BotMove.MoveAhead:
                    calcStateAfterMoveAhead();
                    break;
                case BotMove.Shoot:
                    if (_ammoLeft > 0)
                        _ammoLeft -= 1;
                    break;
                case BotMove.TurnLeft:
                    _dir = (Direction)((int)(_dir + 3) % 4);
                    break;
                case BotMove.TurnRight:
                    _dir = (Direction)((int)(_dir + 1) % 4);
                    break;

                case BotMove.Scan:
                case BotMove.Skip:
                    break;
            }
        }
        private void calcStateAfterMoveAhead()
        {
            switch (_dir)
            {
                case Direction.Down:
                    if (_pos.Y < _startGameInfo.ArenaHeight - 1)
                        _pos.Y++;
                    break;
                case Direction.Up:
                    if (_pos.Y > 0)
                        _pos.Y--;
                    break;
                case Direction.Left:
                    if (_pos.X > 0)
                        _pos.X--;
                    break;
                case Direction.Right:
                    if (_pos.X < _startGameInfo.ArenaWidth - 1)
                        _pos.X++;
                    break;
            }
        }
    }
}
