using System.Text.RegularExpressions;

namespace Utilities.Rules
{
    public class StringRule : IRule<string>
    {
        protected Regex Regex;
        protected StringRule(string regexExpression)
        {
            Regex = new Regex(regexExpression);
        }
        public virtual string Hint { get; } = "错误";
        public virtual bool Pass(string input) => Regex.IsMatch(input);

        public override string ToString() => Regex.ToString();
    }
}
