using System;
using System.Windows.Forms;

namespace Chat.View
{
    [System.ComponentModel.DesignerCategory("")]
    public partial class MessengerMainWindowForm
    {
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

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.ConversationTabControl = new ConversationTabControl();

            this.SuspendLayout();

            //
            // Panel
            //
            this.panel = new TableLayoutPanel();

            this.panel = new TableLayoutPanel();
            this.panel.Dock = DockStyle.Fill;
            this.panel.RowCount = 2;
            this.panel.ColumnCount = 2;

            this.panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
            this.panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));

            this.Controls.Add(this.panel);

            //
            // ConversationTabControl
            //
            this.ConversationTabControl.Name = "ConversationTabControl";
            this.ConversationTabControl.SelectedIndex = 0;
            this.ConversationTabControl.Dock = DockStyle.Fill;
            this.ConversationTabControl.TabIndex = 0;
            this.panel.Controls.Add(this.ConversationTabControl, 0, 0);

            //
            // BuddyListGroupBox
            //
            this.BuddyListGroupBox = new BuddyListGroupBox();
            this.BuddyListGroupBox.Name = "BuddyList";
            this.BuddyListGroupBox.Dock = DockStyle.Fill;
            this.BuddyListGroupBox.TabIndex = 1;
            this.BuddyListGroupBox.TabStop = false;
            this.BuddyListGroupBox.Text = "Buddies";
            this.panel.Controls.Add(this.BuddyListGroupBox, 1, 0);


            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();

            this.statusStrip1.SuspendLayout();

            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Dock = DockStyle.Fill;
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Dock = DockStyle.Fill;
            this.toolStripStatusLabel1.Text = "12:24";

            this.panel.Controls.Add(this.statusStrip1, 0, 1);
            this.panel.SetColumnSpan(this.statusStrip1, 2);
            // 
            // MessengerMainWindowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 491);
            
            
            this.Name = "MessengerMainWindowForm";
            this.Text = "Chat4edu";
            
            
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TableLayoutPanel panel;

        public ConversationTabControl ConversationTabControl;

        public BuddyListGroupBox BuddyListGroupBox;
        
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        
    }
}


