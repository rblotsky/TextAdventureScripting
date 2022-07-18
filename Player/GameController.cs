using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureGame.Player
{
    public class GameController
    {
        //TODO: The GameController should be a superclass with functions to display output and provide input into the game.
        //      Making it a superclass allows integration with other UI or interface systems.
        //      All the real logic is handled within the Runnable classes, meaning this just needs to loop and display
        //      whatever is within the current Block.
        //      In the future, if I add new features like Maps, Inventory, etc. all that I need to do is add new virtual
        //      functions to this class that can be overridden.

        //NOTE: In the future, I need to ensure that this class does exactly ZERO modification of anything within the blocks.
    }
}
