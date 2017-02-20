using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using GL.HttpServer.Managers;
using LiteDB;
using Serilog;
using Logger = GL.HttpServer.Logging.Logger;

namespace GL.HttpServer.Types
{
    internal delegate object CreateObject();
    internal class InstanceActivator
    {
        private static Dictionary<Type, Func<object>> _cacheCtor = new Dictionary<Type, Func<object>>();
        public static object CreateInstance(Type type, params object[] parameters)
        {
            Type[] paramTypes = null;
            if (parameters != null && parameters.Length > 0)
            {
                paramTypes = parameters.Select(a => a.GetType()).ToArray();
            }
            try
            {
                Func<object> c;
                if (_cacheCtor.TryGetValue(type, out c))
                {
                    return c();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "InvalidCtor");
                throw new Exception("InvalidCtor");
            }

            lock (_cacheCtor)
            {
                try
                {
                    Func<object> c = null;
                    if (_cacheCtor.TryGetValue(type, out c))
                    {
                        return c();
                    }
                    if (type.GetTypeInfo().IsClass)
                    {
                        if (type.IsGenericType)
                        {
                            //if (paramTypes != null)
                            //    _cacheCtor.Add(type, c = CreateClass(type, paramTypes));
                            //else _cacheCtor.Add(type, c = CreateClass(type));
                            //return c();
                            if (paramTypes != null)
                                return Activator.CreateInstance(type, parameters);
                            return Activator.CreateInstance(type);
                        }
                        if (paramTypes != null)
                            _cacheCtor.Add(type, c = CreateClass(type, paramTypes));
                        else _cacheCtor.Add(type, c = CreateClass(type));
                    }
                    else if (type.GetTypeInfo().IsInterface)
                    {
                        if (type.GetTypeInfo().IsGenericType)
                        {
                            var typeDef = type.GetGenericTypeDefinition();

                            if (typeDef == typeof(IList<>) ||
                                typeDef == typeof(ICollection<>) ||
                                typeDef == typeof(IEnumerable<>))
                            {
                                return CreateInstance(GetGenericListOfType(UnderlyingTypeOf(type)));
                            }
                            else if (typeDef == typeof(IDictionary<,>))
                            {
                                var k = type.GetTypeInfo().GenericTypeArguments[0];
                                var v = type.GetTypeInfo().GenericTypeArguments[1];
                                return CreateInstance(GetGenericDictionaryOfType(k, v));
                            }
                        }

                        return null;
                    }
                    else 
                    {
                        if (paramTypes != null)
                            _cacheCtor.Add(type, c = CreateStruct(type, paramTypes));
                        else _cacheCtor.Add(type, c = CreateStruct(type));
                    }

                    return c();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "InvalidCtor");
                    return null;
                }
            }
        }

        #region Utils

        public static Type UnderlyingTypeOf(Type type)
        {
            if (!type.GetTypeInfo().IsGenericType) return type;
            return type.GetTypeInfo().GenericTypeArguments[0];
        }

        public static Type GetGenericListOfType(Type type)
        {
            var listType = typeof(List<>);
            return listType.MakeGenericType(type);
        }

        public static Type GetGenericDictionaryOfType(Type k, Type v)
        {
            var listType = typeof(Dictionary<,>);
            return listType.MakeGenericType(k, v);
        }

        public static Func<object> CreateClass(Type type, Type[] paramTypes)
        {
            var dynamicMethod = new DynamicMethod("_", type, paramTypes);
            var il = dynamicMethod.GetILGenerator();

            il.Emit(OpCodes.Newobj, type.GetConstructor(paramTypes));
            il.Emit(OpCodes.Ret);

            return (Func<object>)dynamicMethod.CreateDelegate(typeof(Func<>).MakeGenericType(type));
        }

        public static Func<object> CreateClass(Type type)
        {
            var dynamicMethod = new DynamicMethod("_", type, null);
            var il = dynamicMethod.GetILGenerator();

            il.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Ret);

            return (Func<object>)dynamicMethod.CreateDelegate(typeof(Func<>).MakeGenericType(type));
        }

        public static Func<object> CreateStruct(Type type, Type[] paramTypes = null)
        {
            var dynamicMethod = new DynamicMethod("_", typeof(object), paramTypes);
            var il = dynamicMethod.GetILGenerator();
            var local = il.DeclareLocal(type);

            il.Emit(OpCodes.Ldloca_S, local);
            il.Emit(OpCodes.Initobj, type);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Box, type);
            il.Emit(OpCodes.Ret);

            return (Func<object>)dynamicMethod.CreateDelegate(typeof(Func<>).MakeGenericType(type));
        }

        #endregion
    }
}
