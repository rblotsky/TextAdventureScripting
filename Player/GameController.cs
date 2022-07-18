using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextAdventureGame.Runnable;

namespace TextAdventureGame.Player
{
    public class GameController
    {
        // FUNCTIONS //
        public void PlayGame(Game runnableGame)
        {
            // Starts the game
            runnableGame.StartGame();

            // While the game is active, displays its output and handles input
            while(runnableGame.IsGameActive)
            {
                // Clears what was written before
                Console.Clear();

                //TODO: Continue or prompt for input depending on whether input is required.
            }
        }
        
    }
}
