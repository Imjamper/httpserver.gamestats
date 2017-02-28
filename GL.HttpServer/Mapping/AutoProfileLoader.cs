using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace GL.HttpServer.Mapping
{
    public static class AutoProfileLoader
    {
        public static void Start(params Assembly[] assemblies)
        {
            var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            if (assemblies != null && assemblies.Length > 0)
            {
                foreach (var assembly in assemblies)
                {
                    if (domainAssemblies.All(a => a.FullName != assembly.FullName))
                        domainAssemblies.Add(assembly);
                }
            }
            RegisterProfiles(domainAssemblies);
        }
        
        public static void RegisterProfiles(IEnumerable<Assembly> assemblies)
        {
            Mapper.Initialize(configuration =>
            {
                var assembliesToScan = assemblies as IList<Assembly> ?? assemblies.ToList();

                if (!assembliesToScan.Any())
                    assembliesToScan = new[] { Assembly.GetCallingAssembly() };

                configuration.AddProfiles(assembliesToScan);
            });
        }
    }
}
