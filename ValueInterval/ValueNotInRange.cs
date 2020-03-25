using System;

namespace Utilities.ValueIntervals
{
    public class ValueNotInRange : Exception
    {
        public ValueNotInRange(string msg) : base(msg) { }
    }
}
