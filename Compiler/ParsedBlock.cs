using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextAdventureGame.Runnable;

namespace TextAdventureGame.Compiler
{
    public class ParsedBlock
    {
        // DATA //
        public string blockID;
        public GameText text;
        public DefaultLinkType defaultLinkType;
        public List<string> optionIDs;
        public string defaultLinkID;
        public bool isOption;
        public int indentLevel;


        // CONSTRUCTORS //
        public ParsedBlock(string newID, GameText newText, DefaultLinkType linkType, int indent, bool option)
        {
            blockID = newID;
            text = newText;
            defaultLinkType = linkType;
            optionIDs = new List<string>();
            defaultLinkID = "0";
            isOption = option;
            indentLevel = indent;
        }


        // FUNCTIONS //
        // Creating a Runnable version
        public Block ConvertToRunnable()
        {
            // Creates the runnable version (Note: does not fill in options and default links yet!)
            return new Block(blockID, text);
        }

        public void PopulateRunnableVersion(Block runnable, List<Block> allRunnables)
        {
            // Gets an array of option links, if there are any options
            if (optionIDs.Count > 0)
            {
                Block[] runnableBlocks = new Block[optionIDs.Count];

                for (int i = 0; i < optionIDs.Count; i++)
                {
                    // Finds the block for that ID
                    Block refBlock = allRunnables.Find(x => x.blockID == optionIDs[i]);

                    // Adds it to the array if it exists
                    if (refBlock != null)
                    {
                        runnableBlocks[i] = refBlock;
                    }

                    // Logs an error if it doesn't
                    else
                    {
                        Program.DebugLog(string.Format("[PopulateRunnableVersion] Could not find Block ID {0}, but it is referenced in Block ID {1}!", optionIDs[i], blockID), true);
                    }
                }
            }

            // Sets the default link block if there are no options
            else
            {
                Block defaultLinkBlock = allRunnables.Find(x => x.blockID == defaultLinkID);
            }

        }


        // Auto-Linking
        public void GenerateAllLinks()
        {

        }
    }
}
