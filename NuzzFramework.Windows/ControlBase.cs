using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NuzzFramework.Windows
{
    public abstract class ControlBase : UserControl
    {
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
           // if (EqualityComparer<T>.Default.Equals(field, value)) return false;

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

        public static ControlBase CurrentlyInstantiating { get; set; }

        private ViewModelBase ViewModel { get; set; }

        public ControlBase()
        {
            CurrentlyInstantiating = this;
            //Initializer.Initialize();

            var ns = new NameScope();
            NameScope.SetNameScope(this, ns);
            ns["main"] = this;
        }

        protected override void OnInitialized(EventArgs e)
        {
            if (ViewModel != null)
                this.DataContext = ViewModel;
            ViewModel.ParentWindow = Window.GetWindow(this) as WindowBase;

            //Set resources
          //  Resources = new XAMLResources();

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

        //Stub method here so that the design-time instantiator has a method to call
        void InitializeComponent() { }

        /*
        public static List<T> GetLogicalChildCollection<T>(object parent) where T : DependencyObject
        {
            List<T> logicalCollection = new List<T>();
            GetLogicalChildCollection(parent as DependencyObject, logicalCollection);
            return logicalCollection;
        }

        private static void GetLogicalChildCollection<T>(DependencyObject parent, List<T> logicalCollection) where T : DependencyObject
        {
            IEnumerable children = LogicalTreeHelper.GetChildren(parent);
            foreach (object child in children)
            {
                if (child is DependencyObject)
                {
                    DependencyObject depChild = child as DependencyObject;
                    if (child is T)
                    {
                        logicalCollection.Add(child as T);
                    }
                    GetLogicalChildCollection(depChild, logicalCollection);
                }
            }
        }

        public IList<DependencyProperty> GetAttachedProperties(DependencyObject obj)
        {
            List<DependencyProperty> result = new List<DependencyProperty>();

            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(obj,
                new Attribute[] { new PropertyFilterAttribute(PropertyFilterOptions.All) }))
            {
                DependencyPropertyDescriptor dpd =
                    DependencyPropertyDescriptor.FromProperty(pd);

                if (dpd != null)
                {
                    result.Add(dpd.DependencyProperty);
                }
            }

            return result;
        }

        protected void RebindAll()
        {
            foreach (var control in GetLogicalChildCollection<Control>(this))
            {
                var props = GetAttachedProperties(control);
                foreach (var prop in props)
                {
                    var binding = control.GetBindingExpression(prop);
                    if (binding != null)
                        binding.UpdateTarget();
                }
            }
        }
        */
    }
}
