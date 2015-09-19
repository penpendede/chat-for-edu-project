using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Chat.View
{
    public delegate void OnTextSubmit(string text);

    delegate void AccessAddMessageFromOtherThread(string userName, string text, DateTime time, bool deleted = false);

    /// <summary>
    /// No smiles so far
    /// </summary>
    public class Smileys
    {
        
    }

    /// <summary>
    /// A conversation tab page is a tab page (obviously)
    /// </summary>
    public class ConversationTabPage : TabPage
    {
        private List<string> _userNames;
        private Func<string, bool> _measureLabelText;

        private TableLayoutPanel _tableLayoutPanel;
        private Label _participants;
        private TextBox _inputBox;
        private RichTextBox _messagesBox;
        private Button _sendButton;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="measureLabelText">This is a function to measure the width of the text on the tabs label</param>
        public ConversationTabPage(Func<string, bool> measureLabelText)
        {
            _userNames = new List<string>();
            _measureLabelText = measureLabelText;
            InitializeComponent();
        }

        /// <summary>
        /// Disable tab page
        /// </summary>
        public void Disable()
        {
            _inputBox.ReadOnly = true;
            _sendButton.Enabled = false;
            if (_messagesBox.Text != "")
            {
                _messagesBox.AppendText("\r\n");
            }
            _messagesBox.AppendText("------ CONVERSATION IS CLOSED ------");
        }

        public OnTextSubmit OnTextSubmit;

        /// <summary>
        /// format message for display
        /// </summary>
        /// <param name="userName">user from who the message originated</param>
        /// <param name="text">the message's text</param>
        /// <param name="time">time (and date) the message was sent</param>
        /// <param name="deleted">whether the user in the meantime has been deleted</param>
        /// <returns></returns>
        private string formatMessage(string userName, string text, DateTime time, bool deleted = false)
        {
            string timeStamp = time.ToString("[hh:mm] ");
            string nameTag;

            if (!deleted)
            {
                nameTag = String.Format("{0}: ", userName);
            } 
            else
            {
                nameTag = String.Format("{0} [DELETED]: ", userName);
            }

            //_addImages(text);
            return timeStamp + nameTag + text;
        }

        /// <summary>
        /// This function replaces smileys in the messages with images.
        /// NOT WORKING!
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string _addImages(string text) {

            throw new NotSupportedException();

            MemoryStream stream = new MemoryStream();
            Image img = Image.FromFile("Images/Face-smile200px.png");
            img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

            string str = BitConverter.ToString(stream.ToArray()).Replace("-", string.Empty);

            //string str = BitConverter.ToString(System.IO.File.ReadAllBytes("Images/Face-smile200px.png")).Replace("-", string.Empty);

            int twips_per_inch = 1440;
            int hmm_per_inch = 2540;
            Graphics g = this._messagesBox.CreateGraphics();

            int picw = (int) Math.Round((img.Width/g.DpiX)*hmm_per_inch);
            int pich = (int) Math.Round((img.Height/g.DpiY)*hmm_per_inch);

            //int picwgoal = (int) Math.Round((img.Width/g.DpiX)*twips_per_inch);

            return text + @"{\pict\pngblip\picw" + picw + @"\pich" + pich + @"\picwgoal1300\pichgoal1300 " + str + "}";
        }

        /// <summary>
        /// Add a nessage
        /// </summary>
        /// <param name="userName">sender of the message</param>
        /// <param name="text">text of the message</param>
        /// <param name="time">time (and date) the message was sent)</param>
        /// <param name="deleted">whether the user in the meantime has been deleted</param>
        public void AddMessage(string userName, string text, DateTime time, bool deleted = false)
        {
            if (InvokeRequired)
            {
                Invoke(new AccessAddMessageFromOtherThread(AddMessage), userName, text, time, deleted);
            }
            else
            {
                if (this._messagesBox.Text != "")
                {
                    this._messagesBox.AppendText("\r\n");
                }

                string msg = this.formatMessage(userName, text, time, deleted);

                this._messagesBox.AppendText(msg);
                //string rtf = Regex.Replace(_messagesBox.Rtf, "\\\\par\\s*}\\s*$", msg + "\\par}");
                //string rtf = Regex.Replace(_messagesBox.Rtf, "\\\\par\\\r\n}\\\r\n$", "soso\\par}"); // geht
                this._messagesBox.Select(this._messagesBox.TextLength, 0);

                //StringBuilder rtf = new StringBuilder();

                //rtf.Append()

                //this._messagesBox.Rtf = rtf;
                //this._messagesBox.AppendText(formatMessage(userName, text, time, deleted));
                //this._messagesBox.AppendText("bye");
            }
            
        }

        //private void _updateText()
        //{
        //    this.Text = _userNames[0];
        //    if (_userNames.Count() > 1)
        //    {
        //        this.Text += " (+" + (_userNames.Count() - 1) + ")";
        //    }
        //}

        /// <summary>
        /// update the text on the label of the tab (the usernames of the buddies in the conversation)
        /// </summary>
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

        /// <summary>
        /// update the text below the chat text window
        /// </summary>
        private void _updateParticipantsText()
        {
            _participants.Text = "Du";
            foreach (string userName in _userNames)
            {
                _participants.Text += ", " + userName;
            }
        }

        /// <summary>
        /// Add user
        /// </summary>
        /// <param name="userName">name of the user to be added</param>
        public void AddUser(string userName)
        {
            if (InvokeRequired)
            {
                //Invoke(new AccessAddUserFromOtherThread(AddUser), userName)
                Invoke((Action<string>)AddUser, userName);
            }
            else
            {
                _userNames.Add(userName);
                _updateTabText();
                _updateParticipantsText();
            }
        }

        /// <summary>
        /// Remove user
        /// </summary>
        /// <param name="userName">name of the user to be removed</param>
        public void RemoveUser(string userName)
        {
            //string regExp = string.Format("(^{0}, )|((, )?{0})", userName);
            //this.Text = Regex.Replace(this.Text, regExp, "");
            _userNames.Remove(userName);
            _updateTabText();
            _updateParticipantsText();
        }

        /// <summary>
        /// Handler for "submit button was clicked"
        /// </summary>
        /// <param name="obj">unused</param>
        /// <param name="args">unused</param>
        private void OnSendButtonClick(object obj, EventArgs args)
        {
            if (this.OnTextSubmit != null)
            {
                this.OnTextSubmit(this._inputBox.Text);
            }
            this._inputBox.Text = "";
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

            this._tableLayoutPanel = new TableLayoutPanel();
            
            this._tableLayoutPanel.Dock = DockStyle.Fill;
            this._tableLayoutPanel.RowCount = 3;
            this._tableLayoutPanel.ColumnCount = 2;

            this._tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this._tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            this._tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this._tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 18));
            this._tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
            
            this.Controls.Add(this._tableLayoutPanel);
            
            // 
            // messagesBox
            // 
            this._messagesBox = new RichTextBox();
            this._messagesBox.Multiline = true;
            this._messagesBox.Name = "tabControlTextBox2";
            this._messagesBox.Dock = DockStyle.Fill;
            this._messagesBox.ReadOnly = true;
            this._messagesBox.BackColor = System.Drawing.SystemColors.Window;
            this._messagesBox.ScrollBars = RichTextBoxScrollBars.Vertical;

            this._messagesBox.VisibleChanged += (sender, e) =>
            {
                if (this._messagesBox.Visible)
                {
                    _messagesBox.AppendText(".");
                    _messagesBox.Text = _messagesBox.Text.Substring(0, _messagesBox.Text.Length - 1);
                }
            };

            this._tableLayoutPanel.Controls.Add(this._messagesBox, 0, 0);
            this._tableLayoutPanel.SetColumnSpan(this._messagesBox, 2);

            //
            // LabelText
            //
            _participants = new Label();
            _participants.Dock = DockStyle.Fill;
            this._tableLayoutPanel.Controls.Add(_participants, 0, 1);
            this._tableLayoutPanel.SetColumnSpan(_participants, 2);

            // 
            // inputBox
            //
            this._inputBox = new TextBox();
            this._inputBox.Multiline = true;
            this._inputBox.Name = "tabControlTextBox1";
            this._inputBox.Dock = DockStyle.Fill;
            this._inputBox.Tag = "";
            //this.tabControlTextBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this._tableLayoutPanel.Controls.Add(this._inputBox, 0, 2);

            // 
            // sendButton
            // 
            this._sendButton = new Button();
            this._sendButton.Location = new System.Drawing.Point(537, 377);
            this._sendButton.Name = "tabControlButton2";
            this._sendButton.Size = new System.Drawing.Size(46, 41);
            this._sendButton.Text = "Send";
            this._sendButton.UseVisualStyleBackColor = true;
            this._sendButton.Click += this.OnSendButtonClick;
            this._tableLayoutPanel.Controls.Add(this._sendButton, 1, 2);
            

            //this.Location = new System.Drawing.Point(4, 4);
            //this.Name = "tabPage1";
            //this.Padding = new Padding(3);
            //this.Size = new System.Drawing.Size(589, 425);
            this.UseVisualStyleBackColor = true;

            
        }
    }
}
