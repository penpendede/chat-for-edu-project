using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chat
{
    public delegate void OnPortSubmit(int port);

    /// <summary>
    /// TODO
    /// </summary>
    public class PortForm : Form
    {
        private TableLayoutPanel tableLayoutPanel;

        private Label portLabel;
        private TextBox portTextBox;
        private Button portSubmitButton;

        public OnPortSubmit PortSubmit;

        public PortForm(int standardPort)
        {
            this.tableLayoutPanel = new TableLayoutPanel();
            this.portLabel = new Label();
            this.portSubmitButton = new Button();
            this.portTextBox = new TextBox();
            this.SuspendLayout();

            //
            // TableLayoutPanel
            //
            this.tableLayoutPanel.Dock = DockStyle.Fill;
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.ColumnCount = 2;

            this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            //this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            //this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            //this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));

            this.Controls.Add(this.tableLayoutPanel);

            // 
            // label2
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Name = "label2";
            this.portLabel.Text = "Bitte geben Sie den Port an, den Sie verwenden möchten";
            this.portLabel.Dock = DockStyle.Fill;
            this.portLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox1
            // 
            this.portTextBox.Name = "textBox1";
            this.portTextBox.Text = standardPort.ToString();
            this.portTextBox.TabIndex = 1;
            this.portTextBox.Dock = DockStyle.Fill;
            // 
            // New User
            // 
            this.portSubmitButton.TabIndex = 3;
            this.portSubmitButton.Text = "OK";
            this.portSubmitButton.UseVisualStyleBackColor = true;
            this.portSubmitButton.Click += onPortButtonClick;
            //this.newUserButton.Dock = DockStyle.Right;
            this.portSubmitButton.Anchor = AnchorStyles.Left;
            this.portSubmitButton.Width = 90;
            //this.
            
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(190, 138);
            
            this.tableLayoutPanel.Controls.Add(this.portLabel, 0, 0);
            //this.tableLayoutPanel.SetColumnSpan(this.portLabel, 2);
            
            this.tableLayoutPanel.Controls.Add(this.portTextBox, 0, 1);
            //this.tableLayoutPanel.SetColumnSpan(this.portTextBox, 2);

            this.tableLayoutPanel.Controls.Add(this.portSubmitButton, 0, 2);
            //this.tableLayoutPanel.Controls.Add(this.loginButton, 1, 4);

            this.Name = "PortForm";
            this.Text = "Port";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        /// <summary>
        /// Display "port cannot be parsed" message
        /// </summary>
        public void UnparsablePortMessage()
        {
            MessageBox.Show("Der angegebene Port hat ein ungültiges Format.", "Port ungültig", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="obj">ignored</param>
        /// <param name="args">ignored</param>
        private void onPortButtonClick(object obj, EventArgs args)
        {
            try
            {
                int port = Int32.Parse(this.portTextBox.Text);

                if (this.PortSubmit != null)
                {
                    this.PortSubmit(port);
                }
            }
            catch (System.FormatException e)
            {
                UnparsablePortMessage();
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
