using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureGame.Runnable
{
    public class GameText
    {
        // DATA //
        // Text data (for displaying)
        public string alwaysText;
        public string preSelectionText;
        public string postSelectionText;

        // Conditionals (for filling empty spots in the text)
        public ConditionalText[] alwaysConditionals;
        public ConditionalText[] preSelectionConditionals;
        public ConditionalText[] postSelectionConditionals;


        // FUNCTIONS //
        public string ResolveText(Game context, bool inSelection)
        {
            // Resolves all conditionals
            string[] alwaysConditionalsResolved = new string[alwaysConditionals.Length];
            string[] preConditionalsResolved = new string[preSelectionConditionals.Length];
            string[] postConditionalsResolved = new string[postSelectionConditionals.Length];
            for(int i = 0; i < alwaysConditionals.Length; i++)
            {
                alwaysConditionalsResolved[i] = alwaysConditionals[i].ResolveConditional(context);
            }
            for (int i = 0; i < preSelectionConditionals.Length; i++)
            {
                preConditionalsResolved[i] = preSelectionConditionals[i].ResolveConditional(context);
            }
            for (int i = 0; i < postSelectionConditionals.Length; i++)
            {
                postConditionalsResolved[i] = postSelectionConditionals[i].ResolveConditional(context);
            }

            // Generates the output text
            string resolvedText = string.Format(alwaysText, alwaysConditionalsResolved);
            if (inSelection) resolvedText += string.Format(preSelectionText, preConditionalsResolved);
            else resolvedText += string.Format(postSelectionText, postConditionalsResolved);

            // Returns the resolved text
            return resolvedText;
        }
    }
}
