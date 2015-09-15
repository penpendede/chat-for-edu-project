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
    public delegate void OnBuddyAddSubmit(string name, string IP, int port);

    public class BuddyAddForm : Form
    {
        public OnBuddyAddSubmit BuddyAddSubmit;

        private TableLayoutPanel _tableLayoutPanel;
        private TextBox _buddyNameTextBox;
        private TextBox _ipTextBox;
        private TextBox _portTextBox;
        private Label _buddyNameLabel;
        private Label _ipLabel;
        private Label _portLabel;
        private Button _submitButton;

        public BuddyAddForm(int port)
        {
            _tableLayoutPanel = new TableLayoutPanel();
            _buddyNameTextBox = new TextBox();
            _ipTextBox = new TextBox();
            _buddyNameLabel = new Label();
            _ipLabel = new Label();
            _submitButton = new Button();
            _portTextBox = new TextBox();
            _portLabel = new Label();
            SuspendLayout();

            //
            // Panel
            //

            _tableLayoutPanel = new TableLayoutPanel();
            _tableLayoutPanel.Dock = DockStyle.Fill;
            _tableLayoutPanel.RowCount = 4;
            _tableLayoutPanel.ColumnCount = 1;

            _tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            _tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            _tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            _tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            _tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            _tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));

            Controls.Add(_tableLayoutPanel);

            // 
            // BuddyName
            // 
            _buddyNameTextBox.Name = "BuddyName";
            _buddyNameTextBox.TabIndex = 0;
            _buddyNameTextBox.Dock = DockStyle.Fill;
            _tableLayoutPanel.Controls.Add(_buddyNameTextBox, 0, 1);
            // 
            // IP
            // 
            _ipTextBox.Name = "IP";
            _ipTextBox.TabIndex = 1;
            _ipTextBox.Dock = DockStyle.Fill;
            _tableLayoutPanel.Controls.Add(_ipTextBox, 0, 3);
            // 
            // port
            // 
            _portTextBox.Name = "Port";
            _portTextBox.TabIndex = 1;
            _portTextBox.Text = port.ToString();
            _portTextBox.Dock = DockStyle.Fill;
            _tableLayoutPanel.Controls.Add(_portTextBox, 0, 5);
            // 
            // label1
            // 
            _buddyNameLabel.AutoSize = true;
            _buddyNameLabel.Name = "label1";
            _buddyNameLabel.TabIndex = 2;
            _buddyNameLabel.Text = "Name des Buddies";
            _buddyNameLabel.Dock = DockStyle.Fill;
            _tableLayoutPanel.Controls.Add(_buddyNameLabel, 0, 0);
            _tableLayoutPanel.SetColumnSpan(_buddyNameLabel, 2);
            // 
            // label2
            // 
            _ipLabel.AutoSize = true;
            _ipLabel.Name = "label2";
            _ipLabel.TabIndex = 3;
            _ipLabel.Text = "IP des Buddies";
            _ipLabel.Dock = DockStyle.Fill;
            _tableLayoutPanel.Controls.Add(_ipLabel, 0, 2);
            _tableLayoutPanel.SetColumnSpan(_ipLabel, 2);
            // 
            // label2
            // 
            _portLabel.AutoSize = true;
            _portLabel.Name = "label2";
            _portLabel.TabIndex = 3;
            _portLabel.Text = "Port des Buddies";
            _portLabel.Dock = DockStyle.Fill;
            _tableLayoutPanel.Controls.Add(_portLabel, 0, 4);
            _tableLayoutPanel.SetColumnSpan(_portLabel, 2);
            // 
            // Submit
            // 
            _submitButton.Name = "Submit";
            _submitButton.TabIndex = 4;
            _submitButton.Text = "Submit";
            _submitButton.UseVisualStyleBackColor = true;
            _submitButton.Dock = DockStyle.Fill;
            _submitButton.Click += _buddyAddClick;
            _tableLayoutPanel.Controls.Add(_submitButton, 0, 6);
            // 
            // BuddyAddForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(202, 170);


            Name = "BuddyAddForm";
            Text = "Buddy hinzufügen";
            ResumeLayout(false);
            PerformLayout();
        }

        private void _buddyAddClick(object obj, EventArgs args)
        {
            if (BuddyAddSubmit != null)
            {
                BuddyAddSubmit(_buddyNameTextBox.Text, _ipTextBox.Text, Int32.Parse(_portTextBox.Text));
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
