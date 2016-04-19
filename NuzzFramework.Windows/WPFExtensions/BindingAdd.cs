using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NuzzFramework.Windows
{
    public class BindingAdd : Binding
    {
        public BindingAdd()
            : base()
        {
        }

        public BindingAdd(string path)
            : base(path)
        {
        }

    }
}
