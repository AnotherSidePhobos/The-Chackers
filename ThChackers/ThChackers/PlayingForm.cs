using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Models;

namespace ThChackers
{
    public partial class PlayingForm : Form
    {
        const int mapSize = 8;
        const int cellSize = 50;
        public int countEatSteps = 0;
        Field field;
        public Button[,] buttons = new Button[mapSize, mapSize];
        public Button[,] Buttons { get => buttons; }
        public Checker[,] map;
        public Checker[,] Map { get => map; }
        private delegate void printer(string data);
        private delegate void cleaner();
        printer Printer;
        cleaner Cleaner;
        LogicGame lg;
        
        Image whiteFigure;
        Image blackFigure;
        bool IsMoving = false;
        public Button prevButton = null;
        public Button pressedButton = null;
        public bool isContunue = false;

        public int currentPlayer = 1;
        int CountUsers { get; set; } = 0;
        private string _serverHost;
        private int _serverPort;

        Chat chat;
        public PlayingForm()
        {
            InitializeComponent();
            this.Width = 1060;
            this.Height = 520;
            this.Text = "Checkers";
            lg = new LogicGame(this);
            field = new Field(this);
            chat = new Chat(this);
            Init();
        }





        private void SendMessage()
        {
            try
            {
                string data = " " + chat_msg.Text;
                if (string.IsNullOrEmpty(data)) return;
                lg.Send("#newmsg&" + data);
                chat_msg.Text = string.Empty;
            }
            catch { MessageBox.Show("Ошибка при отправке сообщения!"); }
        }


        public void UpdateTbxClient(string data)
        {

            string[] messages = data.Split('&')[1].Split('|');

            int countMessages = messages.Length;
            if (countMessages <= 0) return;
            for (int i = 0; i < countMessages; i++)
            {
                try
                {
                    if (string.IsNullOrEmpty(messages[i])) continue;
                    string msg = String.Format("{0}.", messages[i].Split('~')[0]);

                    if (lblUsers.Items.Count == 0)
                        lblUsers.Items.Add(msg);
                    else
                        lblUsers.Items.Add((Environment.NewLine + msg));
                    msg = string.Empty;
                }
                catch { continue; }
            }
        }

