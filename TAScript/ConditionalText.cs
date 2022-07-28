using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAScript.Runnable;

namespace TAScript
{
    public class ConditionalText
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


        // FUNCTIONS //
        public virtual string ResolveConditional(Game context)
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
