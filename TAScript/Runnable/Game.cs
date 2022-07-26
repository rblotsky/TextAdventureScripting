using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAScript.Runnable
{
    public class Game
    {
        // DATA //
        // Initial state
        public Block initialBlock;

        // Variables
        public Dictionary<int, int> variables = new Dictionary<int, int>();

        // Cached Data
        private Block activeBlock;
        private List<Block> accessedBlocks;
        private List<string> displayedText;

        // Properties
        public bool IsGameActive { get { return activeBlock != null; } }
        public UserInputType InputType { get { return activeBlock.GetInputType(this); } }
        public Option[] AvailableOptions { get { return activeBlock.GetBlockOptions(this); } }
        public string CurrentTitleText { get { return activeBlock.GetBlockText(this, false); } }
        public Block[] PreviousBlocksOrdered { get { return accessedBlocks.ToArray(); } }
        public string[] PreviouslyDisplayedText { get { return displayedText.ToArray(); } }


        // FUNCTIONS //
        // Game Interface
        public void StartGame()
        {
            // Starts the game by setting the activeBlock to the initialBlock and resetting certain values
            activeBlock = initialBlock;
            accessedBlocks = new List<Block>();
            displayedText = new List<string>();
        }

        public void Continue()
        {
            // Will continue if there is no input required
            if (InputType == UserInputType.None)
            {
                StartNewBlock(activeBlock.RunDefaultLink(this));
            }
        }

        public void StartNewBlock(Block newBlock)
        {
            // If the new block is the same as the active block, does nothing
            if(activeBlock == newBlock)
            {
                DebugLogger.DebugLog(string.Format("[Game] StartNewBlock found newBlock == activeBlock for blockID {0}. Make sure there are no unexitable loops!", "blockIDs have not been implemented!"), false);
                return;
            }

            // Gets the section hash from the old and new block
            int currentSectionHash = activeBlock.blockID.id[0];
            int newSectionHash = activeBlock.blockID.id[0];

            // If the new section is different from the old section, increments the variable for the new section
            if(newSectionHash != currentSectionHash)
            {
                AddToVariableByHash(newSectionHash, 1);
            }

            // Stores the last accessed block and its text
            accessedBlocks.Add(activeBlock);
            displayedText.Add(CurrentTitleText);

            // Goes to the new block
            activeBlock = newBlock;
        }

        public void HandleTextInput(string userInput)
        {
            // Gets the new block and starts it
            Block newBlock = activeBlock.HandleTextInput(this, userInput);
            StartNewBlock(newBlock);
        }
        
        public void HandleOptionSelection(Option selectedOption)
        {
            StartNewBlock(selectedOption.newBlock);
        }

        
        // Variables
        public int GetVariable(string varName)
        {
            // Gets a hash of the variable name
            int varNameHash = varName.GetHashCode();

            // Gets it by its hash
            return GetVariableByHash(varNameHash);
        }

        public int AddToVariable(string varName, int addAmount)
        {   
            // Gets a hash of the variable name
            int varNameHash = varName.GetHashCode();

            // Adds to the variable
            return AddToVariableByHash(varNameHash, addAmount);
        }

        private int AddToVariableByHash(int varNameHash, int addAmount)
        {
            // If the variable exists, adds to it, returns the new value
            if (variables.TryGetValue(varNameHash, out int value))
            {
                value += addAmount;
                variables[varNameHash] = value;
                return value;
            }

            // Otherwise, creates and adds to it, returns the value
            else
            {
                variables.Add(varNameHash, addAmount);
                return addAmount;
            }
        }

        private int GetVariableByHash(int varNameHash)
        {
            // If the variable exists, returns it
            if (variables.TryGetValue(varNameHash, out int value))
            {
                return value;
            }

            // Otherwise, adds it and returns 0
            else
            {
                variables.Add(varNameHash, 0);
                return 0;
            }
        }


        // Debug
        public void LogBlockGraph()
        {
            // Caches the string to display
            string allBlocksDisplay = "";

            // Caches the displayed blocks to prevent loops
            List<Block> displayedBlocks = new List<Block>();

            // Displays it
            Queue<Block> displayBlocks = new Queue<Block>();
            displayBlocks.Enqueue(initialBlock);

            while(displayBlocks.Count > 0)
            {
                // Gets this block
                Block thisBlock = displayBlocks.Dequeue();

                // If this block has already been displayed, does nothing
                if(displayedBlocks.Contains(thisBlock))
                {
                    continue;
                }

                // Adds its links to the queue
                if (thisBlock.optionBlocks != null)
                {
                    foreach (Block option in thisBlock.optionBlocks)
                    {
                        displayBlocks.Enqueue(option);
                    }
                }

                if (thisBlock.defaultLink != null)
                {
                    displayBlocks.Enqueue(thisBlock.defaultLink);
                }
                
                // Adds this block to the display
                allBlocksDisplay += thisBlock.blockID.ToString() + "\n";

                // Remembers that this block has been displayed
                displayedBlocks.Add(thisBlock);
            }

            // Displays the text
            //DebugLogger.DebugLog(allBlocksDisplay, false);
        }
    }
}
