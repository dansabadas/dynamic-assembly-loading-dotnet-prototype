using InterfacesStandardLibrary;
using System;

namespace ClassStandardLibrary1
{
    public class Class12 : Interface1
    {
        public void PrintMessage()
        {
            Console.WriteLine($"{GetType()}");
        }
    }
}
