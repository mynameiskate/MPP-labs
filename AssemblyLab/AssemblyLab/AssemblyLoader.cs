using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AssemblyLab
{
    public class AssemblyLoader
    {
        private string _assemblyPath;

        public AssemblyLoader(string assemblyPath)
        {
            _assemblyPath = assemblyPath;
        }

        private Type[] LoadAssembly()
        {
            if(!File.Exists(_assemblyPath))
            {
                throw new FileNotFoundException($"Assembly cannot be located: {_assemblyPath}");
            }

            Assembly assembly = Assembly.LoadFile(_assemblyPath);

            return assembly.GetTypes();
        }

        public MethodInfo[] GetPublicMethods()
        {
            Type[] assemblyTypes = LoadAssembly();

            if (assemblyTypes == null)
            {
                return null;
            }
            else
            {
                return assemblyTypes
                    .OrderByDescending(type => type.Namespace)
                    .SelectMany(type => type.GetMethods())       
                    .Where(method => method.IsPublic)
                    .OrderByDescending(method => method.Name)
                    .ToArray();
            }
        }

    }
}
