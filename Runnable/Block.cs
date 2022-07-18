using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureGame.Runnable
{
    public class Block
    {
        // DATA //
        // Identifier
        public int contextID;

        // Base data
        public GameText blockText;
        public string returnText;

        // Links
        public Block reroute;
        public Block[] options;
        public Block returnPoint;
        public Block continuePoint;


        // FUNCTIONS //
        // Data Management
        public int GetContextID()
        {
            //TODO: Maybe use some other type of data? byte, long, etc.?
            //TODO: Figure out how to actually generate this ID.
            return 0;
        }


        // Managing State
        public Block HandleUserInput(Game context, int inputValue)
        {
            //TODO: Follow order of priorities: reroute -> options -> return -> continue
            //NOTE: The user's input only really matters in the "options" stage, otherwise we just ignore it and take it as 
            //      permission to continue the game.

            // Return value: returns the new game block, or null if it doesn't change.
            // Try throwing an exception if it has no links whatsoever?
            return null;
        }

        public string GetBlockTextAfterSelection(Game context)
        {
            // Returns the resolved text of the block. We are currently NOT in options selection, it will display post-selection text.
            return blockText.ResolveText(context, false);
        }

        public string GetBlockTextAsOption(Game context)
        {
            // Returns the text as if it were in option selection
            return blockText.ResolveText(context, true);
        }

        public string[] GetBlockOptions(Game context)
        {
            // Gets a list of block text as options from the option links
            string[] optionText = new string[options.Length];
            for(int i = 0; i < options.Length; i++)
            {
                optionText[i] = options[i].GetBlockTextAsOption(context);
            }

            // Returns the list of text
            return optionText;
        }
    }
}
