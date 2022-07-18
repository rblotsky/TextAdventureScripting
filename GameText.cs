using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextAdventureGame.Runnable;

namespace TextAdventureGame
{
    public class GameText
    {
        // DATA //
        // Text data (for displaying)
        public string alwaysText;
        public string asOptionText;
        public string asTitleText;

        // Conditionals (for filling empty spots in the text)
        public ConditionalText[] alwaysConditionals;
        public ConditionalText[] optionConditionals;
        public ConditionalText[] titleConditionals;


        // CONSTRUCTORS //
        public GameText(string always, string asOption, string asTitle)
        {
            alwaysText = always;
            asOptionText = asOption;
            asTitleText = asTitle;

            optionConditionals = new ConditionalText[0];
            alwaysConditionals = new ConditionalText[0]; 
            titleConditionals = new ConditionalText[0];
        }


        // FUNCTIONS //
        public string ResolveText(Game context, bool asOption)
        {
            // Resolves all conditionals
            string[] alwaysConditionalsResolved = new string[alwaysConditionals.Length];
            string[] optionConditionalsResolved = new string[optionConditionals.Length];
            string[] titleConditionalsResolved = new string[titleConditionals.Length];
            for(int i = 0; i < alwaysConditionals.Length; i++)
            {
                alwaysConditionalsResolved[i] = alwaysConditionals[i].ResolveConditional(context);
            }
            for (int i = 0; i < optionConditionals.Length; i++)
            {
                optionConditionalsResolved[i] = optionConditionals[i].ResolveConditional(context);
            }
            for (int i = 0; i < titleConditionals.Length; i++)
            {
                titleConditionalsResolved[i] = titleConditionals[i].ResolveConditional(context);
            }

            // Generates the output text
            string resolvedText = "";
            resolvedText += string.Format(alwaysText, alwaysConditionalsResolved);
            if (asOption) resolvedText += string.Format(asOptionText, optionConditionalsResolved);
            if (!asOption) resolvedText += string.Format(asTitleText, titleConditionalsResolved);

            // Returns the resolved text
            return resolvedText;
        }
    }
}
