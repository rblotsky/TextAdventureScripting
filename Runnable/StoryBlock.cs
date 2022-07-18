using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureGame.Runnable
{
    public class StoryBlock : Block
    {
        // DATA //
        // Text
        public string returnPointText;

        // Links (organized in priority order)
        public Block reroute;
        public Block[] options;
        public Block continuePoint;
        public Block returnPoint;


        // FUNCTIONS //
        // Overrides
        public override Block HandleInput(Game context, int inputValue)
        {
            // Return value: returns the new game block, or null if it doesn't change.
            // If there is a reroute, follows the reroute.
            if (reroute != null)
            {
                return reroute;
            }

            // If there are options, tries selecting one.
            else if (options != null && options.Length > 0)
            {
                // Assumes input is within valid options range.
                return options[inputValue];
            }

            // If there is a continue point, continues.
            else if (continuePoint != null)
            {
                return continuePoint;
            }

            // If there is a return point, returns to it.
            else if (returnPoint != null)
            {
                return returnPoint;
            }

            // If there is nothing, returns null
            else
            {
                return null;
            }
        }

        public override string GetBlockText(Game context, bool asOption)
        {
            //TODO: If this block was the last visited block, adds the reroute text to the display!
            // Returns the resolved text of the block with current context and whether it is an option
            return blockText.ResolveText(context, asOption);
        }

        public override string[] GetBlockOptions(Game context)
        {
            // If there are no options, prints basic "Continue..."
            string[] optionText = null;
            if (options == null || options.Length == 0)
            {
                optionText = new string[1] { "Continue..." };
            }

            // Otherwise, returns a list of options
            else
            {
                optionText = new string[options.Length];
                for (int i = 0; i < options.Length; i++)
                {
                    optionText[i] = options[i].GetBlockText(context, true);
                }
            }
            // Returns the list of text
            return optionText;
        }
    }
}
