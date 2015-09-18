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
    //public delegate void OnNewUser(string userName, string password);
    public delegate void LoginFormOnNewUser();

    /// <summary>
    ///  Login form - a form
    /// </summary>
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
        public LoginFormOnNewUser NewUser;

        /// <summary>
        /// Make the login form
        /// </summary>
        /// <param name="knownUserNames">the list of known users</param>
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
            this.userNameComboBox.FormattingEnabled = true;
            this.userNameComboBox.Name = "UserName";
            this.userNameComboBox.TabIndex = 0;
            this.userNameComboBox.Dock = DockStyle.Fill;
            this.userNameComboBox.Items.AddRange(knownUserNames.ToArray());
            this.userNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            // 
            // label2
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Name = "label2";
            this.passwordLabel.Text = "Passwort";
            this.passwordLabel.Dock = DockStyle.Fill;
            this.passwordLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox1
            // 
            this.passwordTextBox.Name = "textBox1";
            this.passwordTextBox.PasswordChar = '\u25A0';
            this.passwordTextBox.TabIndex = 1;
            this.passwordTextBox.Dock = DockStyle.Fill;
            // 
            // New User
            // 
            this.newUserButton.Name = "New User";
            this.newUserButton.TabIndex = 3;
            this.newUserButton.Text = "Neuer Benutzer";
            this.newUserButton.UseVisualStyleBackColor = true;
            this.newUserButton.Click += onNewUserButtonClick;
            //this.newUserButton.Dock = DockStyle.Right;
            this.newUserButton.Anchor = AnchorStyles.Left;
            this.newUserButton.Width = 90;
            // 
            // Submit
            // 
            this.loginButton.Name = "Submit";
            this.loginButton.TabIndex = 2;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += onSubmitButtonClick;
            //this.loginButton.Dock = DockStyle.Right;
            this.loginButton.Anchor = AnchorStyles.Right;
            this.loginButton.Width = 90;
            //this.

            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(190, 138);

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

        /// <summary>
        /// Display "user unknown" message box
        /// </summary>
        /// <param name="userName">unkown user</param>
        public void UsernameUnknownMessage(string userName)
        {
            MessageBox.Show(string.Format("Der Username {0} ist nicht bekannt.", userName), "Username unbekannt", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Display "wrong password" message box
        /// </summary>
        /// <param name="userName">name of user for who the wrong password was provided</param>
        public void WrongPasswordMessage(string userName)
        {
            MessageBox.Show(string.Format("Der Passwort für User {0} ist falsch.", userName), "Passwort falsch", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Handler for "new user button was clicked"
        /// </summary>
        /// <param name="obj">ignored</param>
        /// <param name="args">ignored</param>
        private void onNewUserButtonClick(object obj, EventArgs args)
        {
            this.NewUser();
        }

        /// <summary>
        ///  Handler for "submit button was clicked"
        /// </summary>
        /// <param name="obj">ignored</param>
        /// <param name="args">ignored</param>
        private void onSubmitButtonClick(object obj, EventArgs args)
        {
            if (LoginSubmit != null)
            {
                this.LoginSubmit(this.userNameComboBox.Text, this.passwordTextBox.Text);
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

