using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuzzFramework;

namespace NuzzFramework.Windows
{
    public class ViewModelBase
    {
        public ModelBase Model { get; set; }
        public IWindow ParentWindow { get; set; }

        /*
        public double HScale
        {
            get
            {
                if (App.InDesignMode)
                    return 100;
                return ParentWindow?.HScale ?? 100;
            }
            set
            {
                SetField(value);
            }
        }
        public double VScale
        {
            get
            {
                if (App.InDesignMode)
                    return 100;
                return ParentWindow?.VScale ?? 100;
            }
            set
            {
                SetField(value);
            }
        }

        public double DesignWidth { get; set; } = 300.0;
        public double DesignHeight { get; set; } = 200.0;
        public new double Width
        {
            get
            {
                if (App.InDesignMode)
                    return DesignWidth;
                return base.Width;
            }
            set
            {
                if (App.InDesignMode)
                    base.Width = Width;
                else
                    base.Width = value;
            }
        }
        public new double Height
        {
            get
            {
                if (App.InDesignMode)
                    return DesignHeight;
                return base.Height;
            }
            set
            {
                if (App.InDesignMode)
                    base.Height = Height;
                else
                    base.Height = value;
            }
        }
        
        public ViewModelBase()
        {
            HScale = 100;
            VScale = 100;
        }*/
    }
}
