using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace GL.HttpServer.Mapping
{
    public static class AutoProfileLoader
    {
        public static void Start()
        {
            RegisterProfiles(AppDomain.CurrentDomain.GetAssemblies());
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
