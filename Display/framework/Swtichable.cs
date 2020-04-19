namespace Utilities.Display
{
    public interface ISwtichable
    {
        void On();
        void Off();
        bool IsOn { get; }
        string Name { get; set; }
    }
}
