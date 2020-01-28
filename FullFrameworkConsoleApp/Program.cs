using InterfacesStandardLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace FullFrameworkConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //ClassStandardLibrary1 ClassStandardLibrary2
            Console.WriteLine("Hello World");
            // Use the file name to load the assembly into the current
            // application domain.
            //Assembly a = Assembly.Load("example");
            //// Get the type to use.
            //Type myType = a.GetType("Example");
            //// Get the method to call.
            //MethodInfo myMethod = myType.GetMethod("MethodA");
            //// Create an instance.
            //object obj = Activator.CreateInstance(myType);
            //// Execute the method.
            //myMethod.Invoke(obj, null);

            var currentAssembly = Assembly.GetExecutingAssembly();
            List<Assembly> scannedAssemblies = new List<Assembly>();
            Interface1 if1 = null;
            foreach (AssemblyName referencedAssemblyName in currentAssembly.GetReferencedAssemblies())
            {
                Console.WriteLine($"{referencedAssemblyName.FullName}");
                Assembly asm = Assembly.Load(referencedAssemblyName.ToString());
                FileInfo currentAssemblyFileInfo = new FileInfo(currentAssembly.Location);
                FileInfo asmFileInfo = new FileInfo(asm.Location);
                if (currentAssemblyFileInfo.DirectoryName == asmFileInfo.DirectoryName)
                {
                    scannedAssemblies.Add(asm);
                }
            }

            // for unloading assemblies - first create App Domains
            //https://docs.microsoft.com/en-us/dotnet/framework/app-domains/use
        }
    }
}
