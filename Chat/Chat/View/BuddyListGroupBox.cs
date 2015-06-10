﻿using System;
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

        public void AddBuddy(int id, string name)
        {
            _buddyIds.Add(id);
            _buddyListBox.Items.Add(name);
        }

        public void RemoveBuddy(int id)
        {
            int index = _buddyIds.IndexOf(id);
            _buddyIds.RemoveAt(index);
            _buddyListBox.Items.RemoveAt(index);
        }

        public void ChangeBuddyName(int id, string name)
        {
            int index = _buddyIds.IndexOf(id);
            _buddyListBox.Items[index] = name;
        }

        public bool AskForBuddyRemove(string name)
        {
            return
                MessageBox.Show(
                    string.Format( "Möchten Sie den Buddy {0} wirklich von Ihrer Freundesliste entfernen? Der Nachrichtenverlauf wird nicht mehr verfügbar sein!", name), 
                    "Entfernen des Buddies bestätigen", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        // invoke custom delegates

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

        private void _rightClickSelect(object sender, MouseEventArgs e)
        {
            _buddyListBox.SelectedIndex = _buddyListBox.IndexFromPoint(e.X, e.Y);
        }

        private void _invokeBuddyAddAction(object sender, EventArgs e)
        {
            if (BuddyAddAction != null)
            {
                BuddyAddAction();
            }
        }

        private void _invokeOpenChatWithSelectedBuddy(object sender, EventArgs e)
        {
            if (OpenChatAction != null)
            {
                OpenChatAction(_buddyIds[_buddyListBox.SelectedIndex]);
            }
        }

        private void _invokeBuddyRemoveWithSelectedBuddy(object sender, EventArgs e)
        {
            if (BuddyRemoveAction != null)
            {
                BuddyRemoveAction(_buddyIds[_buddyListBox.SelectedIndex]);
            }
        }

        private void _invokeAddToChatWithSelectedBuddy(object sender, EventArgs e)
        {
            if (AddToChatAction != null)
            {
                AddToChatAction(_buddyIds[_buddyListBox.SelectedIndex]);
            }
        }

        private void _invokeOpenRecentChatsWithSelectedBuddy(object sender, EventArgs e)
        {
            if (OpenRecentChatsAction != null)
            {
                OpenRecentChatsAction(_buddyIds[_buddyListBox.SelectedIndex]);
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
