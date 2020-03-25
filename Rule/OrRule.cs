namespace Utilities.Rules
{
    public class OrRule<T> : CompositeRule<T>
    {
        public override bool Pass(T input)
        {
            if (rules.Count == 0)
                return true;

            foreach (var r in rules)
            {
                if (r.Pass(input))
                    return true;
            }
            return false;
        }
    }
}
