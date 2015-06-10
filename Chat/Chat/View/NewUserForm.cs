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
    public delegate void OnNewUserSubmit(string userName, string password);
    public delegate void NewUserFormOnNewUser(string userName, string password, string passwordRepetition);

    public class NewUserForm : Form
    {
        private TableLayoutPanel tableLayoutPanel;
        private TextBox userNameTextBox;
        private Label userNameLabel;
        private Label passwordLabel1;
        private Label passwordLabel2;
        private TextBox passwordTextBox1;
        private TextBox passwordTextBox2;
        private Button newUserButton;

        public NewUserFormOnNewUser NewUser;

        public NewUserForm(List<string> knownUserNames)
        {
            this.tableLayoutPanel = new TableLayoutPanel();
            this.userNameTextBox = new TextBox();
            this.userNameLabel = new Label();
            this.passwordLabel1 = new Label();
            this.passwordLabel2 = new Label();
            this.newUserButton = new Button();
            this.passwordTextBox1 = new TextBox();
            this.passwordTextBox2 = new TextBox();
            this.SuspendLayout();

            //
            // TableLayoutPanel
            //
            this.tableLayoutPanel.Dock = DockStyle.Fill;
            this.tableLayoutPanel.RowCount = 7;
            this.tableLayoutPanel.ColumnCount = 1;

            this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));

            this.Controls.Add(this.tableLayoutPanel);

            // 
            // label1
            // 
            this.userNameLabel.AutoSize = true;
            this.userNameLabel.Name = "label1";
            this.userNameLabel.Text = "Benutzername";
            this.userNameLabel.Dock = DockStyle.Fill;
            this.userNameLabel.TextAlign = ContentAlignment.MiddleCenter;
            //this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // UserName
            // 
            this.userNameTextBox.Name = "UserName";
            this.userNameTextBox.TabIndex = 0;
            this.userNameTextBox.Dock = DockStyle.Fill;
            // 
            // label2
            // 
            this.passwordLabel1.AutoSize = true;
            this.passwordLabel1.Name = "label2";
            this.passwordLabel1.Text = "Kennwort";
            this.passwordLabel1.Dock = DockStyle.Fill;
            this.passwordLabel1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox1
            // 
            this.passwordTextBox1.Name = "textBox1";
            this.passwordTextBox1.PasswordChar = '\u25A0';
            this.passwordTextBox1.TabIndex = 1;
            this.passwordTextBox1.Dock = DockStyle.Fill;
            // 
            // label2
            // 
            this.passwordLabel2.AutoSize = true;
            this.passwordLabel2.Name = "label2";
            this.passwordLabel2.Text = "Kennwortwiederholung";
            this.passwordLabel2.Dock = DockStyle.Fill;
            this.passwordLabel2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox1
            // 
            this.passwordTextBox2.Name = "textBox2";
            this.passwordTextBox2.PasswordChar = '\u25A0';
            this.passwordTextBox2.TabIndex = 1;
            this.passwordTextBox2.Dock = DockStyle.Fill;
            // 
            // New User
            // 
            this.newUserButton.Name = "New User";
            this.newUserButton.TabIndex = 3;
            this.newUserButton.Text = "Neuen Benutzer einrichten";
            this.newUserButton.UseVisualStyleBackColor = true;
            this.newUserButton.Click += onNewUserButtonClick;
            //this.newUserButton.Dock = DockStyle.Right;
            this.newUserButton.Anchor = AnchorStyles.Left;
            this.newUserButton.AutoSize = true;
            
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(150, 200);

            this.tableLayoutPanel.Controls.Add(this.userNameLabel, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.userNameTextBox, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.passwordLabel1, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.passwordTextBox1, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.passwordLabel2, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.passwordTextBox2, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.newUserButton, 0, 6);

            this.Name = "NewUserForm";
            this.Text = "Neuer Benutzer";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        public void UsernameIsMissingMessage()
        {
            MessageBox.Show("Benutzername fehlt", "Benutzername fehlt", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void PasswordIsMissingMessage()
        {
            MessageBox.Show("Kennwort fehlt", "Kennwort fehlt", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void PasswordRepeatIsMissingMessage()
        {
            MessageBox.Show("Kennwortwiederholung fehlt", "Kennwortwiederholung fehlt", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public void PasswordsMismatchMessage()
        {
            MessageBox.Show("Kennwort und Wiederholung sind verschieden", "Kennwort und Wiederholung verschieden", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void UsernameExistsMessage(string userName)
        {
            MessageBox.Show(string.Format("Der Benutzername {0} existiert bereits.", userName), "Benutzername existiert bereits", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void onNewUserButtonClick(object obj, EventArgs args)
        {
            if (NewUser != null)
            {
                NewUser(this.userNameTextBox.Text, this.passwordTextBox1.Text, this.passwordTextBox2.Text);
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
