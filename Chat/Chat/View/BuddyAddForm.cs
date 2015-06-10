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
    public delegate void OnBuddyAddSubmit(string name, string IP);

    public class BuddyAddForm : Form
    {
        public OnBuddyAddSubmit BuddyAddSubmit;

        private TableLayoutPanel _tableLayoutPanel;
        private TextBox _buddyNameTextBox;
        private TextBox _ipTextBox;
        private Label _buddyNameLabel;
        private Label _ipLabel;
        private Button _submitButton;

        public BuddyAddForm()
        {
            _tableLayoutPanel = new TableLayoutPanel();
            _buddyNameTextBox = new TextBox();
            _ipTextBox = new TextBox();
            _buddyNameLabel = new Label();
            _ipLabel = new Label();
            _submitButton = new Button();
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
            // Submit
            // 
            _submitButton.Name = "Submit";
            _submitButton.TabIndex = 4;
            _submitButton.Text = "Submit";
            _submitButton.UseVisualStyleBackColor = true;
            _submitButton.Dock = DockStyle.Fill;
            _submitButton.Click += (o, e) => { if (BuddyAddSubmit != null) { BuddyAddSubmit(_buddyNameTextBox.Text, _ipTextBox.Text); } };
            _tableLayoutPanel.Controls.Add(_submitButton, 0, 4);
            // 
            // BuddyAddForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(202, 138);


            Name = "BuddyAddForm";
            Text = "Buddy hinzufügen";
            ResumeLayout(false);
            PerformLayout();
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
