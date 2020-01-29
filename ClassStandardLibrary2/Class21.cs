using InterfacesStandardLibrary;
using System;

namespace ClassStandardLibrary2
{
    [Serializable]
    public class Class21 : Interface1
    {
        public void PrintMessage()
        {
            Console.WriteLine($"{GetType()}");
        }
    }
}
