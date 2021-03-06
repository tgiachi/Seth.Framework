using System.Diagnostics;
using System.Reflection;
using Serilog;

namespace Seth.Api.Utils
{
    /// <summary>
    ///     Class for manage assemblies
    /// </summary>
    public class AssemblyUtils
    {
        private static readonly List<Assembly> _assembliesCache = new();


        public static List<Type> GetAttribute<T>()
        {
            return ScanAllAssembliesFromAttribute(typeof(T));
        }


        /// <summary>
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        public static List<Type> GetTypesWithCustomAttribute(Assembly assembly, Type attributeType)
        {
            return assembly.GetTypes().Where(type => type.GetCustomAttributes(attributeType, true).Length > 0).ToList();
        }


        public static Type GetInterfaceOfType(Type type)
        {
            try
            {
#pragma warning disable CS8603 // Possible null reference return.
                return type.GetInterfaces()[0].Name == $"I{type.Name}" ? type.GetInterfaces()[0] : null;
#pragma warning restore CS8603 // Possible null reference return.
            }
            catch
            {
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }
        }


        /// <summary>
        ///     Restituisce il tipo controllando tutti gli assembly
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static Type GetType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null) return type;

            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }

            foreach (var a in _assembliesCache)
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }

#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }

        /// <summary>
        ///     Controlla tutti gli se implementano una interfaccia
        /// </summary>
        /// <param name="customInterface"></param>
        /// <returns></returns>
        public static List<Type> GetTypesImplentInterface(Type customInterface)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(customInterface.IsAssignableFrom);

            return types.ToList();
        }

        /// <summary>
        ///     Prende tutt gli assembly (*.dll) da un attributo
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static List<Type> ScanAllAssembliesFromAttribute(Type attribute)
        {
            return ScanAssembly(attribute);
        }

        /// <summary>
        ///     Prende gli assembly dell'applicazione
        /// </summary>
        /// <returns></returns>
        public static List<Assembly> GetAppAssemblies()
        {
            BuildAssemblyCache();
            return _assembliesCache;
        }

        public static Assembly[] GetAppAssembliesArray()
        {
            BuildAssemblyCache();

            return _assembliesCache.ToArray();
        }

        private static void BuildAssemblyCache()
        {
            var logger = Log.ForContext<AssemblyUtils>();
            if (_assembliesCache.Count == 0)
            {
                var allAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(s => !s.IsDynamic).ToList();
                var codeBase = Assembly.GetExecutingAssembly().Location;
                //var uri = new UriBuilder(codeBase);
                //var path2 = Uri.UnescapeDataString(uri.Path);
                var path = Path.GetDirectoryName(codeBase);

#pragma warning disable CS8604 // Possible null reference argument for parameter 'path' in 'string[] Directory.GetFiles(string path, string searchPattern)'.
                var files = Directory.GetFiles(path, "*.dll");
#pragma warning restore CS8604 // Possible null reference argument for parameter 'path' in 'string[] Directory.GetFiles(string path, string searchPattern)'.
                foreach (var file in files)
                    try
                    {
                        var existsAssembly = (from s in allAssemblies where s.Location.Contains(file) select s)
                            .FirstOrDefault();

                        if (existsAssembly == null)
                            if (!file.Contains("Microsoft") && !file.Contains("System") &&
                                !file.Contains("DynamicData") && !file.Contains("Avalonia"))
                            {
                                var assembly = Assembly.LoadFile(file);
                                allAssemblies.Add(assembly);
                                logger.Debug($"Adding {file} to assembly");
                            }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                _assembliesCache.AddRange(allAssemblies);
            }
        }

        /// <summary>
        ///     Controlla tutti gli assembly se hanno l'attributo
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="filter"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<Type> ScanAssembly(Type attribute, string filter = "*.dll", string? path = null)
        {
            var result = new List<Type>();

            // InitAppDomain();

            BuildAssemblyCache();

            foreach (var assembly in _assembliesCache)
                try
                {
                    result.AddRange(GetTypesWithCustomAttribute(assembly, attribute));
                }
                catch (Exception ex)
                {
                    Log.Logger.Error($"{ex.Message}");
                    Console.WriteLine(ex);
                }


            return result;
        }

        /// <summary>
        ///     Prende gli attributi di un tipo
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static List<string> GetPropertiesFromAttribute(Type type, Type attribute)
        {
            var _dict = new List<string>();
            var props = type.GetProperties();
            foreach (var prop in props)
            {
                var attrs = prop.GetCustomAttributes(true);
                foreach (var attr in attrs)
                    if (attr.GetType() == attribute)
                        _dict.Add(prop.Name);
            }

            return _dict;
        }

        /// <summary>
        ///     Prende la versione
        /// </summary>
        /// <returns></returns>
        public static string GetVersion()
        {
            var fvi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
#pragma warning disable CS8603 // Possible null reference return.
            return fvi.ProductVersion;
#pragma warning restore CS8603 // Possible null reference return.
        }

        /// <summary>
        ///     Prende il nome del prodotto
        /// </summary>
        /// <returns></returns>
        public static string GetProductName()
        {
            var fvi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
#pragma warning disable CS8603 // Possible null reference return.
            return fvi.ProductName;
#pragma warning restore CS8603 // Possible null reference return.
        }

        /// <summary>
        ///     Aggiunge un assembly alla cache
        /// </summary>
        /// <param name="assembly"></param>
        public static void AddAssemblyToCache(Assembly assembly)
        {
            //if (_assembliesCache.Count == 0)
            //{
            //	var allAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(s => !s.IsDynamic).ToList();
            //	_assembliesCache.AddRange(allAssemblies);
            //}

            BuildAssemblyCache();

            if (!_assembliesCache.Contains(assembly))
                _assembliesCache.Add(assembly);
        }


        private static Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            return Assembly.LoadFrom(args.Name);
        }

        public static Type GetGenericTaskGenericType(Type returnType)
        {
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                return returnType.GetGenericArguments()[0]; // use this...

            return returnType;
        }

        public static bool IsGenericTaskType(Type returnType)
        {
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>)) return true;

            return false;
        }


        /// <summary>
        ///     Se debug
        /// </summary>
        /// <returns></returns>
        public static bool IsDebug()
        {
#if DEBUG
            return true;

#else
            return false;
#endif
        }
    }
}