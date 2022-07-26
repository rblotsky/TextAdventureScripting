using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAScript.Runnable;

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
        public ConditionalText[] conditionals = new ConditionalText[0];

        // Tags
        public string[] textTags;


        // CONSTRUCTORS //
        public GameText(string always, string asOption, string asTitle)
        {
            alwaysText = always;
            asOptionText = asOption;
            asTitleText = asTitle;

            conditionals = new ConditionalText[0];
        }


        // FUNCTIONS //
        public void UpdateText(string always, string asOption, string asTitle)
        {
            alwaysText = always;
            asOptionText = asOption;
            asTitleText = asTitle;
        }

        public string ResolveText(Game context, bool asOption)
        {
            // Resolves all conditionals into one string, then splits it up
            string completeText = string.Format(alwaysText + "|" + asOptionText + "|" + asTitleText, conditionals);
            string[] splitCompleteText = completeText.Split("|");


            // Generates the output text according to whether the always/option/title text was even set, then which one to display.
            string resolvedText = "";
            if (splitCompleteText.Length > 0)
            {
                resolvedText += resolvedText[0];
            }

            if(splitCompleteText.Length > 1 && asOption)
            {
                resolvedText += splitCompleteText[1];
            }

            if(splitCompleteText.Length > 2 && !asOption)
            {
                resolvedText += resolvedText[2];
            }

            // Returns the resolved text
            return resolvedText;
        }
    }
}
