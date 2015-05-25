using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Chat.View
{
    public delegate void BuddyListBuddyAddAction();
    public delegate void BuddyListBuddyRemoveAction(int id);
    public delegate void BuddyListOpenChatAction(int id);
    public delegate void BuddyListAddToChatAction(int id);
    public delegate void BuddyListRemoveFromChatAction(int id);
    public delegate void BuddyListOpenRecentChatsAction(int id);

    public class BuddyListGroupBox : GroupBox
    {
        private List<int> buddyIds;

        public BuddyListBuddyAddAction BuddyAddAction;
        public BuddyListBuddyRemoveAction BuddyRemoveAction;
        public BuddyListOpenChatAction OpenChatAction;
        public BuddyListAddToChatAction AddToChatAction;
        public BuddyListRemoveFromChatAction RemoveFromChatAction;
        public BuddyListOpenRecentChatsAction OpenRecentChatsAction;

        private TableLayoutPanel _tableLayoutPanel;
        private ListBox _buddyListBox;
        private Button _addBuddyButton;
        //private Button _removeBuddyButton;
        //private Button _openChatButton;
        //private Button _addToChatButton;

        //TODO: Add doubleclick -> openChat
        //private ContextMenu contextMenu;

        public BuddyListGroupBox()
        {
            buddyIds = new List<int>();

            #region Initializing Components
            components = new System.ComponentModel.Container();

            this.SuspendLayout();

            this._buddyListBox = new ListBox();
            this._addBuddyButton = new Button();

            //
            // FlowPanel
            //
            this._tableLayoutPanel = new TableLayoutPanel();
            this._tableLayoutPanel.Dock = DockStyle.Fill;
            this._tableLayoutPanel.RowCount = 0;
            this._tableLayoutPanel.ColumnCount = 1;

            this._tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this._tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this._tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

            this.Controls.Add(this._tableLayoutPanel);

            // 
            // BuddyList
            // 

            this.Name = "BuddyList";
            this.Text = "Buddies";


            // 
            // BuddyListListBox
            // 
            this._buddyListBox.FormattingEnabled = true;
            //this.ListBox.Items.AddRange(this.buddyNames.ToArray());
            this._buddyListBox.Name = "BuddyListListBox";
            //this.listBox.Location = new System.Drawing.Point(8, 19);
            //this.listBox.Size = new System.Drawing.Size(172, 394);
            //this.listBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            this._buddyListBox.Dock = DockStyle.Fill;
            this._buddyListBox.MouseDown += this.rightClickSelect;
            this._buddyListBox.DoubleClick += this.invokeOpenChatWithSelectedBuddy;
            this._buddyListBox.TabIndex = 1;
            //this.BuddyListListBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this._tableLayoutPanel.Controls.Add(this._buddyListBox);

            // 
            // BuddyAddButton
            // 
            this._addBuddyButton.Name = "BuddyListSubmitButton";
            //this.addBuddyButton.Location = new System.Drawing.Point(8, 420);
            //this.addBuddyButton.Size = new System.Drawing.Size(174, 25);
            //this.addBuddyButton.Height = 25;
            //this.addBuddyButton.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this._addBuddyButton.Dock = DockStyle.Fill;
            this._addBuddyButton.TabIndex = 2;
            this._addBuddyButton.Text = "Buddy zur Liste hinzufügen";
            this._addBuddyButton.UseVisualStyleBackColor = true;
            this._addBuddyButton.Click += this.invokeBuddyAddAction;
            this._tableLayoutPanel.Controls.Add(this._addBuddyButton);
            //// 
            //// BuddyRemoveButton
            //// 
            //this._removeBuddyButton = new Button();
            ////this.addBuddyButton.Location = new System.Drawing.Point(8, 420);
            //this._removeBuddyButton.Name = "BuddyListSubmitButton";
            ////this.addBuddyButton.Size = new System.Drawing.Size(174, 25);
            //this._removeBuddyButton.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            //this._removeBuddyButton.TabIndex = 3;
            //this._removeBuddyButton.Text = "Buddy von Liste entfernen";
            //this._removeBuddyButton.UseVisualStyleBackColor = true;
            ////this.removeBuddyButton.Click += new EventHandler((o, e) => { if (this.BuddyRemoveAction != null) { this.BuddyRemoveAction(this.listBox.SelectedIndex); } });
            //this._removeBuddyButton.Click += this.invokeBuddyRemoveWithSelectedBuddy;
            //this._tableLayoutPanel.Controls.Add(this._removeBuddyButton);
            //// 
            //// OpenChatButton
            //// 
            //this._openChatButton = new Button();
            ////this.addBuddyButton.Location = new System.Drawing.Point(8, 420);
            //this._openChatButton.Name = "BuddyListSubmitButton";
            ////this.addBuddyButton.Size = new System.Drawing.Size(174, 25);
            //this._openChatButton.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            //this._openChatButton.TabIndex = 4;
            //this._openChatButton.Text = "Öffne Chat mit Buddy";
            //this._openChatButton.UseVisualStyleBackColor = true;
            //this._openChatButton.Click += this.invokeOpenChatWithSelectedBuddy;
            //this._tableLayoutPanel.Controls.Add(this._openChatButton);

            ////
            //// AddToChatButton
            ////
            //this._addToChatButton = new Button();
            //this._addToChatButton.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            //this._addToChatButton.Text = "Füge Buddy zu Chat hinzu";
            //this._addToChatButton.UseVisualStyleBackColor = true;
            //this._addToChatButton.Click += this.invokeAddToChatWithSelectedBuddy;
            //this._tableLayoutPanel.Controls.Add(this._addToChatButton);
            //// TODO: Implement this

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

            MenuItem menuItemRemoveFromChat = new MenuItem();
            menuItemRemoveFromChat.Text = "Buddy aus dem Chat entfernen";
            menuItemRemoveFromChat.Click += this.invokeRemoveFromChatWithSelectedBuddy;
            this.ContextMenu.MenuItems.Add(menuItemRemoveFromChat);

            MenuItem menuItemOpenRecentChats = new MenuItem();
            menuItemOpenRecentChats.Text = "Alle vergangenen Chats mit diesem Buddy öffnen";
            menuItemOpenRecentChats.Click += _invokeOpenRecentChatsWithSelectedBuddy;
            this.ContextMenu.MenuItems.Add(menuItemOpenRecentChats);

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
            this._buddyListBox.Items.Add(name);
        }

        public void RemoveBuddy(int id)
        {
            int index = this.buddyIds.IndexOf(id);
            this.buddyIds.RemoveAt(index);
            //this.buddyNames.RemoveAt(index);
            this._buddyListBox.Items.RemoveAt(index);
        }

        public void ChangeBuddyName(int id, string name)
        {
            int index = this.buddyIds.IndexOf(id);
            //this.buddyNames[index] = name;
            this._buddyListBox.Items[index] = name;
        }

        //public BuddyListGroupBox(IContainer container)
        //{
        //    container.Add(this);

        //    InitializeComponent();
        //}

        private void rightClickSelect(object sender, MouseEventArgs e)
        {
            _buddyListBox.SelectedIndex = _buddyListBox.IndexFromPoint(e.X, e.Y);
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
                this.OpenChatAction(this.buddyIds[this._buddyListBox.SelectedIndex]);
            }
        }

        private void invokeBuddyRemoveWithSelectedBuddy(object sender, EventArgs e)
        {
            if (this.BuddyRemoveAction != null)
            {
                this.BuddyRemoveAction(this.buddyIds[this._buddyListBox.SelectedIndex]);
            }
        }

        private void invokeAddToChatWithSelectedBuddy(object sender, EventArgs e)
        {
            if (this.AddToChatAction != null)
            {
                this.AddToChatAction(this.buddyIds[this._buddyListBox.SelectedIndex]);
            }
        }

        private void invokeRemoveFromChatWithSelectedBuddy(object sender, EventArgs e)
        {
            if (this.RemoveFromChatAction != null)
            {
                this.RemoveFromChatAction(this.buddyIds[this._buddyListBox.SelectedIndex]);
            }
        }

        private void _invokeOpenRecentChatsWithSelectedBuddy(object sender, EventArgs e)
        {
            if (this.OpenRecentChatsAction != null)
            {
                this.OpenRecentChatsAction(this.buddyIds[this._buddyListBox.SelectedIndex]);
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
