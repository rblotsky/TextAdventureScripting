using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAScript.Runnable;

namespace TAScript
{
    public class VariableConditional : AbstractConditional
    {
        // DATA //
        public string variableName;
        public int reqVariableValue;
        public Comparison comparer;

        
        // CONSTRUCTORS //
        public VariableConditional(string varName, char comparisonChar, int reqValue)
        {
            // Tries setting the comparer according to the given character
            try
            {
                comparer = (Comparison)comparisonChar;
            }
            catch(Exception e)
            {
                DebugLogger.DebugLog(string.Format("[VariableConditional.VariableConditional] Could not parse comparisonChar \'{0}\'! Error Message: \n{3}", comparisonChar, e.Message), false);
            }

            // Sets variable name and required value
            variableName = varName;
            reqVariableValue = reqValue;
        }


        // FUNCTIONS //
        public override bool RunConditional(Game context)
        {
            // Gets the current value of the variable
            int currentVariableValue = context.GetVariable(variableName);

            // Runs the comparison
            switch(comparer)
            {
                case Comparison.LessThan:
                    return currentVariableValue < reqVariableValue;
                case Comparison.EqualTo:
                    return currentVariableValue == reqVariableValue;
                case Comparison.GreaterThan:
                    return currentVariableValue > reqVariableValue;
                default:
                    return false;
            }
        }
    }
}
