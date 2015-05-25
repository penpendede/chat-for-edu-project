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
            this._tableLayoutPanel = new TableLayoutPanel();
            this._buddyNameTextBox = new TextBox();
            this._ipTextBox = new TextBox();
            this._buddyNameLabel = new Label();
            this._ipLabel = new Label();
            this._submitButton = new Button();
            this.SuspendLayout();

            //
            // Panel
            //

            this._tableLayoutPanel = new TableLayoutPanel();
            this._tableLayoutPanel.Dock = DockStyle.Fill;
            this._tableLayoutPanel.RowCount = 4;
            this._tableLayoutPanel.ColumnCount = 1;

            this._tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this._tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            this._tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            this._tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            this._tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));

            this.Controls.Add(this._tableLayoutPanel);

            // 
            // BuddyName
            // 
            this._buddyNameTextBox.Name = "BuddyName";
            this._buddyNameTextBox.TabIndex = 0;
            this._buddyNameTextBox.Dock = DockStyle.Fill;
            this._tableLayoutPanel.Controls.Add(this._buddyNameTextBox, 0, 1);
            // 
            // IP
            // 
            this._ipTextBox.Name = "IP";
            this._ipTextBox.TabIndex = 1;
            this._ipTextBox.Dock = DockStyle.Fill;
            this._tableLayoutPanel.Controls.Add(this._ipTextBox, 0, 3);
            // 
            // label1
            // 
            this._buddyNameLabel.AutoSize = true;
            this._buddyNameLabel.Name = "label1";
            this._buddyNameLabel.TabIndex = 2;
            this._buddyNameLabel.Text = "Name des Buddies";
            this._buddyNameLabel.Dock = DockStyle.Fill;
            this._tableLayoutPanel.Controls.Add(this._buddyNameLabel, 0, 0);
            this._tableLayoutPanel.SetColumnSpan(this._buddyNameLabel, 2);
            // 
            // label2
            // 
            this._ipLabel.AutoSize = true;
            this._ipLabel.Name = "label2";
            this._ipLabel.TabIndex = 3;
            this._ipLabel.Text = "IP des Buddies";
            this._ipLabel.Dock = DockStyle.Fill;
            this._tableLayoutPanel.Controls.Add(this._ipLabel, 0, 2);
            this._tableLayoutPanel.SetColumnSpan(this._ipLabel, 2);
            // 
            // Submit
            // 
            this._submitButton.Name = "Submit";
            this._submitButton.TabIndex = 4;
            this._submitButton.Text = "Submit";
            this._submitButton.UseVisualStyleBackColor = true;
            this._submitButton.Dock = DockStyle.Fill;
            this._submitButton.Click += (o, e) => { if (this.BuddyAddSubmit != null) { this.BuddyAddSubmit(this._buddyNameTextBox.Text, this._ipTextBox.Text); } };
            this._tableLayoutPanel.Controls.Add(this._submitButton, 0, 4);
            // 
            // BuddyAddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(202, 138);


            this.Name = "BuddyAddForm";
            this.Text = "Buddy hinzufügen";
            this.ResumeLayout(false);
            this.PerformLayout();
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
