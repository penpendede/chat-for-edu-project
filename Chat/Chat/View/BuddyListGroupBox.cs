using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Chat.View
{
    public delegate void BuddyListBuddyAddAction();
    public delegate void BuddyListBuddyRemoveAction(int id);
    public delegate void BuddyListOpenChatAction(int id);
    public delegate void BuddyListAddToChatAction(int id);
    public delegate void BuddyListOpenRecentChatsAction(int id);

    public class BuddyListGroupBox : GroupBox
    {
        private List<int> _buddyIds;

        // delegates
        public BuddyListBuddyAddAction BuddyAddAction;
        public BuddyListBuddyRemoveAction BuddyRemoveAction;
        public BuddyListOpenChatAction OpenChatAction;
        public BuddyListAddToChatAction AddToChatAction;
        public BuddyListOpenRecentChatsAction OpenRecentChatsAction;

        // form elements
        private TableLayoutPanel _tableLayoutPanel;
        private ListBox _buddyListBox;
        private Button _addBuddyButton;

        public BuddyListGroupBox()
        {
            _buddyIds = new List<int>();

            #region Initializing Components
            components = new System.ComponentModel.Container();

            SuspendLayout();

            // 
            // BuddyList
            // 

            Name = "BuddyList";
            Text = "Buddies";

            //
            // FlowPanel
            //
            _tableLayoutPanel = new TableLayoutPanel();
            _tableLayoutPanel.Dock = DockStyle.Fill;
            _tableLayoutPanel.RowCount = 0;
            _tableLayoutPanel.ColumnCount = 1;

            _tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

            Controls.Add(_tableLayoutPanel);


            // 
            // BuddyListListBox
            // 
            _buddyListBox = new ListBox();
            _buddyListBox.FormattingEnabled = true;
            _buddyListBox.Name = "BuddyListListBox";
            _buddyListBox.Dock = DockStyle.Fill;
            _buddyListBox.MouseDown += _rightClickSelect;
            _buddyListBox.DoubleClick += _doubleClickOpenChat;
            _buddyListBox.TabIndex = 1;
            _tableLayoutPanel.Controls.Add(_buddyListBox);

            // 
            // BuddyAddButton
            // 
            _addBuddyButton = new Button();
            _addBuddyButton.Name = "BuddyListSubmitButton";
            _addBuddyButton.Dock = DockStyle.Fill;
            _addBuddyButton.TabIndex = 2;
            _addBuddyButton.Text = "Buddy zur Liste hinzufügen";
            _addBuddyButton.UseVisualStyleBackColor = true;
            _addBuddyButton.Click += _invokeBuddyAddAction;
            _tableLayoutPanel.Controls.Add(_addBuddyButton);
            

            //
            // ContextMenu
            //

            ContextMenu = new ContextMenu();

            MenuItem menuItemOpenChat = new MenuItem();
            menuItemOpenChat.Text = "Chat öffnen";
            menuItemOpenChat.Click += _invokeOpenChatWithSelectedBuddy;
            ContextMenu.MenuItems.Add(menuItemOpenChat);

            MenuItem menuItemRemoveBuddy = new MenuItem();
            menuItemRemoveBuddy.Text = "Buddy entfernen";
            menuItemRemoveBuddy.Click += _invokeBuddyRemoveWithSelectedBuddy;
            //menuItemRemoveBuddy.Click += new System.EventHandler((o, e) => { if (BuddyRemoveAction != null) { BuddyRemoveAction(listBox.SelectedIndex); } });
            ContextMenu.MenuItems.Add(menuItemRemoveBuddy);

            MenuItem menuItemAddToChat = new MenuItem();
            menuItemAddToChat.Text = "Buddy zu Chat hinzufügen";
            menuItemAddToChat.Click += _invokeAddToChatWithSelectedBuddy;
            ContextMenu.MenuItems.Add(menuItemAddToChat);

            MenuItem menuItemOpenRecentChats = new MenuItem();
            menuItemOpenRecentChats.Text = "Alle vergangenen Chats mit diesem Buddy öffnen";
            menuItemOpenRecentChats.Click += _invokeOpenRecentChatsWithSelectedBuddy;
            ContextMenu.MenuItems.Add(menuItemOpenRecentChats);

            ResumeLayout(false);

            #endregion
        }
        
        // public methods

        /// <summary>
        /// Add buddy to list
        /// </summary>
        /// <param name="id">buddy's ID</param>
        /// <param name="name">buddy's name</param>
        public void AddBuddy(int id, string name)
        {
            if (InvokeRequired)
            {
                Invoke((Action<int, string>)AddBuddy, id, name);
            }
            else
            {
                _buddyIds.Add(id);
                _buddyListBox.Items.Add(name);
            }
        }

        /// <summary>
        /// Remove buddy from lsit
        /// </summary>
        /// <param name="id">buddy's ID</param>
        public void RemoveBuddy(int id)
        {
            int index = _buddyIds.IndexOf(id);
            _buddyIds.RemoveAt(index);
            _buddyListBox.Items.RemoveAt(index);
        }

        /// <summary>
        /// Change the name displayed for a buddy
        /// </summary>
        /// <param name="id">buddy's ID</param>
        /// <param name="name">buddy's (new) name</param>
        public void ChangeBuddyName(int id, string name)
        {
            int index = _buddyIds.IndexOf(id);
            _buddyListBox.Items[index] = name;
        }

        /// <summary>
        /// Ask user to confirm buddy removal
        /// </summary>
        /// <param name="name">buddy's name</param>
        /// <returns>truth value of "user confirmed removed"</returns>
        public bool AskForBuddyRemove(string name)
        {
            return
                MessageBox.Show(
                    string.Format( "Möchten Sie den Buddy {0} wirklich von Ihrer Freundesliste entfernen? Der Nachrichtenverlauf wird nicht mehr verfügbar sein!", name), 
                    "Entfernen des Buddies bestätigen", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        // invoke custom delegates

        /// <summary>
        /// open chat if buddy's name is double-clicked
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void _doubleClickOpenChat(object sender, EventArgs e)
        {
            if (_buddyListBox.SelectedIndex > -1)
            {
                if (OpenChatAction != null)
                {
                    OpenChatAction(_buddyIds[_buddyListBox.SelectedIndex]);
                }
            }
        }

        /// <summary>
        /// Select by right click
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">used to obtain coordinates</param>
        private void _rightClickSelect(object sender, MouseEventArgs e)
        {
            _buddyListBox.SelectedIndex = _buddyListBox.IndexFromPoint(e.X, e.Y);
        }

        /// <summary>
        /// trigger "add buddy" action 
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void _invokeBuddyAddAction(object sender, EventArgs e)
        {
            if (BuddyAddAction != null)
            {
                BuddyAddAction();
            }
        }

        /// <summary>
        /// trigger "open chat with selected buddy"
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void _invokeOpenChatWithSelectedBuddy(object sender, EventArgs e)
        {
            if (OpenChatAction != null)
            {
                OpenChatAction(_buddyIds[_buddyListBox.SelectedIndex]);
            }
        }

        /// <summary>
        /// trigger "remove buddy" action for selected buddy
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void _invokeBuddyRemoveWithSelectedBuddy(object sender, EventArgs e)
        {
            if (BuddyRemoveAction != null)
            {
                BuddyRemoveAction(_buddyIds[_buddyListBox.SelectedIndex]);
            }
        }

        /// <summary>
        /// trigger "add to chat" action for selected buddy
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void _invokeAddToChatWithSelectedBuddy(object sender, EventArgs e)
        {
            if (AddToChatAction != null)
            {
                AddToChatAction(_buddyIds[_buddyListBox.SelectedIndex]);
            }
        }

        /// <summary>
        /// trigger "open recent chats" action for selected buddy
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void _invokeOpenRecentChatsWithSelectedBuddy(object sender, EventArgs e)
        {
            if (OpenRecentChatsAction != null)
            {
                OpenRecentChatsAction(_buddyIds[_buddyListBox.SelectedIndex]);
            }
        }

        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="disposing"></param>
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
