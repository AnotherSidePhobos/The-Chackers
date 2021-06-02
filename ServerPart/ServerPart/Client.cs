using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerPart
{
    public class Client
    {

        private string _userName;
        private Socket _handler;
        private Thread _userThread;
        public Client(Socket socket, int countClients)
        {

            _handler = socket;
            _userThread = new Thread(Listener);
            _userThread.IsBackground = true;
            _userThread.Start();


        }

        public string UserName
        {
            get { return _userName; }
        }



        //private void Send(string data)
        //{
        //    try
        //    {
        //        byte[] buffer = Encoding.UTF8.GetBytes(data);
        //        int bytesSent = _handler.Send(buffer);
        //    }
        //    catch { }
        //}


        private void Listener()
        {

            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesRec = _handler.Receive(buffer);
                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRec);
                    HandleComand(data);
                }
                catch { Server.EndClient(this); return; }

            }
        }

        public void End()
        {
            try
            {
                _handler.Close();
                try
                {
                    _userThread.Abort();
                }
                catch { }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error with End: {0}", ex.Message);
            }

        }

        private void HandleComand(string data)
        {
            if (data.Contains("#setname"))
            {
                _userName = data.Split('&')[1];
                UpdateChat();
                return;
            }
            if (data.Contains("#newmsg"))
            {
                string message = data.Split('&')[1];
                ChatController.AddMessage(_userName, message);
                return;
            }
            if (data.Contains("#Cln"))
            {
                string message = data.Split('&')[1];
                ChatController.AddClntMessage(message);
                return;
            }
            if (data.Contains("#changePos"))
            {
                string message = data.Split('&')[1];
                ChatController.ChangePosition(_userName, data.ToString());
                return;
            }

        }

        public void UpdateBoardd()
        {
            Send(ChatController.GetBoard());
        }
        public void UpdateChat()
        {
            Send(ChatController.GetChat());
        }
        public void UpdateTbxClient()
        {
            Send(ChatController.GetTbxClient());
        }
        

        public void Send(string command)
        {
            try
            {
                int byteSent = _handler.Send(Encoding.UTF8.GetBytes(command));
                if (byteSent > 0) Console.WriteLine("Success");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error with send command {0}", ex.Message); Server.EndClient(this);
            }
        }



    }
}
