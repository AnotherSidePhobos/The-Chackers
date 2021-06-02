using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPart
{
    public static class ChatController
    {
        private const int _maxMessage = 100;

        public static List<Message> Chat = new List<Message>();
        public static List<Message> Move = new List<Message>();
        public static List<Client> Clients = new List<Client>();

        public struct Message
        {
            public string userName;
            public string data;
            public Message(string name, string msg)
            {
                userName = name;
                data = msg;
            }
        }
        

        public class Client
        {
            public int Id;
            public string mess;
            int count = Clients.Count + 1;
            public Client(string msg)
            {
                mess = msg;
                Id = count;
            }
        }

        public static void ChangePosition(string userName, string coords)
        {
            
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(coords)) return;
            Message newMove = new Message(userName, coords);
            Move.Add(newMove);
            Console.WriteLine($"User {userName} made move");
            Server.UpdateBoard();
        }


        public static void AddClntMessage(string msg)
        {
            try
            {
                if (string.IsNullOrEmpty(msg)) return;
                int countMessages = Clients.Count;
                Client newClient = new Client(msg);
                Clients.Add(newClient);
                Server.UpdateTbxWithNewClients();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error with AddMessage {0}", ex.Message);
            }


        }

        public static void AddMessage(string userName, string msg)
        {
            try
            {
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(msg)) return;
                int countMessages = Chat.Count;
                if (countMessages > _maxMessage) ClearChat();
                Message newMessage = new Message(userName, msg);
                Chat.Add(newMessage);
                Console.WriteLine("New message from {0}", userName);
                Server.UpdateAllChats();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error with AddMessage {0}", ex.Message);
            }


        }

        public static void ClearChat()
        {
            Chat.Clear();
        }

        
        public static string GetBoard()
        {
            try
            {
                string data = "#Updateboard&";
                int countMessages = Move.Count;
                if (countMessages <= 0) return string.Empty;
                for (int i = 0; i < countMessages; i++)
                {
                    data += String.Format("{0}~{1}|", Move[i].userName, Move[i].data);
                }
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error with GetBoard {0}", ex.Message); return string.Empty;
            }

        }
        public static string GetTbxClient()
        {
            try
            {
                string data = "#updateTbxClient&";
                int countMessages = Clients.Count;
                if (countMessages <= 0) return string.Empty;
                for (int i = 0; i < countMessages; i++)
                {
                    data += String.Format(Clients[i].mess);
                }
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error with GetTbxClient {0}", ex.Message); return string.Empty;
            }

        }


        public static string GetChat()
        {
            try
            {
                string data = "#updatechat&";
                int countMessages = Chat.Count;
                if (countMessages <= 0) return string.Empty;
                for (int i = 0; i < countMessages; i++)
                {
                    data += String.Format("{0}~{1}|", Chat[i].userName, Chat[i].data);
                }
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error with GetChat {0}", ex.Message); return string.Empty;
            }

        }


    }
}
