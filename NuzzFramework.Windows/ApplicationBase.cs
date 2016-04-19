using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuzzFramework.Windows
{
    public abstract class ApplicationBase : IApplication
    {
        public static ApplicationBase Current { get; set; }
    }
}
