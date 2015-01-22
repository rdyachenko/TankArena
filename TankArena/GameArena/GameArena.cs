using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TankArena.TankBot;
using System.Threading;
using System.Drawing;


namespace TankArena.GameArena
{
    public class GameArena
    {
        #region Local variables

        private GameUI gameUI;
        private bool updateUI;
        private GameLogger gameLogger;
        private Random random;
        private bool isGameRunning = false;
        private BotFactory botFactory = new BotFactory();
        private List<IBot> botsInGame = new List<IBot>();
        private List<BotDetailsHandler> botsInGameDetails = new List<BotDetailsHandler>();
        private Thread gameThread = null;
        private Dictionary<string, Color> botColors = new Dictionary<string, Color>();
        private int defBotHitPoints = 100;
        private int defBotHitValue = 20;
        private int defBotSplashRange = 2;
        private int defBotAmmo = 5;
        private int defBotVisibilityRange = 6;
        private int defBotAttackRange = 12;
        private int defBotScanRange = 24;
        private int defGameMaxRounds = 400;

        #endregion

        #region Public variables

        public bool IsGameRunning { get { return isGameRunning; } }
        public int GameMoveDelay { get; set; }
        public int GameFieldWidth { get; set; }
        public int GameFieldHeight { get; set; }
        public int GameMaxRounds { get; set; }
        public string LogFileName { get { return gameLogger.LastLogFileName; } }

        public delegate void OnGameOverHandler(BotDetailsHandler winnerBotDetails);
        public event OnGameOverHandler OnGameOver;

        #endregion

        #region Public functionality

        public GameArena(GameUI gameUI)
        {
            this.gameUI = gameUI;
            updateUI = (gameUI != null);
            this.gameLogger = new GameLogger();
            GameMaxRounds = defGameMaxRounds;
            random = new Random((int)DateTime.Now.Ticks);
        }

        public bool StartGame()
        {
            return StartGame(updateUI);
        }

        public bool StartGame(bool updateUI)
        {
            if (!isGameRunning)
            {
                this.updateUI = updateUI;
                gameLogger.LogGameStart(GameFieldWidth, GameFieldHeight, GetHPPoints(), GetAmmoPoints());
                if (updateUI)
                {
                    gameUI.Initialize(GameFieldWidth, GameFieldHeight, GetHPPoints(), GetAmmoPoints());
                }
                InitializeBots();
                isGameRunning = true;
                gameThread = new Thread(this.Run);
                gameThread.Start();
                return true;
            }
            return false;
        }

        public void AddColor(string name, Color color)
        {
            if(!botColors.ContainsKey(name))
            {
                botColors.Add(name, color);
            }
            
        }
        public void EndGame()
        {
            isGameRunning = false;
            //gameThread.Join();
            //Thread.Sleep(GameMoveDelay);
            gameThread.Abort();     // Consider to reimplement this
        }

        public void AddBotToArena(int id)
        {
            if (!isGameRunning)
            {
                botsInGame.Add(botFactory.GetBotInstanceByID(id));
            }
        }

        public void RemoveBotFromArena(int id)
        {
            if (!isGameRunning)
            {
                Type typeOfBotToRemove = botFactory.GetBotInstanceByID(id).GetType();
                foreach (IBot bot in botsInGame)
                {
                    if (bot.GetType().Equals(typeOfBotToRemove))
                    {
                        botsInGame.Remove(bot);
                        break;
                    }
                }
            }
        }

        public void RemoveAllBotsFromArena()
        { 
            botsInGame.Clear();
            botsInGameDetails.Clear();
            botColors.Clear();
        }

        public Dictionary<int, string> GetAvailableBots()
        {
            return botFactory.GetAvailableBots();
        }
        #endregion

        #region Helper functionality

        private void InitializeBots()
        {
            botsInGameDetails.Clear();

            foreach (IBot bot in botsInGame)
            {
                StartGameInfo startInfo = new StartGameInfo();
                startInfo.HitPoints = defBotHitPoints;
                startInfo.AmmoCount = defBotAmmo;
                startInfo.VisibilityRange = defBotVisibilityRange;
                startInfo.AttrackRange = defBotAttackRange;
                startInfo.ScanRange = defBotScanRange;
                startInfo.SplashRange = defBotSplashRange;
                startInfo.StartPosition = GetRandomPosition();
                startInfo.StartDirection = (Direction) random.Next(4);
                startInfo.ArenaWidth = GameFieldWidth;
                startInfo.ArenaHeight = GameFieldHeight;
                startInfo.HPPoints = GetHPPoints();
                startInfo.AmmoPoints = GetAmmoPoints();                

                BotDetailsHandler details = new BotDetailsHandler();
                details.Bot = bot;
                details.HitPoints = defBotHitPoints;
                details.AmmoCount = defBotAmmo;
                details.LastMove = null;
                details.BotPosition = new Position() { X = startInfo.StartPosition.X, Y = startInfo.StartPosition.Y };
                details.BotDirection = startInfo.StartDirection;
                details.ArenaWidth = GameFieldWidth;
                details.ArenaHeight = GameFieldHeight;
                Color tmp;
                botColors.TryGetValue(bot.GetName(),out tmp);
                details.botColor = tmp;
                botsInGameDetails.Add(details);

                try
                {
                    bot.Initialize(startInfo);      // You better cehck your initialize con in your bot
                }
                catch 
                {
                    botsInGameDetails[botsInGameDetails.Count - 1].HitPoints = 0;
                }
            }
        }

