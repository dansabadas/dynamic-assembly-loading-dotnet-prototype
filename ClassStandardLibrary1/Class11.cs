using InterfacesStandardLibrary;
using System;

namespace ClassStandardLibrary1
{
    [Serializable]
    public class Class11 : Interface1
    {
        public void PrintMessage()
        {
            Console.WriteLine($"{GetType()} running in {AppDomain.CurrentDomain.FriendlyName}");
        }
    }
}
