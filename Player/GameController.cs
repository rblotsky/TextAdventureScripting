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

                // Gets the text to display
                string promptText = runnableGame.GetCurrentBlockText();
                string[] options = runnableGame.GetCurrentOptions();

                // Displays it
                ColourConsole.WriteLine(promptText, ConsoleColor.White);
                for(int i = 0; i < options.Length; i++)
                {
                    ColourConsole.Write(i + " >", ConsoleColor.Cyan);
                    ColourConsole.Write(options[i], ConsoleColor.White);
                    ColourConsole.Write("\n");
                }

                // Tells the player to enter input
                ColourConsole.WriteLine("Select an option by entering its number: ", ConsoleColor.Magenta);

                // Reads input and handles it if it's an integer
                string userInput = Console.ReadLine();
                if(int.TryParse(userInput, out int result))
                {
                    runnableGame.HandleUserInput(result);
                }
                
            }
        }
        
    }
}
