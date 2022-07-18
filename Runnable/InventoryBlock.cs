using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureGame.Runnable
{
    public class InventoryBlock : Block
    {
        // FUNCTIONS //
        // Override Functions
        public override Block HandleInput(Game context, int inputValue)
        {
            //TODO: Update which item is currently selected, and return the same block.
            //      If the player selected to return, returns to the last visited StoryBlock.
            return null;
        }

        public override string GetBlockText(Game context, bool asOption)
        {
            return base.GetBlockText(context, asOption);
        }
        
        public override string[] GetBlockOptions(Game context)
        {
            //TODO: Return the list of item text from the game context.
            return base.GetBlockOptions(context);
        }
    }
}
