using Utilities.ValueIntervals;

namespace Utilities.Rules
{
    public class IntStrInRange : StringRule
    {
        private ValueInterval interval;
        public IntStrInRange(ValueInterval interval) : base(@"^[+]{0,1}(\d+)$")
        {
            this.interval = interval;
        }

        public IntStrInRange(int min, int max) : this(ValueInterval.CloseClose(min, max))
        {

        }

        public override bool Pass(string input)
        {
            if (!base.Pass(input))
                return false;
            int value = int.Parse(input);
            return interval.IsInRange(value);
        }

        public override string Hint { get => $"请输入{interval.ToString()}范围内的整数";}
    }
}
