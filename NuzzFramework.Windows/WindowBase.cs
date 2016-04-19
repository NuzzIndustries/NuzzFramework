using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace NuzzFramework.Windows
{
    public abstract class WindowBase : Window, IWindow
    {
        /*
        #region INotifyPropertyChanged Code
        public event PropertyChangedEventHandler PropertyChanged;
        private Dictionary<string, object> _notifyproperties = new Dictionary<string, object>();

        protected T GetField<T>([CallerMemberName]string propertyName = null)
        {
            if (!_notifyproperties.ContainsKey(propertyName))
                return default(T);
            return (T)_notifyproperties[propertyName];
        }

        protected bool SetField<T>(T value, [CallerMemberName]string propertyName = null)
        {
            //Determine if equal
            var field = GetField<T>(propertyName);
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;

            //Set the value
            _notifyproperties[propertyName] = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        */

        //UI Configuration class containing System.Drawing.Color 
        //and Brush properties (platform-agnostic styling)
       // public UIStyle UIStyle => Core.UIStyle.Current;

        //IValueConverter that converts System.Drawing.Color properties into WPF-equivalent Colors and Brushes 
        public static UniversalValueConverter UniversalValueConverter { get; } = new UniversalValueConverter();

        /*
        public int HScale
        {
            get { return GetField<int>(); }
            set { SetField(value); }
        }

        public int VScale
        {
            get { return GetField<int>(); }
            set { SetField(value); }
        }*/

        public TopLevelControl TopLevelControl { get; set; }

        public WindowBase()
        {
           // Initializer.Initialize();
           // this.DataContext = this;

            //Add window name to namespace so that runtime properties can be referenced from XAML
            //(Name setting must be done here and not in xaml because this is a base class)
            //You probably won't need to, but working example is here in case you do.
        //    var ns = new NameScope();
        //    NameScope.SetNameScope(this, ns);
        //    ns["window"] = this;
        }

        protected override void OnInitialized(EventArgs e)
        {
            //Set resources
        //    Resources = new XAMLResources();
         //   SetDefaults();

            //Call Initialize Component via Reflection, so you do not need 
            //to call InitializeComponent() every time in the sub class
            this.GetType()
                .GetMethod("InitializeComponent",
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance)
                .Invoke(this, null);
            base.OnInitialized(e);
        }

        private void WindowBase_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Default scale = 100%, 800x600
          //  HScale = (int)e.NewSize.Height;
        }

        //Stub method here so that the design-time instantiator has a method to call
        void InitializeComponent() { }
    }
}
