using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAScript
{
    public class DebugLogger
    {
        // DATA //
        // Delegates
        public delegate void DebugDelegate(string debugText, bool isError);
        public static DebugDelegate debugDisplayer;


        // FUNCTIONS //
        public static void DebugLog(string debugText, bool isError)
        {
            if(debugDisplayer != null)
            {
                debugDisplayer(debugText, isError);
            }
        }
    }
}
