using InterfacesStandardLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FullFrameworkConsoleApp
{
    class Program
    {
        static void Main()
        {
            //ClassStandardLibrary1 ClassStandardLibrary2

            //object obj = Activator.CreateInstance(myType);
            //// Execute the method.
            //myMethod.Invoke(obj, null);

            var currentAssembly = Assembly.GetExecutingAssembly();

            var directlyReferencedAssemblies = RetrieveDirectlyReferencedAssemblies(currentAssembly);
            var directlyReferencedAssembliesExcludingGAC = directlyReferencedAssemblies
                .Where(asm =>
                {
                    var currentAssemblyFileInfo = new FileInfo(currentAssembly.Location);
                    var asmFileInfo = new FileInfo(asm.Location);
                    string printMessage = $"{asm.FullName} ";

                    bool isCustomCreatedAssembly = currentAssemblyFileInfo.DirectoryName == asmFileInfo.DirectoryName;
                    if (isCustomCreatedAssembly)
                    {
                        printMessage += "LOADED";
                    }

                    Console.WriteLine(printMessage);

                    return isCustomCreatedAssembly;
                })
                .ToList();

            // for unloading assemblies - first create App Domains
            //https://docs.microsoft.com/en-us/dotnet/framework/app-domains/use
            // AppDomain.CreateInstanceAndUnwrap to be used - test what happens after unloading!


            var sameFolderAssemblies = RetrieveSameFolderAssemblies(currentAssembly);

            (bool, Type) ImplementsDesiredInterfaceFunc(Assembly asm)
            {
                var areImplementingTypes = asm.GetTypes()
                    .Any(type => typeof(Interface1).IsAssignableFrom(type) && !type.IsInterface);

                if (areImplementingTypes)
                {
                    return (true,
                        asm.GetTypes().First(type => typeof(Interface1).IsAssignableFrom(type) && !type.IsInterface));
                }

                return (false, null);
            }

            var sameFolderAssembliesImplementingDesiredInterface = sameFolderAssemblies
                .Where(asm =>
                {
                    string printMessage = $"{asm.Location} ";
                    var (existsClassImplementingMatchingInterface, _) = ImplementsDesiredInterfaceFunc(asm);

                    if (existsClassImplementingMatchingInterface)
                    {
                        printMessage += "LOADED";
                    }

                    Console.WriteLine(printMessage);

                    return existsClassImplementingMatchingInterface;
                })
                .ToList();

            var finallyScannedAssemblies = directlyReferencedAssembliesExcludingGAC.Union(sameFolderAssembliesImplementingDesiredInterface).ToList();



            var firstAssembly = sameFolderAssembliesImplementingDesiredInterface.First();
            var (_, desiredType) = ImplementsDesiredInterfaceFunc(firstAssembly);

            AppDomainSetup domainSetup = new AppDomainSetup { PrivateBinPath = firstAssembly.Location };
            AppDomain domain = AppDomain.CreateDomain("MyDomain", null, domainSetup);
            Console.WriteLine($"Host domain: {AppDomain.CurrentDomain.FriendlyName}");
            Console.WriteLine($"child domain: {domain.FriendlyName}");

            //Interface1 i1 = (Interface1) domain.CreateInstanceFromAndUnwrap(firstAssembly.Location, desiredType.FullName);
            //i1.PrintMessage();
            //AppDomain.Unload(domain);
            //Console.WriteLine($"unloaded");
            //i1.PrintMessage();

            Demo.Marshalling();
        }

        private static List<Assembly> RetrieveSameFolderAssemblies(Assembly currentAssembly)
        {
            List<Assembly> scannedAssemblies = new List<Assembly>();
            FileInfo currentAssemblyFileInfo = new FileInfo(currentAssembly.Location);
            var files = Directory.GetFiles(currentAssemblyFileInfo.DirectoryName, "*.dll");
            foreach (var path in files)
            {
                var currentlyScannedAssembly = Assembly.LoadFrom(path);
                scannedAssemblies.Add(currentlyScannedAssembly);
            }

            return scannedAssemblies;
        }

        private static List<Assembly> RetrieveDirectlyReferencedAssemblies(Assembly currentAssembly)
        {
            List<Assembly> scannedAssemblies = new List<Assembly>();
            var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
            foreach (AssemblyName referencedAssemblyName in referencedAssemblies)
            {
                Assembly asm = Assembly.Load(referencedAssemblyName.ToString());
                scannedAssemblies.Add(asm);
            }

            return scannedAssemblies;
        }
    }
}
