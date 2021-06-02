using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace ServerPart
{
    class Program
    {
        private const string _serverHost = "localhost";
        private const int _serverPort = 9933;
        private static Thread _serverThread;
        static void Main(string[] args)
        {
            _serverThread = new Thread(StartServer);
            _serverThread.IsBackground = true;
            _serverThread.Start();
            while (true)
            {
                HandlerCommands(Console.ReadLine());
            }
        }

        public static void HandlerCommands(string cmd)
        {
            cmd = cmd.ToLower();
            if (cmd.Contains("/getusers"))
            {
                int countUsers = Server.Clients.Count;
                for (int i = 0; i < countUsers; i++)
                {
                    Console.WriteLine("[{0}]: {1}", i, Server.Clients[i].UserName);
                }
            }
        }

        private static void StartServer()
        {
            IPHostEntry iPHost = Dns.GetHostEntry(_serverHost);
            IPAddress iPAddress = iPHost.AddressList[0];
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, _serverPort);
            Socket socket = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(iPEndPoint);
            socket.Listen(1000);
            Console.WriteLine("Server has been started on IP {0}", iPEndPoint);
            while (true)
            {
                try
                {
                    Socket user = socket.Accept();

                    Server.NewClient(user);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error {0}", ex.Message);
                }

            }
        }
    }
}
