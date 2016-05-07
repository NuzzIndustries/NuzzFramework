using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NuzzFramework.Utilities;

namespace NuzzFramework
{
    /// <summary>
    /// Some hacks to support guaranteed execution of an app's initialize method via reflection
    /// </summary>
    internal static class NonHostedAppInitializer
    {
        
        private static Type FindAppType()
        {
            //Preload assemblies http://stackoverflow.com/questions/3021613/how-to-pre-load-all-deployed-assemblies-for-an-appdomain

            //look up the call stack until we find the assembly we want to use
            var assemblies = ReflectionUtility.LoadedAssemblies
                .Where(a => !a.GlobalAssemblyCache)
                .Where(a => a.GetName().Name != "XDesProc") //Disable reflection from visual studio
                .Where(a => !a.GetName().Name.StartsWith("Microsoft.VisualStudio"))
                .ToList();
            
            var types = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(Application)))
                .Where(t => !t.IsAbstract)
                .ToList();
            if (types.Count == 0)
                throw new InvalidOperationException("You must have an class that inherits from NuzzFramework.AppInitializer.");

            //Attempt to find a type with a primary type attribute
            var typesWithPrimaryAttribute = types.Where(t => t.GetCustomAttribute<PrimaryAppClassAttribute>() != null)
                .ToList();
            if (typesWithPrimaryAttribute.Count > 1)
                throw new InvalidOperationException("Only one class may have the PrimaryAppClassAttribute.");
            if (typesWithPrimaryAttribute.Count == 1)
                return typesWithPrimaryAttribute.Single();

            //Attempt to locate by the inheritance chain. 
            var assemblyInitializerMap = new Dictionary<Assembly, Type>();
            foreach (var t in types)
            {
                if (assemblyInitializerMap.ContainsKey(t.Assembly))
                    throw new InvalidOperationException("An assembly may not have more than one type that inherits from NuzzFramework.Application.");
                assemblyInitializerMap[t.Assembly] = t;
            }

            Type typeToInit = null;

            var entryAssembly = Assembly.GetEntryAssembly();
            if (assemblyInitializerMap.ContainsKey(entryAssembly))
                typeToInit = assemblyInitializerMap[entryAssembly];
            else
            {
                Dictionary<Type, int> map = new Dictionary<Type, int>();
                foreach (var type in types)
                {
                    //Get number of superclasses
                    map[type] = types.Where(t => type.IsSubclassOf(t)).Count();
                }
                typeToInit = map.OrderByDescending(t => t.Value).First().Key;
            }
            if (typeToInit == null)
                throw new InvalidOperationException("Unable to find AppInitializer.");
            return typeToInit;
        }
    /*
        protected NonHostedAppInitializer()
        {
            if (Globals.CurrentAppIsFrameworkHosted && Instance != null)
            {
                throw new InvalidOperationException("Cannot create multiple app initializers.");
            }
            if (Instance == null)
            {
                //Instance should be set for the first time an initializer is created, even for a non-hosted app
                //This is to ensure compatibility with designer mode stuff
                Instance = this;
            }
        }
        */

        internal static void InitializeApp()
        {
            ReflectionUtility.PreLoad();
            Type type = null;
            try
            {
                type = FindAppType();
            }
            catch (ReflectionTypeLoadException ex)
            {
                var message = $@"{ex.Message}
                    {ex.StackTrace}";
                throw new Exception(message);
            }
            var app = (Application)Activator.CreateInstance(type);
                app.Initialize();
            
        }
        /*
        protected virtual void Initialize()
        {
            if (Initialized)
                return;
            Instance._initialized = true;

            if (Configuration.AssetAssembly == null)
                Configuration.AssetAssembly = this.GetType().Assembly;

            var guid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), true)[0]).Value;
            var rando = Guid.NewGuid().ToString();
            Globals.TempFolderForInstance = Path.Combine(Path.GetTempPath(), guid + "nf", rando);
            ResXWebRequestFactory.Register();
        }

        /// <summary>
        /// Cleanup should be allowed to run as many times as needed. However
        /// </summary>
        protected virtual void Cleanup()
        {
            if (_cleanedup)
                throw new InvalidOperationException("Application cleanup has already been called.  Please make sure that this only calls once per application lifecycle.");
            Instance._cleanedup = true;

            var temproot = Path.Combine(Globals.TempFolderForInstance, "..");

            try { Directory.Delete(temproot, true); }
            catch (Exception) { }
        }*/
    }
}
