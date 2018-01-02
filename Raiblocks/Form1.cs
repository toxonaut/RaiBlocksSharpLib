using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
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
            label2.Text = API.AccountBalance(txtAccount.Text).balance + " XRB";
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

        private void btnAvailableSupply_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "Available: " + API.AvailableSupply(XRBUnit.XRB).ToString();
        }

        private void btnBlockAccount_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "account: " + API.BlockAccount(txtBlock.Text);
        }

        private void btnBlockCnt_Click(object sender, EventArgs e)
        {
            var blockCount = API.Rai.BlockCount();
            richTextBox1.Text = "count: " + blockCount.count + "\n";
            richTextBox1.Text += "unchecked: " + blockCount.@unchecked + "\n";
        }
        private void btnBlockCountByType_Click(object sender, EventArgs e)
        {
            var blockCount = API.Rai.BlockCountByType();
            richTextBox1.Text = "send: " + blockCount.send + "\n";
            richTextBox1.Text += "receive: " + blockCount.receive + "\n";
            richTextBox1.Text += "open: " + blockCount.open + "\n";
            richTextBox1.Text += "change: " + blockCount.change + "\n";
        }

        private void btnChain_Click(object sender, EventArgs e)
        {
            ChainBlocks blocks = API.Chain(txtBlock.Text, Convert.ToInt32(txtBlockCount.Text));
            richTextBox1.Text = "blocks: " + blocks.blocks.Aggregate((i, j) => i + "," + j);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            List<string> hashes = new List<string>();
            hashes.Add("E71AF3E9DD86BBD8B4620EFA63E065B34D358CFC091ACB4E103B965F95783321");
            hashes.Add("C6D1A9E4F631E5BD9AF104C9A08535F7FBA69252C655F5EE52FE7B20B13894CB");


            BlocksAdditionalContainer cont = API.RetrieveMultipleBlocksWithAdditionalInfo(hashes);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            List<string> hashes = new List<string>();
            hashes.Add("E71AF3E9DD86BBD8B4620EFA63E065B34D358CFC091ACB4E103B965F95783321");
            hashes.Add("C6D1A9E4F631E5BD9AF104C9A08535F7FBA69252C655F5EE52FE7B20B13894CB");


            List<Block> cont = API.RetrieveMultipleBlocks(hashes);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Block block = API.RetrieveBlock(txtBlock.Text);
        }

        private void btnAccountsPending_Click(object sender, EventArgs e)
        {
            List<string> accounts = new List<string>();
            accounts.Add("xrb_3e77ic1xarfn6mbbxthh38ibz8rr3gx7ikap6589bmcsdsmuyccd4hy8g8f5");
            accounts.Add("xrb_3d8k363kpt1kzh5n11qhmbk8esxyb83wmp69dst9ujiqcx919h4ybpgpwbrt");
            accounts.Add("xrb_3azxczhmrws5sxm4ezy1xf57tuh67crehjw1oi98ygynas71wyop1jt1p8wx");


            List<AccountBlocks> blocks = API.AccountsPending(accounts, 3, "1000000000000000000000000",XRBUnit.raw, XRBUnit.XRB);

        }

        private void button11_Click(object sender, EventArgs e)
        {
            API.Rai.UnitConvert("1.123456789012345", XRBUnit.prai, XRBUnit.prai);

          //  API.Rai.UnitConvert2("10.12", XRBUnit.prai, XRBUnit.prai);

            
         
        }
    }
}
