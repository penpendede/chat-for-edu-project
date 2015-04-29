using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chat.View
{
    delegate void OnTextSubmit(string text);

    class ConversationTabPage : TabPage
    {
        public ConversationTabPage()
        {
            InitializeComponent();
        }

        public OnTextSubmit OnTextSubmit;

        private string formatMessage(string userName, string text, DateTime time)
        {
            string timeStamp = time.ToString("[hh:ss] ");
            string nameTag = String.Format("{0}: ", userName);
            return timeStamp + nameTag + text;
        }

        public void AddMessage(string userName, string text, DateTime time ) {
            if (this.messagesBox.Text != "")
            {
                this.messagesBox.Text += "\r\n";
            }
            this.messagesBox.Text += formatMessage(userName, text, time);
        }

        private void OnSendButtonClick(object obj, EventArgs args)
        {
            this.OnTextSubmit(this.inputBox.Text);
            this.inputBox.Text = "";
        }

        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// </summary>

        private void InitializeComponent()
        {
            this.messagesBox = new System.Windows.Forms.TextBox();
            this.inputBox = new System.Windows.Forms.TextBox();
            this.sendButton = new System.Windows.Forms.Button();
            
            // 
            // tabControlTextBox2
            // 
            this.messagesBox.Location = new System.Drawing.Point(6, 6);
            this.messagesBox.Multiline = true;
            this.messagesBox.Name = "tabControlTextBox2";
            this.messagesBox.Size = new System.Drawing.Size(577, 365);
            // 
            // tabControlTextBox1
            // 
            this.inputBox.Location = new System.Drawing.Point(6, 377);
            this.inputBox.Multiline = true;
            this.inputBox.Name = "tabControlTextBox1";
            this.inputBox.Size = new System.Drawing.Size(522, 42);
            this.inputBox.Tag = "";
            //this.tabControlTextBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // tabControlButton2
            // 
            this.sendButton.Location = new System.Drawing.Point(537, 377);
            this.sendButton.Name = "tabControlButton2";
            this.sendButton.Size = new System.Drawing.Size(46, 41);
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += this.OnSendButtonClick;

            this.Location = new System.Drawing.Point(4, 4);
            //this.Name = "tabPage1";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(589, 425);
            this.UseVisualStyleBackColor = true;

            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.messagesBox);
            this.Controls.Add(this.inputBox);
        }

        private System.Windows.Forms.TextBox inputBox;
        private System.Windows.Forms.TextBox messagesBox;
        private System.Windows.Forms.Button sendButton;
    }
}
