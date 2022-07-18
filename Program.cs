using System;
using System.IO;
using TextAdventureGame.Runnable;
using TextAdventureGame.Compiler;
namespace TextAdventureGame
{

    class Program
    {
        // DATA //
        public static ConsoleColor ERROR_COLOUR = ConsoleColor.Red;
        public static ConsoleColor PROMPT_COLOUR = ConsoleColor.Blue;
        public static ConsoleColor INFO_COLOUR = ConsoleColor.White;


        // FUNCTIONS //
        static void Main(string[] args)
        {
            // Prints prompt
            ColourConsole.WriteLine("Enter a text file to compile into a game. " +
                "\nThis program will output all compiler data as it runs.", INFO_COLOUR);
            ColourConsole.WriteLine("Enter text:", PROMPT_COLOUR);

            // Gets the user input
            string inputFilePath = Console.ReadLine();

            // Creates the compiler w/ default options
            AdventureCompiler compilerToUse = new AdventureCompiler();

            // Tries compiling
            Game compiledGame = CompileGameFromFile(inputFilePath, compilerToUse);

            // Tries playing (this won't actually do anything right now)
            if(compiledGame != null)
            {
                compiledGame.PlayGame();
            }
            else
            {
                ColourConsole.WriteLine("[Main] Your input file could not be compiled!", ERROR_COLOUR);
            }
        }

        private static Game CompileGameFromFile(string absoluteFilePath, AdventureCompiler compiler)
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
