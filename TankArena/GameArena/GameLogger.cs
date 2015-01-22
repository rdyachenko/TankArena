using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TankArena.TankBot;
using System.Xml.Serialization;
using TankArena.GameArena.LoggerXML;

namespace TankArena.GameArena
{
    class GameLogger
    {
        #region Local variables

        private FileStream fileStream = null;
        private static string dateStrTemplate = "yy-MM-dd_HH-mm-ss";
        private static string logFileNameTemplate = @".\Logs\btl_%d.log";
        private string logFileName = "";
        private GameHeaderLog gameHeader;
        private List<GameMovesLog> gameMovesList;

        #endregion

        #region Public functionality

        public GameLogger()
        {
        }

        public void LogGameStart(int gameFieldWidth, int gameFieldHeight, List<Position> hpPoints, List<Position> ammoPoints)
        {
            InitGameHeaderAndList(gameFieldWidth, gameFieldHeight, hpPoints, ammoPoints);
        }

        public void LogMove(List<BotDetailsHandler> botsInGameDetails, List<ShotDescription> collectedShots)
        {
            SaveGameMove(botsInGameDetails, collectedShots);
        }

        public void LogGameOver(BotDetailsHandler winnerBotDetails)
        {
            UpdateGameHeader(winnerBotDetails);
            SerializeToDisk();
        }

        public string LastLogFileName
        {
            get { return logFileName; }
        }

        #endregion

        #region Helper functionality

        private string GetLogFileName()
        {
            string targetName = logFileNameTemplate;
            targetName = targetName.Replace("%d", DateTime.Now.ToString(dateStrTemplate));
            return targetName;
        }

        private void InitGameHeaderAndList(int gameFieldWidth, int gameFieldHeight, List<Position> hpPoints, List<Position> ammoPoints)
        {
            gameHeader = new GameHeaderLog(gameFieldWidth, gameFieldHeight, hpPoints, ammoPoints);
            gameMovesList = new List<GameMovesLog>();
        }

        private void UpdateGameHeader(BotDetailsHandler winnerBotDetails)
        {
            gameHeader.RoundsCount = gameMovesList.Count;
            gameHeader.HasWinner = (winnerBotDetails != null);
            
            if (gameHeader.HasWinner)
            {
                gameHeader.WinnerBotName = winnerBotDetails.Name;
                gameHeader.WinnerBotTypeName = winnerBotDetails.TypeName;
            }
        }

        private void SaveGameMove(List<BotDetailsHandler> botsInGameDetails, List<ShotDescription> collectedShots)
        {
            GameMovesLog gameMovesLog = new GameMovesLog(botsInGameDetails);
            gameMovesList.Add(gameMovesLog);

            if (gameMovesList.Count == 1)
            {
                gameHeader.BotsCount = botsInGameDetails.Count;
            }
        }

        private void SerializeToDisk()
        {
            // Open file
            if (fileStream != null)
            {
                fileStream.Close();
            }
            logFileName = GetLogFileName();
            fileStream = new FileStream(logFileName, FileMode.CreateNew);

            // Setup serializers
            XmlSerializer gameHeaderXmlSerializer = new XmlSerializer(typeof(GameHeaderLog));
            XmlSerializer gameMovesXmlSerializer = new XmlSerializer(typeof(GameMovesLog));

            // Log
            gameHeaderXmlSerializer.Serialize(fileStream, gameHeader);
            foreach (GameMovesLog gameMovesLog in gameMovesList)
            {
                gameMovesXmlSerializer.Serialize(fileStream, gameMovesLog);
            }

            // Close file
            fileStream.Close();
            fileStream = null;
        }

        #endregion
    }
}
