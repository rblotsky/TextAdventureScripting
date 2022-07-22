using System;
using System.IO;
using System.Linq;
using TextAdventureGame.Runnable;
using TextAdventureGame.Compiler;
using TextAdventureGame.Player;

namespace TextAdventureGame
{

    class Program
    {
        // DATA //
        // Constants
        public static ConsoleColor ERROR_COLOUR = ConsoleColor.Red;
        public static ConsoleColor PROMPT_COLOUR = ConsoleColor.Cyan;
        public static ConsoleColor INFO_COLOUR = ConsoleColor.White;
        public static ConsoleColor DEBUG_COLOUR = ConsoleColor.Yellow;

        // Cached Data
        private static bool isDebug = false;


        // FUNCTIONS //
        // Main
        static void Main(string[] args)
        {
            // Sets debug value
            if(args.Contains("--debug"))
            {
                isDebug = true;
            }

            // Prints prompt
            ColourConsole.WriteLine("Enter a text file to compile into a game. " +
                "\nThis program will output all compiler data as it runs.", INFO_COLOUR);
            ColourConsole.WriteLine("Enter text:", PROMPT_COLOUR);

            // Gets the user input
            string inputFilePath = Console.ReadLine();

            // Creates the compiler w/ default options
            Compiler.Compiler compilerToUse = new Compiler.Compiler();

            // Tries compiling
            Game compiledGame = CompileGameFromFile(inputFilePath, compilerToUse);

            // Tries playing (this won't actually do anything right now)
            if(compiledGame != null)
            {
                new GameController().PlayGame(compiledGame);
            }
            else
            {
                ColourConsole.WriteLine("[Main] Your input file could not be compiled!", ERROR_COLOUR);
            }
        }


        // Debugging
        public static void DebugLog(string text, bool isError)
        {
            if (isDebug)
            {
                ColourConsole.WriteLine(text, (isError ? ERROR_COLOUR : DEBUG_COLOUR));
            }
        }


        // Compilation
        private static Game CompileGameFromFile(string absoluteFilePath, Compiler.Compiler compiler)
        {
            // Caches a return value
            Game returnValue = null;

            // Starts reading the file if it exists, logs an error if not
            StreamReader fileReader = null;
            try
            {
                fileReader = new StreamReader(absoluteFilePath);
            }
            catch(IOException e)
            {
                ColourConsole.WriteLine(string.Format("[CompileGameFromFile] Could not load data!:\n {0}", e.Message), ERROR_COLOUR);
            }

            // If the file does exist, reads the entire file and compiles it into a game.
            if(fileReader != null)
            {
                // Extracts all text and trims lead/trail whitespace
                string fileText = fileReader.ReadToEnd().Trim();

                // Closes the file reader
                fileReader.Close();

                // Compiles the text as best as it can. All compilation errors will be logged by the compiler and this doesn't care about that.
                returnValue = compiler.CompileGame(fileText);
            }
            
            // Returns the cached return value
            return returnValue;
        }
    }
}
