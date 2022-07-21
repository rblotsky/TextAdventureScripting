using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TextAdventureGame.Compiler
{
    public class Compiler
    {
        // DATA //
        // Constants
        public static readonly Regex COMMENT_REGEX = new Regex(@"\/\/.*");
        public static readonly Regex SECTION_HEADER_REGEX = new Regex(@"^$\s*(\w+)");
        public static readonly Regex BLOCK_REGEX = new Regex(@"([-]+|[>]+)");
        public static readonly Regex REROUTE_REGEX = new Regex(@"@(\w+)$");


        // CONSTRUCTORS //
        //TODO


        // FUNCTIONS //
        public Runnable.Game CompileGame(string sourceText)
        {
            //TODO: Read all lines, make each block w/ its text and give it an ID.
            //      Afterwards, autogenerate all their links, and then create Blocks from them and link those.
            //      Finally, add the Blocks into a new Game, and return that.

            // Split sourceText into an array of lines
            string[] lines = sourceText.Split("\n");

            // Caches some important data
            int currentSectionHash = 0;
            BlockID currentBlockID = BlockID.ZERO;
            string currentBlockText = "";

            // Creates a list of parsed blocks
            List<ParsedBlock> parsedBlocks = new List<ParsedBlock>();

            // Iterates over all lines, creating blocks where necessary.
            foreach(string line in lines)
            {
                // First removes the comments from the line
                string editedLine = COMMENT_REGEX.Replace(line, "");

                // Then, checks if this is a section header. If so, updates the current section hash and block ID.
                Match regexMatch = SECTION_HEADER_REGEX.Match(editedLine);

                if(regexMatch.Success)
                {
                    currentSectionHash = GetStringHashInt(regexMatch.Groups[0].Value);
                    currentBlockID = new BlockID(currentSectionHash, BlockID.MIN_INDEX_VAL);
                    continue;
                }
                
                // If we are not in a section, does nothing
                if(currentSectionHash == 0)
                {
                    continue;
                }

                // If we are not in a section, checks if this is a prompt or option. If it is, closes the last block and creates a new one.
                regexMatch = BLOCK_REGEX.Match(editedLine);

                if (regexMatch.Success)
                {
                }
            }

            // Returns the generated game
            return null;
        
        }


        // Static
        public static int GetStringHashInt(string stringToHash)
        {
            // Returns a hash of the string
            return stringToHash.GetHashCode();
        }
    }
}
