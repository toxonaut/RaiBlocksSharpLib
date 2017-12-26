using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Raiblocks
{
    public class AccountBalance
    {
        public double balance { get; set; }
        public double pending { get; set; }
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
            AccountBalance ab = new AccountBalance() { balance = accountBalance.balance, pending = accountBalance.pending };
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
        public dynamic AccountBalance(string account, XRBUnit unit = XRBUnit.XRB)
        { 
            string jsonIn = "{\"action\":\"account_balance\"," +
                              "\"account\":\"" + account + "\"}";
            string jsonOut = Post(jsonIn);
            dynamic dynObj = JsonConvert.DeserializeObject(jsonOut);
            dynObj.balance = UnitConvert(Convert.ToDouble(dynObj.balance), XRBUnit.raw, unit);
            return (dynObj);
        }
        public dynamic AccountBlockCount(string account)
        {
            string jsonIn = "{\"action\":\"account_block_count\"," +
                              "\"account\":\"" + account + "\"}";
            string jsonOut = Post(jsonIn);
            dynamic dynObj = JsonConvert.DeserializeObject(jsonOut);
            return (dynObj);
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
                              "\"weight\":\"" + weight.ToString() + "\"," +
                              "\"pending\":\"" + pending.ToString() + "\"" +
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
        public dynamic AccountCreate(string wallet, bool work=false)
        {
            string jsonIn = "{\"action\":\"account_create\"," +
                              "\"wallet\":\"" + wallet + "\"," +
                              "\"work\":\"" + work.ToString() + "\"" +
                              "}";
            string jsonOut = Post(jsonIn);
            dynamic dynObj = JsonConvert.DeserializeObject(jsonOut);
            return (dynObj);
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
            string jsonOut = Post(jsonIn);
            dynamic dynObj = JsonConvert.DeserializeObject(jsonOut);
            return (dynObj);
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
            string jsonOut = Post(jsonIn);
            dynamic dynObj = JsonConvert.DeserializeObject(jsonOut);
            return (dynObj);
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
            string jsonOut = Post(jsonIn);
            dynamic dynObj = JsonConvert.DeserializeObject(jsonOut);
            return (dynObj);
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
    }
}
