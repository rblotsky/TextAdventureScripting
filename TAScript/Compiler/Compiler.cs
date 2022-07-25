using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using TAScript;

namespace TAScript.Compiler
{
    public class Compiler
    {
        // DATA //
        // Constants
        public static readonly Regex COMMENT_REGEX = new Regex(@"\/\/.*");
        public static readonly Regex SECTION_HEADER_REGEX = new Regex(@"^\$\s*(\w+)");
        public static readonly Regex BLOCK_REGEX = new Regex(@"^([-]+|[>]+)(.+)$", RegexOptions.Multiline);
        public static readonly Regex REROUTE_REGEX = new Regex(@"@(\w+)");
        public static readonly Regex WHITESPACE_UNTIL_CONTENT_REGEX = new Regex(@"^\s+");
        public static readonly Regex TEXT_SPLITTER_REGEX = new Regex(@"([^\[\]]*)(?:(?:\[)((?:.|[\r\n])*?)(?:\]))?((?:.|[\r\n])*)", RegexOptions.Multiline);
        public static readonly Regex RETURN_REGEX = new Regex(@"~\s*?(#\w*)?$", RegexOptions.Multiline);
        public static readonly Regex COMMAND_REGEX = new Regex(@"{(\w+):[\w\d><=+-,]*}");
        public static readonly Regex TAG_REGEX = new Regex(@"#\w+");

        // Delegates
        public delegate string CommandDelegate(ParsedBlock block, string[] commandVariables);


        // FUNCTIONS //
        public Runnable.Game CompileGame(string sourceText)
        {
            // Split sourceText into an array of lines
            string[] lines = sourceText.Split("\n");

            // Caches some important data
            int currentLine = 0;
            int currentSectionHash = 0;
            BlockID currentBlockID = BlockID.ZERO;
            string currentBlockText = "";

            // Creates a list of parsed blocks
            List<ParsedBlock> parsedBlocks = new List<ParsedBlock>();

            // Iterates over all lines, creating blocks where necessary.
            foreach(string line in lines)
            {
                // Increments current line
                currentLine++;

                // First removes the comments from the line
                string editedLine = COMMENT_REGEX.Replace(line, "");

                // Then, removes the initial whitespace
                editedLine = WHITESPACE_UNTIL_CONTENT_REGEX.Replace(editedLine, "", 1);

                // Then, checks if this is a section header. If so, updates the current section hash and block ID. (If it is, finishes the current block!)
                Match regexMatch = SECTION_HEADER_REGEX.Match(editedLine);

                if(regexMatch.Success)
                {
                    // Finishes the last ParsedBlock
                    FinishParsingBlock(parsedBlocks, currentBlockText);

                    // Updates the current block ID to start the new section
                    currentSectionHash = GetStringHashInt(regexMatch.Groups[1].Value);
                    currentBlockID = new BlockID(currentSectionHash);
                    continue;
                }
                
                // If we are not in a section, does nothing
                if(currentSectionHash == 0)
                {
                    continue;
                }

                // If we are not in a section, checks if this is a prompt or option. Creates a new ParsedBlock from the data.
                regexMatch = BLOCK_REGEX.Match(editedLine);

                if (regexMatch.Success)
                {
                    // Removes the initial type specifier (-, >)
                    editedLine = editedLine.Replace(regexMatch.Groups[1].Value, "");

                    // Finishes the last ParsedBlock
                    FinishParsingBlock(parsedBlocks, currentBlockText);

                    // Clears the current block text
                    currentBlockText = "";

                    // Starts the new ParsedBlock
                    // Creates a new ParsedBlock from data on this line
                    bool isOption = (regexMatch.Groups[1].Value[0] == '>' ? true : false);

                    // Gets indent amount and uses that to modify the current block ID.
                    int indentAmountDifference = regexMatch.Groups[1].Length - currentBlockID.idLength + 1;
                    int indexModSign = (indentAmountDifference < 0 ? -1 : 1);
                    for (int i = 0; i < Math.Abs(indentAmountDifference); i++)
                    {
                        if (indexModSign < 0)
                        {
                            currentBlockID.RemoveLastIndex();
                        }

                        else
                        {
                            currentBlockID.AddIndex();
                        }
                    }

                    // Adds to the last ID index only if we didn't change level
                    if (indentAmountDifference <= 0)
                    {
                        currentBlockID.AddToLastIndex(1);
                    }


                    // If the indent amount difference is greater than 1, logs an error and stops.
                    if (indentAmountDifference > 1)
                    {
                        DebugLogger.DebugLog(string.Format("[CompileGame] Indentation jump greater than 1 level on line {0}! Aborting.", currentLine), true);
                        return null;
                    }

                    // Creates the new ParsedBlock, adds it to the parsed blocks list
                    parsedBlocks.Add(new ParsedBlock(currentBlockID.CopyID(), DefaultLinkType.Continue, null, isOption, null));

                    // Gets the rest of the text and adds it to the current text
                    editedLine = WHITESPACE_UNTIL_CONTENT_REGEX.Replace(editedLine, "", 1);
                    currentBlockText += editedLine + "\n";
                }

                // If this is not the start of a prompt or section, but currentBlockText isn't empty, adds whatever text (excluding initial whitespace) to it.
                else if (!string.IsNullOrEmpty(currentBlockText) && !string.IsNullOrWhiteSpace(editedLine))
                {
                    currentBlockText += editedLine + "\n";
                }
            }

            // Runs through all the parsed blocks and autolinks them, also generating the new runnable blocks.
            List<Runnable.Block> runnableBlocks = new List<Runnable.Block>();
            foreach (ParsedBlock block in parsedBlocks)
            {
                block.GenerateAllLinks(parsedBlocks);
                runnableBlocks.Add(block.ConvertToRunnable());
            }

            // Populates the runnable blocks (this is done after the previous loop because it requires that ALL runnable blocks be generated)
            foreach(ParsedBlock block in parsedBlocks)
            {
                block.PopulateRunnableVersion(runnableBlocks.Find(x => x.blockID.Equals(block.blockID)), runnableBlocks);
            }

            // Generates a Game object and fills it with the new Blocks
            Runnable.Game compiledGame = new Runnable.Game();
            compiledGame.initialBlock = runnableBlocks[0];

            // Returns the generated game
            return compiledGame;
        }

