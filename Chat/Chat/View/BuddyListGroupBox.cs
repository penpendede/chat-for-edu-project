using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Chat.View
{
    public delegate void BuddyListBuddyAddAction();
    public delegate void BuddyListBuddyRemoveAction(int id);
    public delegate void BuddyListOpenChatAction(int id);

    public partial class BuddyListGroupBox
    {
        private List<int> buddyIds;

        public BuddyListBuddyAddAction BuddyAddAction;
        public BuddyListBuddyRemoveAction BuddyRemoveAction;
        public BuddyListOpenChatAction OpenChatAction;

        public BuddyListGroupBox()
        {
            buddyIds = new List<int>();
            InitializeComponent();
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
    }
}
