using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAScript.Runnable;

namespace TAScript.Runnable
{
    public class VariableConditional : AbstractConditional
    {
        // DATA //
        public string variableName;
        public int reqVariableValue;
        public Comparison comparer;

        
        // CONSTRUCTORS //
        public VariableConditional(string varName, string comparisonString, int reqValue)
        {
            // Tries setting the comparer according to the given character
            try
            {
                if(comparisonString.Equals("="))
                {
                    comparer = Comparison.EqualTo;
                }
                else if (comparisonString.Equals("!"))
                {
                    comparer = Comparison.NotEqualTo;
                }
                else if (comparisonString.Equals(">"))
                {
                    comparer = Comparison.GreaterThan;
                }
                else if (comparisonString.Equals("<"))
                {
                    comparer = Comparison.LessThan;
                }
                else if (comparisonString.Equals(">="))
                {
                    comparer = Comparison.GreaterOrEqual;
                }
                else if (comparisonString.Equals("<="))
                {
                    comparer = Comparison.LessOrEqual;
                }
            }

            catch(Exception e)
            {
                DebugLogger.DebugLog(string.Format("[VariableConditional.Constructor] Could not parse comparisonChar \'{0}\'! Error Message: \n{3}", comparisonString, e.Message), false);
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
                case Comparison.NotEqualTo:
                    return currentVariableValue != reqVariableValue;
                case Comparison.GreaterThan:
                    return currentVariableValue > reqVariableValue;
                case Comparison.GreaterOrEqual:
                    return currentVariableValue >= reqVariableValue;
                case Comparison.LessOrEqual:
                    return currentVariableValue <= reqVariableValue;
                default:
                    return false;
            }
        }
    }
}
