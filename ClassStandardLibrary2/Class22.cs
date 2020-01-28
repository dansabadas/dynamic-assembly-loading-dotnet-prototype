using InterfacesStandardLibrary;
using System;

namespace ClassStandardLibrary2
{
    public class Class22 : Interface2
    {
        public void PrintMessage()
        {
            Console.WriteLine($"{GetType()}");
        }
    }
}
