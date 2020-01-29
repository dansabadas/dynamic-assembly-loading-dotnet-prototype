using InterfacesStandardLibrary;
using System;

namespace ClassStandardLibrary1
{
    [Serializable]
    public class Class12 : Interface2
    {
        public void PrintMessage()
        {
            Console.WriteLine($"{GetType()}");
        }
    }
}
