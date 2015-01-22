using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TankArena.TankBot
{
    // Describes required tank bot functionality
    public interface IBot
    {
        // This method called at the start of the game. Called only once by GameArena.
        void Initialize(StartGameInfo startGameInfo);
        // Shoul return bot action according to your algorithm
        OutGameInfo ProcessMove(InGameInfo inGameInfo);
        // Name of your bot
        string GetName();
    }
}
