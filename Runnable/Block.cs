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
        // Base data
        public int contextID;
        public GameText blockText;
        

        // FUNCTIONS //
        // Data Management
        public int GetContextID()
        {
            //TODO: Maybe use some other type of data? byte, long, etc.?
            //TODO: Figure out how to actually generate this ID.
            return 0;
        }


        // Interfacing Functions
        public virtual Block HandleInput(Game context, int inputValue)
        {
            // No initial value
            return null;
        }

        public virtual string GetBlockText(Game context, bool asOption)
        {
            // Returns the resolved text of the block with current context and whether it is an option
            return blockText.ResolveText(context, asOption);
        }

        public virtual string[] GetBlockOptions(Game context)
        {
            // Returns an empty array by default
            return new string[0];
        }
    }
}