        public void FinishParsingBlock(List<ParsedBlock> parsedBlocks, string currentBlockText)
        {
            // Finishes the last ParsedBlock
            if (parsedBlocks.Count > 0)
            {
                ParsedBlock lastBlock = parsedBlocks[parsedBlocks.Count - 1];

                // Trims the current block text
                currentBlockText = currentBlockText.Trim();

                // Updates the default link type
                Match returnMatch = RETURN_REGEX.Match(currentBlockText);
                if (returnMatch.Success)
                {
                    lastBlock.defaultLinkType = DefaultLinkType.Return;
                    currentBlockText = currentBlockText.Replace(returnMatch.Value, "");
                }

                // Sets the reroute if there is one
                Match rerouteMatch = REROUTE_REGEX.Match(currentBlockText);
                if (rerouteMatch.Success)
                {
                    lastBlock.rerouteSection = rerouteMatch.Groups[1].Value;
                    currentBlockText = currentBlockText.Replace(rerouteMatch.Value, "");
                }

                // Parses the commands in the text, replacing them with whatever their functions says to replace with.
                // They are not replaced with anything if they could not be parsed.
                currentBlockText = ParseCommandsInString(currentBlockText, lastBlock);

                // Splits the text into the alwas, option, and title text
                Match textSplitterMatch = TEXT_SPLITTER_REGEX.Match(currentBlockText);
                string alwaysText = textSplitterMatch.Groups[1].Value;
                string asOptionText = (textSplitterMatch.Groups.Count >= 3 ? textSplitterMatch.Groups[2].Value : "");
                string asTitleText = (textSplitterMatch.Groups.Count >= 4 ? textSplitterMatch.Groups[3].Value : "");
                lastBlock.text = new GameText(alwaysText, asOptionText, asTitleText);
            }
        }

        public string ParseCommandsInString(string text, ParsedBlock block)
        {
            // Makes a new text to edit
            string editedText = text;

            // Extracts all commands
            MatchCollection commandMatches = COMMAND_REGEX.Matches(editedText);

            // Goes through all the matches, replaces them within the text and runs their commands
            foreach(Match match in commandMatches)
            {
                // Gets command name and variables
                string commandName = match.Groups[1].Value;
                string[] commandVariables = new string[0];
                if (match.Groups[2].Value != null)
                {
                    commandVariables = match.Groups[2].Value.Split(",");
                }

                // Finds the command by its name and then runs it with the appropriate variables.
                string commandReplacementString = match.Value;
                CommandDelegate commandToRun = GetCommandByName(commandName);
                if(commandToRun != null)
                {
                    commandReplacementString = commandToRun(block, commandVariables);
                }

                editedText = editedText.Replace(match.Value, commandReplacementString);
            }

            // Returns the edited text
            return editedText;
        }


        // Command Functions
        public CommandDelegate GetCommandByName(string commandName)
        {
            if(commandName.Equals("TEST"))
            {
                return ParseTestCommand;
            }

            else
            {
                return null;
            }
        }

        public string ParseTestCommand(ParsedBlock block, string[] variables)
        {
            return null;
        }


        // Static
        public static int GetStringHashInt(string stringToHash)
        {
            // Returns a hash of the string
            return stringToHash.GetHashCode();
        }
    }
}
