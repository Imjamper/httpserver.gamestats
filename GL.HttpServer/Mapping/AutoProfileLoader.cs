using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace GL.HttpServer.Mapping
{
    public static class AutoProfileLoader
    {
        public static void RegisterDomain()
        {
            RegisterProfiles(AppDomain.CurrentDomain.GetAssemblies());
        }

        public static void RegisterProfiles(params Assembly[] assemblies)
        {
            RegisterProfiles(EnsureAssembly(assemblies, Assembly.GetCallingAssembly()));
        }
        
        public static void RegisterProfiles(IEnumerable<Assembly> assemblies)
        {
            Mapper.Initialize(configuration =>
                GetConfiguration(Mapper.Configuration, EnsureAssembly(assemblies, Assembly.GetCallingAssembly())));
        }
        
        private static IEnumerable<Assembly> EnsureAssembly(IEnumerable<Assembly> argAssemblies, Assembly callingAssembly)
        {
            var assembliesArray = new Assembly[] { };

            if (argAssemblies != null)
                assembliesArray = argAssemblies.ToArray();

            if (!assembliesArray.Any())
                return new[] { callingAssembly };

            return assembliesArray;
        }
        
        private static void GetConfiguration(IConfiguration configuration, IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
                foreach (var profileClass in GetProfileClassesFrom(assembly))
                    configuration.AddProfile((Profile)Activator.CreateInstance(profileClass));
        }

        private static IEnumerable<Type> GetProfileClassesFrom(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes()
                    .Where(t =>
                        t != typeof(Profile)
                        && typeof(Profile).IsAssignableFrom(t)
                        && !t.IsAbstract
                    );
            }
            catch
            {
                return new Type[] { };
            }
        }
    }
}
