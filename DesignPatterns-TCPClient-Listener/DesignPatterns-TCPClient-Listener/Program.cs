using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns_TCPClient_Listener.ServerSide;
using DesignPatterns_TCPClient_Listener.ClientSide;

namespace DesignPatterns_TCPClient_Listener
{
    class Program
    {
        static void Main(string[] args)
        {
            MyServerProxy ServerProxy = new MyServerProxy();
            var serverTask = Task.Run(() => ServerProxy.run_async());

            MyClient ClientProxy1 = new MyClient("ClientProxy1");
            MyClient ClientProxy2 = new MyClient("ClientProxy2");
            var clientTask1 = Task.Run(() => ClientProxy1.Connect("127.0.0.1", "TESTING1"));
            var clientTask2 = Task.Run(() => ClientProxy2.Connect("127.0.0.1", "TESTING2"));


            Console.WriteLine("\n Press Enter to Leave Main Loop...");
            Console.Read();
        }
    }
}
