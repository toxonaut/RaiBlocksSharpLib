using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raiblocks
{
    public class RaiAPI
    {

        public RaiAPI(string url_base)
        {
            this.Rai = new Rai(url_base);
        }
        public Rai Rai
        {
            get; set;
        }
        public AccountBalance AccountBalance(string account, XRBUnit unit)
        {
            dynamic accountBalance = Rai.AccountBalance(account, unit);
            AccountBalance ab = new AccountBalance() { account = account, balance = accountBalance.balance=="" ? "0": accountBalance.balance, pending = accountBalance.pending };
            return ab;
        }
        public Int32 AccountBlockCount(string account)
        {
            dynamic accountBlockCount = Rai.AccountBlockCount(account);
            Int32 block_count = Convert.ToInt32(accountBlockCount.block_count);
            return block_count;
        }
        public string AccountCreate(string wallet)
        {
            dynamic accountCreate = Rai.AccountCreate(wallet);
            string account = accountCreate.account.ToString();
            return account;
        }
        public string AccountGet(string key)
        {
            dynamic accountGet = Rai.AccountCreate(key);
            string account = accountGet.account.ToString();
            return account;
        }
        public List<History> AccountHistory(string account, int count, XRBUnit unit = XRBUnit.XRB)
        {
            List<History> historyList = new List<History>();
            dynamic accountHistory = Rai.AccountHistory(account, count);
            dynamic history = accountHistory.history;
            for (int i = 0; i < history.Count; i++)
            {
                History hist = new History() { account = account, amount = Rai.UnitConvert(history[i].amount.ToString(), XRBUnit.raw, unit), hash = history[i].hash, type = history[i].type };
                historyList.Add(hist);
            }
            return historyList;
        }
        public List<string> AccountList(string account)
        {
            List<string> accountList = new List<string>();
            dynamic accountsList = Rai.AccountList(account);
            dynamic accounts = accountsList.accounts;
            for (int i = 0; i < accounts.Count; i++)
            {
                accountList.Add(accounts[i]);
            }
            return accountList;
        }


        public KeyPair DeterministicKey(string seed, int index)
        {
            KeyPair keyPair = Rai.DeterministicKey(seed, index);
            return keyPair;
        }

        public List<AccountBalance> AccountsBalances(List<string> accounts, XRBUnit unit = XRBUnit.XRB)
        {

            dynamic item = Rai.AccountsBalances(accounts);
            return AccountsBalancesBase(item, unit);
        }

        private List<AccountBalance> AccountsBalancesBase(dynamic item, XRBUnit unit)
        {
            IDictionary<string, object> propertyValues = item.balances;
            List<AccountBalance> accountBalances = new List<AccountBalance>();
            foreach (var property in propertyValues.Keys)
            {
                AccountBalance ab = new AccountBalance();

                ab.account = property;
                IDictionary<string, object> propertyValues2 = ((IDictionary<string, object>)propertyValues[property]);
                foreach (var property2 in propertyValues2.Keys)
                {
                    if (property2 == "balance")
                        ab.balance = Rai.UnitConvert(propertyValues2[property2].ToString(), XRBUnit.raw, unit);

                    if (property2 == "pending")
                        ab.pending = Rai.UnitConvert(propertyValues2[property2].ToString(), XRBUnit.raw, unit);
                }
                accountBalances.Add(ab);
            }
            return accountBalances;
        }

        public List<Frontier> AccountsFrontiers(List<string> accounts)
        {
            return Rai.AccountsFrontiers(accounts);
        }
        public List<AccountBlocks> AccountsPending(List<string> accounts, int count)
        {
            return Rai.AccountsPending(accounts, count);
        }
        public List<AccountBlocks> AccountsPending(List<string> accounts, int count, string threshold, XRBUnit thresholdUnit = XRBUnit.raw, XRBUnit displayUnit = XRBUnit.XRB)
        {
            return Rai.AccountsPending(accounts, count, threshold, thresholdUnit, displayUnit);
        }
        public List<AccountBlocks> AccountsPending(List<string> accounts, int count, bool source, XRBUnit displayUnit = XRBUnit.XRB)
        {
            return Rai.AccountsPending(accounts, count, source, displayUnit);
        }
        public string AvailableSupply(XRBUnit unit)
        {
            string supply = Rai.UnitConvert(Rai.AvailableSupply(), XRBUnit.raw, unit);
            return supply;
        }
        public Block RetrieveBlock(string hash)
        {
            Block block = Rai.RetrieveBlock(hash);

            return Rai.RetrieveBlock(hash);
        }
        public List<Block> RetrieveMultipleBlocks(List<string> hashes, XRBUnit unit = XRBUnit.raw)
        {
            return Rai.RetrieveMultipleBlocks(hashes, unit);
        }
        public List<Block> RetrieveMultipleBlocksWithAdditionalInfo(List<string> hashes, bool pending = false, bool source = false)
        {
            return Rai.RetrieveMultipleBlocksWithAdditionalInfo(hashes);
        }
        public string BlockAccount(string hash)
        {
            return Rai.BlockAccount(hash);
        }
        public (int count, int un_checked) BlockCount()
        {
            dynamic dynObj = Rai.BlockCount();
            return (Convert.ToInt32(dynObj.account), Convert.ToInt32(dynObj.@unchecked));
        }

        public (int send, int receive, int open, int change) BlockCountByType()
        {
            dynamic dynObj = Rai.BlockCountByType();
            return (Convert.ToInt32(dynObj.send), Convert.ToInt32(dynObj.receive), Convert.ToInt32(dynObj.open), Convert.ToInt32(dynObj.change));
        }
        public bool Bootstrap(string address, int port)
        {
            dynamic dynObj = Rai.Bootstrap(address, port);
            bool returnVal = false;
            if (dynObj.success == "")
                returnVal = true;
            return returnVal;
        }
        public bool BootstrapMultiConnection()
        {
            dynamic dynObj = Rai.BootstrapMultiConnection();
            bool returnVal = false;
            if (dynObj.success == "")
                returnVal = true;
            return returnVal;
        }

        public ChainBlocks Chain(string block, int count)
        {
            return Rai.Chain(block, count);
        }


        public string ProcessBlock(string wallet, string blockstring)
        {
            dynamic dynObj = Rai.ProcessBlock(wallet, blockstring);
            return dynObj["hash"];
        }

        public string WalletRepresentative(string wallet)
        {
            dynamic dynObj = Rai.WalletRepresentative(wallet);
            return dynObj["representative"];
        }


        public string Send(string wallet, string source, string destination, string amount, XRBUnit sendUnit)
        {
            dynamic dynObj = Rai.Send(wallet, source, destination, amount, sendUnit);
            try
            {
                return dynObj["block"].ToString();
            }
            catch
            {
                return dynObj["error"].ToString();
            }
        }



        public bool StopNode()
        {
            bool valid = false;
            dynamic dynObj = Rai.StopNode();
            if (dynObj.valid == "")
                valid = true;
            return valid;
        }


        public bool ValidateAccountNumberChecksum(string account)
        {
            bool valid = false;
            dynamic dynObj = Rai.ValidateAccountNumberChecksum(account);
            if (dynObj.valid == "1")
                valid = true;
            return valid;
        }



        public List<string> Successors(string block, int count)
        {
            return Rai.Successors(block, count);
        }


        public (string,string) OfflineSigningReceive(string wallet, string account, string source, string previous)
        {
            
            dynamic dynObj = Rai.OfflineSigningReceive(wallet, account, source, previous);
            //   var x = (dynObj["hash"].ToString(), dynObj["block"].ToString());
            return (dynObj["hash"], dynObj["block"]);
      //      return (x.Item1, x.Item2);
        }


        public (string, string) OfflineSigningOpen(string private_key, string account, string representative, string source)
        {
            
            dynamic dynObj = Rai.OfflineSigningOpen(private_key, account,representative, source);
            //   var x = (dynObj["hash"].ToString(), dynObj["block"].ToString());
            return (dynObj["hash"], dynObj["block"]);
            //      return (x.Item1, x.Item2);
        }

        public NodeVersions RetrieveNodeVersions()
        {
            dynamic dynObj = Rai.RetrieveNodeVersions();
            NodeVersions versions = new NodeVersions();
            versions.node_vendor = dynObj["node_vendor"];
            versions.rpc_version = dynObj["rpc_version"];
            versions.store_version = dynObj["store_version"];
            return versions;
        }

        public List<PeerVersion> RetrieveOnlinePeers()
        {
            return Rai.RetrieveOnlinePeers();
        }


        public List<string> Pending(string account, int count)
        {
            return Rai.Pending(account, count);
        }
        public List<AccountBlock> Pending(string account, int count, string threshold, XRBUnit thresholdUnig, XRBUnit displayUnit)
        {
            return Rai.Pending(account, count, threshold, thresholdUnig, displayUnit);
        }
        public List<AccountBlock> Pending(string account, int count, bool source, XRBUnit displayUnit)
        {
            return Rai.Pending(account, count, source, displayUnit);
        }
        public bool PendingExists(string hash)
        {
            dynamic dynObj = Rai.PendingExists(hash);
            return dynObj["exists"].ToString() == "0" ? false : true;
        }

        public List<Block> UncheckedBlocks(int count)
        {
            return Rai.UncheckedBlocks(count);
        }

        public string ClearUncheckedBlocks()
        {
            dynamic dynObj = Rai.ClearUncheckedBlocks();
            return dynObj["success"].ToString();
        }

        public Block RetrieveUncheckedBlock(string hash)
        {
            return Rai.RetrieveUncheckedBlock(hash);
        }

        public string WalletAddKey(string wallet, string key, bool work = true)
        {
            dynamic dynObj = Rai.WalletAddKey(wallet, key, work);
            return dynObj.account;
        }

        public (string balance, string pending) WalletTotalBalance(string wallet, XRBUnit unit = XRBUnit.XRB)
        {
            dynamic dynObj = Rai.WalletTotalBalance(wallet);
            return (Rai.UnitConvert(dynObj.balance.ToString(), XRBUnit.raw, unit), Rai.UnitConvert(dynObj.pending.ToString(), XRBUnit.raw, unit));
        }

 

        /* todo: implement threshold parameter */
        public List<AccountBalance> WalletAccountsBalances(string wallet, XRBUnit unit = XRBUnit.XRB)
        {
            dynamic item = Rai.WalletAccountsBalances(wallet);
            return AccountsBalancesBase(item, unit);
        }

        public bool WalletChangeSeed(string wallet, string seed)
        {
            bool success = false;
            dynamic dynObj = Rai.WalletChangeSeed(wallet, seed);
            if (dynObj.success == "")
                success = true;
            return success;
        }

        public bool WalletContains(string wallet, string account)
        {
            bool exists = false;
            dynamic dynObj = Rai.WalletContains(wallet, account);
            if (dynObj.exists == "1")
                exists = true;
            return exists;
        }

        public string WalletCreate()
        {
            dynamic dynObj = Rai.WalletCreate();
            return dynObj.ToString();
        }

        public void WalletDestroy(string wallet)
        {
            Rai.WalletDestroy(wallet);
        }
        public string WalletExport(string wallet)
        {
            dynamic dynObj = Rai.WalletExport(wallet);
            return dynObj["json"].ToString();
        }

        public List<Frontier> WalletFrontiers(string wallet)
        {
            return Rai.WalletFrontiers(wallet);
        }

        public List<AccountBlocks> WalletPending(string wallet, int count)
        {
            return Rai.WalletPending(wallet, count);
        }

        public List<AccountBlocks> WalletPending(string wallet, int count, string threshold, XRBUnit thresholdUnit = XRBUnit.raw, XRBUnit displayUnit = XRBUnit.XRB)
        {
            return Rai.WalletPending(wallet, count, threshold, thresholdUnit, displayUnit);
        }
        public List<AccountBlocks> WalletPending(string wallet, int count, bool source, XRBUnit displayUnit = XRBUnit.XRB)
        {
            return Rai.WalletPending(wallet, count, source, displayUnit);
        }

        public List<string> WalletRepublish(string wallet, int count)
        {
            List<string> blocks = new List<string>();
            dynamic dynObj = Rai.WalletRepublish(wallet, count);
            foreach (var item in dynObj["blocks"])
            {
                blocks.Add(item.ToString());
            }
            return blocks;
        }

        public List<Work> WalletWorkGet(string wallet)
        {
            return Rai.WalletWorkGet(wallet);
        }

        public string WalletChangePassword(string wallet, string password)
        {
            dynamic dynObj = Rai.WalletChangePassword(wallet, password);
            return dynObj["changed"].ToString();
        }

        public string WalletPasswordEnter(string wallet, string password)
        {
            dynamic dynObj = Rai.WalletPasswordEnter(wallet, password);
            return dynObj["valid"].ToString();
        }
        public string WalletValidPW(string wallet)
        {
            dynamic dynObj = Rai.WalletValidPW(wallet);
            return dynObj["valid"].ToString();
        }
        public string WalletLockedCheck(string wallet)
        {
            dynamic dynObj = Rai.WalletLockedCheck(wallet);
            return dynObj["locked"].ToString();
        }

        public void WorkCancel(string hash)
        {
            dynamic dynObj = Rai.WorkCancel(hash);
        }

        public string WorkGenerate(string hash)
        {
            dynamic dynObj = Rai.WorkGenerate(hash);
            return dynObj["work"].ToString();
        }

        public string WorkGet(string wallet, string account)
        {
            dynamic dynObj = Rai.WorkGet(wallet, account);
            return dynObj["work"].ToString();
        }

        public string WorkSet(string wallet, string account, string work)
        {
            dynamic dynObj = Rai.WorkSet(wallet, account, work);
            return dynObj["success"].ToString();
        }

        public string AddWorkPeer(string address, string port)
        {
            dynamic dynObj = Rai.AddWorkPeer(address, port);
            try
            {
                return dynObj["success"].ToString();
            }
            catch
            {
                return dynObj["error"].ToString();
            }
            
        }

        public List<string> RetrieveWorkPeers()
        {
            return Rai.RetrieveWorkPeers();
        }

        public string ClearWorkPeers()
        {
            dynamic dynObj = Rai.ClearWorkPeers();
            return dynObj["success"].ToString();
        }

        public string WorkValidate(string work , string hash)
        {
            dynamic dynObj = Rai.WorkValidate(work, hash);

            try
            {
                return dynObj["success"].ToString();
            }
            catch
            {
                return dynObj["error"].ToString();
            }
            
        }
    }
}
