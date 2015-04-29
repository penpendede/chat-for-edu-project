using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace Chat.View
{
    delegate void OnTabFocus(ConversationTabPage tabPage);
    delegate void OnTabClose(ConversationTabPage tabPage);

    //[Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    class ConversationTabControl : TabControl
    {
        public ConversationTabControl()
        {
            InitializeComponent();
        }

        // FIXME: Choose either one or the other option, depending on MVC design decision
        public ConversationTabPage AddTab()
        {
            ConversationTabPage tabPage = new ConversationTabPage();

            this.Controls.Add(tabPage);
            return tabPage;
        }

        public void AddTab(ConversationTabPage tabPage)
        {
            this.Controls.Add(tabPage);
        }

        public void RemoveTab(ConversationTabPage tabPage)
        {
            this.Controls.Remove(tabPage);
        }

        public void ChangeActiveTab(ConversationTabPage tabPage)
        {
            this.SelectTab(tabPage);
        }

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

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// </summary>
       
        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Alignment = System.Windows.Forms.TabAlignment.Bottom;

            this.ResumeLayout(false);
            
            this.PerformLayout();
        }

        //public System.Windows.Forms.Control ActiveControl { get; set; }
        //public bool ActiveControl(System.Windows.Forms.Control active) {
        //    return true;
        //}

    }
}
