using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAScript
{
    public class VariableConditional
    {
        // DATA //
        public string variableName;
        public int reqVariableValue;
        public Comparison comparer;

        
        // FUNCTIONS //
        public bool RunComparison(Dictionary<string, int> variables)
        {
            // Gets the current value of the variable
            if(variables.TryGetValue(variableName, out int currentVariableValue))
            {
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

            // Return false if the variable doesn't exist (also logs an error)
            DebugLogger.DebugLog(string.Format("The variable {0} doesn't exist!", variableName), false)
            return false;
        }
    }
}
