using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
