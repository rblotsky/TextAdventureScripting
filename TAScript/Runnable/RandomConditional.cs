using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAScript.Runnable
{
    public class RandomConditional : AbstractConditional
    {
        // DATA //
        private int percentChance;


        // CONSTRUCTOR //
        public RandomConditional(int percent)
        {
            percentChance = percent;
        }


        // FUNCTIONS //
        public override bool RunConditional(Game context)
        {
            // Generates a new Random object
            Random rng = new Random();

            // Returns true if the random falls within 0 - percentChance, out of 100.
            return rng.Next(100) <= percentChance;
        }
    }
}
