using System.Text.RegularExpressions;

namespace Utilities.Rules
{
    public class LetterLengthRule : StringRule
    {
        private uint max, min;
        public LetterLengthRule(uint minLength, uint maxLength) : base("")
        {
            max = maxLength;
            min = minLength;
            Regex = new Regex($"^[a-zA-Z]{{{minLength},{maxLength}}}$");
        }

        public override string Hint { get => $"请输入长度{min}-{max}的字母"; }
    }
}
