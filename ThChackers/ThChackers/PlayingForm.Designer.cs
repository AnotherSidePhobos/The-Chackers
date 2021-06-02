
namespace ThChackers
{
    partial class PlayingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayingForm));
            this.btnDisc = new System.Windows.Forms.Button();
            this.lblUsers = new System.Windows.Forms.ListBox();
            this.lblSignedIn = new System.Windows.Forms.Label();
            this.btnConSer = new System.Windows.Forms.Button();
            this.tbxPort = new System.Windows.Forms.TextBox();
            this.tbxHost = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chat_msg = new System.Windows.Forms.TextBox();
            this.Chat_Send = new System.Windows.Forms.Button();
            this.ChatBox = new System.Windows.Forms.TextBox();
            this.tbxUserName = new System.Windows.Forms.TextBox();
            this.gui_userName = new System.Windows.Forms.Label();
            this.enterApp = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDisc
            // 
            this.btnDisc.Enabled = false;
            this.btnDisc.Location = new System.Drawing.Point(601, 394);
            this.btnDisc.Name = "btnDisc";
            this.btnDisc.Size = new System.Drawing.Size(75, 23);
            this.btnDisc.TabIndex = 51;
            this.btnDisc.Text = "Disconnect";
            this.btnDisc.UseVisualStyleBackColor = true;
            // 
            // lblUsers
            // 
            this.lblUsers.FormattingEnabled = true;
            this.lblUsers.Location = new System.Drawing.Point(700, 82);
            this.lblUsers.Name = "lblUsers";
            this.lblUsers.Size = new System.Drawing.Size(233, 212);
            this.lblUsers.TabIndex = 50;
            // 
            // lblSignedIn
            // 
            this.lblSignedIn.AutoSize = true;
            this.lblSignedIn.Location = new System.Drawing.Point(697, 66);
            this.lblSignedIn.Name = "lblSignedIn";
            this.lblSignedIn.Size = new System.Drawing.Size(79, 13);
            this.lblSignedIn.TabIndex = 49;
            this.lblSignedIn.Text = "Signed in users";
            // 
            // btnConSer
            // 
            this.btnConSer.Location = new System.Drawing.Point(601, 368);
            this.btnConSer.Name = "btnConSer";
            this.btnConSer.Size = new System.Drawing.Size(75, 23);
            this.btnConSer.TabIndex = 48;
            this.btnConSer.Text = "Connect";
            this.btnConSer.UseVisualStyleBackColor = true;
            this.btnConSer.Click += new System.EventHandler(this.btnConSer_Click);
            // 
            // tbxPort
            // 
            this.tbxPort.Location = new System.Drawing.Point(481, 394);
            this.tbxPort.Name = "tbxPort";
            this.tbxPort.Size = new System.Drawing.Size(100, 20);
            this.tbxPort.TabIndex = 47;
            this.tbxPort.Text = "9933";
            // 
            // tbxHost
            // 
            this.tbxHost.Location = new System.Drawing.Point(481, 368);
            this.tbxHost.Name = "tbxHost";
            this.tbxHost.Size = new System.Drawing.Size(100, 20);
            this.tbxHost.TabIndex = 46;
            this.tbxHost.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(481, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 45;
            this.label1.Text = "Chat";
            // 
            // chat_msg
            // 
            this.chat_msg.Enabled = false;
            this.chat_msg.Location = new System.Drawing.Point(481, 306);
            this.chat_msg.Name = "chat_msg";
            this.chat_msg.Size = new System.Drawing.Size(195, 20);
            this.chat_msg.TabIndex = 44;
            // 
            // Chat_Send
            // 
            this.Chat_Send.Enabled = false;
            this.Chat_Send.Location = new System.Drawing.Point(601, 332);
            this.Chat_Send.Name = "Chat_Send";
            this.Chat_Send.Size = new System.Drawing.Size(75, 23);
            this.Chat_Send.TabIndex = 43;
            this.Chat_Send.Text = "Send";
            this.Chat_Send.UseVisualStyleBackColor = true;
            this.Chat_Send.Click += new System.EventHandler(this.Chat_Send_Click);
            // 
            // ChatBox
            // 
            this.ChatBox.Enabled = false;
            this.ChatBox.Location = new System.Drawing.Point(481, 82);
            this.ChatBox.Multiline = true;
            this.ChatBox.Name = "ChatBox";
            this.ChatBox.Size = new System.Drawing.Size(195, 215);
            this.ChatBox.TabIndex = 42;
            // 
            // tbxUserName
            // 
            this.tbxUserName.Enabled = false;
            this.tbxUserName.Location = new System.Drawing.Point(481, 30);
            this.tbxUserName.Name = "tbxUserName";
            this.tbxUserName.Size = new System.Drawing.Size(100, 20);
            this.tbxUserName.TabIndex = 41;
            // 
            // gui_userName
            // 
            this.gui_userName.AutoSize = true;
            this.gui_userName.Location = new System.Drawing.Point(481, 14);
            this.gui_userName.Name = "gui_userName";
            this.gui_userName.Size = new System.Drawing.Size(83, 13);
            this.gui_userName.TabIndex = 40;
            this.gui_userName.Text = "Input your name";
            // 
            // enterApp
            // 
            this.enterApp.Enabled = false;
            this.enterApp.Location = new System.Drawing.Point(601, 28);
            this.enterApp.Name = "enterApp";
            this.enterApp.Size = new System.Drawing.Size(75, 23);
            this.enterApp.TabIndex = 39;
            this.enterApp.Text = "Enter";
            this.enterApp.UseVisualStyleBackColor = true;
            this.enterApp.Click += new System.EventHandler(this.enterApp_Click);
            // 
            // PlayingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 450);
            this.Controls.Add(this.btnDisc);
            this.Controls.Add(this.lblUsers);
            this.Controls.Add(this.lblSignedIn);
            this.Controls.Add(this.btnConSer);
            this.Controls.Add(this.tbxPort);
            this.Controls.Add(this.tbxHost);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chat_msg);
            this.Controls.Add(this.Chat_Send);
            this.Controls.Add(this.ChatBox);
            this.Controls.Add(this.tbxUserName);
            this.Controls.Add(this.gui_userName);
            this.Controls.Add(this.enterApp);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PlayingForm";
            this.Text = "PlayingForm";
            this.Load += new System.EventHandler(this.PlayingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDisc;
        private System.Windows.Forms.ListBox lblUsers;
        private System.Windows.Forms.Label lblSignedIn;
        private System.Windows.Forms.Button btnConSer;
        private System.Windows.Forms.TextBox tbxPort;
        private System.Windows.Forms.TextBox tbxHost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox chat_msg;
        private System.Windows.Forms.Button Chat_Send;
        private System.Windows.Forms.TextBox ChatBox;
        private System.Windows.Forms.TextBox tbxUserName;
        private System.Windows.Forms.Label gui_userName;
        private System.Windows.Forms.Button enterApp;
    }
}