using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureGame.Runnable
{
    public struct Option
    {
        // DATA //
        public string displayText;
        public Block newBlock;
        
        
        // CONSTRUCTORS //
        public Option(string text, Block block)
        {
            displayText = text;
            newBlock = block;
        }
    }
}
