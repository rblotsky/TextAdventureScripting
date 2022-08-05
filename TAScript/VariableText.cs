﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAScript.Runnable;

namespace TAScript
{
    public class VariableText : RuntimeText
    {
        // DATA //
        public string varName;


        // CONSTRUCTOR //
        public VariableText(string variable)
        {
            varName = variable;
        }


        // OVERRIDES //
        public override string GetText(Game context)
        {
            return context.GetVariable(varName).ToString();
        }
    }
}
