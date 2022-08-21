using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAScript.Runnable
{
    public class RandomText : RuntimeInsertedText
    {
        // DATA //
        private string[] possibleValues;


        // CONSTRUCTOR //
        public RandomText(params string[] stringValues)
        {
            possibleValues = stringValues;
        }


        // FUNCTIONS //
        public override string GetText(Game context)
        {
            // If no possible values, returns null
            if(possibleValues.Length == 0)
            {
                return null;
            }    

            // Creates a random number generator
            Random rng = new Random();

            // Returns a random value from the possibleValues array
            return possibleValues[rng.Next(possibleValues.Length)];
        }
    }
}
