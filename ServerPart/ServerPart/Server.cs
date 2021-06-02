using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerPart
{
    public static class Server
    {
        public static List<Client> Clients = new List<Client>();

        public static void NewClient(Socket handle)
        {
            try
            {
                if (Clients.Count >= 2) return;
                    
                Client newClient = new Client(handle, Clients.Count);
                
                Clients.Add(newClient);
                Console.WriteLine("New Client connected {0}", handle.RemoteEndPoint);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error with addNewclient {0}", ex.Message);
            }

        }

        public static void EndClient(Client client)
        {
            try
            {
                client.End();
                Clients.Remove(client);
                Console.WriteLine("User {0} has been disconnected", client.UserName);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error with endClient {0}", ex.Message);
            }

        }


        public static void UpdateBoard()
        {
            try
            {
                int countUsers = Clients.Count;
                for (int i = 0; i < countUsers; i++)
                {
                    Clients[i].UpdateBoardd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error with UpdateBoard {0}", ex.Message);
            }
        }



        public static void UpdateAllChats()
        {
            try
            {
                int countUsers = Clients.Count;
                for (int i = 0; i < countUsers; i++)
                {
                    Clients[i].UpdateChat();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error with UpdateAllChats {0}", ex.Message);
            }
        }
        public static void UpdateTbxWithNewClients()
        {
            try
            {
                int countUsers = Clients.Count;
                for (int i = 0; i < countUsers; i++)
                {
                    Clients[i].UpdateTbxClient();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error with UpdateAllChats {0}", ex.Message);
            }
        }

    }
}
