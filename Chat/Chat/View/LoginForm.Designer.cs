using System;
namespace Chat
{
    public delegate void OnLoginSubmit(string userName, string password);

    [System.ComponentModel.DesignerCategory("")]
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.userNameComboBox = new System.Windows.Forms.ComboBox();
            this.userNameLabel = new System.Windows.Forms.Label();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.submitButton = new System.Windows.Forms.Button();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();

            // 
            // UserName
            // 
            this.userNameComboBox.FormattingEnabled = true;
            this.userNameComboBox.Location = new System.Drawing.Point(12, 25);
            this.userNameComboBox.Name = "UserName";
            this.userNameComboBox.Size = new System.Drawing.Size(178, 21);
            this.userNameComboBox.TabIndex = 0;
            // 
            // label1
            // 
            this.userNameLabel.AutoSize = true;
            this.userNameLabel.Location = new System.Drawing.Point(9, 9);
            this.userNameLabel.Name = "label1";
            this.userNameLabel.Size = new System.Drawing.Size(75, 13);
            this.userNameLabel.TabIndex = 2;
            this.userNameLabel.Text = "Benutzername";
            //this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(9, 62);
            this.passwordLabel.Name = "label2";
            this.passwordLabel.Size = new System.Drawing.Size(50, 13);
            this.passwordLabel.TabIndex = 3;
            this.passwordLabel.Text = "Passwort";
            // 
            // Submit
            // 
            this.submitButton.Location = new System.Drawing.Point(126, 104);
            this.submitButton.Name = "Submit";
            this.submitButton.Size = new System.Drawing.Size(64, 21);
            this.submitButton.TabIndex = 4;
            this.submitButton.Text = "Login";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += onSubmitButtonClick;
            // 
            // textBox1
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(13, 79);
            this.passwordTextBox.Name = "textBox1";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(177, 20);
            this.passwordTextBox.TabIndex = 5;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(202, 138);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.userNameLabel);
            this.Controls.Add(this.userNameComboBox);
            this.Name = "LoginForm";
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox userNameComboBox;
        private System.Windows.Forms.Label userNameLabel;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.TextBox passwordTextBox;

        public OnLoginSubmit OnLoginSubmit;

        private void onSubmitButtonClick(object obj, EventArgs args)
        {
            if (OnLoginSubmit != null)
            {
                this.OnLoginSubmit(this.userNameComboBox.Text, this.passwordTextBox.Text);
            }
        }
    }
}