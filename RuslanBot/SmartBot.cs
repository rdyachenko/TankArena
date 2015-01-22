using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TankArena.TankBot
{
    public class SmartBot : IBot
    {
        public int ID
        {
            get;
            set;
        }

        public List<Enemy> Attackers
        {
            get;
            set;
        }

        public List<Enemy> Targets
        {
            get;
            set;
        }

        public Position Location
        {
            get;
            set;
        }

        public Position TargetPosition
        {
            get;
            set;
        }

        public Direction Direction
        {
            get;
            set;
        }

        public int Ammo
        {
            get;
            set;
        }

        public int Hit
        {
            get;
            set;
        }

        public bool WasScaning
        {
            get;
            set;
        }

        public Position PreviosLocation
        {
            get;
            set;
        }

        #region IBot Members

        public void Initialize(StartGameInfo startGameInfo)
        {
            Brain.GetBrain().Init(startGameInfo.ArenaWidth, startGameInfo.ArenaHeight,
                                  startGameInfo.AmmoPoints, startGameInfo.HPPoints,
                                  startGameInfo.AttrackRange, startGameInfo.ScanRange, startGameInfo.SplashRange, startGameInfo.VisibilityRange,
                                  startGameInfo.AmmoCount, startGameInfo.HitPoints);

            Location = startGameInfo.StartPosition;
            PreviosLocation = Location;
            Direction = startGameInfo.StartDirection;
            Ammo = startGameInfo.AmmoCount;
            Hit = startGameInfo.HitPoints;
            ID = Brain.GetBrain().GetBotID();

            Brain.GetBrain().AddBotToCommand(this);
        }

        public OutGameInfo ProcessMove(InGameInfo inGameInfo)
        {
            Hit = inGameInfo.HitPoints;
            Attackers = inGameInfo.Attackers;
            Targets = inGameInfo.Targets;

            BotMove move = Brain.GetBrain().GetMove(this.ID);

            return ToOutInfo(move);
        }

        public string GetName()
        {
            return "Botos Sapiens";
        }

        #endregion

        private OutGameInfo ToOutInfo(BotMove move)
        {
            OutGameInfo outInfo = new OutGameInfo();
            WasScaning = false;

            switch (move)
            {
                case BotMove.MoveAhead:
                    PreviosLocation = new Position() { X = Location.X, Y = Location.Y };
                    ChangeLocation(Direction);
                break;
                case BotMove.Scan:
                    WasScaning = true;
                break;
                case BotMove.Shoot:
                    outInfo.TargetPosition = TargetPosition;
                    Ammo--;
                break;
                case BotMove.Skip:
                break;
                case BotMove.TurnLeft:
                    if ((int)Direction != 0)
                        Direction = (Direction)(((int)Direction - 1) % 4);
                    else
                        Direction = Direction.Left;
                break;
                case BotMove.TurnRight:
                    Direction = (Direction)(((int)Direction + 1) % 4);
                break;
            }

            outInfo.Move = move;

            return outInfo;
        }

        private void ChangeLocation(Direction dir)
        {
            switch (dir)
            {
                case Direction.Down:
                    Location.Y++;
                    break;
                case Direction.Up:
                    Location.Y--;
                    break;
                case Direction.Right:
                    Location.X++;
                    break;
                case Direction.Left:
                    Location.X--;
                    break;
            }
        }
    }

    public class Brain
    {
        static Brain _Brain;

        private TacticManager _Tactic;
        private StrategManager _Strateg;
        private ComandManager _Command;

        bool _initialised;

        private Brain()
        {
            _Tactic = new TacticManager();
            _Strateg = new StrategManager();
            _Command = new ComandManager();

            _initialised = false;
        }

        public static Brain GetBrain()
        {
            if(_Brain == null)
                _Brain = new Brain();

            return _Brain;
        }

        public void Init(int arrenaWidth, int arrenaHeight,
                         List<Position> ammoPoints, List<Position> hpPoints,
                         int attackRange, int scanRange, int splashRange, int visibilityRange,
                         int ammoCount, int hpCount)
        {
            if(!_initialised)
            {
                _Tactic.Init(arrenaWidth, arrenaHeight, ammoPoints, hpPoints, _Command);
                _Strateg.Init(attackRange, scanRange, splashRange, visibilityRange, ammoCount, hpCount);
                _initialised = true;
            }
        }

        public int GetBotID()
        {
            return _Command.Count;
        }

        public void AddBotToCommand(SmartBot bot)
        {
            _Command.AddBot(bot);
        }

        public BotMove GetMove(int ID)
        {
            SmartBot bot = _Command.GetBot(ID);
            _Command.UpdateEnemies(bot.Attackers);
            _Command.UpdateEnemies(bot.Targets);
            List<Desition> desitions = _Strateg.GetListOfDesitions(bot, _Command.Comanda, _Command.Enemies);
            _Tactic.UpdateDesitionListOfBot(ID, desitions);
            return _Tactic.GetMove(bot);
        }
    }
    
//     public class ComandManager
//     {
//         SortedList<int,SmartBot> _comandosList;
//         SortedList<int,Enemy> _enemies;
//         static ComandManager _Comandos;
//         SmartBot _currentBot;
// 
//         int _ArenaWidth;
//         int _ArenaHeight;
//         List<Position> _hitPoints;
//         List<Position> _ammoPoints;
//         int _AttrackRange;
//         int _ScanRange;
//         int _SplashRange;
//         int _VisibilityRange;
//         int _MaxAmmor;
//         int _MaxHP;
// 
//         int CRITICAL_AMMO;
//         int CRITICAL_HP;
//         int MAX_FREE_MOVES;
// 
//         int _FreeMoves;
//         int _ActivBotID;
//         bool _onlyMyCommand;
// 
//         public bool HasStartInfo
//         {
//             get;
//             set;
//         }
// 
//         private ComandManager()
//         {
//             _comandosList = new SortedList<int, SmartBot>();
//             _enemies = new SortedList<int, Enemy>();
//             HasStartInfo = false;
//         }
// 
//         public static ComandManager GetComandos()
//         {
//             if (_Comandos == null)
//             {
//                 _Comandos = new ComandManager();
//             }
// 
//             return _Comandos;
//         }
// 
//         public void Add(SmartBot bot)
//         {
//             _comandosList.Add(bot.ID, bot);
//         }
// 
//         public int GetID()
//         {
//             return _comandosList.Count;
//         }
// 
//         public void SetStartInfo(StartGameInfo startGameInfo)
//         {
//             _ArenaWidth = startGameInfo.ArenaWidth;
//             _ArenaHeight = startGameInfo.ArenaHeight;
//             _ammoPoints = startGameInfo.AmmoPoints;
//             _hitPoints = startGameInfo.HPPoints;
//             _AttrackRange = startGameInfo.AttrackRange;
//             _ScanRange = startGameInfo.ScanRange;
//             _SplashRange = startGameInfo.SplashRange;
//             _VisibilityRange = startGameInfo.VisibilityRange;
//             _MaxAmmor = startGameInfo.AmmoCount;
//             _MaxHP = startGameInfo.HitPoints;
// 
//             MAX_FREE_MOVES = (startGameInfo.ArenaWidth + startGameInfo.ArenaHeight)/2;
//             _FreeMoves = MAX_FREE_MOVES;
//             _ActivBotID = -1;
//             _onlyMyCommand = false;
// 
//             CRITICAL_AMMO = 4;
//             CRITICAL_HP = 90;
// 
//             _comandosList.Clear();
//             _enemies.Clear();
// 
//             HasStartInfo = true;
//         }
// 
//         public OutGameInfo GetMoveForBot(int id)
//         {
//             OutGameInfo outInfo = new OutGameInfo();
//             Enemy enemy = null;
// 
//             if (_comandosList[id] != null)
//             {
//                 _currentBot = _comandosList[id];
//                 RemoveDeadTank();
//                 UpdateBotEnemyList(_currentBot.Attackers);
//                 UpdateBotEnemyList(_currentBot.Targets);
// 
//                 if (_currentBot.WasScaning)
//                     _currentBot.WasScaning = false;
// 
//                 if (_onlyMyCommand && id != _ActivBotID)
//                 {
//                     outInfo.Move = BotMove.Skip;
//                     return outInfo;
//                 }
// 
//                 if (((_currentBot.Hit > CRITICAL_HP && _currentBot.Ammo > CRITICAL_AMMO) || _currentBot.Targets.Count > 0) && _currentBot.Ammo > 0)
//                 {
//                     if (IsWeOnDeadPoint())
//                     {
//                         Position nextPos = new Position() { X = _currentBot.Location.X, Y = _currentBot.Location.Y };
// 
//                         switch (_currentBot.Direction)
//                         {
//                             case Direction.Down:
//                                 nextPos.Y++;
//                                 break;
//                             case Direction.Up:
//                                 nextPos.Y--;
//                                 break;
//                             case Direction.Right:
//                                 nextPos.X++;
//                                 break;
//                             case Direction.Left:
//                                 nextPos.X--;
//                                 break;
//                         }
//                         outInfo.Move = GetNextMove(nextPos);
//                         return outInfo;
//                     }
// 
//                     enemy = FindBestEnemyInAttrackRange();
//                     if (enemy != null)
//                     {
//                         outInfo.Move = BotMove.Shoot;
//                         outInfo.TargetPosition = enemy.Position;
//                         enemy.HitPoints -= 20;
//                         _currentBot.Ammo--;
//                         return outInfo;
//                     }
//                     else
//                     {
//                         enemy = FindBestEnemy();
//                         if (enemy != null)
//                         {
//                             if (GetDistance(_currentBot.Location, enemy.Position) <= _AttrackRange)
//                             {
//                                 outInfo.Move = BotMove.Scan;
//                                 _currentBot.WasScaning = true;
//                             }
//                             else
//                                 outInfo.Move = GetNextMove(enemy.Position);
//                             return outInfo;
//                         }
//                         else
//                         {
//                             if ((_currentBot.TargetPosition == null || _currentBot.TargetPosition.Equals(_currentBot.Location)))
//                             {
//                                 _currentBot.TargetPosition = GetRandPos();
//                                 outInfo.Move = BotMove.Scan;
//                                 _currentBot.WasScaning = true;
//                             }
//                             else
//                                 outInfo.Move = GetNextMove(_currentBot.TargetPosition);
//                             return outInfo;
//                         }
//                     }
//                 }
//                 else
//                 {
//                     if (_currentBot.Hit <= CRITICAL_HP)
//                     {
//                         Position p = GetMinPath(_hitPoints);
// 
//                         outInfo.Move = GetNextMove(p);
//                         return outInfo;
//                     }
// 
//                     if (_currentBot.Ammo <= CRITICAL_AMMO)
//                     {
//                         Position p = GetMinPath(_ammoPoints);
// 
//                         if (p.Equals(_currentBot.Location))
//                             _currentBot.Ammo = _MaxAmmor;
// 
//                         outInfo.Move = GetNextMove(p);
//                         return outInfo;
//                     }
//                 }
//             }
// 
//             return outInfo;
//         }
// 
//         private BotMove GetNextMove(Position pos)
//         {
//             BotMove move = _currentBot.GetNextMove(pos);
// 
//             return move;
//         }
// 
//         private bool IsWeOnDeadPoint()
//         {
//             if(_currentBot.Location.Equals(_ammoPoints[0]) ||
//                _currentBot.Location.Equals(_ammoPoints[1]) ||
//                _currentBot.Location.Equals(_hitPoints[0]) ||
//                _currentBot.Location.Equals(_hitPoints[1]))
//                 return true;
//             return false;
//         }
// 
//         private void UpdateBotEnemyList(List<Enemy> enemys)
//         {
//             if (enemys!= null && enemys.Count > 0)
//             {
//                 foreach (Enemy en in enemys)
//                 {
//                     if (!_enemies.ContainsKey(en.ID))
//                     {
//                         AddEnemy(en);
//                     }
//                     else
//                     {
//                         _enemies[en.ID].HitPoints = en.HitPoints;
//                         _enemies[en.ID].Position = en.Position;
//                     }
//                 }
//             }
// 
//             RemoveDeadEnemy();
// 
//             if (_enemies.Count == 0 && _FreeMoves > 0)
//             {
//                 _FreeMoves--;
//             }
// 
//             if (_FreeMoves <= 0 && _enemies.Count == 0)
//             {
//                 _onlyMyCommand = true;
//                 _ActivBotID = _currentBot.ID;
//                 CRITICAL_AMMO = 0;
//             }
//         }
// 
//         private void AddEnemy(Enemy en)
//         {
//             if (!IsMyTank(en))
//             {
//                 if (_onlyMyCommand)
//                 {
//                     _FreeMoves = MAX_FREE_MOVES;
//                     CRITICAL_AMMO = 4;
//                     _ActivBotID = -1;
//                     _enemies.Clear();
//                     _onlyMyCommand = false;
//                 }
//                 _enemies.Add(en.ID, en);
//             }
// 
//             if (_onlyMyCommand && en.ID != _ActivBotID)
//                 _enemies.Add(en.ID, en);
//         }
// 
//         private void RemoveDeadEnemy()
//         {
//             for (int i = 0; i < _enemies.Count; i++)
//             {
//                 if (_currentBot.WasScaning)
//                 {
//                     if (GetDistance(_currentBot.Location, _enemies.Values[i].Position) <= _ScanRange
//                         && !IsEnemyExistIn(_enemies.Values[i], _currentBot.Targets))
//                     {
//                         _enemies.RemoveAt(i);
//                         i--;
//                         continue;
//                     }
//                 }
//                 if (_enemies.Values[i].HitPoints <= 0)
//                 {
//                     _enemies.RemoveAt(i);
//                     i--;
//                 }
//             }
//         }
// 
//         private void RemoveDeadTank()
//         {
// 
//             for (int i = 0; i < _comandosList.Count; i++)
//             {
//                 if (GetDistance(_currentBot.Location, _comandosList.Values[i].Location) < (_currentBot.WasScaning ? _ScanRange : _VisibilityRange)
//                     && !IsBotExistIn(_comandosList.Values[i], _currentBot.Targets) && _currentBot.ID != _comandosList.Values[i].ID)
//                 {
//                     _comandosList.RemoveAt(i);
//                     i--;
//                 }
//             }
// 
// //             if(!_comandosList.ContainsKey(_ActivBotID) && _onlyMyCommand)
// //             {
// //                 _ActivBotID = _comandosList.Values[0].ID;
// //             }
//         }
// 
//         private bool IsBotExistIn(SmartBot bot, List<Enemy> enemys)
//         {
//             foreach (Enemy en in enemys)
//                 if (en.Position.Equals(bot.Location) || en.Position.Equals(bot.PreviosLocation))
//                     return true;
//             return false;
//         }
// 
//         private bool IsMyTank(Enemy en)
//         {
//             foreach (SmartBot bot in _comandosList.Values)
//             {
//                 if (bot.Location.Equals(en.Position) || bot.PreviosLocation.Equals(en.Position))
//                     return true;
//             }
// 
//             return false;
//         }
// 
//         private int GetDistance(Position p1, Position p2)
//         {
//             int dx;
//             int dy;
//             int distance;
// 
//             dx = p1.X - p2.X;
//             dy = p1.Y - p2.Y;
// 
//             distance = dx * dx + dy * dy;
//             if (distance != 0)
//                 distance = (int)Math.Sqrt(distance);
// 
//             return distance;
//         }
// 
//         private Position GetMinPath(List<Position> poss)
//         {
//             Position retPos = null;
//             int dist = 0;
//             int mindist = GetDistance(new Position() { X = 0, Y = 0 }, new Position() { X = _ArenaWidth, Y = _ArenaHeight });
// 
//             if (poss.Count > 1)
//             {
//                 foreach (Position pos in poss)
//                 {
//                     dist = GetDistance(_currentBot.Location, pos);
//                     if (dist < mindist)
//                     {
//                         mindist = dist;
//                         retPos = pos;
//                     }
//                 }
//             }
//             else
//             {
//                 retPos = poss[0];
//             }
// 
//             return retPos;
//         }
// 
//         private Position GetMaxPath(List<Position> poss)
//         {
//             Position retPos = null;
//             int dist = 0;
//             int maxdist = 0;
// 
//             if (poss.Count > 1)
//             {
//                 foreach (Position pos in poss)
//                 {
//                     dist = GetDistance(_currentBot.Location, pos);
//                     if (dist > maxdist)
//                     {
//                         maxdist = dist;
//                         retPos = pos;
//                     }
//                 }
//             }
//             else
//             {
//                 retPos = poss[0];
//             }
// 
//             return retPos;
//         }
// 
//         private Enemy FindBestEnemyInAttrackRange()
//         {
//             Enemy retEnemy = null;
//             if(_enemies.Count > 0)
//             {
//                 int minHP = _MaxHP + 1;
//                 int dist = 0;
// 
//                 foreach (Enemy en in _enemies.Values)
//                 {
//                     if (IsEnemyExistIn(en, _currentBot.Targets) || IsEnemyExistIn(en, _currentBot.Attackers))
//                     {
//                         dist = GetDistance(_currentBot.Location, en.Position);
//                         if (dist <= _AttrackRange && minHP > en.HitPoints)
//                         {
//                             minHP = en.HitPoints;
//                             retEnemy = en;
//                         }
//                     }
//                 }
//             }
// 
//             return retEnemy;
//         }
// 
//         private Enemy FindBestEnemy()
//         {
//             Enemy retEnemy = null;
//             if (_enemies.Count > 0)
//             {
//                 int minDist = GetDistance(_currentBot.Location, _enemies.Values[0].Position);
//                 retEnemy = _enemies.Values[0];
//                 int dist = 0;
// 
//                 foreach (Enemy en in _enemies.Values)
//                 {
//                     dist = GetDistance(_currentBot.Location, en.Position);
//                     if (dist < minDist)
//                     {
//                         minDist = dist;
//                         retEnemy = en;
//                     }
//                 }
//             }
//             else if(_onlyMyCommand)
//             {
//                 retEnemy = new Enemy();
//                 retEnemy.Position = GetNearBotPosition();
//             }
// 
//             return retEnemy;
//         }
// 
//         private SmartBot GetNearBot()
//         {
//             Position p = GetNearBotPosition();
//             SmartBot retBot = null;
// 
//             foreach (SmartBot bot in _comandosList.Values)
//             {
//                 if (p.Equals(bot.Location))
//                     retBot = bot;
//             }
// 
//             return retBot;
//         }
// 
//         private Position GetNearBotPosition()
//         {
//             List<Position> poss = new List<Position>();
// 
//             foreach (SmartBot bot in _comandosList.Values)
//             {
//                 if (bot.ID != _currentBot.ID)
//                     poss.Add(bot.Location);
//             }
// 
//             return GetMinPath(poss);
//         }
// 
//         private bool IsEnemyExistIn(Enemy enemy, List<Enemy> enemys)
//         {
//             foreach (Enemy en in enemys)
//             {
//                 if (enemy.ID == en.ID)
//                     return true;
//             }
// 
//             return false;
//         }
// 
//         private Position GetRandPos()
//         {
//             Position pos = new Position();
//             Random rnd = new Random();
//             pos.X = rnd.Next(0, _ArenaWidth);
//             pos.Y = rnd.Next(0, _ArenaHeight);
//             return pos;
//         }
//     }

    public enum BotAction
    {
        //single actions
        MoveToHPPoint,
        MoveToAmmoPoint,
        AttackEnemy,
        GoToEnemy,
        FindEnemy,

        //command actions
        OcupatePoint,
        LineAttack,
    }

    public enum ActionType
    {
        Command,
        Single
    }

    public class Desition
    {
        public BotAction Action;
        public ActionType Type;
        public int Priority;
    }

    public class ComandManager
    {
        SortedList<int, SmartBot> _comanda;
        
        SortedList<int, Enemy> _enemies;
        
        public List<SmartBot> Comanda
        {
            get { return new List<SmartBot>(_comanda.Values);}
        }

        public List<Enemy> Enemies
        {
            get { return new List<Enemy>(_enemies.Values); }
        }

        public int Count
        {
            get { return _comanda.Count; }
        }

        public ComandManager()
        {
            _comanda = new SortedList<int, SmartBot>();
            _enemies = new SortedList<int, Enemy>();
        }

        public void AddBot(SmartBot bot)
        {
            if (!_comanda.ContainsKey(bot.ID))
                _comanda.Add(bot.ID, bot);
        }

        public void UpdateEnemies(List<Enemy> enemies)
        {
            foreach (Enemy enemy in enemies)
            {
                if (!_enemies.ContainsKey(enemy.ID))
                    if(!IsMyTank(enemy))
                        _enemies.Add(enemy.ID, enemy);
            }
        }

        public SmartBot GetBot(int ID)
        {
            if (_comanda.ContainsKey(ID))
                return _comanda[ID];
            return null;
        }

        //////////////////////////////////////////////////////////////////////////
        private bool IsMyTank(Enemy enemy)
        {
            foreach (SmartBot bot in _comanda.Values)
            {
                if (bot.Location.Equals(enemy.Position) || bot.PreviosLocation.Equals(enemy.Position))
                    return true;
            }
            return false;
        }

    }

    public class TacticManager
    {
        List<Position> _ammoPoints;
        List<Position> _hpPoints;
        int _arrenaWidth;
        int _arrenaHeight;
        ComandManager _commanda;
        SmartBot _currentBot;

        SortedList<int, List<DesitionEtap>> _botDesitions;

        ///PUBLIC SONA////////////////////////////////////////////////////////////

        public TacticManager()
        {
            _botDesitions = new SortedList<int, List<DesitionEtap>>();
        }

        public void Init(int arrenaWidth, int arrenaHeight,
                         List<Position> ammoPoints, List<Position> hpPoints,
                         ComandManager commanda)
        {
            _ammoPoints = ammoPoints;
            _hpPoints = hpPoints;
            _arrenaWidth = arrenaWidth;
            _arrenaHeight = arrenaHeight;
            _commanda = commanda;
        }

        public void UpdateDesitionListOfBot(int ID, List<Desition> desitions)
        {
            if (_botDesitions.ContainsKey(ID))
                MargeDesitions( _botDesitions[ID], ToListDesitionEtap(desitions));
            else
                _botDesitions.Add(ID, ToListDesitionEtap(desitions));

            CalculateEtap(_botDesitions[ID]);
        }

        public BotMove GetMove(SmartBot bot)
        {
            _currentBot = bot;
            DesitionEtap desition = GetFirstPriorityDesition(bot.ID);
            BotMove move = GetMoveFromDesition(desition);

            return move;
        }

        ///PRIVATE SONA///////////////////////////////////////////////////////////
        private class DesitionEtap
        {
            public Desition Desition;
            public int EtapCount;
            public int CurentEtap;
        }

        private List<DesitionEtap> ToListDesitionEtap(List<Desition> desitions)
        {
            List<DesitionEtap> desitionEtaps = new List<DesitionEtap>();

            foreach (Desition desition in desitions)
            {
                DesitionEtap desitionEtap = new DesitionEtap();

                desitionEtap.Desition = desition;
                desitionEtap.EtapCount = 0;
                desitionEtap.CurentEtap = 0;

                desitionEtaps.Add(desitionEtap);
            }

            return desitionEtaps;
        }

        private void MargeDesitions(List<DesitionEtap> des1, List<DesitionEtap> des2)
        {
            //update priority
            for(int i = 0; i < des1.Count; i++)
            {
                foreach (DesitionEtap des in des2)
                {
                    if (des1[i].Desition.Action == des.Desition.Action)
                    {
                        if (des.Desition.Priority == 0)
                        {
                            des1.RemoveAt(i);
                            i--;
                        }
                        else
                            des1[i].Desition.Priority = des.Desition.Priority;
                        break;
                    }
                }
            }

            //add new desitions
            bool isFind = false;
            for (int i = 0; i < des2.Count; i++)
            {
                foreach (DesitionEtap des in des1)
                {
                    if (des2[i].Desition.Action == des.Desition.Action)
                    {
                        isFind = true;
                        break;
                    }
                }
                if (!isFind)
                {
                    des1.Add(des2[i]);
                    isFind = false;
                }
            }
        }

        private void CalculateEtap(List<DesitionEtap> desitions)
        {
            for (int i = 0; i < desitions.Count; i++ )
            {
                if (desitions[i].EtapCount == 0)
                    desitions[i].EtapCount = GetEtapCount(desitions[i].Desition);
            }
        }

        private int GetEtapCount(Desition desition)
        {
            if (desition.Action == BotAction.MoveToAmmoPoint || desition.Action == BotAction.MoveToHPPoint)
                return 2;
            return 1;
        }

        private DesitionEtap GetFirstPriorityDesition(int ID)
        {
            DesitionEtap retDesition = new DesitionEtap();
            if(_botDesitions.ContainsKey(ID))
            {
                int maxPriority = 0;
               foreach (DesitionEtap des in _botDesitions[ID])
               {
                   if(maxPriority < des.Desition.Priority)
                   {
                       maxPriority = des.Desition.Priority;
                       retDesition = des;
                   }
               }
            }
            return retDesition;
        }

        private BotMove GetMoveFromDesition(DesitionEtap desition)
        {
            BotMove retMove = BotMove.Skip;

            if (_currentBot.TargetPosition != null && _currentBot.TargetPosition.Equals(_currentBot.Location))
                desition.CurentEtap++;

            switch (desition.Desition.Type)
            {
                case ActionType.Command:
                    retMove = GetCommandMove(desition);
                break;
                case ActionType.Single:
                    retMove = GetSingleMove(desition);
                break;
            }

            if (desition.CurentEtap + 1 == desition.EtapCount)
                _botDesitions[_currentBot.ID].Remove(desition);

            return retMove;
        }

        private BotMove GetCommandMove(DesitionEtap desition)
        {
            return BotMove.Skip;
        }

        private BotMove GetSingleMove(DesitionEtap desition)
        {
            BotMove retMove = BotMove.Skip;
            switch (desition.Desition.Action)
            {
                case BotAction.MoveToHPPoint:
                break;
                case BotAction.MoveToAmmoPoint:
                    _currentBot.TargetPosition = GetMinPath(_ammoPoints);
                    if(desition.CurentEtap == 0)
                        retMove = GetNextMove(_currentBot.TargetPosition);
                    if (desition.CurentEtap == 1)
                        retMove = BotMove.MoveAhead;
                break;
                case BotAction.GoToEnemy:
                break;
                case BotAction.FindEnemy:
                break;
            }
            return retMove;
        }

        private Position GetMinPath(List<Position> poss)
        {
            Position retPos = null;
            int dist = 0;
            int mindist = GetDistance(new Position() { X = 0, Y = 0 }, new Position() { X = _arrenaWidth, Y = _arrenaHeight });

            if (poss.Count > 1)
            {
                foreach (Position pos in poss)
                {
                    dist = GetDistance(_currentBot.Location, pos);
                    if (dist < mindist)
                    {
                        mindist = dist;
                        retPos = pos;
                    }
                }
            }
            else
            {
                retPos = poss[0];
            }

            return retPos;
        }

        private Position GetMaxPath(List<Position> poss)
        {
            Position retPos = null;
            int dist = 0;
            int maxdist = 0;

            if (poss.Count > 1)
            {
                foreach (Position pos in poss)
                {
                    dist = GetDistance(_currentBot.Location, pos);
                    if (dist > maxdist)
                    {
                        maxdist = dist;
                        retPos = pos;
                    }
                }
            }
            else
            {
                retPos = poss[0];
            }

            return retPos;
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

        public BotMove GetNextMove(Position targetPos)
        {
            BotMove nextMove = BotMove.Skip;

            if (_currentBot.Location.X < targetPos.X)
            {
                switch (_currentBot.Direction)
                {
                    case Direction.Down:
                        nextMove = BotMove.TurnLeft;
                        break;
                    case Direction.Up:
                        nextMove = BotMove.TurnRight;
                        break;
                    case Direction.Right:
                        nextMove = BotMove.MoveAhead;
                        break;
                    case Direction.Left:
                        nextMove = BotMove.TurnRight;
                        break;
                }
            }
            if (_currentBot.Location.X > targetPos.X)
            {
                switch (_currentBot.Direction)
                {
                    case Direction.Down:
                        nextMove = BotMove.TurnRight;
                        break;
                    case Direction.Up:
                        nextMove = BotMove.TurnLeft;
                        break;
                    case Direction.Right:
                        nextMove = BotMove.TurnLeft;
                        break;
                    case Direction.Left:
                        nextMove = BotMove.MoveAhead;
                        break;
                }
            }

            if (_currentBot.Location.X == targetPos.X)
            {
                if (_currentBot.Location.Y < targetPos.Y)
                {
                    switch (_currentBot.Direction)
                    {
                        case Direction.Down:
                            nextMove = BotMove.MoveAhead;
                            break;
                        case Direction.Up:
                            nextMove = BotMove.TurnLeft;
                            break;
                        case Direction.Right:
                            nextMove = BotMove.TurnRight;
                            break;
                        case Direction.Left:
                            nextMove = BotMove.TurnLeft;
                            break;
                    }
                }
                if (_currentBot.Location.Y > targetPos.Y)
                {
                    switch (_currentBot.Direction)
                    {
                        case Direction.Down:
                            nextMove = BotMove.TurnRight;
                            break;
                        case Direction.Up:
                            nextMove = BotMove.MoveAhead;
                            break;
                        case Direction.Right:
                            nextMove = BotMove.TurnLeft;
                            break;
                        case Direction.Left:
                            nextMove = BotMove.TurnRight;
                            break;
                    }
                }
            }

            return nextMove;
        }
    }

    public class StrategManager
    {
        int _attackRange;
        int _scanRange;
        int _splashRange;
        int _visibilityRange;
        int _ammoCount;
        int _hpCount;

        public StrategManager()
        {
            
        }

        public void Init(int attackRange, int scanRange, int splashRange, int visibilityRange,
                         int ammoCount, int hpCount)
        {
            _attackRange = attackRange;
            _scanRange = scanRange;
            _splashRange = scanRange;
            _visibilityRange = visibilityRange;
            _ammoCount = ammoCount;
            _hpCount = hpCount;
        }

        public List<Desition> GetListOfDesitions(SmartBot bot, List<SmartBot> friends, List<Enemy> enemies)
        {
            List<Desition> desitions = new List<Desition>();

            Desition desition = new Desition();

            desition.Action = BotAction.MoveToAmmoPoint;
            desition.Type = ActionType.Single;
            desition.Priority = 3;

            desitions.Add(desition);

            return desitions;
        }
    }
}
