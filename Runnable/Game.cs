using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureGame.Runnable
{
    public class Game
    {
        // DATA //
        // Initial state
        public Block initialBlock;

        // Cached Data
        private Block activeBlock;
        private List<Block> accessedBlocks;
        private List<string> displayedText;

        // Properties
        public bool IsGameActive { get { return activeBlock != null; } }
        public UserInputType InputType { get { return activeBlock.GetInputType(this); } }
        public Option[] AvailableOptions { get { return activeBlock.GetBlockOptions(this); } }
        public string CurrentTitleText { get { return activeBlock.GetBlockText(this, false); } }


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
                Program.DebugLog(string.Format("[Game] StartNewBlock found newBlock == activeBlock for blockID {0}. Make sure there are no unexitable loops!", "blockIDs have not been implemented!"), false);
                return;
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
    }
}
