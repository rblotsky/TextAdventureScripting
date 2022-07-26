using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAScript.Runnable;

namespace TAScript.Player
{
    public class GameController
    {
        // CONSTANTS //
        public static readonly int MAX_PREV_TEXT_DISPLAYED = 1;

        // FUNCTIONS //
        public void PlayGame(Game runnableGame)
        {
            // Starts the game
            runnableGame.StartGame();

            // Asks for input to start
            ColourConsole.WriteLine("Press ENTER to start!", Program.PROMPT_COLOUR);
            Console.ReadLine();

            // While the game is active, displays its output and handles input
            while(runnableGame.IsGameActive)
            {
                // Clears what was written before
                Console.Clear();

                // Displays the already-displayed text
                for(int i = 0; i < runnableGame.PreviouslyDisplayedText.Length; i++)
                {
                    if (runnableGame.PreviouslyDisplayedText.Length - i <= MAX_PREV_TEXT_DISPLAYED)
                    {
                        ColourConsole.WriteLine(runnableGame.PreviouslyDisplayedText[i], ConsoleColor.Gray);
                        ColourConsole.WriteLine("---------------------------", ConsoleColor.DarkGray);
                    }
                }

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
                    //TODO
                }
            }

            // After it is finished, displays the list of blocks
            runnableGame.LogBlockGraph();
        }
        
    }
}
