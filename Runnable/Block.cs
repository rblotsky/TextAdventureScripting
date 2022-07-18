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
        public string blockID;
        public GameText blockText;

        // Links Links
        public Block defaultLink;
        public Block[] optionBlocks;


        // CONSTRUCTORS //
        public Block(string id, GameText text)
        {
            blockID = id;
            blockText = text;
        }


        // FUNCTIONS //
        // Data Management
        // Interfacing Functions
        public virtual Block HandleTextInput(Game context, string input)
        {
            // Returns the same block by default: this function is only really needed for UserInputType.Text
            return this;
        }

        public virtual string GetBlockText(Game context, bool asOption)
        {
            // Returns the resolved text of the block with current context and whether it is an option
            return blockText.ResolveText(context, asOption);
        }

        public virtual Option[] GetBlockOptions(Game context)
        {
            // Returns a list of options if there are any
            if(optionBlocks != null)
            {
                Option[] optionsList = new Option[optionBlocks.Length];
                for (int i = 0; i < optionBlocks.Length; i++)
                {
                    optionsList[i] = new Option(optionBlocks[i].GetBlockText(context, true), optionBlocks[i]);
                }

                // Returns the list of options
                return optionsList;
            }

            // If no options, returns null
            return null;
        }

        public virtual UserInputType GetInputType(Game context)
        {
            // If there are options, input type is Option
            if(optionBlocks != null)
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
