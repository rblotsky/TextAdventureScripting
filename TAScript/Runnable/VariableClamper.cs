using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAScript.Runnable
{
    public class VariableClamper : BlockCompletionFunction
    {
        // DATA //
        public string varName;
        public int minMaxValue;
        public bool isMin;


        // CONSTRUCTOR //
        public VariableClamper(string newVarName, int newValue, bool newIsMin)
        {
            varName = newVarName;
            minMaxValue = newValue;
            isMin = newIsMin;
        }


        // FUNCTIONS //
        public override void RunModifier(Game context)
        {
            // Gets variable
            int variableValue = context.GetVariable(varName);

            // Clamps according to isMin
            if(isMin)
            {
                variableValue = Math.Min(variableValue, minMaxValue);
            }

            else
            {
                variableValue = Math.Max(variableValue, minMaxValue);
            }

            // Sets variable in context
            context.SetVariable(varName, variableValue);
        }
    }
}
