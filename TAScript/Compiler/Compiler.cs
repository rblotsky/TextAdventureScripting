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
        public static readonly Regex INDENT_LEVEL_REGEX = new Regex(@"^([-]+|[>]+)");
        public static readonly Regex BLOCK_REGEX = new Regex(@"^([-]+|[>]+)(.+)$", RegexOptions.Multiline);
        public static readonly Regex REROUTE_REGEX = new Regex(@"@(\w+)");
        public static readonly Regex WHITESPACE_UNTIL_CONTENT_REGEX = new Regex(@"^\s+");
        public static readonly Regex TEXT_SPLITTER_REGEX = new Regex(@"([^\[\]]*)(?:(?:\[)((?:.|[\r\n])*?)(?:\]))?((?:.|[\r\n])*)", RegexOptions.Multiline);
        public static readonly Regex RETURN_REGEX = new Regex(@"~+(?=\s*(?:#\w+)?$)", RegexOptions.Multiline);
        public static readonly Regex COMMAND_REGEX = new Regex(@"{(\w+):((?:[^\n|{}\[\]]*\\?)*)}");
        public static readonly Regex TAG_REGEX = new Regex(@"#\w+");
        public static readonly Regex ONE_TIME_REGEX = new Regex(@"^\s*\?");

        // Delegates
        public delegate string CommandDelegate(ParsedBlock block, string[] commandVariables);


        // FUNCTIONS //
        public Runnable.Game CompileGame(string sourceText)
        {
            // Removes all carriage returns from source text
            string editedSourceText = sourceText.Replace("\r","");

            // Split sourceText into an array of lines
            string[] lines = editedSourceText.Split("\n");

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
                    editedLine = INDENT_LEVEL_REGEX.Replace(editedLine, "");

                    // Finishes the last ParsedBlock
                    FinishParsingBlock(parsedBlocks, currentBlockText);

                    // Clears the current block text
                    currentBlockText = "";

                    // Starts the new ParsedBlock
                    currentBlockID = ParseNewBlock(parsedBlocks, regexMatch, currentBlockID, currentLine);

                    // Stops compiling if a ZERO id was returned (means there was an error)
                    if(currentBlockID == BlockID.ZERO)
                    {
                        return null;
                    }

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

        public BlockID ParseNewBlock(List<ParsedBlock> parsedBlocks, Match blockMatch, BlockID currentID, int lineNumber)
        {
            // Creates a new ParsedBlock from data on this line
            bool isOption = (blockMatch.Groups[1].Value[0] == '>' ? true : false);

            // Gets indent amount and uses that to modify the current block ID.
            int indentAmountDifference = blockMatch.Groups[1].Length - currentID.idLength + 1;
            int indexModSign = (indentAmountDifference < 0 ? -1 : 1);
            for (int i = 0; i < Math.Abs(indentAmountDifference); i++)
            {
                if (indexModSign < 0)
                {
                    currentID.RemoveLastIndex();
                }

                else
                {
                    currentID.AddIndex();
                }
            }

            // Adds to the last ID index only if we didn't change level
            if (indentAmountDifference <= 0)
            {
                currentID.AddToLastIndex(1);
            }


            // If the indent amount difference is greater than 1, logs an error and stops.
            if (indentAmountDifference > 1)
            {
                DebugLogger.DebugLog(string.Format("[Compiler.ParseNewBlock] Indentation jump greater than 1 level on line {0}! Aborting.", lineNumber), true);
                return BlockID.ZERO;
            }

            // Creates the new ParsedBlock, adds it to the parsed blocks list
            parsedBlocks.Add(new ParsedBlock(currentID.CopyID(), DefaultLinkType.Continue, new GameText("EMPTY", "EMPTY", "EMPTY"), isOption, null));

            return currentID;
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
                    lastBlock.returnLevelsDown = returnMatch.Length;
                    currentBlockText = currentBlockText.Replace(returnMatch.Value, "");
                }

                // Sets the reroute if there is one
                Match rerouteMatch = REROUTE_REGEX.Match(currentBlockText);
                if (rerouteMatch.Success)
                {
                    lastBlock.rerouteSection = rerouteMatch.Groups[1].Value;
                    currentBlockText = currentBlockText.Replace(rerouteMatch.Value, "");
                }

                // If the text contains a single-appearance symbol, adds a conditional so it only appears once
                Match oneTimeMatch = ONE_TIME_REGEX.Match(currentBlockText);
                if(oneTimeMatch.Success)
                {
                    // Replaces the symbol
                    currentBlockText = ONE_TIME_REGEX.Replace(currentBlockText, "", 1);

                    // Adds a conditional and a variable modifier, so it only runs if it hasnt run yet and if it does run, it adds to its variable to store that it has run.
                    VariableConditional oneTimeConditional = new VariableConditional(lastBlock.blockID.ToString(), "=", 0);
                    lastBlock.blockConditionals.Add(oneTimeConditional);

                    VariableModifier modifier = new VariableModifier(lastBlock.blockID.ToString(), 1, true);
                    lastBlock.variableModifiers.Add(modifier);
                }

                // Parses the commands in the text, replacing them with whatever their functions says to replace with.
                // They are not replaced with anything if they could not be parsed.
                currentBlockText = ParseCommandsInString(currentBlockText, lastBlock);

                // Splits the text into the alwas, option, and title text
                Match textSplitterMatch = TEXT_SPLITTER_REGEX.Match(currentBlockText);
                string alwaysText = textSplitterMatch.Groups[1].Value;
                string asOptionText = (textSplitterMatch.Groups.Count >= 3 ? textSplitterMatch.Groups[2].Value : "");
                string asTitleText = (textSplitterMatch.Groups.Count >= 4 ? textSplitterMatch.Groups[3].Value : "");
                lastBlock.text.UpdateText(alwaysText, asOptionText, asTitleText);
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
                    commandVariables = match.Groups[2].Value.Split("\\");
                }

                // Finds the command by its name and then runs it with the appropriate variables.
                string commandReplacementString = "INVALID_COMMAND";
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
            if(commandName.Equals("RAND"))
            {
                return ParseRandomCommand;
            }

            else if(commandName.Equals("COND"))
            {
                return ParseConditionalCommand;
            }

            else if(commandName.Equals("ADD"))
            {
                return ParseAddCommand;
            }

            else if(commandName.Equals("SET"))
            {
                return ParseSetCommand;
            }

            else if(commandName.Equals("NOEMPTY"))
            {
                return ParseNoEmptyCommand;
            }

            else
            {
                return null;
            }
        }

        public string ParseConditionalCommand(ParsedBlock block, string[] variables)
        {
            /*
             * Possible Overloads:
             * 1. Var,Operator,ReqValue      : Displays the option if the conditional evaluates to True
             * 2. Var,Operator,ReqValue,SuccessText,FailText : Creates a ConditionalText for the Text variable.
             */

            // If there are less than 3 variables, does nothing.
            if(variables.Length < 3)
            {
                return null;
            }

            // Gets first 3 variables
            string varName = variables[0];
            string condOperator = variables[1];
            string reqValue = variables[2];

            // Attempts parsing value
            if(int.TryParse(reqValue, out int reqValueInt))
            {
                // If parsing was successful, creates a VariableConditional for comparison.
                VariableConditional conditionalComparison = new VariableConditional(varName, condOperator, reqValueInt);

                // If there is a 4th variable, gets it as a string and creates a ConditionalText for it.
                if (variables.Length > 3)
                {
                    string successText = variables[3];
                    string failText = "";

                    // If there is a 4th failure text variable, gets it too
                    if(variables.Length > 4)
                    {
                        failText = variables[4];
                    }

                    // Creates the conditional text object, inserts it into the GameText for the current block
                    ConditionalText conditionalText = new ConditionalText(successText, failText, conditionalComparison);
                    block.text.conditionals.Add(conditionalText);

                    // Returns a '{#}' string to be filled in with string.Format()
                    return "{#}";
                }

                // If there is no 4th variable, adds this conditional to the block and returns null.
                else
                {
                    block.blockConditionals.Add(conditionalComparison);
                    return null;
                }
            }

            else
            {
                DebugLogger.DebugLog(string.Format("[Compiler.ParseConditionalCommand] ReqValue {0} is not an integer!", reqValue), true);
                return "PARSING_ERROR";
            }
        }

        public string ParseRandomCommand(ParsedBlock block, string[] variables)
        {
            return null;
        }

        public string ParseAddCommand(ParsedBlock block, string[] variables)
        {
            // Sends to the VariableModifierCommandParser
            return VariableModifierCommandParser(block, variables, true);
        }

        public string ParseSetCommand(ParsedBlock block, string[] variables)
        {
            // Sends to the VariableModifierCommandParser
            return VariableModifierCommandParser(block, variables, false);
        }

        public string VariableModifierCommandParser(ParsedBlock block, string[] variables, bool isAdd)
        {
            /*
             * Possible Overloads:
             * 1. Var,Value
             */

            // Ensures there are enough variables
            if (variables.Length == 2)
            {
                // Tries getting the variables as the required datatypes
                string varName = variables[0];

                if (int.TryParse(variables[1], out int addValue))
                {
                    // Adds the new Variable Modifier object and returns null.
                    block.variableModifiers.Add(new VariableModifier(varName, addValue, isAdd));
                    return null;
                }

                else
                {
                    DebugLogger.DebugLog($"[Compiler.VariableModifierCommandParser] Could not parse value {variables[1]} as an integer!", true);
                }
            }

            // If not, prints an error message
            else
            {
                DebugLogger.DebugLog($"[Compiler.VariableModifierCommandParser] Invalid amount of variables! Required: 2, Given: {variables.Length}!", true);
            }

            // Returns "PARSING_ERROR" by default.
            return "PARSING_ERROR";
        }

        public string ParseNoEmptyCommand(ParsedBlock block, string[] variables)
        {
            // No variables needed for this command
            // Sets the block text to have no empty lines.
            block.text.noEmptyLines = true;

            // Returns null to signify successful compilation
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
