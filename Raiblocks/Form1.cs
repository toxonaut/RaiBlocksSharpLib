using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Raiblocks
{
    public partial class Form1 : Form
    {
        RaiAPI API;
        public Form1()
        {
            InitializeComponent();
            API = new RaiAPI("http://localhost:7076");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label2.Text = Math.Round(API.AccountBalance(txtAccount.Text).balance,4).ToString()+ " XRB";
        }

        private void btnBlockCount_Click(object sender, EventArgs e)
        {
            btnBlockCount.Text = API.AccountBlockCount(txtAccount.Text).ToString() + " Blocks";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dynamic account_info = API.Rai.AccountInformation(txtAccount.Text,XRBUnit.XRB);
            richTextBox1.Text = "frontier: " + account_info.frontier + "\n";
            richTextBox1.Text += "open_block: " + account_info.open_block + "\n";
            richTextBox1.Text += "representative_block: " + account_info.representative_block + "\n";
            richTextBox1.Text += "balance: " + account_info.balance + "\n";
            richTextBox1.Text += "modified_timestamp: " + account_info.modified_timestamp + "\n";
            richTextBox1.Text += "block_count: " + account_info.block_count + "\n";

        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "Account: " + API.AccountCreate(txtWallet.Text);
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<History> historyList = API.AccountHistory(txtAccount.Text,1);
            richTextBox1.Text = "hash: " + historyList[0].hash + "\n";
            richTextBox1.Text += "type: " + historyList[0].type + "\n";
            richTextBox1.Text += "account: " + historyList[0].account + "\n";
            richTextBox1.Text += "amount: " + historyList[0].amount + "\n";
        }

        private void button6_Click(object sender, EventArgs e)
        {
           KeyPair kp =  API.DeterministicKey(txtSeed.Text, Convert.ToInt32(txtIndex.Text));
            richTextBox1.Text = "private: " + kp.privateKey + "\n";
            richTextBox1.Text += "public: " + kp.publicKey + "\n";
            richTextBox1.Text += "account: " + kp.account + "\n";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "account: " + API.AccountGet(txtWallet.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            List<string> accounts = new List<string>();
            accounts.Add(txtAccount.Text);
            List<AccountBalance> accountsBalances= API.AccountsBalances(accounts);
            richTextBox1.Text = "account: " + accountsBalances[0].account + "\n";
            richTextBox1.Text += "balance: " + accountsBalances[0].balance + "\n";
            richTextBox1.Text += "pending: " + accountsBalances[0].pending + "\n";
        }

        private void btnWalletAddKey_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "account: " + API.Rai.WalletAddKey(txtWallet.Text, txtPrivateKey.Text, true);
        }

        private void btnWalletCreate_Click(object sender, EventArgs e)
        {

            txtWallet.Text = API.Rai.WalletCreate();

        }

        private void btnAccountsCreate_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "Accounts: " + API.Rai.AccountsCreate(txtWallet.Text, Convert.ToInt16(txtNumAccounts.Text));
        }
    }
}
