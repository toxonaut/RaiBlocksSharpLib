using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Raiblocks
{
    public class AccountBalance
    {
        public string account { get; set; }
        public double balance { get; set; }
        public double pending { get; set; }
    }
    public class Frontier
    {
        public string account { get; set; }
        public string hash { get; set; }
    }

    public class History
    {
        public string hash { get; set; }
        public string type { get; set; }
        public string account { get; set; }
        public double amount { get; set; }
    }
    public class KeyPair
    {
        [JsonProperty("private")]
        public string privateKey { get; set; }
        [JsonProperty("public")]
        public string publicKey { get; set; }
        [JsonProperty("account")]
        public string account { get; set; }
    }
    public enum XRBUnit
    {
        raw, XRB, Trai, Grai, Mrai, krai, rai, mrai, urai, prai
    }

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
            string account =accountCreate.account.ToString();
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
            for (int i=0; i<history.Count;i++)
            {
                History hist = new History() { account = account, amount = Rai.UnitConvert(Convert.ToDouble(history[i].amount), XRBUnit.raw, unit), hash = history[i].hash, type = history[i].type };
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
            List<AccountBalance> accountBalances = new List<AccountBalance>();
            dynamic item = Rai.AccountsBalances(accounts, unit);

            IDictionary<string, object> propertyValues = item.balances;

            foreach (var property in propertyValues.Keys)
            {
                AccountBalance ab = new AccountBalance();
                
                ab.account = property;
                IDictionary<string, object> propertyValues2 = ((IDictionary<string, object>)propertyValues[property]);
                foreach (var property2 in propertyValues2.Keys)
                {
                    if (property2=="balance")
                        ab.balance = Rai.UnitConvert(Convert.ToDouble(propertyValues2[property2]), XRBUnit.raw, unit);
                    
                    if (property2 == "pending")
                        ab.pending = Rai.UnitConvert(Convert.ToDouble(propertyValues2[property2]), XRBUnit.raw, unit); 
                } 
                accountBalances.Add(ab);
            }
            return accountBalances;
        }


    }
    class Rai
    {
        string url_base;

        public Rai(string url_base)
        {
            this.url_base = url_base;
        }
        public string rpc(string request)
        {
            string result="";
            try
            {
                 result = Post(url_base);
            }
            catch (Exception x)
            {
                 
            }
            return result;
        }
        
        public string Post(string json)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url_base);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string result = "";
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var resultemp = streamReader.ReadToEnd();
                result = resultemp.ToString();
            }
            return result;
        }
        public double UnitConvert(double input, XRBUnit input_unit, XRBUnit output_unit)
        {
            double output=0;
            switch (input_unit)
            {
                case XRBUnit.raw: output = input; break;
                case XRBUnit.XRB: output = input* 1000000000000000000000000000000d; break; //shift 30, 1 with 31 zeros
                case XRBUnit.Trai: output = input * 1000000000000000000000000000000000000d; break; // shift 36
                case XRBUnit.Grai: output = input * 1000000000000000000000000000000000d; break; // 33
                case XRBUnit.Mrai: output = input * 1000000000000000000000000000000d; break; // 30
                case XRBUnit.krai: output = input * 1000000000000000000000000000d; break; // 27
                case XRBUnit.rai: output = input * 1000000000000000000000000d; break; // 24
                case XRBUnit.mrai: output = input * 1000000000000000000000d; break; // 21
                case XRBUnit.urai: output = input * 1000000000000000000d; break; // 18
                case XRBUnit.prai: output = input * 1000000000000000d; break; // 15
                default: output = input; break;
            }
            input = output;
            switch (output_unit)
            {
                case XRBUnit.raw: output = input; break;
                case XRBUnit.XRB: output = input / 1000000000000000000000000000000d; break; //shift 30, 1 with 31 zeros
                case XRBUnit.Trai: output = input / 1000000000000000000000000000000000000d; break; // shift 36
                case XRBUnit.Grai: output = input / 1000000000000000000000000000000000d; break; // 33
                case XRBUnit.Mrai: output = input / 1000000000000000000000000000000d; break; // 30
                case XRBUnit.krai: output = input / 1000000000000000000000000000d; break; // 27
                case XRBUnit.rai: output = input / 1000000000000000000000000d; break; // 24
                case XRBUnit.mrai: output = input / 1000000000000000000000d; break; // 21
                case XRBUnit.urai: output = input / 1000000000000000000d; break; // 18
                case XRBUnit.prai: output = input / 1000000000000000d; break; // 15
                default: output = input; break;
            }

            return output;
        }
        private dynamic GetDynamicObj(string jsonIn)
        {
            string jsonOut = Post(jsonIn);
            dynamic dynObj = JsonConvert.DeserializeObject(jsonOut);
            return dynObj;
        }
        private dynamic GetExpandoObj(string jsonIn)
        {
            string jsonOut = Post(jsonIn);
            var converter = new ExpandoObjectConverter();
            dynamic dynObj = JsonConvert.DeserializeObject<ExpandoObject>(jsonOut, converter);
            return dynObj;
        }
        private string CommaSeparated(List<string> separateStrings)
        {
            string commaSeparated = "";
            foreach (string s in separateStrings)
            {
                commaSeparated += "\"" + s + "\",";
            }
            commaSeparated = commaSeparated.TrimEnd(',');
            return commaSeparated;
        }

        /*  AccountBalance: Returns how many RAW is owned and how many have not yet been received by account
         *  Response:
            {  
              "balance": "10000",  
              "pending": "10000"  
            }
        */
        public dynamic AccountBalance(string account, XRBUnit unit = XRBUnit.XRB)
        { 
            string jsonIn = "{\"action\":\"account_balance\"," +
                              "\"account\":\"" + account + "\"}";
            string jsonOut = Post(jsonIn);
            dynamic dynObj = JsonConvert.DeserializeObject(jsonOut);
            dynObj.balance = UnitConvert(Convert.ToDouble(dynObj.balance), XRBUnit.raw, unit);
            return (dynObj);
        }

        /*  AccountBlockCount: Get number of blocks for a specific account
         *  Response:
            {  
              "block_count" : "19"  
            }
        */
        public dynamic AccountBlockCount(string account)
        {
            string jsonIn = "{\"action\":\"account_block_count\"," +
                              "\"account\":\"" + account + "\"}";
            return (GetDynamicObj(jsonIn));
        }

        /*  AccountInformation: Returns frontier, open block, change representative block, balance, last modified timestamp from local database & block count for account
         *  Response:
            {  
                "frontier": "FF84533A571D953A596EA401FD41743AC85D04F406E76FDE4408EAED50B473C5",   
                "open_block": "991CF190094C00F0B68E2E5F75F6BEE95A2E0BD93CEAA4A6734DB9F19B728948",   
                "representative_block": "991CF190094C00F0B68E2E5F75F6BEE95A2E0BD93CEAA4A6734DB9F19B728948",   
                "balance": "235580100176034320859259343606608761791",   
                "modified_timestamp": "1501793775",   
                "block_count": "33",   
                "representative": "xrb_3t6k35gi95xu6tergt6p69ck76ogmitsa8mnijtpxm9fkcm736xtoncuohr3",   
                "weight": "1105577030935649664609129644855132177",   
                "pending": "2309370929000000000000000000000000"   
            }
        */
        public dynamic AccountInformation(string account, XRBUnit unit= XRBUnit.raw, bool representative=false, bool weight=false, bool pending=false)
        {
            string jsonIn = "{\"action\":\"account_info\"," +
                              "\"account\":\"" + account + "\"," +
                              "\"representative\":\"" + representative.ToString() + "\"," +
                              "\"weight\":\"" + weight.ToString().ToLower() + "\"," +
                              "\"pending\":\"" + pending.ToString().ToLower() + "\"" +
                              "}";
            string jsonOut = Post(jsonIn);
            dynamic dynObj = JsonConvert.DeserializeObject(jsonOut);

            dynObj.balance = UnitConvert(Convert.ToDouble(dynObj.balance), XRBUnit.raw, unit);
            if (weight) dynObj.weight = UnitConvert(Convert.ToDouble(dynObj.weight), XRBUnit.raw, unit);
            if (pending) dynObj.pending = UnitConvert(Convert.ToDouble(dynObj.pending), XRBUnit.raw, unit);

            return (dynObj);
        }

        /*  AccountCreate:  Creates a new account, insert next deterministic key in wallet
         *  Response:
         *  {  
              "action": "account_create",  
              "wallet": "000D1BAEC8EC208142C99059B393051BAC8380F9B5A2E6B2489A277D81789F3F",  
              "work": "false"  
            }
         */
        public dynamic AccountCreate(string wallet, bool work=true)
        {
            string jsonIn = "{\"action\":\"account_create\"," +
                              "\"wallet\":\"" + wallet + "\"," +
                              "\"work\":\"" + work.ToString().ToLower() + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  AccountGet: Get account number for the public key
         *  Response:
         *  {  
                "account" : "xrb_1e5aqegc1jb7qe964u4adzmcezyo6o146zb8hm6dft8tkp79za3sxwjym5rx"  
            }
         */
        public dynamic AccountGet(string key)
        {
            string jsonIn = "{\"action\":\"account_get\"," +
                              "\"key\":\"" + key + "\"}";
            return (GetDynamicObj(jsonIn));
        }

        /*  AccountHistory:  Reports send/receive information for a account
         *  Response:
            {
                "history": [{
                        "hash": "000D1BAEC8EC208142C99059B393051BAC8380F9B5A2E6B2489A277D81789F3F",
                        "type": "receive",
                        "account": "xrb_3e3j5tkog48pnny9dmfzj1r16pg8t1e76dz5tmac6iq689wyjfpi00000000",
                        "amount": "100000000000000000000000000000000"
                }]
            }
         */
        public dynamic AccountHistory(string account, int count)
        {
            string jsonIn = "{\"action\":\"account_history\"," +
                              "\"account\":\"" + account + "\"," +
                              "\"count\":\"" + count.ToString() + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  AccountList: Lists all the accounts inside wallet
         *  Response:
            {  
                "accounts" : [
                "xrb_3e3j5tkog48pnny9dmfzj1r16pg8t1e76dz5tmac6iq689wyjfpi00000000"  
                ]
            }
         */
        public dynamic AccountList(string wallet)
        {
            string jsonIn = "{\"action\":\"account_list\"," +
                              "\"wallet\":\"" + wallet + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  AccountMove: Moves accounts from source to wallet
         *  Response:
            {  
                "moved" : "1"
            }
         */
        public dynamic AccountMove(string wallet, string source, List<string> accounts)
        {
            string accountsString = CommaSeparated(accounts);
            string jsonIn = "{\"action\":\"account_move\"," +
                              "\"wallet\":\"" + wallet + "\"," +
                              "\"source\":\"" + source + "\"," +
                              "\"accounts\": [" + accountsString + "]" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  AccountPublicKey: Get the public key for account
         *  Response:
            {  
              "key": "3068BB1CA04525BB0E416C485FE6A67FD52540227D267CC8B6E8DA958A7FA039"  
            }
         */
        public dynamic AccountPublicKey(string account)
        {
            string jsonIn = "{\"action\":\"account_key\"," +
                              "\"account\":\"" + account + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }


        /*  AccounRemove: Remove account from wallet
         *  Response:
            {  
              "removed": "1"
            }
         */
        public dynamic AccountRemove(string wallet, string account)
        {
            string jsonIn = "{\"action\":\"account_remove\"," +
                              "\"wallet\":\"" + wallet + "\"," +
                              "\"account\":\"" + account + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  AccountRepresentative: Returns the representative for account
        *   Response:
            {  
                "representative" : "xrb_16u1uufyoig8777y6r8iqjtrw8sg8maqrm36zzcm95jmbd9i9aj5i8abr8u5"
            }
        */
        public dynamic AccountRepresentative(string account)
        {
            string jsonIn = "{\"action\":\"account_representative\"," +
                              "\"account\":\"" + account + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }


        /*  AccountRepresentativeSet: Sets the representative for account in wallet
        *   Response:
            {  
              "block": "000D1BAEC8EC208142C99059B393051BAC8380F9B5A2E6B2489A277D81789F3F"
            }
        */
        public dynamic AccountRepresentativeSet(string wallet, string account, string representative)
        {
            string jsonIn = "{\"action\":\"account_representative\"," +
                              "\"wallet\":\"" + wallet + "\"," +
                              "\"account\":\"" + account + "\"," +
                              "\"representative\":\"" + representative + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  AccountWeight: Returns the voting weight for account
        *   Response:
            {  
              "weight": "10000"  
            }
        */
        public dynamic AccountWeight(string account)
        {
            string jsonIn = "{\"action\":\"account_weight\"," +
                              "\"account\":\"" + account + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  AccountsBalances: Returns how many RAW is owned and how many have not yet been received by accounts list
        *   Response:
            {  
              "balances" : {  
                "xrb_3e3j5tkog48pnny9dmfzj1r16pg8t1e76dz5tmac6iq689wyjfpi00000000":  
                {  
                  "balance": "10000",  
                  "pending": "10000"  
                },  
                "xrb_3i1aq1cchnmbn9x5rsbap8b15akfh7wj7pwskuzi7ahz8oq6cobd99d4r3b7":  
                {  
                  "balance": "10000000",  
                  "pending": "0"  
                }  
              }  
            }
        */
        public dynamic AccountsBalances(List<string> accounts, XRBUnit unit = XRBUnit.XRB)
        {
            string accountsString = CommaSeparated(accounts);
            string jsonIn = "{\"action\":\"accounts_balances\"," +
                              "\"accounts\": [" + accountsString + "]" +
                              "}";

            return (GetExpandoObj(jsonIn));
        }

        /*  AccountsCreate: Creates new accounts, insert next deterministic keys in wallet up to count
        *   Response:
            {  
              "accounts": [    
                 "xrb_3e3j5tkog48pnny9dmfzj1r16pg8t1e76dz5tmac6iq689wyjfpi00000000",   
                 "xrb_1e5aqegc1jb7qe964u4adzmcezyo6o146zb8hm6dft8tkp79za3s00000000"
              ]   
            }
        */
        public dynamic AccountsCreate(string wallet, int count, bool work=true)
        {
            string jsonIn = "{\"action\":\"accounts_create\"," +
                              "\"wallet\":\"" + wallet + "\"," +
                              "\"count\":\"" + count.ToString() + "\"," +
                              "\"work\":\"" + work.ToString().ToLower() + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  AccountsFrontiers: Returns a list of pairs of account and block hash representing the head block for accounts list
        *   Response:
            {  
              "frontiers" : {  
                "xrb_3t6k35gi95xu6tergt6p69ck76ogmitsa8mnijtpxm9fkcm736xtoncuohr3": "791AF413173EEE674A6FCF633B5DFC0F3C33F397F0DA08E987D9E0741D40D81A",  
                "xrb_3i1aq1cchnmbn9x5rsbap8b15akfh7wj7pwskuzi7ahz8oq6cobd99d4r3b7": "6A32397F4E95AF025DE29D9BF1ACE864D5404362258E06489FABDBA9DCCC046F"  
              }  
            }
        */
        public dynamic AccountsFrontiers(List<string> accounts)
        {
            string accountsString = CommaSeparated(accounts);
            string jsonIn = "{\"action\":\"accounts_frontiers\"," +
                              "\"account\": [" + accountsString + "]" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  AccountsPending: Returns a list of block hashes which have not yet been received by these accounts
         *   Response:
             {  
              "blocks" : {  
                "xrb_1111111111111111111111111111111111111111111111111117353trpda": ["142A538F36833D1CC78B94E11C766F75818F8B940771335C6C1B8AB880C5BB1D"],  
                "xrb_3t6k35gi95xu6tergt6p69ck76ogmitsa8mnijtpxm9fkcm736xtoncuohr3": ["4C1FEEF0BEA7F50BE35489A1233FE002B212DEA554B55B1B470D78BD8F210C74"]  
              }  
             }
         */
        public dynamic AccountsPending(List<string> accounts, int count)
        {
            string accountsString = CommaSeparated(accounts);
            string jsonIn = "{\"action\":\"accounts_pending\"," +
                              "\"account\": [" + accountsString + "]," +
                              "\"count\":\"" + count.ToString() + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }
        /*  AccountsPending: Returns a list of block hashes which have not yet been received by these accounts
         *  Optional "threshold": Returns a list of pending block hashes with amount more or equal to threshold
         *  Response:
            {  
              "blocks" : {
                "xrb_1111111111111111111111111111111111111111111111111117353trpda": {    
                    "142A538F36833D1CC78B94E11C766F75818F8B940771335C6C1B8AB880C5BB1D": "6000000000000000000000000000000"    
                },    
                "xrb_3t6k35gi95xu6tergt6p69ck76ogmitsa8mnijtpxm9fkcm736xtoncuohr3": {    
                    "4C1FEEF0BEA7F50BE35489A1233FE002B212DEA554B55B1B470D78BD8F210C74": "106370018000000000000000000000000"    
                }  
            }
        */
        public dynamic AccountsPending(List<string> accounts, int count, double threshold, XRBUnit thresholdUnit= XRBUnit.raw)
        {
            string accountsString = CommaSeparated(accounts);
            string jsonIn = "{\"action\":\"accounts_pending\"," +
                              "\"account\": [" + accountsString + "]," +
                              "\"count\":\"" + count.ToString() + "\"," +
                              "\"threshold\":\"" + UnitConvert(threshold, XRBUnit.raw, thresholdUnit).ToString() + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }
        /*  AccountsPending: Returns a list of block hashes which have not yet been received by these accounts
                 *  Optional "source": Returns a list of pending block hashes with amount and source accounts
                 *  Response:
                    {  
                      "blocks" : {
                        "xrb_1111111111111111111111111111111111111111111111111117353trpda": {    
                            "142A538F36833D1CC78B94E11C766F75818F8B940771335C6C1B8AB880C5BB1D": {   
                                 "amount": "6000000000000000000000000000000",       
                                 "source": "xrb_3dcfozsmekr1tr9skf1oa5wbgmxt81qepfdnt7zicq5x3hk65fg4fqj58mbr"
                            }
                        },    
                        "xrb_3t6k35gi95xu6tergt6p69ck76ogmitsa8mnijtpxm9fkcm736xtoncuohr3": {    
                            "4C1FEEF0BEA7F50BE35489A1233FE002B212DEA554B55B1B470D78BD8F210C74": {   
                                 "amount": "106370018000000000000000000000000",       
                                 "source": "xrb_13ezf4od79h1tgj9aiu4djzcmmguendtjfuhwfukhuucboua8cpoihmh8byo"
                            }   
                        }  
                    }
                */
        public dynamic AccountsPending(List<string> accounts, int count, bool source)
        {
            string accountsString = CommaSeparated(accounts);
            string jsonIn = "{\"action\":\"accounts_pending\"," +
                              "\"account\": [" + accountsString + "]," +
                              "\"count\":\"" + count.ToString() + "\"," +
                              "\"source\":\"" + source.ToString().ToLower() + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  AvailableSupply:  Returns how many rai are in the public supply
         *  Response:
            {  
              "available": "10000"  
            }
         */
        public double AvailableSupply()
        {
            string jsonIn = "{\"action\":\"available_supply\"" +
                              "}";
            dynamic dynObj = GetDynamicObj(jsonIn);            
            return (Convert.ToDouble(dynObj.available));
        }

        /*  RetrieveBlock: Retrieves a json representation of block
         *  Response:
            {  
              "contents" : "{
                "type": "open",
                "account": "xrb_3e3j5tkog48pnny9dmfzj1r16pg8t1e76dz5tmac6iq689wyjfpi00000000",
                "representative": "xrb_3e3j5tkog48pnny9dmfzj1r16pg8t1e76dz5tmac6iq689wyjfpi00000000",
                "source": "FA5B51D063BADDF345EFD7EF0D3C5FB115C85B1EF4CDE89D8B7DF3EAF60A04A4",
                "work": "0000000000000000",
                "signature": "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"
            }"
            }
        */
        public dynamic RetrieveBlock(string hash)
        {

            string jsonIn = "{\"action\":\"block\"," +
                              "\"hash\":\"" + hash + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  RetrieveMultipleBlocks: Retrieves a json representations of blocks
           *  Response:
              {  
                  "blocks" : {  
                    "000D1BAEC8EC208142C99059B393051BAC8380F9B5A2E6B2489A277D81789F3F": "{  
                      "type": "open",  
                      "account": "xrb_3e3j5tkog48pnny9dmfzj1r16pg8t1e76dz5tmac6iq689wyjfpi00000000",  
                      "representative": "xrb_3e3j5tkog48pnny9dmfzj1r16pg8t1e76dz5tmac6iq689wyjfpi00000000",  
                      "source": "FA5B51D063BADDF345EFD7EF0D3C5FB115C85B1EF4CDE89D8B7DF3EAF60A04A4",  
                      "work": "0000000000000000",  
                      "signature": "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"  
                    }"
                  }
                }
          */
        public dynamic RetrieveMultipleBlocks(List<string> hashes)
        {
            string hashesString = CommaSeparated(hashes);
            string jsonIn = "{\"action\":\"block\"," +
                              "\"account\": [" + hashesString + "]" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  RetrieveMultipleBlocksWithAdditionalInfo: Retrieves a json representations of blocks with transaction amount & block account Request:
           *  Response:
              {  
                  "blocks" : {   
                    "000D1BAEC8EC208142C99059B393051BAC8380F9B5A2E6B2489A277D81789F3F": {   
                       "block_account": "xrb_3e3j5tkog48pnny9dmfzj1r16pg8t1e76dz5tmac6iq689wyjfpi00000000",   
                       "amount": "1000000000000000000000000000000",   
                       "contents": "{  
                          "type": "open",  
                          "account": "xrb_3e3j5tkog48pnny9dmfzj1r16pg8t1e76dz5tmac6iq689wyjfpi00000000",  
                          "representative": "xrb_3e3j5tkog48pnny9dmfzj1r16pg8t1e76dz5tmac6iq689wyjfpi00000000",  
                          "source": "FA5B51D063BADDF345EFD7EF0D3C5FB115C85B1EF4CDE89D8B7DF3EAF60A04A4",  
                          "work": "0000000000000000",  
                          "signature": "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"  
                         }"
                     }
                  }
                }
          */
        public dynamic RetrieveMultipleBlocksWithAdditionalInfo(List<string> hashes, bool pending = false, bool source = false)
        {
            string hashesString = CommaSeparated(hashes);
            string jsonIn = "{\"action\":\"blocks_info\"," +
                              "\"account\": [" + hashesString + "]," +
                              "\"pending\":\"" + pending.ToString().ToLower() + "\"," +
                              "\"source\":\"" + source.ToString().ToLower() + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }
















        /*  DeterministicKey:  Derive deterministic keypair from seed based on index
         *  Response:
            {  
                  "private": "9F0E444C69F77A49BD0BE89DB92C38FE713E0963165CCA12FAF5712D7657120F",  
                  "public": "C008B814A7D269A1FA3C6528B19201A24D797912DB9996FF02A1FF356E45552B",  
                  "account": "xrb_3i1aq1cchnmbn9x5rsbap8b15akfh7wj7pwskuzi7ahz8oq6cobd99d4r3b7"  
                }
         */
        public KeyPair DeterministicKey(string seed, int index)
        {
            string jsonIn = "{\"action\":\"deterministic_key\"," +
                              "\"seed\":\"" + seed + "\"," +
                              "\"index\":\"" + index.ToString() + "\"" +
                              "}";
            string jsonOut = Post(jsonIn);
            KeyPair keyPair = JsonConvert.DeserializeObject<KeyPair>(jsonOut);
            return (keyPair);
        }


        /*  WalletAddKey:  Add an adhoc private key key to wallet
         *  Response:
            {  
              "account" : "xrb_3e3j5tkog48pnny9dmfzj1r16pg8t1e76dz5tmac6iq689wyjfpi00000000"
            }
         */
        public string WalletAddKey(string wallet, string privateKey, bool work)
        {
            string jsonIn = "{\"action\":\"wallet_add\"," +
                              "\"wallet\":\"" + wallet + "\"," +
                              "\"key\":\"" + privateKey + "\"," +
                              "\"work\":\"" + work.ToString().ToLower() + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn).account);
        }


        /*  WalletChangeSeed:  Changes seed for wallet to seed
         *  Response:
            {  
              "success" : ""
            }
         */
        public dynamic WalletChangeSeed(string wallet, string seed)
        {
            string jsonIn = "{\"action\":\"wallet_change_seed\"," +
                              "\"wallet\":\"" + wallet + "\"," +
                              "\"seed\":\"" + seed + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  WalletCreate:  Creates a new random wallet id
         *  Response:
            {  
              "wallet" : "000D1BAEC8EC208142C99059B393051BAC8380F9B5A2E6B2489A277D81789F3F"
            }
         */
        public string WalletCreate()
        {
            string jsonIn = "{\"action\":\"wallet_create\"" +
                              "}";
            return (GetDynamicObj(jsonIn).wallet);
        }

        /*  WalletDestroy:  Creates a new random wallet id
         *  Response:
            {  
            }
         */
        public void WalletDestroy()
        {
            string jsonIn = "{\"action\":\"wallet_create\"" +
                              "}";
            dynamic d =GetDynamicObj(jsonIn);
        }
    }
}
