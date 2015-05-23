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

        private TableLayoutPanel tableLayoutPanel;
        private TextBox buddyName;
        private TextBox IP;
        private Label label1;
        private Label label2;
        private Button submit;

        public BuddyAddForm()
        {
            this.tableLayoutPanel = new TableLayoutPanel();
            this.buddyName = new TextBox();
            this.IP = new TextBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.submit = new Button();
            this.SuspendLayout();

            //
            // Panel
            //

            this.tableLayoutPanel = new TableLayoutPanel();
            this.tableLayoutPanel.Dock = DockStyle.Fill;
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.ColumnCount = 1;

            this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));

            this.Controls.Add(this.tableLayoutPanel);

            // 
            // BuddyName
            // 
            this.buddyName.Name = "BuddyName";
            this.buddyName.TabIndex = 0;
            this.buddyName.Dock = DockStyle.Fill;
            this.tableLayoutPanel.Controls.Add(this.buddyName, 0, 1);
            // 
            // IP
            // 
            this.IP.Name = "IP";
            this.IP.TabIndex = 1;
            this.IP.Dock = DockStyle.Fill;
            this.tableLayoutPanel.Controls.Add(this.IP, 0, 3);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Name = "label1";
            this.label1.TabIndex = 2;
            this.label1.Text = "Name des Buddies";
            this.label1.Dock = DockStyle.Fill;
            this.tableLayoutPanel.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel.SetColumnSpan(this.label1, 2);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Name = "label2";
            this.label2.TabIndex = 3;
            this.label2.Text = "IP des Buddies";
            this.label2.Dock = DockStyle.Fill;
            this.tableLayoutPanel.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel.SetColumnSpan(this.label2, 2);
            // 
            // Submit
            // 
            this.submit.Name = "Submit";
            this.submit.TabIndex = 4;
            this.submit.Text = "Submit";
            this.submit.UseVisualStyleBackColor = true;
            this.submit.Dock = DockStyle.Fill;
            this.submit.Click += (o, e) => { if (this.BuddyAddSubmit != null) { this.BuddyAddSubmit(this.buddyName.Text, this.IP.Text); } };
            this.tableLayoutPanel.Controls.Add(this.submit, 0, 4);
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