        private Position GetRandomPosition()
        {
            // Note: 2 or more bots can be in 1 start position
            return (new Position() { X = random.Next(GameFieldWidth), Y = random.Next(GameFieldHeight) });
        }

        private List<Position> GetHPPoints()
        {
            List<Position> targetList = new List<Position>();
            targetList.Add(new Position() { X = 1, Y = 1 } );
            targetList.Add(new Position() { X = GameFieldWidth - 2, Y = GameFieldHeight - 2 });
            return targetList;
        }

        private List<Position> GetAmmoPoints()
        {
            List<Position> targetList = new List<Position>();
            targetList.Add(new Position() { X = 1, Y = GameFieldHeight - 2 });
            targetList.Add(new Position() { X = GameFieldWidth - 2, Y = 1 });
            return targetList;
        }

        private List<Enemy> GetBotTargets(BotDetailsHandler botDetailsHandler)
        {
            int range = (botDetailsHandler.LastMove.Move == BotMove.Scan) ? defBotScanRange : defBotVisibilityRange;
            List<Enemy> targetList = new List<Enemy>();

            List<BotDetailsHandler> foundBotList = GetBotsInPosition(botDetailsHandler.BotPosition, range);            
            foreach (BotDetailsHandler foundBotDetails in foundBotList)
            {
                if (!botDetailsHandler.Equals(foundBotDetails) && (foundBotDetails.HitPoints > 0))
                {
                    Enemy foundEnemy = new Enemy();
                    foundEnemy.ID = botsInGameDetails.IndexOf(foundBotDetails);
                    foundEnemy.HitPoints = foundBotDetails.HitPoints;
                    foundEnemy.Position = new Position() { X = foundBotDetails.BotPosition.X, Y = foundBotDetails.BotPosition.Y };
                    targetList.Add(foundEnemy);
                }
            }
            return targetList;
        }

        private int GetDistance(Position pos1, Position pos2)
        {
            int dx;
            int dy;
            int distance;

            dx = pos1.X - pos2.X;
            dy = pos1.Y - pos2.Y;

            distance = dx*dx + dy*dy;
            if (distance != 0)
                distance = (int)Math.Sqrt(distance);

            return distance;
        }

        private List<BotDetailsHandler> GetBotsInPosition(Position position, int range)
        {
            List<BotDetailsHandler> targetList = new List<BotDetailsHandler>();

            foreach (BotDetailsHandler botDetails in botsInGameDetails)
            {
                if (GetDistance(position, botDetails.BotPosition) <= range)
                {
                    //if ((position.X != botDetails.BotPosition.X) || (position.Y != botDetails.BotPosition.Y))
                    {
                        targetList.Add(botDetails);
                    }
                }
            }

            return targetList;
        }

        private ShotDescription ProcessBotShot(BotDetailsHandler botDetails, Position targetPosition)
        {
            ShotDescription shotDescription = null;

            if (botDetails.AmmoCount > 0)
            {
                botDetails.AmmoCount--;         // Remove 1 ammo even if you missed

                if (GetDistance(botDetails.BotPosition, targetPosition) <= defBotAttackRange)
                {
                    shotDescription = new ShotDescription();
                    shotDescription.FromPosition = botDetails.BotPosition;
                    shotDescription.ToPosition = targetPosition;

                    List<BotDetailsHandler> targetList = GetBotsInPosition(targetPosition, defBotSplashRange);

                    Enemy enemy = new Enemy();
                    enemy.ID = botsInGameDetails.IndexOf(botDetails);
                    enemy.HitPoints = botDetails.HitPoints;
                    enemy.Position = new Position() { X = botDetails.BotPosition.X, Y = botDetails.BotPosition.Y };

                    foreach (BotDetailsHandler target in targetList)
                    {
                        target.HitPoints -= defBotHitValue;
                        if (target.HitPoints < 0)
                        {
                            target.HitPoints = 0;
                        }
                        else
                        {
                            target.AddAtacker(enemy);
                        }
                    }
                }
            }

            return shotDescription;
        }

