using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Chat.View
{
    public delegate void OnTextSubmit(string text);

    public class ConversationTabPage : TabPage
    {
        public ConversationTabPage(Func<string, bool> measureLabelText)
        {
            _userNames = new List<string>();
            _measureLabelText = measureLabelText;
            InitializeComponent();
        }

        public OnTextSubmit OnTextSubmit;

        private string formatMessage(string userName, string text, DateTime time)
        {
            string timeStamp = time.ToString("[hh:mm] ");
            string nameTag = String.Format("{0}: ", userName);
            return timeStamp + nameTag + text;
        }

        public void AddMessage(string userName, string text, DateTime time ) {
            
            if (this.messagesBox.Text != "")
            {
                this.messagesBox.AppendText("\r\n");
            }
            this.messagesBox.AppendText(formatMessage(userName, text, time));
        }

        private List<string> _userNames;
        private Func<string, bool> _measureLabelText;

        //private void _updateText()
        //{
        //    this.Text = _userNames[0];
        //    if (_userNames.Count() > 1)
        //    {
        //        this.Text += " (+" + (_userNames.Count() - 1) + ")";
        //    }
        //}

        private void _updateTabText()
        {
            string text = "";
            if (_userNames.Count() > 0)
            {
                text = _userNames[0];
                string add = "";
                string ellipsis = "";
                if (_userNames.Count() > 1)
                {
                    add = " (+" + (_userNames.Count() - 1) + ")";
                }
                else
                {
                    add = "";
                }

                while (!_measureLabelText(text + ellipsis + add) && text.Length > 0)
                {
                    ellipsis = "...";
                    text = text.Substring(0, text.Length - 1);
                }

                this.Text = text + ellipsis + add;
            }
            else
            {

                this.Text = "";
            }
        }

        private void _updateParticipantsText()
        {
            _participants.Text = "Du";
            foreach (string userName in _userNames)
            {
                _participants.Text += ", " + userName;
            }
        }

        public void AddUser(string userName)
        {
            _userNames.Add(userName);
            _updateTabText();
            _updateParticipantsText();
        }

        public void RemoveUser(string userName)
        {
            //string regExp = string.Format("(^{0}, )|((, )?{0})", userName);
            //this.Text = Regex.Replace(this.Text, regExp, "");
            _userNames.Remove(userName);
            _updateTabText();
            _updateParticipantsText();
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
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.ColumnCount = 2;

            this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 18));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
            
            this.Controls.Add(this.tableLayoutPanel);
            
            // 
            // messagesBox
            // 
            this.messagesBox = new TextBox();
            this.messagesBox.Multiline = true;
            this.messagesBox.Name = "tabControlTextBox2";
            this.messagesBox.Dock = DockStyle.Fill;
            this.messagesBox.ReadOnly = true;
            this.messagesBox.BackColor = System.Drawing.SystemColors.Window;
            this.messagesBox.ScrollBars = ScrollBars.Vertical;

            this.messagesBox.VisibleChanged += (sender, e) =>
            {
                if (this.messagesBox.Visible)
                {
                    this.messagesBox.AppendText(".");
                }
            };

            this.tableLayoutPanel.Controls.Add(this.messagesBox, 0, 0);
            this.tableLayoutPanel.SetColumnSpan(this.messagesBox, 2);

            //
            // LabelText
            //
            _participants = new Label();
            _participants.Dock = DockStyle.Fill;
            this.tableLayoutPanel.Controls.Add(_participants, 0, 1);
            this.tableLayoutPanel.SetColumnSpan(_participants, 2);

            // 
            // inputBox
            //
            this.inputBox = new TextBox();
            this.inputBox.Multiline = true;
            this.inputBox.Name = "tabControlTextBox1";
            this.inputBox.Dock = DockStyle.Fill;
            this.inputBox.Tag = "";
            //this.tabControlTextBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.tableLayoutPanel.Controls.Add(this.inputBox, 0, 2);

            // 
            // sendButton
            // 
            this.sendButton = new Button();
            this.sendButton.Location = new System.Drawing.Point(537, 377);
            this.sendButton.Name = "tabControlButton2";
            this.sendButton.Size = new System.Drawing.Size(46, 41);
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += this.OnSendButtonClick;
            this.tableLayoutPanel.Controls.Add(this.sendButton, 1, 2);
            

            //this.Location = new System.Drawing.Point(4, 4);
            //this.Name = "tabPage1";
            //this.Padding = new Padding(3);
            //this.Size = new System.Drawing.Size(589, 425);
            this.UseVisualStyleBackColor = true;

            
        }

        private TableLayoutPanel tableLayoutPanel;
        private Label _participants;
        private TextBox inputBox;
        private TextBox messagesBox;
        private Button sendButton;
    }
}
