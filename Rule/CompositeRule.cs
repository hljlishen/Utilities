using System.Collections.Generic;

namespace Utilities.Rules
{
    public abstract class CompositeRule<T> : IRule<T>
    {
        protected List<IRule<T>> rules = new List<IRule<T>>();

        public void AddRule(IRule<T> rule)
        {
            if(!rules.Contains(rule))
                rules.Add(rule);
        }

        public void RemoveRule(IRule<T> rule)
        {
            if (rules.Contains(rule))
                rules.Remove(rule);
        }

        public abstract bool Pass(T input);
    }
}
