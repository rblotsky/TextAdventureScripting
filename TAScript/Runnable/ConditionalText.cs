using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAScript.Runnable;

namespace TAScript.Runnable
{
    public class ConditionalText : RuntimeInsertedText
    {
        // DATA //
        public string successText;
        public string failureText;
        public AbstractConditional conditional;


        // CONSTRUCTORS //
        public ConditionalText(string success, string fail, AbstractConditional cond)
        {
            successText = success;
            failureText = fail;
            conditional = cond;
        }


        // OVERRIDES //
        public override string GetText(Game context)
        {
            // Returns sucess if conditional succeeds
            if (conditional.RunConditional(context))
            {
                return successText;
            }

            return failureText;
        }
    }
}
