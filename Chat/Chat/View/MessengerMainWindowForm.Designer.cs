using System;
namespace Chat.View
{
    partial class MessengerMainWindowForm
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

            ConversationTabPage tabPage1 = new ConversationTabPage();
            ConversationTabPage tabPage2 = new ConversationTabPage();
            ConversationTabPage tabPage3 = new ConversationTabPage();

            tabPage1.Text = "guy";
            tabPage1.AddMessage("you", "hello", new DateTime(2015, 2, 16, 12, 55, 12));
            tabPage1.AddMessage("guy", "hello", new DateTime(2015, 2, 16, 13, 0, 0));

            tabPage2.Text = "gal";

            tabPage3.Text = "alien";
            tabPage3.AddMessage("alien", "WHOOOOT", DateTime.Now);

            this.ConversationTabControl.AddTab(tabPage1);
            this.ConversationTabControl.AddTab(tabPage2);
            this.ConversationTabControl.AddTab(tabPage3);
            this.ConversationTabControl.SelectTab(tabPage3);
            this.ConversationTabControl.ChangeActiveTab(tabPage3);
            //this.ConversationTabControl.AddTab(tabPage1);
            //this.ConversationTabControl.RemoveTab(tabPage1);


            this.BuddyList = new System.Windows.Forms.GroupBox();
            this.BuddyListListBox = new System.Windows.Forms.ListBox();
            this.BuddyListSubmitButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.BuddyList.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            
            //
            // ConversationTabControl
            //
            this.ConversationTabControl.Location = new System.Drawing.Point(13, 12);
            this.ConversationTabControl.Name = "ConversationTabControl";
            this.ConversationTabControl.SelectedIndex = 0;
            this.ConversationTabControl.Size = new System.Drawing.Size(597, 451);
            this.ConversationTabControl.TabIndex = 0;

            // 
            // BuddyList
            // 
            this.BuddyList.Controls.Add(this.BuddyListListBox);
            this.BuddyList.Controls.Add(this.BuddyListSubmitButton);
            this.BuddyList.Location = new System.Drawing.Point(616, 12);
            this.BuddyList.Name = "BuddyList";
            this.BuddyList.Size = new System.Drawing.Size(188, 451);
            this.BuddyList.TabIndex = 1;
            this.BuddyList.TabStop = false;
            this.BuddyList.Text = "Buddies";
            // 
            // BuddyListListBox
            // 
            this.BuddyListListBox.FormattingEnabled = true;
            this.BuddyListListBox.Items.AddRange(new object[] {
            "gal1",
            "gal2",
            "gal3",
            "guy1",
            "guy2",
            "guy3",
            "squirrel1",
            "squirrel2",
            "squirrel3"});
            this.BuddyListListBox.Location = new System.Drawing.Point(8, 19);
            this.BuddyListListBox.Name = "BuddyListListBox";
            this.BuddyListListBox.Size = new System.Drawing.Size(172, 394);
            this.BuddyListListBox.TabIndex = 1;
            this.BuddyListListBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // BuddyListSubmitButton
            // 
            this.BuddyListSubmitButton.Location = new System.Drawing.Point(8, 420);
            this.BuddyListSubmitButton.Name = "BuddyListSubmitButton";
            this.BuddyListSubmitButton.Size = new System.Drawing.Size(174, 25);
            this.BuddyListSubmitButton.TabIndex = 0;
            this.BuddyListSubmitButton.Text = "Buddy zur Liste hinzufügen";
            this.BuddyListSubmitButton.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 469);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(816, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(35, 17);
            this.toolStripStatusLabel1.Text = "12:24";
            
            // 
            // MessengerMainWindowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 491);
            this.Controls.Add(this.ConversationTabControl);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.BuddyList);
            this.Name = "MessengerMainWindowForm";
            this.Text = "Chat4edu";
            
            this.BuddyList.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ConversationTabControl ConversationTabControl;

        private System.Windows.Forms.GroupBox BuddyList;
        private System.Windows.Forms.ListBox BuddyListListBox;
        private System.Windows.Forms.Button BuddyListSubmitButton;
        
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        
    }
}


