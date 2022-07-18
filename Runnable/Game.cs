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
        private int currentOptionsCount;
        private List<Block> accessedBlocks;

        // Properties
        public bool IsGameActive { get { return activeBlock != null; } }
        public bool RequireInput { get { return activeBlock.GetInputType(this) != UserInputType.None; } }


        // FUNCTIONS //
        // Game Interface
        public void StartGame()
        {
            // Starts the game by setting the activeBlock to the initialBlock and resetting certain values
            activeBlock = initialBlock;
            currentOptionsCount = 0;
            accessedBlocks = new List<Block>();
        }

        public void FinishGame()
        {
            // Sets the active block to null
            activeBlock = null;
        }

        public void ContinueToNextBlock()
        {
            // Runs the default link for the current block
            activeBlock = activeBlock.RunDefaultLink(this);
        }

        public string GetCurrentBlockText()
        {
            return activeBlock.GetBlockText(this, false);
        }

        public string[] GetCurrentOptions()
        {
            // Gets the list of options from the active block and caches the amount.
            string[] options = activeBlock.GetBlockOptions(this);
            currentOptionsCount = (options != null ? options.Length : 0);

            // Returns the list of options.
            return options;
        }

        public string GetCurrentMetadata()
        {
            return null;
        }

        public void HandleUserInput(string userInput)
        {
            // Tries selecting an option and updates the active block based on the output.
            activeBlock = activeBlock.HandleInput(this, userInput);
        }
        
    }
}
