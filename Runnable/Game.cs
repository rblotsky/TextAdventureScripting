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
        private List<StoryBlock> accessedBlocks;

        // Properties
        public bool IsGameActive { get { return activeBlock != null; } }


        // FUNCTIONS //
        // Game Interface
        public void StartGame()
        {
            // Starts the game by setting the activeBlock to the initialBlock and resetting certain values
            activeBlock = initialBlock;
            currentOptionsCount = 0;
            accessedBlocks = new List<StoryBlock>();
        }

        public void FinishGame()
        {
            // Sets the active block to null
            activeBlock = null;
        }

        public string GetCurrentBlockText()
        {
            return activeBlock.GetBlockText(this, false);
        }

        public string[] GetCurrentOptions()
        {
            // Gets the list of options from the active block and caches the amount.
            string[] options = activeBlock.GetBlockOptions(this);
            currentOptionsCount = options.Length;

            // Returns the list of options.
            return options;
        }

        public string GetCurrentMetadata()
        {
            return null;
        }

        public bool HandleUserInput(int inputValue)
        {
            // Attempts selecting an option. Returns true if succeeded, false if could not select any option.
            // If within valid range, sends input to the active block.
            if(inputValue > 0 && inputValue < currentOptionsCount)
            {
                // Gets the new block and sets the current block to that. If it's null, finishes the game.
                Block newBlock = activeBlock.HandleInput(this, inputValue);
                if(newBlock == null)
                {
                    Program.DebugLog(string.Format("Block ID {0} w/ text {1} returned NULL for input value {2}!", activeBlock.GetContextID(), activeBlock.GetBlockText(this,true), inputValue), false);
                    FinishGame();
                }

                else
                {
                    activeBlock = newBlock;
                }

                // Returns true after it successfully handles input
                return true;
            }

            // Returns false if input was invalid
            return false;
        }
        
    }
}
