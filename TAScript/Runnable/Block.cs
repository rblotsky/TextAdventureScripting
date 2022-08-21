using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAScript.Runnable
{
    public class Block
    {
        // DATA //
        // Base data
        public BlockID blockID;
        public GameText blockText;

        // Links
        public Block defaultLink;
        public Block[] optionBlocks;

        // Conditionals
        public AbstractConditional[] blockConditionals;

        // Var Modifiers
        public BlockCompletionFunction[] completionFunctions;


        // CONSTRUCTORS //
        public Block(BlockID id, GameText text)
        {
            blockID = id;
            blockText = text;
        }


        // FUNCTIONS //
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
                List<Option> optionsList = new List<Option>();
                for (int i = 0; i < optionBlocks.Length; i++)
                {
                    // Only adds the option if it passes its conditionals
                    if (optionBlocks[i].EvaluateConditionals(context))
                    {
                        optionsList.Add(new Option(optionBlocks[i].GetBlockText(context, true), optionBlocks[i]));
                    }
                }

                // Returns the list of options.
                return optionsList.ToArray();
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


        // Command functions
        public void RunCompletionFunctions(Game context)
        {
            foreach (BlockCompletionFunction function in completionFunctions)
            {
                function.RunModifier(context);
            }
        }

        public bool EvaluateConditionals(Game context)
        {
            // Iterates over all the conditionals, returning false if any are false.
            foreach (AbstractConditional conditional in blockConditionals)
            {
                if (!conditional.RunConditional(context))
                {
                    return false;
                }
            }

            // Returns true if all conditionals pass
            return true;
        }
    }
}
