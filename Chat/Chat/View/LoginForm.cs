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
    public delegate void OnLoginSubmit(string userName, string password);
    public delegate void OnNewUser(string userName, string password);

    public class LoginForm : Form
    {
        private TableLayoutPanel tableLayoutPanel;
        private ComboBox userNameComboBox;
        private Label userNameLabel;
        private Label passwordLabel;
        private Button loginButton;
        private TextBox passwordTextBox;
        private Button newUserButton;

        public OnLoginSubmit LoginSubmit;
        public OnNewUser NewUser;

        public LoginForm(List<string> knownUserNames)
        {
            this.tableLayoutPanel = new TableLayoutPanel();
            this.userNameComboBox = new ComboBox();
            this.userNameLabel = new Label();
            this.passwordLabel = new Label();
            this.loginButton = new Button();
            this.newUserButton = new Button();
            this.passwordTextBox = new TextBox();
            this.SuspendLayout();

            //
            // TableLayoutPanel
            //
            this.tableLayoutPanel.Dock = DockStyle.Fill;
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.ColumnCount = 2;

            this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));

            this.Controls.Add(this.tableLayoutPanel);

            // 
            // label1
            // 
            this.userNameLabel.AutoSize = true;
            this.userNameLabel.Name = "label1";
            this.userNameLabel.TabIndex = 2;
            this.userNameLabel.Text = "Benutzername";
            this.userNameLabel.Dock = DockStyle.Fill;
            //this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // UserName
            // 
            this.userNameComboBox.FormattingEnabled = true;
            this.userNameComboBox.Name = "UserName";
            this.userNameComboBox.TabIndex = 0;
            this.userNameComboBox.Dock = DockStyle.Fill;
            this.userNameComboBox.Items.AddRange(knownUserNames.ToArray());
            // 
            // label2
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Name = "label2";
            this.passwordLabel.TabIndex = 3;
            this.passwordLabel.Text = "Passwort";
            this.passwordLabel.Dock = DockStyle.Fill;
            // 
            // textBox1
            // 
            this.passwordTextBox.Name = "textBox1";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.TabIndex = 5;
            this.passwordTextBox.Dock = DockStyle.Fill;
            // 
            // New User
            // 
            this.newUserButton.Name = "New User";
            this.newUserButton.TabIndex = 4;
            this.newUserButton.Text = "Neuer Benutzer";
            this.newUserButton.UseVisualStyleBackColor = true;
            this.newUserButton.Click += onNewUserButtonClick;
            //this.newUserButton.Dock = DockStyle.Right;
            this.newUserButton.Anchor = AnchorStyles.Right;
            // 
            // Submit
            // 
            this.loginButton.Name = "Submit";
            this.loginButton.TabIndex = 4;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += onSubmitButtonClick;
            //this.loginButton.Dock = DockStyle.Right;
            this.loginButton.Anchor = AnchorStyles.Right;
            
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(202, 138);

            this.tableLayoutPanel.Controls.Add(this.userNameLabel, 0, 0);
            this.tableLayoutPanel.SetColumnSpan(this.userNameLabel, 2);
            
            this.tableLayoutPanel.Controls.Add(this.userNameComboBox, 0, 1);
            this.tableLayoutPanel.SetColumnSpan(this.userNameComboBox, 2);
            
            this.tableLayoutPanel.Controls.Add(this.passwordLabel, 0, 2);
            this.tableLayoutPanel.SetColumnSpan(this.passwordLabel, 2);
            
            this.tableLayoutPanel.Controls.Add(this.passwordTextBox, 0, 3);
            this.tableLayoutPanel.SetColumnSpan(this.passwordTextBox, 2);

            this.tableLayoutPanel.Controls.Add(this.newUserButton, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.loginButton, 1, 4);

            this.Name = "LoginForm";
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void onNewUserButtonClick(object obj, EventArgs args)
        {
            if (NewUser != null)
            {
                this.NewUser(this.userNameComboBox.Text, this.passwordTextBox.Text);
            }
        }

        private void onSubmitButtonClick(object obj, EventArgs args)
        {
            if (LoginSubmit != null)
            {
                this.LoginSubmit(this.userNameComboBox.Text, this.passwordTextBox.Text);
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
