using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Chat.View
{
    public delegate void BuddyListBuddyAddAction();
    public delegate void BuddyListBuddyRemoveAction(int id);
    public delegate void BuddyListOpenChatAction(int id);
    public delegate void BuddyListAddToChatAction(int id);

    public class BuddyListGroupBox : GroupBox
    {
        private List<int> buddyIds;

        public BuddyListBuddyAddAction BuddyAddAction;
        public BuddyListBuddyRemoveAction BuddyRemoveAction;
        public BuddyListOpenChatAction OpenChatAction;
        public BuddyListAddToChatAction AddToChatAction;

        private TableLayoutPanel tableLayoutPanel;
        private ListBox listBox;
        private Button addBuddyButton;
        private Button removeBuddyButton;
        private Button openChatButton;
        private Button addToChatButton;

        //TODO: Add doubleclick -> openChat
        //private ContextMenu contextMenu;

        public BuddyListGroupBox()
        {
            buddyIds = new List<int>();

            #region Initializing Components
            components = new System.ComponentModel.Container();

            this.SuspendLayout();

            this.listBox = new ListBox();
            this.addBuddyButton = new Button();

            //
            // FlowPanel
            //
            this.tableLayoutPanel = new TableLayoutPanel();
            this.tableLayoutPanel.Dock = DockStyle.Fill;
            this.tableLayoutPanel.RowCount = 0;
            this.tableLayoutPanel.ColumnCount = 1;

            this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

            this.Controls.Add(this.tableLayoutPanel);

            // 
            // BuddyList
            // 

            this.Name = "BuddyList";
            this.Text = "Buddies";


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
            this.listBox.DoubleClick += this.invokeOpenChatWithSelectedBuddy;
            this.listBox.TabIndex = 1;
            //this.BuddyListListBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.tableLayoutPanel.Controls.Add(this.listBox);

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
            this.addBuddyButton.Click += this.invokeBuddyAddAction;
            this.tableLayoutPanel.Controls.Add(this.addBuddyButton);
            // 
            // BuddyRemoveButton
            // 
            this.removeBuddyButton = new Button();
            //this.addBuddyButton.Location = new System.Drawing.Point(8, 420);
            this.removeBuddyButton.Name = "BuddyListSubmitButton";
            //this.addBuddyButton.Size = new System.Drawing.Size(174, 25);
            this.removeBuddyButton.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.removeBuddyButton.TabIndex = 3;
            this.removeBuddyButton.Text = "Buddy von Liste entfernen";
            this.removeBuddyButton.UseVisualStyleBackColor = true;
            //this.removeBuddyButton.Click += new EventHandler((o, e) => { if (this.BuddyRemoveAction != null) { this.BuddyRemoveAction(this.listBox.SelectedIndex); } });
            this.removeBuddyButton.Click += this.invokeBuddyRemoveWithSelectedBuddy;
            this.tableLayoutPanel.Controls.Add(this.removeBuddyButton);
            // 
            // OpenChatButton
            // 
            this.openChatButton = new Button();
            //this.addBuddyButton.Location = new System.Drawing.Point(8, 420);
            this.openChatButton.Name = "BuddyListSubmitButton";
            //this.addBuddyButton.Size = new System.Drawing.Size(174, 25);
            this.openChatButton.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.openChatButton.TabIndex = 4;
            this.openChatButton.Text = "Öffne Chat mit Buddy";
            this.openChatButton.UseVisualStyleBackColor = true;
            this.openChatButton.Click += this.invokeOpenChatWithSelectedBuddy;
            this.tableLayoutPanel.Controls.Add(this.openChatButton);

            //
            // AddToChatButton
            //
            this.addToChatButton = new Button();
            this.addToChatButton.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.addToChatButton.Text = "Füge Buddy zu Chat hinzu";
            this.addToChatButton.UseVisualStyleBackColor = true;
            this.addToChatButton.Click += this.invokeAddToChatWithSelectedBuddy;
            this.tableLayoutPanel.Controls.Add(this.addToChatButton);
            // TODO: Implement this

            //
            // ContextMenu
            //

            this.ContextMenu = new ContextMenu();

            MenuItem menuItemOpenChat = new MenuItem();
            menuItemOpenChat.Text = "Chat öffnen";
            menuItemOpenChat.Click += this.invokeOpenChatWithSelectedBuddy;
            this.ContextMenu.MenuItems.Add(menuItemOpenChat);

            MenuItem menuItemRemoveBuddy = new MenuItem();
            menuItemRemoveBuddy.Text = "Buddy entfernen";
            menuItemRemoveBuddy.Click += this.invokeBuddyRemoveWithSelectedBuddy;
            //menuItemRemoveBuddy.Click += new System.EventHandler((o, e) => { if (this.BuddyRemoveAction != null) { this.BuddyRemoveAction(this.listBox.SelectedIndex); } });
            this.ContextMenu.MenuItems.Add(menuItemRemoveBuddy);

            MenuItem menuItemAddToChat = new MenuItem();
            menuItemAddToChat.Text = "Buddy zu Chat hinzufügen";
            menuItemAddToChat.Click += this.invokeAddToChatWithSelectedBuddy;
            this.ContextMenu.MenuItems.Add(menuItemAddToChat);

            this.ResumeLayout(false);

            #endregion
        }

        //public BuddyListGroupBox(List<int> buddyIds, List<string> buddyNames)
        //{
        //    this.buddyNames = buddyNames;
        //    this.buddyIds = buddyIds;
        //    InitializeComponent();
        //}

        public void AddBuddy(int id, string name)
        {
            //this.buddyNames.Add(name);
            this.buddyIds.Add(id);
            this.listBox.Items.Add(name);
        }

        public void RemoveBuddy(int id)
        {
            int index = this.buddyIds.IndexOf(id);
            this.buddyIds.RemoveAt(index);
            //this.buddyNames.RemoveAt(index);
            this.listBox.Items.RemoveAt(index);
        }

        public void ChangeBuddyName(int id, string name)
        {
            int index = this.buddyIds.IndexOf(id);
            //this.buddyNames[index] = name;
            this.listBox.Items[index] = name;
        }

        //public BuddyListGroupBox(IContainer container)
        //{
        //    container.Add(this);

        //    InitializeComponent();
        //}

        private void rightClickSelect(object sender, MouseEventArgs e)
        {
            listBox.SelectedIndex = listBox.IndexFromPoint(e.X, e.Y);

            //if (e.Button == MouseButtons.Right)
            //{
            //    //now show your context menu...
            //}
        }

        private void invokeBuddyAddAction(object sender, EventArgs e)
        {
            if (this.BuddyAddAction != null)
            {
                this.BuddyAddAction();
            }
        }

        private void invokeOpenChatWithSelectedBuddy(object sender, EventArgs e)
        {
            if (this.OpenChatAction != null)
            {
                this.OpenChatAction(this.buddyIds[this.listBox.SelectedIndex]);
            }
        }

        private void invokeBuddyRemoveWithSelectedBuddy(object sender, EventArgs e)
        {
            if (this.BuddyRemoveAction != null)
            {
                this.BuddyRemoveAction(this.buddyIds[this.listBox.SelectedIndex]);
            }
        }

        private void invokeAddToChatWithSelectedBuddy(object sender, EventArgs e)
        {
            if (this.AddToChatAction != null)
            {
                this.AddToChatAction(this.buddyIds[this.listBox.SelectedIndex]);
            }
        }


        private System.ComponentModel.IContainer components = null;

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