        private bool ProcessExtraPoints(BotDetailsHandler botDetails, OutGameInfo outGameInfo)
        {
            bool isMoveAllowed = true;
            
            // Process Add hitpoints locations
            foreach (Position position in GetHPPoints())
            {
                if (botDetails.BotPosition.Equals(position))
                {
                    if (outGameInfo.Move != BotMove.MoveAhead)
                    {
                        isMoveAllowed = false;
                    }
                    else
                    {
                        botDetails.HitPoints = defBotHitPoints;
                    }
                    break;
                }
            }

            // Process Add ammo locations
            if (isMoveAllowed)
            {
                foreach (Position position in GetAmmoPoints())
                {
                    if (botDetails.BotPosition.Equals(position))
                    {
                        if (outGameInfo.Move != BotMove.MoveAhead)
                        {
                            isMoveAllowed = false;
                        }
                        else
                        {
                            botDetails.AmmoCount = defBotAmmo;
                        }
                        break;
                    }
                }
            }

            if (!isMoveAllowed)
            {
                botDetails.HitPoints = 0;
            }
            
            return isMoveAllowed;
        }

        private BotDetailsHandler GetWinner()
        {
            foreach (BotDetailsHandler botDetails in botsInGameDetails)
            { 
                if (botDetails.HitPoints > 0)
                {
                    return botDetails;
                }
            }
            return null;
        }

        #endregion

        #region Main game loop

        private void Run()
        {
            int moveCount = 0;
            int gameMaxRounds = GameMaxRounds;
            int liveBotCount = 0;
            int delay = GameMoveDelay;
            List<OutGameInfo> collectedMoves = new List<OutGameInfo>();
            List<ShotDescription> collectedShots = new List<ShotDescription>();

            // Main game loop
            while (isGameRunning)
            {
                // Collect Bot moves
                liveBotCount = 0;
                collectedMoves.Clear();
                collectedShots.Clear();
                for (int i = 0; i < botsInGameDetails.Count; i++)
                {
                    collectedMoves.Add(null);
                    if ((botsInGameDetails[i].HitPoints > 0))
                    {
                        InGameInfo inGameInfo = new InGameInfo();
                        inGameInfo.HitPoints = botsInGameDetails[i].HitPoints;
                        inGameInfo.Targets = GetBotTargets(botsInGameDetails[i]);
                        OutGameInfo outGameInfo = botsInGameDetails[i].ProcessMove(inGameInfo);
                        if (outGameInfo == null)
                        {
                            botsInGameDetails[i].HitPoints = 0;     // There is no place for exceptions in bots!
                        }
                        else
                        {
                            if (ProcessExtraPoints(botsInGameDetails[i], outGameInfo))
                            {
                                collectedMoves[i] = outGameInfo;
                                liveBotCount++;
                            }
                        }
                    }
                }

                // Apply collected moves
                for (int i = 0; i < botsInGameDetails.Count; i++)
                {
                    // Process this move
                    if (collectedMoves[i] != null)
                    {
                        switch (collectedMoves[i].Move)
                        {
                            case BotMove.TurnLeft:
                            case BotMove.TurnRight:
                                botsInGameDetails[i].UpdateDirection(collectedMoves[i].Move);
                                break;
                            case BotMove.MoveAhead:
                                botsInGameDetails[i].MoveAhead();
                                break;
                            case BotMove.Shoot:
                                {
                                    ShotDescription shotDescription = ProcessBotShot(botsInGameDetails[i], collectedMoves[i].TargetPosition);
                                    if (shotDescription != null)
                                        collectedShots.Add(shotDescription);
                                }
                                break;
                            case BotMove.Scan:
                            case BotMove.Skip:
                                // Do nothing
                                break;
                        }
                    }

                    // Update bot details last move
                    if (collectedMoves[i] == null)      // if dead
                    {
                        botsInGameDetails[i].LastMove = new OutGameInfo() { Move = BotMove.Skip };
                    }
                    else
                    {
                        botsInGameDetails[i].LastMove = collectedMoves[i];
                    }
                }

                // Wait to update UI
                moveCount++;
                gameLogger.LogMove(botsInGameDetails, collectedShots);
                if (updateUI)                       // if update UI enabled
                {
                    gameUI.Update(botsInGameDetails, collectedShots);
                    Thread.Sleep(delay);
                }
                if (!isGameRunning ||               // if game is over by user interrupt
                    (liveBotCount < 2) ||           // if game is over by bots count
                    (moveCount >= gameMaxRounds))   // if game is over by max rounds reached
                {
                    isGameRunning = false;
                    BotDetailsHandler winner = null;
                    
                    // If there is only one winner- specify it
                    if (liveBotCount < 2)
                        winner = GetWinner();

                    gameLogger.LogGameOver(winner);
                    if (updateUI)
                    {
                        gameUI.GameOver(winner);
                    }
                    if (OnGameOver != null)
                    {
                        OnGameOver(winner);
                    }
                }
            }
        }
        #endregion
    }
}
