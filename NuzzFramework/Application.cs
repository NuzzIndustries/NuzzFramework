using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NuzzFramework.Utilities;

namespace NuzzFramework
{
    public abstract class Application
    {
        public static Application Current { get; internal set; }
        public static bool IsFrameworkHosted {  get { return Globals.CurrentAppIsFrameworkHosted; } }

        static bool Initialized { get; set; } = false;

        public virtual void Initialize()
        {
            if (Globals.CurrentApp == null)
            {
                //If CurrentApp is set before this initialize method is called, then it is not framework hosted
                Globals.CurrentApp = this;
                Globals.CurrentAppIsFrameworkHosted = true;
            }

            if (Initialized)
                return;
            Initialized = true;

            ReflectionUtility.PreLoad();

            if (Configuration.AssetAssembly == null)
                Configuration.AssetAssembly = this.GetType().Assembly;

            var guid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), true)[0]).Value;
            var rando = Guid.NewGuid().ToString();
            Globals.TempFolderForInstance = Path.Combine(Path.GetTempPath(), guid + "nf", rando);
            ResXWebRequestFactory.Register();
        }

        public virtual void Cleanup()
        {
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal static void InitAppFromNonHostedProcess()
        {
            if (Globals.CurrentApp != null)
                return;
            Globals.CurrentAppIsFrameworkHosted = false;

            //Get app type
            NonHostedAppInitializer.InitializeApp();
        }
    }
}
