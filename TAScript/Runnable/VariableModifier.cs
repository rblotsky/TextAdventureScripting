using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAScript.Runnable
{
    public class VariableModifier : BlockCompletionFunction
    {
        // DATA //
        public string varName;
        public int modifyAmount;
        public bool isAdding;


        // CONSTRUCTORS //
        public VariableModifier(string name, int amount, bool isAdd)
        {
            varName = name;
            modifyAmount = amount;
            isAdding = isAdd;
        }

        
        // FUNCTIONS //
        public override void RunModifier(Game context)
        {
            // If adding, adds to the variable
            if(isAdding)
            {
                context.AddToVariable(varName, modifyAmount);
            }

            // Otherwise, sets the variable
            else
            {
                context.SetVariable(varName, modifyAmount);
            }
        }
    }
}
