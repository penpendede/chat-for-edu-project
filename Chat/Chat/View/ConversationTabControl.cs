using System.Windows.Forms;
using System.Drawing;
using System;

namespace Chat.View
{
    public delegate void TabControlOnTabFocus(TabPage tabPage);
    public delegate void TabControlOnTabClose(TabPage tabPage, bool closeConversation);

    delegate void AccessAddTabPageFromOtherThread(ConversationTabPage tabPage);


    //[Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public class ConversationTabControl : TabControl
    {
        public TabControlOnTabClose TabClose;

        public ConversationTabControl()
        {
            components = new System.ComponentModel.Container();

            this.SuspendLayout();

            Name = "ConversationTabControl";

            this.Alignment = System.Windows.Forms.TabAlignment.Bottom;

            this.ResumeLayout(false);

            this.PerformLayout();

            this.DrawItem += _onDrawItem;

            this.DrawMode = TabDrawMode.OwnerDrawFixed;

            this.SizeMode = TabSizeMode.Fixed;

            this.MouseDown += _closeTabPageWithClickOnX;

            this.ItemSize = new Size(100, 20);
        }

        #region Close Buttons on Tab Page
        // credits to http://www.dotnetthoughts.net/implementing-close-button-in-tab-pages/

        public bool MeasureLabelText(string labelText)
        {
            return TextRenderer.MeasureText(labelText + " x", this.Font).Width < this.ItemSize.Width - 12;
        }

        private void _onDrawItem(object sender, DrawItemEventArgs e) 
        {

            //This code will render a "x" mark at the end of the Tab caption. 
            e.Graphics.DrawString("\u00D7", e.Font, Brushes.Black, e.Bounds.Right - 15, e.Bounds.Top + 4);
            e.Graphics.DrawString(this.TabPages[e.Index].Text, e.Font, Brushes.Black, e.Bounds.Left + 12, e.Bounds.Top + 4);
            e.DrawFocusRectangle();

            //this.ItemSize = new Size(TextRenderer.MeasureText(this.TabPages[e.Index].Text + "  x", e.Font).Width + 70, this.ItemSize.Height);
        }

        private void _closeTabPageWithClickOnX(object sender, MouseEventArgs e)
        {
            //Looping through the controls.
            for (int i = 0; i < this.TabPages.Count; i++)
            {
                Rectangle r = GetTabRect(i);
                //Getting the position of the "x" mark.
                Rectangle closeButton = new Rectangle(r.Right - 15, r.Top + 4, 9, 7);
                if (closeButton.Contains(e.Location))
                {
                        if (TabClose != null)
                        {
                            TabClose(this.TabPages[i], MessageBox.Show("Möchten Sie weiterhin Nachrichten aus dieser Konservation erhalten?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No);
                        }
                        break;
                }
            }
        }

        #endregion

        /// <summary>
        /// Add tab (for a new conversation) to tab control
        /// </summary>
        /// <param name="tabPage">tab to be added</param>
        public void AddTab(ConversationTabPage tabPage)
        {
            if (InvokeRequired)
            {
                Invoke(new AccessAddTabPageFromOtherThread(AddTab), tabPage);
            }
            else
            {
                this.Controls.Add(tabPage);
            }
        }

        /// <summary>
        /// Remove tab (for existing conversation) from tab control
        /// </summary>
        /// <param name="tabPage"></param>
        public void RemoveTab(ConversationTabPage tabPage)
        {
            this.Controls.Remove(tabPage);
        }

        /// <summary>
        /// Change activity of a tab
        /// </summary>
        /// <param name="tabPage">tab of which the activity is to be changed</param>
        public void ChangeActiveTab(ConversationTabPage tabPage)
        {
            if (InvokeRequired)
            {
                Invoke((Action<ConversationTabPage>)ChangeActiveTab, tabPage);
            }
            else
            {
                this.SelectTab(tabPage);
            }
        }

        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">1, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False. TODO</param>
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
