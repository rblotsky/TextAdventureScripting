using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextAdventureGame.Runnable;

namespace TextAdventureGame.Compiler
{
    public class ParsedBlock
    {
        // DATA //
        public BlockID blockID;
        public GameText text;
        public DefaultLinkType defaultLinkType;
        public BlockID[] optionIDs;
        public BlockID defaultLinkID;
        public bool isOption;
        public int indentLevel;


        // CONSTRUCTORS //
        public ParsedBlock(BlockID newID, DefaultLinkType linkType, GameText newText, int indent, bool option)
        {
            blockID = newID;
            text = newText;
            defaultLinkType = linkType;
            optionIDs = new BlockID[0];
            defaultLinkID = BlockID.ZERO;
            isOption = option;
            indentLevel = indent;
        }


        // FUNCTIONS //
        // Creating a Runnable version
        public Block ConvertToRunnable()
        {
            // Creates the runnable version (Note: does not fill in options and default links yet!)
            return new Block(blockID, text);
        }

        public void PopulateRunnableVersion(Block runnable, List<Block> allRunnables)
        {
            // Gets an array of option links, if there are any options
            if (optionIDs.Length > 0)
            {
                Block[] runnableBlocks = new Block[optionIDs.Length];

                for (int i = 0; i < optionIDs.Length; i++)
                {
                    // Finds the block for that ID
                    Block refBlock = allRunnables.Find(x => x.blockID.Compare(optionIDs[i]));

                    // Adds it to the array if it exists
                    if (refBlock != null)
                    {
                        runnableBlocks[i] = refBlock;
                    }

                    // Logs an error if it doesn't
                    else
                    {
                        Program.DebugLog(string.Format("[PopulateRunnableVersion] Could not find Block ID {0}, but it is referenced in Block ID {1}!", optionIDs[i], blockID), true);
                    }
                }
            }

            // Sets the default link block if there are no options
            else
            {
                Block defaultLinkBlock = allRunnables.Find(x => x.blockID.Compare(defaultLinkID));

                // Logs an error if nothing was found
                if(defaultLinkBlock == null)
                {
                    Program.DebugLog(string.Format("[PopulateRunnableVersion] Block ID {0} was unable to link to either options or default! This means that it cannot be exited.", blockID.ToString()), true);
                }
            }

        }


        // Auto-Linking
        public void GenerateAllLinks(List<ParsedBlock> allBlocks)
        {
            //TODO: Figure out what to do with the reroutes

            // Tries getting options first
            optionIDs = GetOptionLinks(allBlocks);

            // If there are no options, uses the default link type to decide what to link to
            if(optionIDs.Length == 0)
            {
                if(defaultLinkType == DefaultLinkType.Continue)
                {
                    defaultLinkID = GetContinueLink(allBlocks);
                }

                else if(defaultLinkType == DefaultLinkType.Return)
                {
                    defaultLinkID = GetReturnLink(allBlocks);
                }
            }
        }

        public BlockID[] GetOptionLinks(List<ParsedBlock> allBlocks)
        {
            //TODO
            return new BlockID[0];
        }

        public BlockID GetRerouteLink(List<ParsedBlock> allBlocks)
        {
            //TODO: Reroute ID = Section hash (whatever algorithm I can use) + 0 (in second index)
            //NOTE: Maybe have reroutes just get replaced w/ an ID and then get parsed and located at runtime?
            return new BlockID(0);
        }

        public BlockID GetReturnLink(List<ParsedBlock> allBlocks)
        {
            // Returns a zero ID if this block isn't an option
            if(!isOption)
            {
                return BlockID.ZERO;
            }

            // Goes backwards in IDs until it finds one that is a prompt.
            BlockID checkID = new BlockID(blockID.id);
            checkID.AddToLastIndex(-1);
            ParsedBlock checkBlock = allBlocks.Find(x => x.blockID.Compare(checkID));
            
            while(checkBlock.isOption && checkBlock != null)
            {
                checkID.AddToLastIndex(-1);
                checkBlock = allBlocks.Find(x => x.blockID.Compare(checkID));
            }

            // When it finds one, returns it.
            return checkID;
        }

        public BlockID GetContinueLink(List<ParsedBlock> allBlocks)
        {
            // Returns a zero ID if this block isn't an option
            if(!isOption)
            {
                return BlockID.ZERO;
            }

            // Goes forwards in IDs until it finds one that is a prompt. If it reaches null, it drops the last index and continues.
            BlockID checkID = new BlockID(blockID.id);
            checkID.AddToLastIndex(1);
            ParsedBlock checkBlock = allBlocks.Find(x => x.blockID.Compare(checkID));

            // Loop will run infinitely until it returns a value.
            while(true)
            {
                // If the check block is a prompt, returns the ID
                if(!checkBlock.isOption)
                {
                    return checkID;
                }

                // If the check block is null, drops last index
                else if(checkBlock == null)
                {
                    checkID.RemoveLastIndex();
                }

                // If the ID length is now the minimum, can only be followed by null - so will return ZERO.
                else if(checkID.idLength == BlockID.MIN_ID_LEN)
                {
                    return BlockID.ZERO;
                }

                // Adds 1 to the last index and keeps going
                checkID.AddToLastIndex(1);
                checkBlock = allBlocks.Find(x => x.blockID.Compare(checkID));
            }
        }
    }
}
