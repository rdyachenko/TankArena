using TankArena.TankBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace TankArena.GameArena
{
    class BotFactory
    {
        string botsDirectory;
        private Dictionary<int, Type> availableBotTypes = new Dictionary<int, Type>();
        private Dictionary<int, string> availableBots = new Dictionary<int, string>();


        public BotFactory()
        {
            botsDirectory = AppDomain.CurrentDomain.BaseDirectory + "Bots";
            FindExternalBots();
        }

        public IBot GetBotInstanceByID(int id)
        { 
            if (availableBotTypes.Keys.Contains(id))
            {
                return (IBot)Activator.CreateInstance(availableBotTypes[id]);
            }
            return null;
        }

        public Dictionary<int, string> GetAvailableBots()
        {
            return availableBots;
        }

        private void FindExternalBots()
        {
            string[] files = Directory.GetFiles(botsDirectory, "*.dll");
            int i = 0;
            StringBuilder errMessages = new StringBuilder();

            foreach (string fileName in files)
        	{
                try
                {
                    Assembly assembly = Assembly.LoadFile(fileName);

                    foreach (Type type in assembly.GetTypes())
                    {
                        Type iType = type.GetInterface(typeof(IBot).FullName);

                        if (iType != null)
                        {
                            IBot externalBot = (IBot)Activator.CreateInstance(type);
                            availableBotTypes.Add(i, type);
                            availableBots.Add(i++, externalBot.GetName());
                        }
                    }
                }
                catch (Exception ex)
                {
                    errMessages.AppendLine(fileName).Append("\t");
                    errMessages.AppendLine(ex.Message).AppendLine();
                }
	        }

            // Error handling
            if (!String.IsNullOrEmpty(errMessages.ToString()))
            {
                MessageBox.Show(errMessages.ToString(), "Errors in bot DLLs");
            }
        }
    }
}
