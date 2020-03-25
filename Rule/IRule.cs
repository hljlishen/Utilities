namespace Utilities.Rules
{
    public interface IRule<T>
    {
        bool Pass(T input);
    }
}
