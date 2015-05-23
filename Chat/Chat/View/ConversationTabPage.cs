using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Chat.View
{
    public delegate void OnTextSubmit(string text);

    public class ConversationTabPage : System.Windows.Forms.TabPage
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

        public void AddUser(string userName)
        {
            if (this.Text != "")
            {
                this.Text += ", ";
            }
            this.Text += userName;
        }

        public void RemoveUser(string userName)
        {
            Regex.Replace(this.Text, userName + "(, )?", "");
        }

        private void OnSendButtonClick(object obj, EventArgs args)
        {
            if (this.OnTextSubmit != null)
            {
                this.OnTextSubmit(this.inputBox.Text);
            }
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
            components = new System.ComponentModel.Container();

            //
            // tableLayoutPanel
            //

            this.tableLayoutPanel = new TableLayoutPanel();
            
            this.tableLayoutPanel.Dock = DockStyle.Fill;
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.ColumnCount = 2;

            this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
            
            this.Controls.Add(this.tableLayoutPanel);
            
            // 
            // messagesBox
            // 
            this.messagesBox = new System.Windows.Forms.TextBox();
            this.messagesBox.Multiline = true;
            this.messagesBox.Name = "tabControlTextBox2";
            this.messagesBox.Dock = DockStyle.Fill;
            this.tableLayoutPanel.Controls.Add(this.messagesBox, 0, 0);
            this.tableLayoutPanel.SetColumnSpan(this.messagesBox, 2);

            // 
            // inputBox
            //
            this.inputBox = new System.Windows.Forms.TextBox();
            this.inputBox.Multiline = true;
            this.inputBox.Name = "tabControlTextBox1";
            this.inputBox.Dock = DockStyle.Fill;
            this.inputBox.Tag = "";
            //this.tabControlTextBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.tableLayoutPanel.Controls.Add(this.inputBox, 0, 1);

            // 
            // sendButton
            // 
            this.sendButton = new System.Windows.Forms.Button();
            this.sendButton.Location = new System.Drawing.Point(537, 377);
            this.sendButton.Name = "tabControlButton2";
            this.sendButton.Size = new System.Drawing.Size(46, 41);
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += this.OnSendButtonClick;
            this.tableLayoutPanel.Controls.Add(this.sendButton, 1, 1);
            

            //this.Location = new System.Drawing.Point(4, 4);
            //this.Name = "tabPage1";
            //this.Padding = new System.Windows.Forms.Padding(3);
            //this.Size = new System.Drawing.Size(589, 425);
            this.UseVisualStyleBackColor = true;

            
        }

        private TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TextBox inputBox;
        private System.Windows.Forms.TextBox messagesBox;
        private System.Windows.Forms.Button sendButton;
    }
}
