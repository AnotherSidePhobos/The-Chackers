using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;

namespace ThChackers
{
    public class LogicGame
    {
        const int mapSize = 8;
        const int cellSize = 50;
        private Thread _clientThread;

        private Socket _serverSocket;

        Chat chat;

        private delegate void printer(string data);
        private delegate void cleaner();
        printer Printer;
        cleaner Cleaner;

        public Button[,] buttons;
        public Checker[,] map;
        
        PlayingForm pf;
        List<Button> simpleSteps = new List<Button>();
        
        public void Init()
        {
            buttons = pf.Buttons;
            map = pf.Map;
        }
        public LogicGame(PlayingForm pf)
        {
            this.pf = pf;
            Printer = new printer(pf.Print);
            Cleaner = new cleaner(pf.ClearChat);
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

        public Color GetPrevButColor(Button prevButton)
        {
            if ((prevButton.Location.Y / cellSize % 2) != 0)
            {
                if ((prevButton.Location.X / cellSize % 2) == 0)
                {
                    return Color.Gray;
                }
            }
            if ((prevButton.Location.Y / cellSize % 2) == 0)
            {
                if ((prevButton.Location.X / cellSize % 2) != 0)
                {
                    return Color.Gray;
                }
            }
            return Color.White;
        }
        private void Listener()
        {
            while (_serverSocket.Connected)
            {
                byte[] buffer = new byte[8196];
                int bytesRec = _serverSocket.Receive(buffer);
                string data = Encoding.UTF8.GetString(buffer, 0, bytesRec);
                if (data.Contains("#updatechat"))
                {
                    UpdateChat(data);
                    continue;
                }
                if (data.Contains("#Updateboard"))
                {
                    UpdateBoardd(data);
                    continue;
                }
                if (data.Contains("#updateTbxClient"))
                {
                    pf.UpdateTbxClient(data);
                    continue;
                }
            }
        }

        public void Send(string data)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                int bytesSent = _serverSocket.Send(buffer);
            }
            catch { pf.Print("Server disconnected"); }
        }
        public void SendNewClntMess(string host, int port)
        {
            try
            {
                //string data = $"New client connected host {host} | port {port}   ";
                //if (string.IsNullOrEmpty(data)) return;
                string data = $"{host}|{port}";
                Common.Message message = data.toMessageAboutClnt();
                if (message == null) return;
                Send("#Cln&" + data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "Ошибка при отправке сообщения о добавлении клиента!");
            }
        }
        public void Connect(string _serverHost, int _serverPort)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(_serverHost);
                IPEndPoint iPEndPoint = new IPEndPoint(ip, _serverPort);
                _serverSocket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _serverSocket.Connect(iPEndPoint);
                SendNewClntMess(_serverHost, _serverPort);
                pf.SwitchControls();

