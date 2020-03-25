namespace Utilities.Rules
{
    public class AndRule<T> : CompositeRule<T>
    {
        public override bool Pass(T input)
        {
            if (rules.Count == 0)
                return true;

            foreach (var r in rules)
            {
                if (!r.Pass(input))
                    return false;
            }
            return true;
        }
    }
}
