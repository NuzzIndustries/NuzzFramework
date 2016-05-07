using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NuzzFramework.Utilities
{
    public static class ReflectionUtility
    {
        internal static HashSet<Assembly> LoadedAssemblies { get; private set; } = new HashSet<Assembly>();

        internal static void PreLoad(Assembly appAssembly = null)
        {
            Thread.Sleep(50);

            //If we don't know what the app assembly is, get a list of loaded assemblies and start there
            if (appAssembly == null)
            {
                AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach(a => LoadedAssemblies.Add(a));
            }
            
            //Get all loaded assemblies and load all of those
            ForceLoadDependencies(appAssembly);
        }

        private static void ForceLoadDependencies(Assembly root)
        {
            AssemblyName[] referencedAssemblies = null;

            if (root == null) //No root defined = recursively load from what's already loaded
            {
                foreach (var a in LoadedAssemblies)
                    ForceLoadDependencies(a);
                return;
            }
            else
                referencedAssemblies = root.GetReferencedAssemblies();

            //Return if already loaded
            bool alreadyLoaded = !LoadedAssemblies.Add(root);
            if (alreadyLoaded)
                return;

            //Load the assemblies
            foreach (AssemblyName nextReferencedAssemblyName in referencedAssemblies)
            {
                Assembly nextReferencedAssembly = null;
                try
                {
                    nextReferencedAssembly = Assembly.Load(nextReferencedAssemblyName);
                    LoadedAssemblies.Add(nextReferencedAssembly);
                }
                catch
                {
                    continue;
                }

                if (nextReferencedAssembly.GlobalAssemblyCache)
                    continue;

                ForceLoadDependencies(nextReferencedAssembly);
            }
        }
    }
}