                _clientThread = new Thread(Listener);
                _clientThread.IsBackground = true;
                _clientThread.Start();

            }
            catch
            {
                pf.Print("Server is not avaible");
            }
        }

        public void SendPosition(Button pressedButton)
        {
            string stringCords = pressedButton.Location.X + ", " + pressedButton.Location.Y;
            Send("#changePos&" + "%" + stringCords + "%");
        }


        public void UpdateBoardd(string data)
        {
            string coords = data;
            var reg = new Regex(@"(?<open>\%).*?(?<final-open>\%)");
            var matches = reg.Matches(coords).Cast<Match>()
                .Select(m => m.Groups["final"].Value).ToList();
            foreach (string strCoords in matches)
            {
                coords = strCoords;
            }
            string[] subs = coords.Split(',');
            string ast = subs[0];
            string bst = subs[1];
            int a = int.Parse(ast);
            int b = int.Parse(bst);
            foreach (var btn in buttons)
            {
                if ((btn.Location.X == a) && (btn.Location.Y == b))
                {

                    pf.ActionsWithBoard(btn);
                }
            }
        }




        public void SwitchButtonToCheat(Button button)
        {
            if (map[button.Location.Y / cellSize, button.Location.X / cellSize].playerId == 1 && button.Location.Y / cellSize == mapSize - 1)
            {
                button.Text = "D";
            }
            if (map[button.Location.Y / cellSize, button.Location.X / cellSize].playerId == 2 && button.Location.Y / cellSize == 0)
            {
                button.Text = "D";
            }
        }
        public void DeleteEaten(Button endButton, Button startButton)
        {
            int count = Math.Abs(endButton.Location.Y / cellSize - startButton.Location.Y / cellSize);
            int startIndexX = endButton.Location.Y / cellSize - startButton.Location.Y / cellSize;
            int startIndexY = endButton.Location.X / cellSize - startButton.Location.X / cellSize;
            startIndexX = startIndexX < 0 ? -1 : 1;
            startIndexY = startIndexY < 0 ? -1 : 1;
            int currCount = 0;
            int i = startButton.Location.Y / cellSize + startIndexX;
            int j = startButton.Location.X / cellSize + startIndexY;

            while (currCount < count - 1)
            {
                map[i, j].playerId = 0;
                buttons[i, j].Image = null;
                buttons[i, j].Text = "";
                i += startIndexX;
                j += startIndexY;
                currCount++;
            }
        }


        public void ShowPossibleSteps()
        {
            bool isOneStep = true;
            bool isEatStep = false;
            DeactivateButtons();
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (map[i, j].playerId == pf.currentPlayer)
                    {
                        if (buttons[i, j].Text == "D")
                            isOneStep = false;
                        else isOneStep = true;
                        if (IsButtonHasEatStep(i, j, isOneStep, new int[2] { 0, 0 }))
                        {
                            isEatStep = true;
                            buttons[i, j].Enabled = true;
                        }
                    }
                }
            }
            if (!isEatStep)
                ActivateButtons();
        }
        public void CloseSteps()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    buttons[i, j].BackColor = GetPrevButColor(buttons[i, j]);
                }
            }
        }
        public bool IsInsideBorders(int ti, int tj)
        {
            if (ti >= mapSize || tj >= mapSize || ti < 0 || tj < 0)
            {
                return false;
            }
            return true;
        }

        public void ActivateButtons()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    buttons[i, j].Enabled = true;
                }
            }
        }
        public void ShowDiagonal(int IcurrFigure, int JcurrFigure, bool isOneStep = false)
        {
            int j = JcurrFigure + 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (pf.currentPlayer == 1 && isOneStep && !pf.isContunue) break;
                if (IsInsideBorders(i, j))
                {
                    if (!DeterminePath(i, j)) // делает поле активным и окрашивает в жёлтый
                        break;
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (pf.currentPlayer == 1 && isOneStep && !pf.isContunue) break;
                if (IsInsideBorders(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (pf.currentPlayer == 2 && isOneStep && !pf.isContunue) break;
                if (IsInsideBorders(i, j)) // выходит ли за границы доски
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure + 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (pf.currentPlayer == 2 && isOneStep && !pf.isContunue) break;
                if (IsInsideBorders(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }
        }



        public void ShowSteps(int iCurrFigure, int jCurrFigure, bool isOnestep = true)
        {
            simpleSteps.Clear();
            ShowDiagonal(iCurrFigure, jCurrFigure, isOnestep);
            if (pf.countEatSteps > 0)
                CloseSimpleSteps(simpleSteps);
        }

        public void CloseSimpleSteps(List<Button> simpleSteps)
        {
            if (simpleSteps.Count > 0)
            {
                for (int i = 0; i < simpleSteps.Count; i++)
                {
                    simpleSteps[i].BackColor = GetPrevButColor(simpleSteps[i]);
                    simpleSteps[i].Enabled = false;
                }
            }
        }

        public bool DeterminePath(int ti, int tj)
        {
            if (map[ti, tj].playerId == 0 && !pf.isContunue)
            {
                buttons[ti, tj].BackColor = Color.Yellow;
                buttons[ti, tj].Enabled = true;
                simpleSteps.Add(buttons[ti, tj]);
            }
            else
            {
                if (map[ti, tj].playerId != pf.currentPlayer)
                {
                    if (pf.pressedButton.Text == "D")
                        ShowProceduralEat(ti, tj, false);
                    else ShowProceduralEat(ti, tj);
                }
                return false;
            }
            return true;
        }

        public bool IsButtonHasEatStep(int IcurrFigure, int JcurrFigure, bool isOneStep, int[] dir)
        {
            bool eatStep = false;
            int j = JcurrFigure + 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (pf.currentPlayer == 1 && isOneStep && !pf.isContunue) break;
                if (dir[0] == 1 && dir[1] == -1 && !isOneStep) break;
                if (IsInsideBorders(i, j))
                {
                    if (map[i, j].playerId != 0 && map[i, j].playerId != pf.currentPlayer)
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i - 1, j + 1))
                            eatStep = false;
                        else if (map[i - 1, j + 1].playerId != 0)
                            eatStep = false;
                        else return eatStep;
                    }
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                {
                    break;
                }
            }
            j = JcurrFigure - 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (pf.currentPlayer == 1 && isOneStep && !pf.isContunue) break;
                if (dir[0] == 1 && dir[1] == 1 && !isOneStep) break;
                if (IsInsideBorders(i, j))
                {
                    if (map[i, j].playerId != 0 && map[i, j].playerId != pf.currentPlayer)
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i - 1, j - 1))
                            eatStep = false;
                        else if (map[i - 1, j - 1].playerId != 0)
                            eatStep = false;
                        else return eatStep;
                    }
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (pf.currentPlayer == 2 && isOneStep && !pf.isContunue) break;
                if (dir[0] == -1 && dir[1] == 1 && !isOneStep) break;
                if (IsInsideBorders(i, j))
                {
                    if (map[i, j].playerId != 0 && map[i, j].playerId != pf.currentPlayer)
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i + 1, j - 1))
                            eatStep = false;
                        else if (map[i + 1, j - 1].playerId != 0)
                            eatStep = false;
                        else return eatStep;
                    }
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure + 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (pf.currentPlayer == 2 && isOneStep && !pf.isContunue) break;
                if (dir[0] == -1 && dir[1] == -1 && !isOneStep) break;
                if (IsInsideBorders(i, j))
                {
                    if (map[i, j].playerId != 0 && map[i, j].playerId != pf.currentPlayer)
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i + 1, j + 1))
                            eatStep = false;
                        else if (map[i + 1, j + 1].playerId != 0)
                            eatStep = false;
                        else return eatStep;
                    }
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }
            return eatStep;
        }


        public void ShowProceduralEat(int i, int j, bool isOneStep = true)
        {
            int dirX = i - pf.pressedButton.Location.Y / cellSize;
            int dirY = j - pf.pressedButton.Location.X / cellSize;
            dirX = dirX < 0 ? -1 : 1;
            dirY = dirY < 0 ? -1 : 1;
            int i1 = i;
            int j1 = j;
            bool isEmpty = true;
            while (IsInsideBorders(i1, j1))
            {
                if (map[i1, j1].playerId != 0 && map[i1, j1].playerId != pf.currentPlayer)
                {
                    isEmpty = false;
                    break;
                }
                i1 += dirX;
                j1 += dirY;
                if (isOneStep)
                    break;
            }
            if (isEmpty)
                return;
            List<Button> toClose = new List<Button>();
            bool closeSimple = false;
            int ik = i1 + dirX;
            int jk = j1 + dirY;
            while (IsInsideBorders(ik, jk))
            {
                if (map[ik, jk].playerId == 0)
                {
                    if (IsButtonHasEatStep(ik, jk, isOneStep, new int[2] { dirX, dirY }))
                    {
                        closeSimple = true;
                    }
                    else
                    {
                        toClose.Add(buttons[ik, jk]);
                    }
                    buttons[ik, jk].BackColor = Color.Yellow;
                    buttons[ik, jk].Enabled = true;
                    pf.countEatSteps++;
                }
                else break;
                if (isOneStep)
                    break;
                jk += dirY;
                ik += dirX;
            }
            if (closeSimple && toClose.Count > 0)
            {
                CloseSimpleSteps(toClose);
            }
        }
        public void SwitchPlayer()
        {
            pf.currentPlayer = pf.currentPlayer == 1 ? 2 : 1;
            pf.ResetGame();
        }

        public void DeactivateButtons()
        {
            try
            {
                for (int i = 0; i < mapSize; i++)
                {
                    for (int j = 0; j < mapSize; j++)
                    {
                        buttons[i, j].Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "Error with Deactivate");
            }

        }

        







        ////
        ///


    }
}
