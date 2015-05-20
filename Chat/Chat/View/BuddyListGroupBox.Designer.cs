using System.Windows.Forms;
namespace Chat.View
{
    [System.ComponentModel.DesignerCategory("")]
    partial class BuddyListGroupBox : System.Windows.Forms.GroupBox
    {
        // TODO: Implement methods and delegates

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            this.SuspendLayout();
            
            this.listBox = new System.Windows.Forms.ListBox();
            this.addBuddyButton = new System.Windows.Forms.Button();

            //
            // FlowPanel
            //
            this.panel = new TableLayoutPanel();
            this.panel.Dock = DockStyle.Fill;
            this.panel.RowCount = 0;
            this.panel.ColumnCount = 1;

            this.panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

            this.Controls.Add(this.panel);
            
            // 
            // BuddyList
            // 
            
            
            
            // 
            // BuddyListListBox
            // 
            this.listBox.FormattingEnabled = true;
            //this.ListBox.Items.AddRange(this.buddyNames.ToArray());
            this.listBox.Name = "BuddyListListBox";
            //this.listBox.Location = new System.Drawing.Point(8, 19);
            //this.listBox.Size = new System.Drawing.Size(172, 394);
            //this.listBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            this.listBox.Dock = DockStyle.Fill;
            this.listBox.MouseDown += this.rightClickSelect;
            this.listBox.TabIndex = 1;
            //this.BuddyListListBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.panel.Controls.Add(this.listBox);
            
            // 
            // BuddyAddButton
            // 
            this.addBuddyButton.Name = "BuddyListSubmitButton";
            //this.addBuddyButton.Location = new System.Drawing.Point(8, 420);
            //this.addBuddyButton.Size = new System.Drawing.Size(174, 25);
            //this.addBuddyButton.Height = 25;
            //this.addBuddyButton.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.addBuddyButton.Dock = DockStyle.Fill;
            this.addBuddyButton.TabIndex = 2;
            this.addBuddyButton.Text = "Buddy zur Liste hinzufügen";
            this.addBuddyButton.UseVisualStyleBackColor = true;
            this.panel.Controls.Add(this.addBuddyButton);
            // 
            // BuddyAddButton
            // 
            this.removeBuddyButton = new Button();
            //this.addBuddyButton.Location = new System.Drawing.Point(8, 420);
            this.removeBuddyButton.Name = "BuddyListSubmitButton";
            //this.addBuddyButton.Size = new System.Drawing.Size(174, 25);
            this.removeBuddyButton.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.removeBuddyButton.TabIndex = 3;
            this.removeBuddyButton.Text = "Buddy von Liste entfernen";
            this.removeBuddyButton.UseVisualStyleBackColor = true;
            this.panel.Controls.Add(this.removeBuddyButton);
            // 
            // BuddyAddButton
            // 
            this.openChatButton = new Button();
            //this.addBuddyButton.Location = new System.Drawing.Point(8, 420);
            this.openChatButton.Name = "BuddyListSubmitButton";
            //this.addBuddyButton.Size = new System.Drawing.Size(174, 25);
            this.openChatButton.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.openChatButton.TabIndex = 4;
            this.openChatButton.Text = "Öffne Chat mit Buddy";
            this.openChatButton.UseVisualStyleBackColor = true;
            this.panel.Controls.Add(this.openChatButton);

            //
            // ContextMenu
            //

            this.ContextMenu = new System.Windows.Forms.ContextMenu();
            
            MenuItem menuItemOpenChat = new MenuItem();
            menuItemOpenChat.Text = "Chat öffnen";
            menuItemOpenChat.Click += new System.EventHandler((o, e) => { if (this.OpenChatAction != null) { this.OpenChatAction(this.listBox.SelectedIndex); } });
            this.ContextMenu.MenuItems.Add(menuItemOpenChat);
            
            MenuItem menuItemRemoveBuddy = new MenuItem();
            menuItemRemoveBuddy.Text = "Buddy entfernen";
            menuItemRemoveBuddy.Click += new System.EventHandler((o, e) => { if (this.BuddyRemoveAction != null) { this.BuddyRemoveAction(this.listBox.SelectedIndex); } });
            this.ContextMenu.MenuItems.Add(menuItemRemoveBuddy);


            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel panel;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.Button addBuddyButton;
        private System.Windows.Forms.Button removeBuddyButton;
        private System.Windows.Forms.Button openChatButton;

        //TODO: Add doubleclick -> openChat
        //private System.Windows.Forms.ContextMenu contextMenu;

        private void rightClickSelect(object sender, MouseEventArgs e)
        {
            listBox.SelectedIndex = listBox.IndexFromPoint(e.X, e.Y);

            //if (e.Button == System.Windows.Forms.MouseButtons.Right)
            //{
            //    //now show your context menu...
            //}
        }
    }
}
