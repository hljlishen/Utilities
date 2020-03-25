using System;

namespace Utilities.ValueIntervals
{
    public class MaxNotBiggerThanMin : Exception
    {
        public MaxNotBiggerThanMin(string msg) : base(msg) { }
    }
}
