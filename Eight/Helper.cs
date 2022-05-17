using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eight
{
    public class Helper
    {
        private static EightContext s_context;
        public static EightContext GetContext()
        {
            if (s_context == null)
                s_context = new EightContext();
            return s_context;
        }
    }
}
