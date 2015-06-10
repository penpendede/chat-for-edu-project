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
    public class MessengerMainWindowForm : Form
    {
        private System.ComponentModel.IContainer components = null;

        private TableLayoutPanel _tableLayoutPanel;

        public ConversationTabControl ConversationTabControl;

        public BuddyListGroupBox BuddyListGroupBox;

        private StatusStrip _statusStrip;
        private ToolStripStatusLabel _statusStripTimeLabel;

        private string _programName;

        public MessengerMainWindowForm()
        {
            this._programName = "ChatChatChat";
            this.ConversationTabControl = new ConversationTabControl();

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

            //
            // ConversationTabControl
            //
            this.ConversationTabControl.Name = "ConversationTabControl";
            this.ConversationTabControl.SelectedIndex = 0;
            this.ConversationTabControl.Dock = DockStyle.Fill;
            this.ConversationTabControl.TabIndex = 0;
            this._tableLayoutPanel.Controls.Add(this.ConversationTabControl, 0, 0);


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

        private System.Timers.Timer _timer;

        private void _updateClock()
        {

            this._statusStripTimeLabel.Text = DateTime.Now.ToString("hh:mm");

            if (_timer == null)
            {
                _timer = new System.Timers.Timer();
                _timer.AutoReset = false;
            }
            
            _timer.Interval = 60000 - DateTime.Now.Second * 1000 - DateTime.Now.Millisecond;
            _timer.Elapsed += (o, e) => { _updateClock(); };
            _timer.Start();
        }

        public void AddBuddyListGroupBox(BuddyListGroupBox buddyList)
        {
            //
            // BuddyListGroupBox
            //
            buddyList.Dock = DockStyle.Fill;
            buddyList.TabIndex = 1;
            buddyList.TabStop = false;
            this._tableLayoutPanel.Controls.Add(buddyList, 1, 0);
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

