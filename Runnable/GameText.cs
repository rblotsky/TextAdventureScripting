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
        public string selectedText;

        // Conditionals (for filling empty spots in the text)
        public ConditionalText[] alwaysConditionals;
        public ConditionalText[] preSelectionConditionals;
        public ConditionalText[] selectedConditionals;


        // FUNCTIONS //
        public string ResolveText(Game context, bool inSelection)
        {
            // Resolves all conditionals
            string[] alwaysConditionalsResolved = new string[alwaysConditionals.Length];
            string[] preSelectionConditionalsResolved = new string[preSelectionConditionals.Length];
            string[] selectedConditionalsResolved = new string[selectedConditionals.Length];
            for(int i = 0; i < alwaysConditionals.Length; i++)
            {
                alwaysConditionalsResolved[i] = alwaysConditionals[i].ResolveConditional(context);
            }
            for (int i = 0; i < preSelectionConditionals.Length; i++)
            {
                preSelectionConditionalsResolved[i] = preSelectionConditionals[i].ResolveConditional(context);
            }
            for (int i = 0; i < selectedConditionals.Length; i++)
            {
                selectedConditionalsResolved[i] = selectedConditionals[i].ResolveConditional(context);
            }

            // Generates the output text
            string resolvedText = string.Format(alwaysText, alwaysConditionalsResolved);
            if (inSelection) resolvedText += string.Format(preSelectionText, preSelectionConditionalsResolved);
            else resolvedText += string.Format(selectedText, selectedConditionalsResolved);

            // Returns the resolved text
            return resolvedText;
        }
    }
}
