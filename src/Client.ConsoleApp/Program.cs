using Library.Sdk;
using System.Threading;

namespace Client.ConsoleApp
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                Tip.Run();

                Thread.Sleep(1000);
            }
        }
    }
}
