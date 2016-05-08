using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuzzFramework.Windows
{
    public class Binding : CalcBinding.Binding
    {
        public Binding()
            : base()
        {
          //  this.Converter = WindowBase.UniversalValueConverter;
        }

        public Binding(string path)
            : base(path)
        {
           // this.Converter = WindowBase.UniversalValueConverter;
        }
    }
}
