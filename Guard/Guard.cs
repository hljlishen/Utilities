using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Guards
{
    public static class Guard
    {
        public static void AssertNotNull(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException();
        }
    }
}
