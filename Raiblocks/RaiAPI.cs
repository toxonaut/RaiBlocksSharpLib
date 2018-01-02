using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raiblocks
{
    class RaiAPI
    {
        public RaiAPI(string url_base)
        {
            this.Rai = new Rai(url_base);
        }
        public Rai Rai
        {
            get;
        }
        public AccountBalance AccountBalance(string account)
        {
            dynamic accountBalance = Rai.AccountBalance(account);
            AccountBalance ab = new AccountBalance() { account = account, balance = accountBalance.balance, pending = accountBalance.pending };
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
                History hist = new History() { account = account, amount = Rai.UnitConvert(Convert.ToDecimal(history[i].amount), XRBUnit.raw, unit), hash = history[i].hash, type = history[i].type };
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
                        ab.balance = Rai.UnitConvert(Convert.ToDecimal(propertyValues2[property2]), XRBUnit.raw, unit);

                    if (property2 == "pending")
                        ab.pending = Rai.UnitConvert(Convert.ToDecimal(propertyValues2[property2]), XRBUnit.raw, unit);
                }
                accountBalances.Add(ab);
            }
            return accountBalances;
        }

        public List<AccountBlocks> AccountsPending(List<string> accounts, int count, XRBUnit unit = XRBUnit.XRB)
        {
            return Rai.AccountsPending(accounts, count, unit);
        }
        public List<AccountBlocks> AccountsPending(List<string> accounts, int count, decimal threshold,  XRBUnit thresholdUnit = XRBUnit.raw, XRBUnit displayUnit = XRBUnit.XRB)
        {
            return Rai.AccountsPending(accounts, count, threshold, thresholdUnit, displayUnit);
        }
        public decimal AvailableSupply(XRBUnit unit)
        {
            decimal supply = Rai.UnitConvert(Rai.AvailableSupply(), XRBUnit.raw, unit);
            return supply;
        }
        public Block RetrieveBlock(string hash)
        {
            Block block = Rai.RetrieveBlock(hash);

            return Rai.RetrieveBlock(hash);
        }
        public List<Block> RetrieveMultipleBlocks(List<string> hashes,  XRBUnit unit = XRBUnit.raw)
        {
            return Rai.RetrieveMultipleBlocks(hashes, unit);
        }
        public BlocksAdditionalContainer RetrieveMultipleBlocksWithAdditionalInfo(List<string> hashes, bool pending = false, bool source = false)
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

        
        public string WalletAddKey(string wallet, string key, bool work=true)
        {
            dynamic dynObj = Rai.WalletAddKey(wallet, key, work);
            return dynObj.account;
        }
        
        public (decimal balance, decimal pending) WalletTotalBalance(string wallet, XRBUnit unit = XRBUnit.XRB)
        {
            dynamic dynObj = Rai.WalletTotalBalance(wallet);
            return (Rai.UnitConvert(Convert.ToDecimal(dynObj.balance), XRBUnit.raw, unit), Rai.UnitConvert(Convert.ToDecimal(dynObj.pending), XRBUnit.raw, unit));
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
            return dynObj.wallet.ToString();
        }

        public void WalletDestroy(string wallet)
        {
            Rai.WalletDestroy(wallet);
        }
    }
}
