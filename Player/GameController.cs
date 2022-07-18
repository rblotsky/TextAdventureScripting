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

                // Displays the already-displayed text
                //TODO

                // Gets the data for the current game block
                string currentText = runnableGame.CurrentTitleText;
                Option[] currentOptions = runnableGame.AvailableOptions;
                UserInputType inputRequired = runnableGame.InputType;

                // Displays info for this level
                ColourConsole.WriteLine(currentText, ConsoleColor.White);

                // If there are options, displays them
                if(currentOptions != null)
                {
                    for(int i = 0; i < currentOptions.Length; i++)
                    {
                        ColourConsole.Write(i + " > ", ConsoleColor.Cyan);
                        ColourConsole.WriteLine(currentOptions[i].displayText, ConsoleColor.White);
                    }
                }

                // Prompts for input depending on what the input type is
                if(inputRequired == UserInputType.None)
                {
                    ColourConsole.WriteLine("Press ENTER to continue...", ConsoleColor.Magenta);
                    Console.ReadLine();
                    runnableGame.Continue();
                }

                else if(inputRequired == UserInputType.Option)
                {
                    ColourConsole.Write("Select an option to continue: ", ConsoleColor.Magenta);
                    if(int.TryParse(Console.ReadLine(), out int result))
                    {
                        if(result >= 0 && result < currentOptions.Length)
                        {
                            runnableGame.HandleOptionSelection(currentOptions[result]);
                        }
                    }
                }

                else if(inputRequired == UserInputType.Text)
                {
                    ColourConsole.Write("I have no idea what to do for text input yet? Maybe have the Block provide a string that says what the prompt should be.");
                    //TODO
                }
            }
        }
        
    }
}
