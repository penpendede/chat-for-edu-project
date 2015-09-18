using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chat.View
{
    /// <summary>
    /// The main window is a form
    /// </summary>
    public class MessengerMainWindowForm : Form
    {
        private System.ComponentModel.IContainer components = null;

        private TableLayoutPanel _tableLayoutPanel;

        private StatusStrip _statusStrip;
        private ToolStripStatusLabel _statusStripTimeLabel;

        private string _programName;

        public MessengerMainWindowForm()
        {
			this._programName = "ChatChatChat";

            this.SuspendLayout();

            //
            // Panel
            //
            this._tableLayoutPanel = new TableLayoutPanel();

            this._tableLayoutPanel = new TableLayoutPanel();
            this._tableLayoutPanel.Dock = DockStyle.Fill;
            this._tableLayoutPanel.RowCount = 2;
            this._tableLayoutPanel.ColumnCount = 2;

            this._tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this._tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
            this._tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this._tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));

            this.Controls.Add(this._tableLayoutPanel);

            this._statusStrip = new StatusStrip();
            this._statusStripTimeLabel = new ToolStripStatusLabel();

            this._statusStrip.SuspendLayout();

            // 
            // statusStrip1
            // 
            this._statusStrip.Items.AddRange(new ToolStripItem[] {
            this._statusStripTimeLabel});
            this._statusStrip.Name = "statusStrip1";
            this._statusStrip.Dock = DockStyle.Fill;
            this._statusStrip.TabIndex = 2;
            this._statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this._statusStripTimeLabel.Name = "toolStripStatusLabel1";
            this._statusStripTimeLabel.Dock = DockStyle.Fill;
            
            _updateClock();

            this._tableLayoutPanel.Controls.Add(this._statusStrip, 0, 1);
            this._tableLayoutPanel.SetColumnSpan(this._statusStrip, 2);
            // 
            // MessengerMainWindowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 491);


            this.Name = "MessengerMainWindowForm";
            this.Text = this._programName;

            this._statusStrip.ResumeLayout(false);
            this._statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        /// <summary>
        /// Set user name to be displayed
        /// </summary>
        /// <param name="userName">user name</param>
        public void SetUserName(string userName)
        {
            if (userName == "")
            {
                this.Text = this._programName;
            }
            else
            {
                this.Text =  userName + " @ " + this._programName;
            }
        }

        /// <summary>
        /// Add a tab control
        /// </summary>
        /// <param name="tabControl">Tab control to be added</param>
        public void AddConversationTabControl(ConversationTabControl tabControl)
        {
            tabControl.TabIndex = 0;
            tabControl.Dock = DockStyle.Fill;
            this._tableLayoutPanel.Controls.Add(tabControl, 0, 0);
        }

        /// <summary>
        /// Add buddy list grup box
        /// </summary>
        /// <param name="buddyList">Buddy lists group box</param>
        public void AddBuddyListGroupBox(BuddyListGroupBox buddyList)
        {
            buddyList.Dock = DockStyle.Fill;
            buddyList.TabIndex = 1;
            buddyList.TabStop = false;
            this._tableLayoutPanel.Controls.Add(buddyList, 1, 0);
        }

        /// <summary>
        /// Ask user to confirm adding a buddy
        /// </summary>
        /// <param name="userName">the buddy's user name</param>
        /// <param name="ip">the buddy's IP</param>
        /// <param name="port">the buddy's port</param>
        /// <returns>user confirmed adding</returns>
        public bool AskForBuddyAdd(string userName, string ip, int port)
        {
             return MessageBox.Show(string.Format("Wollen Sie den Benutzer {0} mit der Addresse {1}:{2} Ihrer Buddyliste hinzufügen?", userName, ip, port), "Bestätigen",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private System.Timers.Timer _timer;

        /// <summary>
        /// Keep clock updated
        /// </summary>
        private void _updateClock()
        {
            this._statusStripTimeLabel.Text = DateTime.Now.ToString("HH:mm");

            if (_timer == null)
            {
                _timer = new System.Timers.Timer();
                _timer.AutoReset = false;
                _timer.Elapsed += (o, e) => { _updateClock(); };
            }
            
            _timer.Interval = 60000 - 1000 * DateTime.Now.Second - DateTime.Now.Millisecond;
            _timer.Start();
        }

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
    }
}

