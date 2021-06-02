using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThChackers
{
    public class Chat
    {
        PlayingForm pf;
        public Chat(PlayingForm pf)
        {
            this.pf = pf;
        }

        public void UpdateChat(string data)
        {
            pf.ClearChat();
            string[] messages = data.Split('&')[1].Split('|');
            int countMessages = messages.Length;
            if (countMessages <= 0) return;
            for (int i = 0; i < countMessages; i++)
            {
                try
                {
                    if (string.IsNullOrEmpty(messages[i])) continue;
                    pf.Print(String.Format("[{0}]:{1}.", messages[i].Split('~')[0], messages[i].Split('~')[1]));

                }
                catch { continue; }
            }
        }
    }
}
