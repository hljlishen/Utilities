using System;

namespace Utilities.InputValidation
{
    public class TextBoxValueDoesntMatchRuleException : Exception
    {
        public TextBoxValueDoesntMatchRuleException(string message) : base(message)
        {
        }
    }
}
