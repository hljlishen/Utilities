using System;

namespace Utilities.IniBind.AttributeInterception
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class IniKeyAttribute : Attribute
    {
        public IniKeyAttribute(string file, string section, string key)
        {
            File = file;
            Section = section;
            Key = key;
        }

        public string File { get; set; }
        public string Section { get; set; }
        public string Key { get; set; }
    }
}
