using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace DesignPatterns_TCPClient_Listener.ServerSide
{
    class MyServerProxy
    {
        // Set the TcpListener on Local host, port 13000.
        private TcpListener server;
        private IPAddress localAddr = IPAddress.Parse("127.0.0.1");
        private Int32 port = 13000;
        public bool Active = false;

        // Server Data
        private Stack<string> stack1 = new Stack<string>();
        private Stack<string> stack2 = new Stack<string>();
        private Queue<string> queue1 = new Queue<string>();
        private Queue<string> queue2 = new Queue<string>();



        public void run()
        {
            SpoolUpListener("Listener1");
            Task.Run(() => WaitForClient("Client1"));
            Task.Run(() => WaitForClient("Client2"));
            //Task.Run(() => WaitForClient("Client3"));
            //WaitForClient("Client1");
            Console.ReadLine();
        }

        public bool SpoolUpListener(string ListenerID)
        {
            try
            {
                this.server = new TcpListener(this.localAddr, this.port);
                this.server.Start();
                return true;
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                // Stop listening for new clients.
                this.server.Stop();
                return false;
            }

        }

        async public Task WaitForClient(string ClientID)
        {
            Console.Write("\n{0} client listening for a connection", ClientID);
            TcpClient Client = server.AcceptTcpClient();
            
            Console.WriteLine("\n{0} connected", ClientID);
            NetworkStream ClientStream = Client.GetStream();
  
            while (Client.Connected)
            {
                while (ClientStream.DataAvailable)
                {
                    string Data = WaitForIncomingStream(ClientStream);
                    char[] splitChar = new char[' '];
                    string[] dataParse = Data.Split();
                    Console.WriteLine("SERVER DEBUG WRITE: I Parsed and the Command word is: {0}", dataParse[1]);


                }
                System.Threading.Thread.Sleep(500);
            }
            Console.ReadLine();
        }

        public string WaitForIncomingStream(NetworkStream Stream)
        {
            try
            {
                Byte[] bytes = new Byte[256];
                String data = null;
                int streamLength;
                streamLength = Stream.Read(bytes, 0, bytes.Length);
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, streamLength);
                Console.WriteLine("\n{0}<<: ", data);
                return data;
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                // Stop listening for new clients.
                this.server.Stop();
                return null;
            }
        }


        public void OriginalConnect()
        {
          
            try
            {
                

                // Start Listening for Client requests
                this.server.Start();
                this.Active = true;

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop. 
                while (true)
                {
                    Console.Write("SERVER Waiting for a connection...\n ");

                    // Perform a blocking call to accept requests. 
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("SERVER DEBUG WRITE Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();
                    int streamLength;

                    // Loop to receive all the data sent by the client. 
                    while ((streamLength = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, streamLength);
                        Console.WriteLine("\n\n------\nSERVER Received: {0}", data);

                        char[] splitChar = new char[' '];
                        string[] dataParse= data.Split();
                        Console.WriteLine("SERVER DEBUG WRITE: I Parsed and the first word is: {0}", dataParse[0]);


                        byte[] msg = System.Text.Encoding.ASCII.GetBytes("Hey I recieved something!!");

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("\n\n------\nSERVER Sent:{0}", msg);
                    }

                    // Shutdown and end connection
                    client.Close();
                    
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
                this.Active = true;
            }

            Console.WriteLine("\nHit enter to Leave Server Loop...");
            Console.Read();
        }



    }
}
