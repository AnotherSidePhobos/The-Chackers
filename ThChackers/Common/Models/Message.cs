using System;

namespace Common
{
    public class Message
    {
        public string Sender { get; set; }
        public string Destination { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }


        public string ToStrMes(string str)
        {
            str = str.Split('&')[0];
            return str;
        }

        public override string ToString()
        {
            return $"{Sender}|{Destination}|{Host}|{Port}";
        }
    }

    public static class MessageExtension
    {
        public static Message toMessage(this string message)
        {
            Message recievedMessage = new Message();
            var args = message.Split('|');
            recievedMessage.Sender = args[0];
            recievedMessage.Destination = args[1];
            recievedMessage.Host = args[2];
            recievedMessage.Port = args[3];
            return recievedMessage;
        }
        public static Message toMessageAboutClnt(this string message)
        {
            Message recievedMessage = new Message();
            var args = message.Split('|');
            recievedMessage.Host = args[0];
            recievedMessage.Port = args[1];
            return recievedMessage;
        }
    }
}
