namespace Raiblocks
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.txtAccount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBlockCount = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.txtWallet = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.txtSeed = new System.Windows.Forms.TextBox();
            this.txtIndex = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.btnWalletCreate = new System.Windows.Forms.Button();
            this.txtPrivateKey = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnWalletAddKey = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btnWalletDestroy = new System.Windows.Forms.Button();
            this.btnAccountsCreate = new System.Windows.Forms.Button();
            this.txtNumAccounts = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(593, 24);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Balance";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtAccount
            // 
            this.txtAccount.Location = new System.Drawing.Point(109, 26);
            this.txtAccount.Margin = new System.Windows.Forms.Padding(2);
            this.txtAccount.Name = "txtAccount";
            this.txtAccount.Size = new System.Drawing.Size(461, 20);
            this.txtAccount.TabIndex = 1;
            this.txtAccount.Text = "xrb_3qtrcf8q11u3g33keqk6cnjjsa9zrp1dguzuqeu5f4fw4x8fzaze9gugrw4m";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Account";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(683, 29);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "0 XRB";
            // 
            // btnBlockCount
            // 
            this.btnBlockCount.Location = new System.Drawing.Point(742, 29);
            this.btnBlockCount.Name = "btnBlockCount";
            this.btnBlockCount.Size = new System.Drawing.Size(128, 23);
            this.btnBlockCount.TabIndex = 4;
            this.btnBlockCount.Text = "Account Block Count";
            this.btnBlockCount.UseVisualStyleBackColor = true;
            this.btnBlockCount.Click += new System.EventHandler(this.btnBlockCount_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(742, 71);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(128, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Account Information";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(11, 64);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(559, 190);
            this.richTextBox1.TabIndex = 6;
            this.richTextBox1.Text = "";
            // 
            // txtWallet
            // 
            this.txtWallet.Location = new System.Drawing.Point(80, 269);
            this.txtWallet.Name = "txtWallet";
            this.txtWallet.Size = new System.Drawing.Size(490, 20);
            this.txtWallet.TabIndex = 7;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(742, 112);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(128, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "Account Create";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(742, 153);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(128, 23);
            this.button4.TabIndex = 9;
            this.button4.Text = "Account Get";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(742, 193);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(128, 23);
            this.button5.TabIndex = 10;
            this.button5.Text = "Account History";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(593, 309);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(103, 23);
            this.button6.TabIndex = 11;
            this.button6.Text = "Deterministic Key";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // txtSeed
            // 
            this.txtSeed.Location = new System.Drawing.Point(80, 312);
            this.txtSeed.Name = "txtSeed";
            this.txtSeed.Size = new System.Drawing.Size(409, 20);
            this.txtSeed.TabIndex = 12;
            this.txtSeed.Text = "0000000100000000000000000000000000000000000000000000000000000000";
            // 
            // txtIndex
            // 
            this.txtIndex.Location = new System.Drawing.Point(534, 311);
            this.txtIndex.Name = "txtIndex";
            this.txtIndex.Size = new System.Drawing.Size(35, 20);
            this.txtIndex.TabIndex = 13;
            this.txtIndex.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 314);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Seed";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(495, 314);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "index";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(742, 234);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(128, 23);
            this.button7.TabIndex = 16;
            this.button7.Text = "Accounts Balances";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // btnWalletCreate
            // 
            this.btnWalletCreate.Location = new System.Drawing.Point(593, 269);
            this.btnWalletCreate.Name = "btnWalletCreate";
            this.btnWalletCreate.Size = new System.Drawing.Size(103, 23);
            this.btnWalletCreate.TabIndex = 17;
            this.btnWalletCreate.Text = "Wallet create";
            this.btnWalletCreate.UseVisualStyleBackColor = true;
            this.btnWalletCreate.Click += new System.EventHandler(this.btnWalletCreate_Click);
            // 
            // txtPrivateKey
            // 
            this.txtPrivateKey.Location = new System.Drawing.Point(80, 355);
            this.txtPrivateKey.Name = "txtPrivateKey";
            this.txtPrivateKey.Size = new System.Drawing.Size(409, 20);
            this.txtPrivateKey.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 355);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Private key";
            // 
            // btnWalletAddKey
            // 
            this.btnWalletAddKey.Location = new System.Drawing.Point(593, 352);
            this.btnWalletAddKey.Name = "btnWalletAddKey";
            this.btnWalletAddKey.Size = new System.Drawing.Size(103, 23);
            this.btnWalletAddKey.TabIndex = 20;
            this.btnWalletAddKey.Text = "Wallet add key";
            this.btnWalletAddKey.UseVisualStyleBackColor = true;
            this.btnWalletAddKey.Click += new System.EventHandler(this.btnWalletAddKey_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 275);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Wallet";
            // 
            // btnWalletDestroy
            // 
            this.btnWalletDestroy.Location = new System.Drawing.Point(742, 270);
            this.btnWalletDestroy.Name = "btnWalletDestroy";
            this.btnWalletDestroy.Size = new System.Drawing.Size(128, 23);
            this.btnWalletDestroy.TabIndex = 22;
            this.btnWalletDestroy.Text = "Wallet destroy";
            this.btnWalletDestroy.UseVisualStyleBackColor = true;
            // 
            // btnAccountsCreate
            // 
            this.btnAccountsCreate.Location = new System.Drawing.Point(913, 112);
            this.btnAccountsCreate.Name = "btnAccountsCreate";
            this.btnAccountsCreate.Size = new System.Drawing.Size(100, 23);
            this.btnAccountsCreate.TabIndex = 23;
            this.btnAccountsCreate.Text = "Accounts Create";
            this.btnAccountsCreate.UseVisualStyleBackColor = true;
            this.btnAccountsCreate.Click += new System.EventHandler(this.btnAccountsCreate_Click);
            // 
            // txtNumAccounts
            // 
            this.txtNumAccounts.Location = new System.Drawing.Point(1020, 112);
            this.txtNumAccounts.Name = "txtNumAccounts";
            this.txtNumAccounts.Size = new System.Drawing.Size(39, 20);
            this.txtNumAccounts.TabIndex = 24;
            this.txtNumAccounts.Text = "2";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1172, 509);
            this.Controls.Add(this.txtNumAccounts);
            this.Controls.Add(this.btnAccountsCreate);
            this.Controls.Add(this.btnWalletDestroy);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnWalletAddKey);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtPrivateKey);
            this.Controls.Add(this.btnWalletCreate);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtIndex);
            this.Controls.Add(this.txtSeed);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.txtWallet);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnBlockCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtAccount);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtAccount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBlockCount;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox txtWallet;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox txtSeed;
        private System.Windows.Forms.TextBox txtIndex;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button btnWalletCreate;
        private System.Windows.Forms.TextBox txtPrivateKey;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnWalletAddKey;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnWalletDestroy;
        private System.Windows.Forms.Button btnAccountsCreate;
        private System.Windows.Forms.TextBox txtNumAccounts;
    }
}

