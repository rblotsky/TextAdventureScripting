using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAScript.Runnable;
using System.Text.RegularExpressions;

namespace TAScript
{
    public class GameText
    {
        // DATA //
        // Text sections
        private string alwaysText;
        private string asOptionText;
        private string asTitleText;

        // Conditionals within the text
        public List<ConditionalText> conditionals = new List<ConditionalText>();

        // Regex
        private static readonly Regex conditionalReplacementRegex = new Regex(@"{#}");
        private static readonly Regex emptyLineRegex = new Regex(@"\n{2,}");

        // Tags
        public string[] textTags;

        // Flags
        public bool noEmptyLines = false;


        // CONSTRUCTORS //
        public GameText(string always, string asOption, string asTitle)
        {
            alwaysText = always;
            asOptionText = asOption;
            asTitleText = asTitle;
        }


        // FUNCTIONS //
        public void UpdateText(string always, string asOption, string asTitle)
        {
            alwaysText = always;
            asOptionText = asOption;
            asTitleText = asTitle;
        }

        public void NumberEmptyFormatSpecifiers()
        {
            // Assembles the full text string
            string combinedText = alwaysText + "|" + asOptionText + "|" + asTitleText;

            // Replaces all "{#}" with "{i}" where i is the order of its appearance in the string.
            bool hasMoreEmptyFormats = true;
            int currentSpecifierIndex = 0;
            while(hasMoreEmptyFormats)
            {
                // For each regex match, replaces it with the order of appearance index.
                Match formatSpecMatch = conditionalReplacementRegex.Match(combinedText);
                if(formatSpecMatch.Success)
                {
                    combinedText = conditionalReplacementRegex.Replace(combinedText, "{" + currentSpecifierIndex + "}", 1);
                    currentSpecifierIndex++;
                }

                // If no match was found, finishes
                else
                {
                    hasMoreEmptyFormats = false;
                }
            }

            // Re-splits the text and changes the always, asOption, and asTitle text to the new versions.
            string[] splitFilledText = combinedText.Split("|");
            alwaysText = (splitFilledText.Length >= 1 ? splitFilledText[0] : "");
            asOptionText = (splitFilledText.Length >= 2 ? splitFilledText[1] : "");
            asTitleText = (splitFilledText.Length >= 3 ? splitFilledText[2] : "");
        }

        public string ResolveText(Game context, bool asOption)
        {
            // Resolves all conditionals
            string[] resolvedConditionals = new string[conditionals.Count];
            for(int i = 0; i < conditionals.Count; i++)
            {
                resolvedConditionals[i] = conditionals[i].ResolveConditional(context);
            }

            // Insers them into the complete text, then splits it back up
            string completeText = string.Format(alwaysText + "|" + asOptionText + "|" + asTitleText, resolvedConditionals);
            string[] splitCompleteText = completeText.Split("|");

            // Generates the output text according to whether the always/option/title text was even set, then which one to display.
            string resolvedText = "";
            if (splitCompleteText.Length > 0)
            {
                resolvedText += splitCompleteText[0];
            }

            if(splitCompleteText.Length > 1 && asOption)
            {
                resolvedText += splitCompleteText[1];
            }

            if(splitCompleteText.Length > 2 && !asOption)
            {
                resolvedText += splitCompleteText[2];
            }

            // Applies different flags
            if(noEmptyLines)
            {
                resolvedText = emptyLineRegex.Replace(resolvedText, "\n");
            }

            // Returns the resolved text, trimmed at the start and end.
            return resolvedText.Trim();
        }
    }
}
