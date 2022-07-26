using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAScript.Runnable;

namespace TAScript.Compiler
{
    public class ParsedBlock
    {
        // DATA //
        // ID
        public BlockID blockID;

        // Text
        public GameText text;

        // Links
        public DefaultLinkType defaultLinkType;
        public BlockID[] optionIDs;
        public BlockID defaultLinkID;
        public bool isOptionBlock;
        public string rerouteSection;

        // Conditionals
        public List<AbstractConditional> blockConditionals = new List<AbstractConditional>();


        // CONSTRUCTORS //
        public ParsedBlock(BlockID newID, DefaultLinkType linkType, GameText newText, bool option, string reroute)
        {
            blockID = newID;
            text = newText;
            defaultLinkType = linkType;
            optionIDs = new BlockID[0];
            defaultLinkID = BlockID.ZERO;
            isOptionBlock = option;
            rerouteSection = reroute;
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
                    Block refBlock = allRunnables.Find(x => x.blockID.Equals(optionIDs[i]));

                    // Adds it to the array if it exists
                    if (refBlock != null)
                    {
                        runnableBlocks[i] = refBlock;
                    }

                    // Logs an error if it doesn't
                    else
                    {
                        DebugLogger.DebugLog(string.Format("[PopulateRunnableVersion] Could not find Block ID {0}, but it is referenced in Block ID {1}!", optionIDs[i], blockID), true);
                    }
                }

                // Adds options to the runnable block
                runnable.optionBlocks = runnableBlocks;
            }

            // Sets the default link block if there are no options
            else
            {
                Block defaultLinkBlock = allRunnables.Find(x => x.blockID.Equals(defaultLinkID));
                runnable.defaultLink = defaultLinkBlock;

                // Logs an error if nothing was found, but the reroute isn't END
                if(defaultLinkBlock == null)
                {
                    DebugLogger.DebugLog(string.Format("[PopulateRunnableVersion] Block ID {0} was unable to link to either options or default! This means that it cannot be exited.", blockID.ToString()), true);
                }
            }

            // Populates the block conditionals
            runnable.blockConditionals = blockConditionals.ToArray();

        }


        // Auto-Linking
        public void GenerateAllLinks(List<ParsedBlock> allBlocks)
        {
            // If the reroute section isn't empty, tries setting it as default link
            if(!string.IsNullOrEmpty(rerouteSection))
            {
                defaultLinkID = GetRerouteLink(allBlocks);
                return; // Reroutes CAN be null (@END) so we need to return to avoid making it a return link instead.
            }

            // If the default link ID is still ZERO, tries getting options and other default links
            if (defaultLinkID == BlockID.ZERO)
            {
                // Tries getting options first
                optionIDs = GetOptionLinks(allBlocks);

                // If there are no options, uses the default link type to decide what to link to
                if (optionIDs.Length == 0)
                {
                    if (defaultLinkType == DefaultLinkType.Continue)
                    {
                        defaultLinkID = GetContinueLink(allBlocks);
                    }

                    else if (defaultLinkType == DefaultLinkType.Return)
                    {
                        defaultLinkID = GetReturnLink(allBlocks);
                    }
                }
            }
        }

        public BlockID[] GetOptionLinks(List<ParsedBlock> allBlocks)
        {
            // Moves upwards one index and adds all options until the next prompt on that level
            List<BlockID> optionIDs = new List<BlockID>();
            BlockID checkID = new BlockID(blockID.id);
            checkID.AddIndex();
            ParsedBlock checkBlock = allBlocks.Find(x => x.blockID.Equals(checkID));

            // Until it reaches NULL, adds all option blocks to the optionIDs list.
            while(checkBlock != null)
            {
                if (checkBlock.isOptionBlock)
                {
                    optionIDs.Add(checkID.CopyID());
                }

                checkID.AddToLastIndex(1);
                checkBlock = allBlocks.Find(x => x.blockID.Equals(checkID));
            }

            // Returns the found option IDs
            return optionIDs.ToArray();
        }

        public BlockID GetRerouteLink(List<ParsedBlock> allBlocks)
        {
            // Generates the new ID
            BlockID checkID = new BlockID(Compiler.GetStringHashInt(rerouteSection));
            checkID.AddIndex();

            // Checks if that ID exists
            if(allBlocks.Find(x => x.blockID.Equals(checkID)) != null)
            {
                return checkID.CopyID();
            }

            // Returns a ZERO ID if it doesn't
            return BlockID.ZERO;
        }

        public BlockID GetReturnLink(List<ParsedBlock> allBlocks)
        {
            // Goes to the last ID on the previous level (this option's prompt)
            BlockID checkID = new BlockID(blockID.id);
            checkID.RemoveLastIndex();
            ParsedBlock checkBlock = allBlocks.Find(x => x.blockID.Equals(checkID));
            
            // If there is no block below this or it finds itself, returns ZERO.
            if(checkBlock == null || checkBlock == this)
            {
                return BlockID.ZERO;
            }

            // When it finds one, returns it.
            return checkID.CopyID();
        }

        public BlockID GetContinueLink(List<ParsedBlock> allBlocks)
        {
            // Goes down one index, then forward in IDs until it finds one that is a prompt. If it reaches null, it drops the last index and continues.
            BlockID checkID = new BlockID(blockID.id);
            checkID.RemoveLastIndex();
            checkID.AddToLastIndex(1);
            ParsedBlock checkBlock = allBlocks.Find(x => x.blockID.Equals(checkID));

            // Loop will run infinitely until it returns a value.
            while(true)
            {
                // If the check block is null, drops the last index
                if (checkBlock == null)
                {
                    checkID.RemoveLastIndex();
                }

                // If the check block is prompt, returns the ID
                else if (!checkBlock.isOptionBlock)
                {
                    return checkID.CopyID();
                }

                // If the ID length is now the minimum, can only be followed by null - so will return ZERO.
                if(checkID.idLength == BlockID.MIN_ID_LEN)
                {
                    return BlockID.ZERO;
                }

                // Adds 1 to the last index and keeps going
                checkID.AddToLastIndex(1);
                checkBlock = allBlocks.Find(x => x.blockID.Equals(checkID));
            }
        }
    }
}
