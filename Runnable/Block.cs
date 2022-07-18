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

        // Links Links
        public Block defaultLink;
        public Block[] options;


        // CONSTRUCTORS //
        public Block(GameText text)
        {
            blockText = text;
        }


        // FUNCTIONS //
        // Data Management
        public int GetContextID()
        {
            //TODO: Maybe use some other type of data? byte, long, etc.?
            //TODO: Figure out how to actually generate this ID.
            return 0;
        }


        // Interfacing Functions
        public virtual Block HandleInput(Game context, string input)
        {
            // Tries parsing input
            if(int.TryParse(input, out int value))
            {
                // If it's a valid option, returns the block for that option.
                if(value >= 0 && value < options.Length)
                {
                    return options[value];
                }
            }

            // Returns the same block by default
            return this;
        }

        public virtual string GetBlockText(Game context, bool asOption)
        {
            // Returns the resolved text of the block with current context and whether it is an option
            return blockText.ResolveText(context, asOption);
        }

        public virtual string[] GetBlockOptions(Game context)
        {
            // Returns a list of options if there are any
            if(options != null)
            {
                string[] optionsList = new string[options.Length];
                for (int i = 0; i < options.Length; i++)
                {
                    optionsList[i] = options[i].GetBlockText(context, true);
                }

                // Returns the list of text
                return optionsList;
            }

            // If no options, returns null and logs a warning (this should not be called when there are no options)
            return null;
        }

        public virtual UserInputType GetInputType(Game context)
        {
            // If there are options, input type is Option
            if(options != null)
            {
                return UserInputType.Option;
            }

            // Otherwise, None
            else
            {
                return UserInputType.None;
            }

        }

        public Block RunDefaultLink(Game context)
        {
            return defaultLink;
        }
    }
}
