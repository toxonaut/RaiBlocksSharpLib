using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

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
            label2.Text = API.AccountBalance(txtAccount.Text, XRBUnit.XRB).balance + " XRB";
        }

        private void btnBlockCount_Click(object sender, EventArgs e)
        {
            btnBlockCount.Text = API.AccountBlockCount(txtAccount.Text).ToString() + " Blocks";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dynamic account_info = API.Rai.AccountInformation(txtAccount.Text,XRBUnit.XRB);
            richTextBox1.Text = "--------------------------------------\n" + "frontier: " + account_info.frontier + "\n";
            richTextBox1.Text += "open_block: " + account_info.open_block + "\n";
            richTextBox1.Text += "representative_block: " + account_info.representative_block + "\n";
            richTextBox1.Text += "balance: " + account_info.balance + "\n";
            richTextBox1.Text += "modified_timestamp: " + account_info.modified_timestamp + "\n";
            richTextBox1.Text += "block_count: " + account_info.block_count + "\n";

        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + "Account: " + API.AccountCreate(txtWallet.Text);
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<History> historyList = API.AccountHistory(txtAccount.Text,1);
            richTextBox1.Text = "--------------------------------------\n" + "hash: " + historyList[0].hash + "\n";
            richTextBox1.Text += "type: " + historyList[0].type + "\n";
            richTextBox1.Text += "account: " + historyList[0].account + "\n";
            richTextBox1.Text += "amount: " + historyList[0].amount + "\n";
        }

        private void button6_Click(object sender, EventArgs e)
        {
           KeyPair kp =  API.DeterministicKey(txtSeed.Text, Convert.ToInt32(txtIndex.Text));
            richTextBox1.Text = "--------------------------------------\n" + "private: " + kp.privateKey + "\n";
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
            richTextBox1.Text = "--------------------------------------\n" + "account: " + accountsBalances[0].account + "\n";
            richTextBox1.Text += "balance: " + accountsBalances[0].balance + "\n";
            richTextBox1.Text += "pending: " + accountsBalances[0].pending + "\n";
        }

        private void btnWalletAddKey_Click(object sender, EventArgs e)
        {
            txtAccount.Text = API.WalletAddKey(txtWallet.Text, txtPrivateKey.Text, true);
            richTextBox1.Text += "--------------------------------------\n" + "account: " + txtAccount.Text;

        }

        private void btnWalletCreate_Click(object sender, EventArgs e)
        {

            txtWallet.Text = API.Rai.WalletCreate();

        }

        private void btnAccountsCreate_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + "Accounts: " + API.Rai.AccountsCreate(txtWallet.Text, Convert.ToInt16(txtNumAccounts.Text));
        }

        private void btnAvailableSupply_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + "Available: " + API.AvailableSupply(XRBUnit.XRB).ToString();
        }

        private void btnBlockAccount_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + "account: " + API.BlockAccount(txtBlock.Text);
        }

        private void btnBlockCnt_Click(object sender, EventArgs e)
        {
            var blockCount = API.Rai.BlockCount();
            richTextBox1.Text = "--------------------------------------\n" + "count: " + blockCount.count + "\n";
            richTextBox1.Text += "unchecked: " + blockCount.@unchecked + "\n";
        }
        private void btnBlockCountByType_Click(object sender, EventArgs e)
        {
            var blockCount = API.Rai.BlockCountByType();
            richTextBox1.Text = "--------------------------------------\n" + "send: " + blockCount.send + "\n";
            richTextBox1.Text += "receive: " + blockCount.receive + "\n";
            richTextBox1.Text += "open: " + blockCount.open + "\n";
            richTextBox1.Text += "change: " + blockCount.change + "\n";
        }

        private void btnChain_Click(object sender, EventArgs e)
        {
            ChainBlocks blocks = API.Chain(txtBlock.Text, Convert.ToInt32(txtBlockCount.Text));
            richTextBox1.Text = "--------------------------------------\n" + "blocks: " + blocks.blocks.Aggregate((i, j) => i + "," + j);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            List<string> hashes = new List<string>();
            hashes.Add("E71AF3E9DD86BBD8B4620EFA63E065B34D358CFC091ACB4E103B965F95783321");
            hashes.Add("C6D1A9E4F631E5BD9AF104C9A08535F7FBA69252C655F5EE52FE7B20B13894CB");


            List<Block> cont = API.RetrieveMultipleBlocksWithAdditionalInfo(hashes);
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
           /* accounts.Add("xrb_3e77ic1xarfn6mbbxthh38ibz8rr3gx7ikap6589bmcsdsmuyccd4hy8g8f5");
            accounts.Add("xrb_3d8k363kpt1kzh5n11qhmbk8esxyb83wmp69dst9ujiqcx919h4ybpgpwbrt");
            accounts.Add("xrb_3azxczhmrws5sxm4ezy1xf57tuh67crehjw1oi98ygynas71wyop1jt1p8wx");*/
            accounts.Add(txtAccount.Text);
            List<AccountBlocks> blocks = API.AccountsPending(accounts, 3);
            richTextBox1.Text = "--------------------------------------\n" + Helper.SerializeObject<List<AccountBlocks>>(blocks);
        }
        private void btnAccountsPending2_Click(object sender, EventArgs e)
        {
            List<string> accounts = new List<string>();
           /* accounts.Add("xrb_3e77ic1xarfn6mbbxthh38ibz8rr3gx7ikap6589bmcsdsmuyccd4hy8g8f5");
            accounts.Add("xrb_3d8k363kpt1kzh5n11qhmbk8esxyb83wmp69dst9ujiqcx919h4ybpgpwbrt");
            accounts.Add("xrb_3azxczhmrws5sxm4ezy1xf57tuh67crehjw1oi98ygynas71wyop1jt1p8wx");*/
            accounts.Add(txtAccount.Text);
            List<AccountBlocks> blocks = API.AccountsPending(accounts, 3, "1000000000000000000000000", XRBUnit.raw, XRBUnit.XRB);
            richTextBox1.Text = "--------------------------------------\n" + Helper.SerializeObject<List<AccountBlocks>>(blocks);
        }
        private void btnAccountsPending3_Click(object sender, EventArgs e)
        {
            List<string> accounts = new List<string>();
            /*accounts.Add("xrb_3e77ic1xarfn6mbbxthh38ibz8rr3gx7ikap6589bmcsdsmuyccd4hy8g8f5");
            accounts.Add("xrb_3d8k363kpt1kzh5n11qhmbk8esxyb83wmp69dst9ujiqcx919h4ybpgpwbrt");
            accounts.Add("xrb_3azxczhmrws5sxm4ezy1xf57tuh67crehjw1oi98ygynas71wyop1jt1p8wx");*/

            accounts.Add(txtAccount.Text);
            List<AccountBlocks> blocks = API.AccountsPending(accounts, 3, true, XRBUnit.XRB);
            richTextBox1.Text = "--------------------------------------\n" + Helper.SerializeObject<List<AccountBlocks>>(blocks);
        }


        private void btnAccountsFrontiers_Click(object sender, EventArgs e)
        {
            List<string> accounts = new List<string>();
            /*  
              accounts.Add("xrb_3e77ic1xarfn6mbbxthh38ibz8rr3gx7ikap6589bmcsdsmuyccd4hy8g8f5");
              accounts.Add("xrb_3d8k363kpt1kzh5n11qhmbk8esxyb83wmp69dst9ujiqcx919h4ybpgpwbrt");
              accounts.Add("xrb_3azxczhmrws5sxm4ezy1xf57tuh67crehjw1oi98ygynas71wyop1jt1p8wx");*/
            accounts.Add(txtAccount.Text);
            List<Frontier> fr =API.AccountsFrontiers(accounts);
            richTextBox1.Text = "--------------------------------------\n" + Helper.SerializeObject<List<Frontier>>(fr);
        }

        private void btnPending_Click(object sender, EventArgs e)
        {
            List<string> pending = API.Pending(txtAccount.Text, 1);
            richTextBox1.Text = "--------------------------------------\n" + Helper.SerializeObject<List<string>>(pending);
        }

        private void btnPending2_Click(object sender, EventArgs e)
        {
            List<AccountBlock> pending = API.Pending(txtAccount.Text, 1, "10", XRBUnit.raw, XRBUnit.XRB);
            richTextBox1.Text = "--------------------------------------\n" + Helper.SerializeObject<List<AccountBlock>>(pending);
        }

        private void btnPending3_Click(object sender, EventArgs e)
        {
            List<AccountBlock> pending = API.Pending(txtAccount.Text, 1, true, XRBUnit.XRB);
            richTextBox1.Text = "--------------------------------------\n" + Helper.SerializeObject<List<AccountBlock>>(pending);
        }

        private void btnPendingExists_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + API.PendingExists(txtBlock.Text);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + Helper.SerializeObject<List<Block>>(API.UncheckedBlocks(1)); 
        }

        private void button13_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + API.ClearUncheckedBlocks();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + Helper.SerializeObject<Block>(API.RetrieveUncheckedBlock(txtBlock.Text));
        }

        private void button16_Click_1(object sender, EventArgs e)
        {
            var balance = API.WalletTotalBalance(txtWallet.Text);
            richTextBox1.Text = "--------------------------------------\n" + "balance: " + balance.balance + " pending: " + balance.pending;
        }



        private void btnWalletExport_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + API.WalletExport(txtWallet.Text);
        }

        private void btnWalletFrontiers_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + Helper.SerializeObject<List<Frontier>>(API.WalletFrontiers(txtWallet.Text));
        }

        private void btnWalletPending_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + Helper.SerializeObject<List<AccountBlocks>>(API.WalletPending(txtWallet.Text,1));
        }

        private void btnWalletsPending2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + Helper.SerializeObject<List<AccountBlocks>>(API.WalletPending(txtWallet.Text, 1,"10", XRBUnit.raw, XRBUnit.XRB));
        }

        private void btnWalletPending3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + Helper.SerializeObject<List<AccountBlocks>>(API.WalletPending(txtWallet.Text, 1, true, XRBUnit.XRB));
        }

        private void btnWalletRepublish_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + Helper.SerializeObject<List<string>>(API.WalletRepublish(txtWallet.Text, 10));

        }

        private void btnWalletWorkGet_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + Helper.SerializeObject<List<Work>>(API.WalletWorkGet(txtWallet.Text));
        }

        private void btnWalletChangePW_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + API.WalletChangePassword(txtWallet.Text, txtPW.Text);
        }

        private void btnWalletEnterPW_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + API.WalletPasswordEnter(txtWallet.Text, txtPW.Text);
        }

        private void btnWalletValidPW_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + API.WalletValidPW(txtWallet.Text);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + API.WalletLockedCheck(txtWallet.Text);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            API.WorkCancel(txtBlock.Text);
            richTextBox1.Text = "--------------------------------------\n" + "executed" ;
        }

        private void btnWorkGenerate_Click(object sender, EventArgs e)
        {
            string work = API.WorkGenerate(txtBlock.Text);
            richTextBox1.Text = "--------------------------------------\n" + work;
            txtWork.Text = work;
        }

        private void btnWorkGet_Click(object sender, EventArgs e)
        {
            string work = API.WorkGet(txtWallet.Text, txtAccount.Text);
            richTextBox1.Text = "--------------------------------------\n" + work;
            txtWork.Text = work;
        }

        private void btnWorkSet_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + API.WorkSet(txtWallet.Text, txtAccount.Text, txtWork.Text);
        }

        private void btnAddWorkPeer_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + API.AddWorkPeer("::ffff:172.17.0.1:7076", "7076");
        }

        private void btnRetrieveWorkPeers_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + Helper.SerializeObject<List<string>>(API.RetrieveWorkPeers());
        }

        private void btnClearWorkPeers_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + API.ClearWorkPeers();
        }

        private void btnWorkValidate_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + API.WorkValidate(txtWork.Text, txtBlock.Text);
        }

        private void btnRetrieveOnlinePeers_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + Helper.SerializeObject<List<PeerVersion>>(API.RetrieveOnlinePeers());
        }

        private void btnRetrieveNodeVersions_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + Helper.SerializeObject<NodeVersions>(API.RetrieveNodeVersions());
        }

        private void btnSuccessors_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + Helper.SerializeObject<List<string>>(API.Successors(txtBlock.Text, Convert.ToInt16(txtSuccessors.Text)));
        }

        private void btnValAccNoChksum_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + API.ValidateAccountNumberChecksum(txtAccount.Text).ToString();
        }

        private void button16_Click_2(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + API.StopNode().ToString();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "--------------------------------------\n" + API.Send(txtWallet.Text, txtAccount.Text, txtSendTo.Text, txtSendAmount.Text, XRBUnit.XRB);
        }

        private void ConvertUnit_Click(object sender, EventArgs e)
        {
            API.Rai.UnitConvert("1.123456789012345", XRBUnit.prai, XRBUnit.prai);
        }

        private void txtSuccessors_TextChanged(object sender, EventArgs e)
        {

        }
    }

    public static class Helper
    {
        public static string SerializeObject<T>(this T toSerialize)
        {
            if (toSerialize == null)
                return "";
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }
    }
}
