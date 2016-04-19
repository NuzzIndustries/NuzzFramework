using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuzzFramework.Windows
{
    /// <summary>
    /// Represents a root-level window control
    /// </summary>
    public abstract class TopLevelControl : ControlBase
    {
        public TopLevelControl()
        {
        }

        protected override void OnInitialized(EventArgs e)
        {
         //   if (this.ParentWindow != null)
        //        this.ParentWindow.TopLevelControl = this;
            base.OnInitialized(e);
        }

        //Stub method here so that the design-time instantiator has a method to call
        void InitializeComponent() { }
    }
}
