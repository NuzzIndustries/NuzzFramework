using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuzzFramework
{
    //Internal global state
    internal static class Globals
    {
        internal static string TempFolderForInstance { get; set; }

        //Set this before calling initialize on the app to specify a non hosted app
        static Application m_CurrentApp;
        internal static Application CurrentApp {
            get { return m_CurrentApp; }
            set { m_CurrentApp = value; Application.Current = value; }
        }

        static bool m_CurrentAppIsFrameworkHosted = false;
        internal static bool CurrentAppIsFrameworkHosted
        {
            get { return CurrentApp != null ? m_CurrentAppIsFrameworkHosted : false; }
            set { m_CurrentAppIsFrameworkHosted = value; }
        }
    }
}