        private void Init()
        {
            map = field.GenerateField();
            MakeField();
        }
        public void ClearChat()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(Cleaner);
                return;
            }
            ChatBox.Clear();
        }

        public void Print(string msg)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(Printer, msg);
                return;
            }
            if (ChatBox.Text.Length == 0)
                ChatBox.AppendText(msg);
            else
                ChatBox.AppendText(Environment.NewLine + msg);
        }

        private void enterApp_Click(object sender, EventArgs e)
        {
            string Name = tbxUserName.Text;
            if (string.IsNullOrEmpty(Name)) return;
            ViewFigures();
            lg.Send("#setname&" + Name);
            ChatBox.Enabled = true;
            chat_msg.Enabled = true;
            Chat_Send.Enabled = true;
            tbxUserName.Enabled = false;
            enterApp.Enabled = false;
        }

        private void ViewFigures()
        {
            string pathB = Directory.GetCurrentDirectory() + "\\Resources\\b.png";
            string pathW = Directory.GetCurrentDirectory() + "\\Resources\\w.png";
            blackFigure = new Bitmap(new Bitmap(pathB), new Size(cellSize - 10, cellSize - 10));
            whiteFigure = new Bitmap(new Bitmap(pathW), new Size(cellSize - 10, cellSize - 10));
            CreateMap(buttons);
        }


        public void ResetGame()
        {
            bool player1 = false;
            bool player2 = false;
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (map[i, j].playerId == 1)
                        player1 = true;
                    if (map[i, j].playerId == 2)
                        player2 = true;
                }
            }
            if (!player1 || !player2)
            {
                this.Controls.Clear();
                MessageBox.Show("End of game");
                Init();
            }
        }

        public void CreateMap(Button[,] buttons)
        {
            foreach (var but in buttons)
            {
                if (map[but.Location.Y / cellSize, but.Location.X / cellSize].playerId == 1)
                {
                    but.Image = whiteFigure;
                }
                else if (map[but.Location.Y / cellSize, but.Location.X / cellSize].playerId == 2)
                {
                    but.Image = blackFigure;
                }
            }
        }

        private void MakeField()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    Button button = new Button();
                    button.Location = new Point(j * cellSize, i * cellSize);
                    button.Size = new Size(cellSize, cellSize);
                    button.Click += new EventHandler(OnFigPress);
                    button.BackColor = lg.GetPrevButColor(button);
                    button.ForeColor = Color.Red;
                    buttons[i, j] = button;
                    this.Controls.Add(button);
                }
            }
        }
        private void OnFigPress(object sender, EventArgs e)
        {
            pressedButton = sender as Button;
            lg.SendPosition(pressedButton);
        }


        private void Chat_Send_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        public void SwitchControls()
        {
            Chat_Send.Enabled = true;
            ChatBox.Enabled = true;
            tbxUserName.Enabled = true;
            enterApp.Enabled = true;
            chat_msg.Enabled = true;
            btnDisc.Enabled = true;
        }

        private void btnConSer_Click(object sender, EventArgs e)
        {
            _serverHost = tbxHost.Text; //"127.0.0.1";
            _serverPort = int.Parse(tbxPort.Text); //9933;
            lg.Connect(_serverHost, _serverPort);

        }

        private void PlayingForm_Load(object sender, EventArgs e)
        {
            lg.Init();
        }










        public void ActionsWithBoard(Button pressedButton)
        {
            if (prevButton != null)
                prevButton.BackColor = lg.GetPrevButColor(prevButton);
            if (map[pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize].playerId != 0 && map[pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize].playerId == currentPlayer)
            {
                lg.CloseSteps();
                pressedButton.BackColor = Color.Green;
                lg.DeactivateButtons();
                pressedButton.Enabled = true;
                countEatSteps = 0;
                if (pressedButton.Text == "D")
                {
                    lg.ShowSteps(pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize, false);
                }
                else
                {
                    lg.ShowSteps(pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize);
                }
                if (IsMoving)
                {
                    lg.CloseSteps();
                    pressedButton.BackColor = lg.GetPrevButColor(pressedButton);
                    lg.ShowPossibleSteps();
                    IsMoving = false;
                }
                else
                    IsMoving = true;
            }
            else
            {
                if (IsMoving)
                {
                    isContunue = false;
                    if (Math.Abs(pressedButton.Location.X / cellSize - prevButton.Location.X / cellSize) > 1)
                    {
                        isContunue = true;
                        lg.DeleteEaten(pressedButton, prevButton);
                    }
                    int temp = map[pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize].playerId;
                    map[pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize].playerId = map[prevButton.Location.Y / cellSize, prevButton.Location.X / cellSize].playerId;
                    map[prevButton.Location.Y / cellSize, prevButton.Location.X / cellSize].playerId = temp;
                    pressedButton.Image = prevButton.Image;
                    prevButton.Image = null;
                    pressedButton.Text = prevButton.Text;
                    prevButton.Text = "";
                    lg.SwitchButtonToCheat(pressedButton);
                    countEatSteps = 0;
                    IsMoving = false;
                    lg.CloseSteps();
                    lg.DeactivateButtons();
                    if (pressedButton.Text == "D")
                        lg.ShowSteps(pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize, false);
                    else lg.ShowSteps(pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize);
                    if (countEatSteps == 0 || !isContunue)
                    {
                        lg.CloseSteps();
                        lg.SwitchPlayer();
                        lg.ShowPossibleSteps();
                        isContunue = false;
                    }
                    else if (isContunue)
                    {
                        pressedButton.BackColor = Color.Red;
                        pressedButton.Enabled = true;
                        IsMoving = true;
                    }
                }
            }

            prevButton = pressedButton;
        }


    }
}
