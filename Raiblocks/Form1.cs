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
        RaiAPI raiAPI;
        public Form1()
        {
            InitializeComponent();
            raiAPI = new RaiAPI("http://localhost:7076");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label2.Text = Math.Round(raiAPI.AccountBalance(txtAccount.Text).balance,4).ToString()+ " XRB";
        }

        private void btnBlockCount_Click(object sender, EventArgs e)
        {
            btnBlockCount.Text = raiAPI.AccountBlockCount(txtAccount.Text).ToString() + " Blocks";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dynamic account_info = raiAPI.Rai.AccountInformation(txtAccount.Text,XRBUnit.XRB);
            richTextBox1.Text = "frontier: " + account_info.frontier + "\n";
            richTextBox1.Text += "open_block: " + account_info.open_block + "\n";
            richTextBox1.Text += "representative_block: " + account_info.representative_block + "\n";
            richTextBox1.Text += "balance: " + account_info.balance + "\n";
            richTextBox1.Text += "modified_timestamp: " + account_info.modified_timestamp + "\n";
            richTextBox1.Text += "block_count: " + account_info.block_count + "\n";

        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "Account: " + raiAPI.AccountCreate(txtWallet.Text);
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<History> historyList = raiAPI.AccountHistory(txtAccount.Text,1);
            richTextBox1.Text = "hash: " + historyList[0].hash + "\n";
            richTextBox1.Text += "type: " + historyList[0].type + "\n";
            richTextBox1.Text += "account: " + historyList[0].account + "\n";
            richTextBox1.Text += "amount: " + historyList[0].amount + "\n";
        }

        private void button6_Click(object sender, EventArgs e)
        {
           KeyPair kp =  raiAPI.DeterministicKey(txtSeed.Text, Convert.ToInt32(txtIndex.Text));
            richTextBox1.Text = "private: " + kp.privateKey + "\n";
            richTextBox1.Text += "public: " + kp.publicKey + "\n";
            richTextBox1.Text += "account: " + kp.account + "\n";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "account: " + raiAPI.AccountGet(txtWallet.Text);
        }
    }
}
