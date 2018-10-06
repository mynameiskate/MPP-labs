using System;
using System.Reflection;

namespace AssemblyLab
{
    class Program
    {
        private const int ARG_COUNT = 1;

        static int Main(string[] args)
        {
            if (args.Length != ARG_COUNT)
            {
                Console.WriteLine("You should provide pass to assembly as parameter.");
                return 1;
            }

            try
            {
                var assemblyLoader = new AssemblyLoader(args[0]);
                var methodInfos = assemblyLoader.GetPublicMethods();

                PrintAssemblyMethodsInfo(methodInfos);

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }

        private static void PrintAssemblyMethodsInfo(MethodInfo[] methodInfos)
        {
            if (methodInfos == null)
            {
                Console.WriteLine("No public methods found.");
            }
            else
            {
                foreach (MethodInfo methodInfo in methodInfos)
                {
                    Console.WriteLine($"{methodInfo.DeclaringType.FullName}.{methodInfo.Name}");
                }
            }
        }
    }
}
