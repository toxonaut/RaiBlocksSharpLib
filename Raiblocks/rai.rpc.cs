﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Raiblocks
{
    

    
    public class Rai
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
           // httpWebRequest.ContentType = "text/json";
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
        public string Shift (int numerics, string input)
        {
            string result = "";
            string numBeforeComma = "";
            string numAfterComma = input.Substring(input.IndexOf(".") + 1);
            if (input.IndexOf(".") > 0)
            {
                numBeforeComma = input.Substring(0, input.IndexOf("."));


                if (numerics > 0)
                {
                    string zeros = new string('0', numerics);
                    int missingZeros = numerics - numAfterComma.Length;
                    if (missingZeros > -1)
                    {
                        result = numBeforeComma + numAfterComma + zeros.Substring(0, missingZeros);
                    }
                    else
                    {
                        result = numBeforeComma + numAfterComma.Substring(0, numAfterComma.Length + missingZeros) + "." + numAfterComma.Substring(numAfterComma.Length + missingZeros);
                    }

                }
                else
                {
                    if (numBeforeComma.Length + numerics > 0)
                    {
                        result = numBeforeComma.Substring(0, numBeforeComma.Length + numerics) + "." + numBeforeComma.Substring(numBeforeComma.Length + numerics) + numAfterComma;
                    }
                    else
                    {
                        result = "0" + "." + new string('0', Math.Abs(numBeforeComma.Length + numerics)) + numBeforeComma + numAfterComma;
                    }
                }
            }
            else
            {
                if (numerics>0)
                {
                    result = (input + new string('0', numerics));
                }
                else
                {
                    if (input.Length+numerics<1)
                    {
                        result = ("0." + new string('0', Math.Abs(input.Length + numerics)) + input);
                    }
                    else
                    {
                        result = (input.Substring(0, input.Length + numerics) + "." + input.Substring(input.Length + numerics));
                    }
                    
                }
            }
            if (result.Contains("."))
            {
                result = result.TrimEnd('0').TrimEnd('.');
            }
            if (!result.StartsWith("0.") && result !="0")
                result = result.TrimStart('0');
            return result;
        }


        public string UnitConvert(string input, XRBUnit input_unit, XRBUnit output_unit)
        {
            string output = "";
            switch (input_unit)
            {
                case XRBUnit.raw: output = input; break;
                case XRBUnit.XRB: output = Shift(30,input); break; //shift 30, 1 with 31 zeros
                case XRBUnit.Trai: output = Shift(36, input); break; // shift 36
                case XRBUnit.Grai: output = Shift(33, input); break; // 33
                case XRBUnit.Mrai: output = Shift(30, input); break; // 30
                case XRBUnit.krai: output = Shift(27, input); break; // 27
                case XRBUnit.rai: output = Shift(24, input); break; // 24
                case XRBUnit.mrai: output = Shift(21, input); break; // 21
                case XRBUnit.urai: output = Shift(18, input); break; // 18
                case XRBUnit.prai: output = Shift(15, input); break; // 15
                default: output = input; break;
            }
            input = output;
            switch (output_unit)
            {
                case XRBUnit.raw: output = input; break;
                case XRBUnit.XRB: output = Shift(-30, input); break; //shift 30, 1 with 31 zeros
                case XRBUnit.Trai: output = Shift(-36, input); break; // shift 36
                case XRBUnit.Grai: output = Shift(-33, input); break; // 33
                case XRBUnit.Mrai: output = Shift(-30, input); break; // 30
                case XRBUnit.krai: output = Shift(-27, input); break; // 27
                case XRBUnit.rai: output = Shift(-24, input); break; // 24
                case XRBUnit.mrai: output = Shift(-21, input); break; // 21
                case XRBUnit.urai: output = Shift(-18, input); break; // 18
                case XRBUnit.prai: output = Shift(-15, input); break; // 15
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
        private string JsonCleanUp(string jsonOut)
        {
            jsonOut = jsonOut.Replace("\\n", "\n");
            jsonOut = jsonOut.Replace("\\\"", "\"");
            jsonOut = jsonOut.Replace("\"{", "{");
            jsonOut = jsonOut.Replace("}\"", "}");
            jsonOut = jsonOut.Replace("}\n\"", "}");
            return jsonOut;
        }
        /*  AccountBalance: Returns how many RAW is owned and how many have not yet been received by account
         *  Response:
            {  
              "balance": "10000",  
              "pending": "10000"  
            }
        */
        public dynamic AccountBalance(string account, XRBUnit unit)
        { 
            string jsonIn = "{\"action\":\"account_balance\"," +
                              "\"account\":\"" + account + "\"}";
            string jsonOut = Post(jsonIn);
            dynamic dynObj = JsonConvert.DeserializeObject(jsonOut);
            dynObj.balance = UnitConvert(dynObj.balance==null? "0": dynObj.balance.ToString(), XRBUnit.raw, unit);
            dynObj.pending = UnitConvert(dynObj.pending == null ? "0" : dynObj.pending.ToString(), XRBUnit.raw, unit);
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

            dynObj.balance = UnitConvert(dynObj.balance==null? "0": dynObj.balance.ToString(), XRBUnit.raw, unit);
            if (weight) dynObj.weight = UnitConvert(dynObj.weight==null? "0": dynObj.weight.ToString(), XRBUnit.raw, unit);
            if (pending) dynObj.pending = UnitConvert(dynObj.pending==null? "0": dynObj.pending.ToString(), XRBUnit.raw, unit);

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
        public dynamic AccountsBalances(List<string> accounts)
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
        public List<Frontier> AccountsFrontiers(List<string> accounts)
        {
            string accountsString = CommaSeparated(accounts);
            string jsonIn = "{\"action\":\"accounts_frontiers\"," +
                              "\"accounts\": [" + accountsString + "]" +
                              "}";
            string jsonOut = Post(jsonIn);
            List<Frontier> frontiers = new List<Frontier>();
            FrontiersResponse frontiersResponse = JsonConvert.DeserializeObject<FrontiersResponse>(jsonOut);

            foreach (var item in frontiersResponse.frontiers._extraStuff)
            {
                Frontier frontier = new Frontier();
                frontier.account= item.Key;
                frontier.hash = item.Value.ToString();
                frontiers.Add(frontier);
            }
            return frontiers;
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
        public List<AccountBlocks> AccountsPending(List<string> accounts, int count)
        {
            string accountsString = CommaSeparated(accounts);
            string jsonIn = "{\"action\":\"accounts_pending\"," +
                              "\"accounts\": [" + accountsString + "]," +
                              "\"count\": " + count.ToString()  +
                              "}";

            string jsonOut = Post(jsonIn);
            // test:         jsonOut = jsonOut.Replace("\"0C99DA21FC6EC324E9AAB619B08BFE402EA92516F39D6FD250B0290894A07140\"", "\"0C99DA21FC6EC324E9AAB619B08BFE402EA92516F39D6FD250B0290894A07140\",\"0XXXXXXXXXXXXXXXXX\"");

            BlocksMultipleResponse blocksResponse = JsonConvert.DeserializeObject<BlocksMultipleResponse>(jsonOut);
            List<AccountBlocks> accountBlocksList = new List<AccountBlocks>();
            foreach (var item in blocksResponse.blocks._extraStuff)
            {
                AccountBlocks accountBlocks = new AccountBlocks();
                accountBlocks.blocks = new List<AccountBlock>();
                accountBlocks.account = item.Key;
                foreach (var account in item.Value)
                {
                    AccountBlock accountBlock = new AccountBlock();
                    accountBlock.hash = account.ToString();
                    accountBlocks.blocks.Add(accountBlock);
                }
                accountBlocksList.Add(accountBlocks);
            }  
            return (accountBlocksList);
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
        public List<AccountBlocks> AccountsPending(List<string> accounts, int count, string threshold, XRBUnit thresholdUnit, XRBUnit displayUnit)
        {
            string accountsString = CommaSeparated(accounts);
            string jsonIn = "{\"action\":\"accounts_pending\"," +
                              "\"accounts\": [" + accountsString + "]," +
                              "\"count\":\"" + count.ToString() + "\"," +
                              "\"threshold\":\"" + UnitConvert(threshold, XRBUnit.raw, thresholdUnit).ToString() + "\"" +
                              "}";
            string jsonOut = Post(jsonIn);
            BlocksMultipleResponse blocksResponse = JsonConvert.DeserializeObject<BlocksMultipleResponse>(jsonOut);
            List<AccountBlocks> accountBlocksList = new List<AccountBlocks>();
            foreach (var item in blocksResponse.blocks._extraStuff)
            {
                AccountBlocks accountBlocks = new AccountBlocks();
                accountBlocks.blocks = new List<AccountBlock>();
                accountBlocks.account = item.Key;
                foreach (var hash in item.Value)
                {
                    AccountBlock accountBlock = new AccountBlock();
                    accountBlock.hash = hash.Path;
                    accountBlock.amount = UnitConvert(hash.First==null ? "0": hash.First.ToString(), XRBUnit.raw, displayUnit);
                    accountBlocks.blocks.Add(accountBlock);
                }
                accountBlocksList.Add(accountBlocks);
            }
            return (accountBlocksList);
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
        public dynamic AccountsPending(List<string> accounts, int count, bool source,  XRBUnit displayUnit)
        {
            string accountsString = CommaSeparated(accounts);
            string jsonIn = "{\"action\":\"accounts_pending\"," +
                              "\"accounts\": [" + accountsString + "]," +
                              "\"count\":\"" + count.ToString() + "\"," +
                              "\"source\":\"" + source.ToString().ToLower() + "\"" +
                              "}";
            string jsonOut = Post(jsonIn);
        //    jsonOut = jsonOut.Replace("\"xrb_3e77ic1xarfn6mbbxthh38ibz8rr3gx7ikap6589bmcsdsmuyccd4hy8g8f5\": {", "\"xrb_3e77ic1xarfn6mbbxthh38ibz8rr3gx7ikap6589bmcsdsmuyccd4hy8g8f5\": { " + " \"0C99DA21FCXXXXXX\": {\n"+
        //       " \"amount\": \"200000000000000\",\n"+
        //       "\"source\":\"xrb_test\"},");

            BlocksMultipleResponse blocksResponse = JsonConvert.DeserializeObject<BlocksMultipleResponse>(jsonOut);
            List<AccountBlocks> accountBlocksList = new List<AccountBlocks>();
            foreach (var item in blocksResponse.blocks._extraStuff)
            {
                AccountBlocks accountBlocks = new AccountBlocks();
                accountBlocks.blocks = new List<AccountBlock>();
                accountBlocks.account = item.Key;
                foreach (var hash in item.Value)
                {
                    AccountBlock accountBlock = new AccountBlock();
                    accountBlock.hash = hash.Path;
                    accountBlock.amount = UnitConvert(hash.First["amount"].ToString(), XRBUnit.raw, displayUnit);
                    accountBlock.source = hash.First["source"].ToString();
                    accountBlocks.blocks.Add(accountBlock);
                }
                accountBlocksList.Add(accountBlocks);
            }
            return (accountBlocksList);
        }

        /*  AvailableSupply:  Returns how many rai are in the public supply
         *  Response:
            {  
              "available": "10000"  
            }
         */
        public string AvailableSupply()
        {
            string jsonIn = "{\"action\":\"available_supply\"" +
                              "}";
            dynamic dynObj = GetDynamicObj(jsonIn);            
            return (dynObj.available.ToString());
        }

        /*  Retrieve block:  Retrieves a json representation of block
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
        public Block RetrieveBlock(string hash)
        {

            string jsonIn = "{\"action\":\"block\"," +
                              "\"hash\":\"" + hash + "\"" +
                              "}";

            string jsonOut = Post(jsonIn);
            jsonOut = JsonCleanUp(jsonOut);
            BlocksResponse blocksResponse = JsonConvert.DeserializeObject<BlocksResponse>(jsonOut);
            return (blocksResponse.contents);
        }

        /*  RetrieveMultipleBlocks: Retrieves a json representation of blocks
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
        public List<Block> RetrieveMultipleBlocks(List<string> hashes, XRBUnit unit)
        {
            List<Block> blocks = new List<Block>();
            string hashesString = CommaSeparated(hashes);
            string jsonIn = "{\"action\":\"blocks\"," +
                              "\"hashes\": [" + hashesString + "]" +
                              "}";
            string jsonOut = Post(jsonIn);
            jsonOut = JsonCleanUp(jsonOut);
            BlocksMultipleResponse blocksResponse = JsonConvert.DeserializeObject<BlocksMultipleResponse>(jsonOut);
            foreach (var item in blocksResponse.blocks._extraStuff)
            {
                Block block = new Block() { hash = item.Key, type= item.Value["type"]?.ToString(), source= item.Value["source"]?.ToString(), amount = UnitConvert(item.Value["amount"]==null ? "0": item.Value["amount"].ToString(), XRBUnit.raw, unit), block_account= item.Value["block_account"]?.ToString(), representative= item.Value["representative"]?.ToString(), previous = item.Value["previous"]?.ToString(), destination= item.Value["destination"]?.ToString(), balance= UnitConvert(item.Value["balance"].ToString(), XRBUnit.raw, unit), work= item.Value["work"].ToString(), signature= item.Value["signature"].ToString() };
                blocks.Add(block);
            }
            return (blocks);
        }

        /*  RetrieveMultipleBlocksWithAdditionalInfo: Retrieves a json representations of blocks with transaction amount & block account 
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
        public List<Block> RetrieveMultipleBlocksWithAdditionalInfo(List<string> hashes, bool pending = false, bool source = false)
        {
            List<Block> blocks = new List<Block>();
            string hashesString = CommaSeparated(hashes);
            string jsonIn = "{\"action\":\"blocks_info\"," +
                              "\"hashes\": [" + hashesString + "]," +
                              "\"pending\":\"" + pending.ToString().ToLower() + "\"," +
                              "\"source\":\"" + source.ToString().ToLower() + "\"" +
                              "}";
            string jsonOut = Post(jsonIn);
            jsonOut = JsonCleanUp(jsonOut);
            BlocksAdditionalResponse blocksResponse = JsonConvert.DeserializeObject<BlocksAdditionalResponse>(jsonOut);
            foreach (var item in  blocksResponse.blocks._extraStuff)
            {
                Block block = new Block() { hash = item.Key, block_account= item.Value["block_account"]?.ToString(), amount = item.Value["amount"].ToString(),  type = item.Value["contents"]["type"]?.ToString(), previous = item.Value["contents"]["previous"]?.ToString(), destination = item.Value["contents"]["destination"]?.ToString(), representative= item.Value["contents"]["representative"]?.ToString(), source= item.Value["contents"]["source"]?.ToString(), balance = item.Value["contents"]["balance"].ToString(), work = item.Value["contents"]["work"].ToString(), signature = item.Value["contents"]["signature"].ToString() };
                blocks.Add(block);
            }
            return (blocks);
        }

        /*  BlockAccount: Returns the account containing block
           *  Response:
                {  
                  "account": "xrb_3e3j5tkog48pnny9dmfzj1r16pg8t1e76dz5tmac6iq689wyjfpi00000000"  
                }
          */
        public string BlockAccount(string hash)
        {
            string jsonIn = "{\"action\":\"block_account\"," +
                              "\"hash\":\"" + hash + "\"" +
                              "}";

            return (GetDynamicObj(jsonIn).account);
        }


        /*  BlockCount: Reports the number of blocks in the ledger and unchecked synchronizing blocks
        *   Response:
            {
              "count": "1000",  
              "unchecked": "10"  
            }
        */
        public dynamic BlockCount()
        {
            string jsonIn = "{\"action\":\"block_count\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  BlockCountByType: Reports the number of blocks in the ledger by type (send, receive, open, change)
        *   Response:
            {
                "send": "1000",   
                "receive": "900",   
                "open": "100",   
                "change": "50"  
            }
        */
        public dynamic BlockCountByType()
        {
            string jsonIn = "{\"action\":\"block_count_type\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  Bootstrap: Initialize bootstrap to specific IP address and port
        *   Response:
            {
              "success": ""  
            }
        */
        public dynamic Bootstrap(string address, int port)
        {
            string jsonIn = "{\"action\":\"bootstrap\"," +
                            "\"address\":\"" + address + "\"," +
                            "\"port\":\"" + port + "\"," +
                              "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  BootstrapMultiConnection: Initialize multi-connection bootstrap to random peers
        *   Response:
            {
              "success": ""  
            }
        */
        public dynamic BootstrapMultiConnection()
        {
            string jsonIn = "{\"action\":\"bootstrap_any\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  Chain: Returns a list of block hashes in the account chain starting at block up to count
        *   Response:
            {    
              "blocks" : [  
              "000D1BAEC8EC208142C99059B393051BAC8380F9B5A2E6B2489A277D81789F3F"  
              ]  
            }
        */
        public ChainBlocks Chain(string block, int count)
        {
            string jsonIn = "{\"action\":\"chain\"," +
                            "\"block\":\"" + block + "\"," +
                            "\"count\":\"" + count + "\"" +
                              "}";
            string jsonOut = Post(jsonIn);
            ChainBlocks blocksResponse = JsonConvert.DeserializeObject<ChainBlocks>(jsonOut);
            return (blocksResponse);
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


        



        /*  OfflineSigning:  Creates a json representations of new block based on input data & signed with private key or account in wallet*
         *  Response:
            {  
              "hash": "314BA8D9057678C1F53371C2DB3026C1FAC01EC8E7802FD9A2E8130FC523429E",
              "block": "{\n    \"type\": \"receive\",\n    \"previous\": \"F47B23107E5F34B2CE06F562B5C435DF72A533251CB414C51B2B62A8F63A00E4\",\n    \"source\": \"19D3D919475DEED4696B5D13018151D1AF88B2BD3BCFF048B45031C1F36D1858\",\n    \"work\": \"6acb5dd43a38d76a\",\n    \"signature\": \"A13FD22527771667D5DFF33D69787D734836A3561D8A490C1F4917A05D77EA09860461D5FBFC99246A4EAB5627F119AD477598E22EE021C4711FACF4F3C80D0E\"\n}\n"   
            }
         */
        public dynamic OfflineSigningReceive(string wallet, string account, string source, string previous)
        {
            string jsonIn = "{\"action\":\"block_create\"," +
                              "\"type\": \"receive\"," +
                              "\"wallet\": \"" + wallet + "\"," +
                              "\"account\": \"" + account + "\"," +
                              "\"source\": \"" + source + "\"," +
                              "\"previous\": \"" + previous + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }

        public dynamic OfflineSigningOpen(string private_key, string account, string representative, string source)
        {
            string jsonIn = "{\"action\":\"block_create\"," +
                              "\"type\": \"open\"," +
                              "\"key\": \"" + private_key + "\"," +
                              "\"account\": \"" + account + "\"," +
                              "\"representative\": \"" + representative + "\"," +
                              "\"source\": \"" + source + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }

       



        /*  ProcessBlock:  Publish block to the network
          *  Response:
            {  
                "hash": "42A723D2B60462BF7C9A003FE9A70057D3A6355CA5F1D0A57581000000000000"   
            }
          */
        public dynamic ProcessBlock(string wallet, string blockstring)
        {
            string jsonIn = "{\n  \"action\": \"process\",\n" +
                              "   \"block\": \"" + blockstring.Replace("\"", "\\\"").TrimEnd('\n') + "\"" +
                              "\n}";
            string jsonTest = jsonIn.Replace("\n", "");
                       string jsonIn3 = "{  \n" +
"  \"action\": \"process\",  \n" +
"  \"block\": \"{\n" +
"    \\\"type\\\": \\\"open\\\",\n" +
"    \\\"account\\\": \\\"xrb_1iqkgmz1wh3m8kzz4dc1763emepob99fgqwkrpmge56ujgn3oiikbrxps3kx\\\",\n" +
"    \\\"representative\\\": \\\"xrb_3dmtrrws3pocycmbqwawk6xs7446qxa36fcncush4s1pejk16ksbmakis78m\\\",\n" +
"    \\\"source\\\": \\\"D9BD39DEB89E1E5A009D4DCE77CFA6BD3008D609AFA54B81460A78AE0FD2702F\\\",\n" +
"    \\\"work\\\": \\\"26dad9eeaa7eec9f\\\",\n" +
"    \\\"signature\\\": \\\"6E1FB922A4DEF003CAA38F3D5D1209BDF3EF47F6F1789605B547AEB78BED506C1EAC6075A0F2266BDCD03C4CEC1557FFB5A5F120F32EEC30D35892E1E0923A04\\\"\n" +
"}\"\n" +
"}\n";
            return (GetDynamicObj(jsonTest));
        }






        /*  WalletRepresentative:  Returns the default representative for wallet
        *  Response:
            {  
              "representative" : "xrb_3e3j5tkog48pnny9dmfzj1r16pg8t1e76dz5tmac6iq689wyjfpi00000000"
            }
        */
        public dynamic WalletRepresentative(string wallet)
        {
            string jsonIn = "{\"action\":\"wallet_representative\"," +
                              "\"wallet\": \"" + wallet + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }


        //wallet representative set

        //republish

        // search pending

        //search pending for all wallets


        /*  Send:  Send amount from source in wallet to destination
         *  Response:
            {  
              "block": "000D1BAEC8EC208142C99059B393051BAC8380F9B5A2E6B2489A277D81789F3F"  
            }
         */
        public dynamic Send(string wallet, string source, string destination, string amount, XRBUnit sendUnit)
        {
            string jsonIn = "{\"action\":\"send\"," +
                            "\"wallet\":\"" + wallet + "\"," +
                            "\"source\":\"" + source + "\"," +
                            "\"destination\":\"" + destination + "\"," +
                            "\"amount\":\"" + UnitConvert(amount, sendUnit, XRBUnit.raw) + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }


        /*  StopNode:  
         *  Response:
            {  
              "success": ""  
            }
         */
        public dynamic StopNode()
        {
            string jsonIn = "{\"action\":\"stop\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }


        /*  ValidateAccountNumberChecksum:  Check whether account is a valid account number
         *  Response:
            {    
              "blocks" : [  
              "A170D51B94E00371ACE76E35AC81DC9405D5D04D4CEBC399AEACE07AE05DD293"  
              ]  
            }
         */
        public dynamic ValidateAccountNumberChecksum(string account)
        {
            string jsonIn = "{\"action\":\"validate_account_number\"," +
                              "\"account\":\"" + account + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }



        /*  Successors:  Returns a list of block hashes in the account chain ending at block up to count
         *  Response:
            {    
              "blocks" : [  
              "A170D51B94E00371ACE76E35AC81DC9405D5D04D4CEBC399AEACE07AE05DD293"  
              ]  
            }
         */
        public List<string> Successors(string block, int count)
        {
            string jsonIn = "{\"action\":\"successors\"," +
                              "\"block\": \"" + block + "\"," +
                              "\"count\": " + count.ToString() +
                              "}";

            string jsonOut = Post(jsonIn);
            PendingBlocks blocksResponse = JsonConvert.DeserializeObject<PendingBlocks>(jsonOut);
            return (blocksResponse.blocks);
        }



        /*  RetrieveNodeVersions: 
        *   Response:
            {  
              "rpc_version" : "1",
              "store_version" : "2",
              "node_vendor" : "RaiBlocks 7.5.0"
            }
        */

        public dynamic RetrieveNodeVersions()
        {
            string jsonIn = "{\"action\":\"version\"" +                          
                              "}";
            return (GetDynamicObj(jsonIn));
        }


        /*  RetrieveOnlinePeers: Returns a list of pairs of peer IPv6:port and its node network version
        *   Response:
            {
                "peers": {  
                    "[::ffff:172.17.0.1]:32841": "3"  
                }  
            }
        */
        public List<PeerVersion> RetrieveOnlinePeers()
        {
            string jsonIn = "{\"action\":\"peers\"" +
                             "}";
            string jsonOut = Post(jsonIn);
            PeersResponse blocksResponse = JsonConvert.DeserializeObject<PeersResponse>(jsonOut);
            List<PeerVersion> peerList = new List<PeerVersion>();
            foreach (var item in blocksResponse.peers._extraStuff)
            {
                PeerVersion peer = new PeerVersion();
                peer.address = item.Key.ToString();
                peer.version = item.Value.ToString();
                peerList.Add(peer);
            }
            return (peerList);
        }



        /*  Pending:  Returns a list of block hashes which have not yet been received by this account.
         *  Response:
            {    
              "blocks" : [ "000D1BAEC8EC208142C99059B393051BAC8380F9B5A2E6B2489A277D81789F3F" ]  
            }
         */
        public List<string> Pending(string account, int count)
        {
            string jsonIn = "{\"action\":\"pending\"," +
                              "\"account\": \"" + account + "\"," +
                              "\"count\": " + count.ToString() +
                              "}";

            string jsonOut = Post(jsonIn);
            PendingBlocks blocksResponse = JsonConvert.DeserializeObject<PendingBlocks>(jsonOut);
            return (blocksResponse.blocks);
        }

        /*  Pending:  Returns a list of block hashes which have not yet been received by this account.
         *  Optional: Returns a list of pending block hashes with amount more or equal to threshold
         *  Response:
            {  
              "blocks" : {    
                    "000D1BAEC8EC208142C99059B393051BAC8380F9B5A2E6B2489A277D81789F3F": "6000000000000000000000000000000"    
                }  
            }
        */
        public List<AccountBlock> Pending(string account, int count, string threshold, XRBUnit thresholdUnit, XRBUnit displayUnit)
        {
            string jsonIn = "{\"action\":\"pending\"," +
                              "\"account\": \"" + account + "\"," +
                              "\"count\":\"" + count.ToString() + "\"," +
                              "\"threshold\":\"" + UnitConvert(threshold, XRBUnit.raw, thresholdUnit).ToString() + "\"" +
                              "}";
            string jsonOut = Post(jsonIn);
            BlocksMultipleResponse blocksResponse = JsonConvert.DeserializeObject<BlocksMultipleResponse>(jsonOut);
            List<AccountBlock> accountBlockList = new List<AccountBlock>();
            foreach (var item in blocksResponse.blocks._extraStuff)
            {
                AccountBlock accountBlock = new AccountBlock();
                accountBlock.hash = item.Key;
                accountBlock.amount = UnitConvert(item.Value.ToString(), thresholdUnit, displayUnit);
                
                accountBlockList.Add(accountBlock);
            }
            return (accountBlockList);
        }
        /*  Pending:  Returns a list of block hashes which have not yet been received by this account.
         *  Optional: Returns a list of pending block hashes with amount and source accounts
         *  Response:
            {  
              "blocks" : {    
                    "000D1BAEC8EC208142C99059B393051BAC8380F9B5A2E6B2489A277D81789F3F": {   
                         "amount": "6000000000000000000000000000000",       
                         "source": "xrb_3dcfozsmekr1tr9skf1oa5wbgmxt81qepfdnt7zicq5x3hk65fg4fqj58mbr"  
                    }   
                }  
            }
        */
        public List<AccountBlock> Pending(string account, int count, bool source, XRBUnit displayUnit)
        {
            string jsonIn = "{\"action\":\"pending\"," +
                              "\"account\": \"" + account + "\"," +
                              "\"count\":\"" + count.ToString() + "\"," +
                              "\"source\":\"" + source.ToString().ToLower()+ "\"" +
                              "}";
            string jsonOut = Post(jsonIn);
            BlocksMultipleResponse blocksResponse = JsonConvert.DeserializeObject<BlocksMultipleResponse>(jsonOut);
            List<AccountBlock> accountBlockList = new List<AccountBlock>();
            foreach (var item in blocksResponse.blocks._extraStuff)
            {
                AccountBlock accountBlock = new AccountBlock();
                accountBlock.hash = item.Key;
                accountBlock.amount = UnitConvert(item.Value["amount"].ToString(), XRBUnit.raw, displayUnit);
                accountBlock.source = item.Value["source"].ToString();
                accountBlockList.Add(accountBlock);
            }
            return (accountBlockList);
        }

        /*  Pending exists:  Check whether block is pending by hash
         *  Response:
            {  
              "exists" : "1"
            }
         */
        public dynamic PendingExists(string hash)
        {
            string jsonIn = "{\"action\":\"pending_exists\"," +
                              "\"hash\":\"" + hash + "\"" +
                              
                              "}";
            return (GetDynamicObj(jsonIn));
        }


        /*  Unchecked blocks:  Returns a list of pairs of unchecked synchronizing block hash and its json representation up to count
         *  Response:
            {  
                "blocks": {  
                   "000D1BAEC8EC208142C99059B393051BAC8380F9B5A2E6B2489A277D81789F3F": "{
                      "type": "open",
                      "account": "xrb_3e3j5tkog48pnny9dmfzj1r16pg8t1e76dz5tmac6iq689wyjfpi00000000",
                      "representative": "xrb_3e3j5tkog48pnny9dmfzj1r16pg8t1e76dz5tmac6iq689wyjfpi00000000",
                      "source": "FA5B51D063BADDF345EFD7EF0D3C5FB115C85B1EF4CDE89D8B7DF3EAF60A04A4",
                      "work": "0000000000000000",
                      "signature": 
             "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"
                   }"
                }
            }
         */
        public List<Block> UncheckedBlocks(int count)
        {
            List<Block> blocks = new List<Block>();
            string jsonIn = "{\"action\":\"unchecked\"," +
                              "\"count\":\"" + count.ToString() + "\"" +
                              "}";
            string jsonOut = Post(jsonIn);
            jsonOut = JsonCleanUp(jsonOut);
            BlocksAdditionalResponse blocksResponse = JsonConvert.DeserializeObject<BlocksAdditionalResponse>(jsonOut);
            foreach (var item in blocksResponse.blocks._extraStuff)
            {
                Block block = new Block() { hash = item.Key, type = item.Value["type"]?.ToString(), account = item.Value["account"]?.ToString(), block_account = item.Value["block_account"]?.ToString(), amount = item.Value["amount"]?.ToString(), previous = item.Value["previous"]?.ToString(), destination = item.Value["destination"]?.ToString(), representative = item.Value["representative"]?.ToString(), source = item.Value["source"]?.ToString(), balance = item.Value["balance"]?.ToString(), work = item.Value["work"]?.ToString(), signature = item.Value["signature"]?.ToString() };
                blocks.Add(block);
            }
            return (blocks);
        }

        /*  Pending exists:  Clear unchecked blocks
         *  Response:
            {  
                "success": ""  
            }
         */
        public dynamic ClearUncheckedBlocks()
        {
            string jsonIn = "{\"action\":\"unchecked_clear\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  Retrieve unchecked block:  Retrieves a json representation of unchecked synchronizing block by hash
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
        public Block RetrieveUncheckedBlock(string hash)
        {
 
            string jsonIn = "{\"action\":\"unchecked_get\"," +
                              "\"hash\":\"" + hash + "\"" +
                              "}";
            string jsonOut = Post(jsonIn);
            jsonOut = JsonCleanUp(jsonOut);
            BlocksResponse blocksResponse = JsonConvert.DeserializeObject<BlocksResponse>(jsonOut);
           
            return (blocksResponse.contents);
        }



        /// <summary>
        /// TODO UncheckedBlocksWithDatabaseKeys
        /// </summary>
        /// <param name="wallet"></param>
        /// <param name="privateKey"></param>
        /// <param name="work"></param>
        /// <returns></returns>






        /*  WalletAddKey:  Add an adhoc private key key to wallet
         *  Response:
            {  
              "account" : "xrb_3e3j5tkog48pnny9dmfzj1r16pg8t1e76dz5tmac6iq689wyjfpi00000000"
            }
         */
        public dynamic WalletAddKey(string wallet, string privateKey, bool work)
        {
            string jsonIn = "{\"action\":\"wallet_add\"," +
                              "\"wallet\":\"" + wallet + "\"," +
                              "\"key\":\"" + privateKey + "\"," +
                              "\"work\":\"" + work.ToString().ToLower() + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  WalletTotalBalance:  Returns the sum of all accounts balances in wallet
         *  Response:
            {  
                "balance": "10000",  
                "pending": "10000"  
            }
         */
        public dynamic WalletTotalBalance(string wallet)
        {
            string jsonIn = "{\"action\":\"wallet_balance_total\"," +
                              "\"wallet\":\"" + wallet + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  WalletAccountsBalances:  Returns how many rai is owned and how many have not yet been received by all accounts in wallet
         *  Response:
            {  
              "balances" : {  
                "xrb_3e3j5tkog48pnny9dmfzj1r16pg8t1e76dz5tmac6iq689wyjfpi00000000": {  
                  "balance": "10000",  
                  "pending": "10000"  
                }
              }   
            }
         */
        public dynamic WalletAccountsBalances(string wallet)
        {
            string jsonIn = "{\"action\":\"wallet_balances\"," +
                              "\"wallet\":\"" + wallet + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
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

        /*  WalletContains:  Check whether wallet contains account
         *  Response:
            {  
            "exists" : "1"
            }
         */
        public dynamic WalletContains(string wallet, string account)
        {
            string jsonIn = "{\"action\":\"wallet_contains\"," +
                              "\"wallet\":\"" + wallet + "\"," +
                              "\"account\":\"" + account + "\"" +
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
        public void WalletDestroy(string wallet)
        {
            string jsonIn = "{\"action\":\"wallet_destroy\"," +
                              "\"wallet\":\"" + wallet + "\"" +
                              "}";
            dynamic d =GetDynamicObj(jsonIn);
        }

        /*  WalletExport:  Return a json representation of wallet
         *  Response:
            {  
              "json" : "{\"0000000000000000000000000000000000000000000000000000000000000000\": \"0000000000000000000000000000000000000000000000000000000000000001\"}"
            }
         */
        public dynamic WalletExport(string wallet)
        {
            string jsonIn = "{\"action\":\"wallet_export\"," +
                              "\"wallet\":\"" + wallet + "\"" +
                              "}";
            return(GetDynamicObj(jsonIn));
        }

        /*  WalletFrontiers:  Returns a list of pairs of account and block hash representing the head block starting for accounts from wallet
         *  Response:
            {    
              "frontiers" : {  
              "xrb_3e3j5tkog48pnny9dmfzj1r16pg8t1e76dz5tmac6iq689wyjfpi00000000": "000D1BAEC8EC208142C99059B393051BAC8380F9B5A2E6B2489A277D81789F3F"  
              }  
            }
         */
        public List<Frontier> WalletFrontiers(string wallet)
        {
            List<Frontier> frontiers = new List<Frontier>();
            string jsonIn = "{\"action\":\"wallet_frontiers\"," +
                              "\"wallet\":\"" + wallet + "\"" +
                              "}";
            string jsonOut = Post(jsonIn);
            jsonOut = JsonCleanUp(jsonOut);
            FrontiersResponse frontiersResponse = JsonConvert.DeserializeObject<FrontiersResponse>(jsonOut);
             foreach (var item in frontiersResponse.frontiers._extraStuff)
             {
                Frontier frontier = new Frontier() { account = item.Key.ToString(), hash = item.Value.ToString() };
                frontiers.Add(frontier);
             }
            return frontiers;
        }


        /*  WalletPending: Returns a list of block hashes which have not yet been received by accounts in this wallet
         *   Response:
            {  
              "blocks" : {  
                "xrb_1111111111111111111111111111111111111111111111111117353trpda": ["142A538F36833D1CC78B94E11C766F75818F8B940771335C6C1B8AB880C5BB1D"],  
                "xrb_3t6k35gi95xu6tergt6p69ck76ogmitsa8mnijtpxm9fkcm736xtoncuohr3": ["4C1FEEF0BEA7F50BE35489A1233FE002B212DEA554B55B1B470D78BD8F210C74"]  
              }  
            }
         */
        public List<AccountBlocks> WalletPending(string wallet, int count)
        {
            string jsonIn = "{\"action\":\"wallet_pending\"," +
                              "\"wallet\": \"" + wallet + "\"," +
                              "\"count\": " + count.ToString() +
                              "}";

            string jsonOut = Post(jsonIn);
            // test:         jsonOut = jsonOut.Replace("\"0C99DA21FC6EC324E9AAB619B08BFE402EA92516F39D6FD250B0290894A07140\"", "\"0C99DA21FC6EC324E9AAB619B08BFE402EA92516F39D6FD250B0290894A07140\",\"0XXXXXXXXXXXXXXXXX\"");

            BlocksMultipleResponse blocksResponse = JsonConvert.DeserializeObject<BlocksMultipleResponse>(jsonOut);
            List<AccountBlocks> accountBlocksList = new List<AccountBlocks>();
            if (blocksResponse.blocks !=null)
            {
                foreach (var item in blocksResponse.blocks._extraStuff)
                {
                    AccountBlocks accountBlocks = new AccountBlocks();
                    accountBlocks.blocks = new List<AccountBlock>();
                    accountBlocks.account = item.Key;
                    foreach (var account in item.Value)
                    {
                        AccountBlock accountBlock = new AccountBlock();
                        accountBlock.hash = account.ToString();
                        accountBlocks.blocks.Add(accountBlock);
                    }
                    accountBlocksList.Add(accountBlocks);
                }
            }
            
            return (accountBlocksList);
        }

        /*  WalletPending: Returns a list of pending block hashes with amount more or equal to threshold
         *  Optional: threshold
         *   Response:
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
        public List<AccountBlocks> WalletPending(string wallet, int count, string threshold, XRBUnit thresholdUnit, XRBUnit displayUnit)
        {
            string jsonIn = "{\"action\":\"wallet_pending\"," +
                              "\"wallet\": \"" + wallet + "\"," +
                              "\"count\": \"" + count.ToString() + "\"," +
                              "\"threshold\": \"" + UnitConvert(threshold, XRBUnit.raw, thresholdUnit).ToString() + "\"" +
                              "}";

            string jsonOut = Post(jsonIn);
            BlocksMultipleResponse blocksResponse = JsonConvert.DeserializeObject<BlocksMultipleResponse>(jsonOut);
            List<AccountBlocks> accountBlocksList = new List<AccountBlocks>();
            if (blocksResponse.blocks !=null)
            {
                foreach (var item in blocksResponse.blocks._extraStuff)
                {
                    AccountBlocks accountBlocks = new AccountBlocks();
                    accountBlocks.blocks = new List<AccountBlock>();
                    accountBlocks.account = item.Key;
                    foreach (var hash in item.Value)
                    {
                        AccountBlock accountBlock = new AccountBlock();
                        accountBlock.hash = hash.Path;
                        accountBlock.amount = UnitConvert(hash.First == null ? "0" : hash.First.ToString(), XRBUnit.raw, displayUnit);
                        accountBlocks.blocks.Add(accountBlock);
                    }
                    accountBlocksList.Add(accountBlocks);
                }
            }
            return (accountBlocksList);
        }

        /*  WalletPending: Returns a list of block hashes which have not yet been received by these accounts
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
        public dynamic WalletPending(string wallet, int count, bool source, XRBUnit displayUnit)
        {
            string jsonIn = "{\"action\":\"wallet_pending\"," +
                              "\"wallet\":\"" + wallet + "\"," +
                              "\"count\":\"" + count.ToString() + "\"," +
                              "\"source\":\"" + source.ToString().ToLower() + "\"" +
                              "}";
            string jsonOut = Post(jsonIn);
    
            BlocksMultipleResponse blocksResponse = JsonConvert.DeserializeObject<BlocksMultipleResponse>(jsonOut);
            List<AccountBlocks> accountBlocksList = new List<AccountBlocks>();
            if (blocksResponse.blocks !=null)
            {
                foreach (var item in blocksResponse.blocks._extraStuff)
                {
                    AccountBlocks accountBlocks = new AccountBlocks();
                    accountBlocks.blocks = new List<AccountBlock>();
                    accountBlocks.account = item.Key;
                    foreach (var hash in item.Value)
                    {
                        AccountBlock accountBlock = new AccountBlock();
                        accountBlock.hash = hash.Path;
                        accountBlock.amount = UnitConvert(hash.First["amount"].ToString(), XRBUnit.raw, displayUnit);
                        accountBlock.source = hash.First["source"].ToString();
                        accountBlocks.blocks.Add(accountBlock);
                    }
                    accountBlocksList.Add(accountBlocks);
                }
            }
            
            return (accountBlocksList);
        }

        /*  WalletRepublish: Rebroadcast blocks for accounts from wallet starting at frontier down to count to the network
        *   Response:
            {    
              "blocks": [   
                  "991CF190094C00F0B68E2E5F75F6BEE95A2E0BD93CEAA4A6734DB9F19B728948",   
                  "A170D51B94E00371ACE76E35AC81DC9405D5D04D4CEBC399AEACE07AE05DD293",   
                  "90D0C16AC92DD35814E84BFBCC739A039615D0A42A76EF44ADAEF1D99E9F8A35"   
              ]       
            }
        */
        public dynamic WalletRepublish(string wallet, int count)
        {
            string jsonIn = "{\"action\":\"wallet_republish\"," +
                              "\"wallet\":\"" + wallet + "\"," +
                              "\"count\":\"" + count.ToString() + "\"" +
                              "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  WalletWorkGet: Returns a list of pairs of account and work from wallet
        *   Response:
            {  
               "works": {
                   "xrb_1111111111111111111111111111111111111111111111111111hifc8npp": "432e5cf728c90f4f"   
               }
            }
        */
        public List<Work> WalletWorkGet(string wallet)
        {
            List<Work> works = new List<Work>();
            string jsonIn = "{\"action\":\"wallet_work_get\"," +
                              "\"wallet\":\"" + wallet + "\"" +
                              "}";
            string jsonOut = Post(jsonIn);
            jsonOut = JsonCleanUp(jsonOut);
            WorksResponse worksResponse = JsonConvert.DeserializeObject<WorksResponse>(jsonOut);
            foreach (var item in worksResponse.works._extraStuff)
            {
                Work work = new Work() { account = item.Key.ToString(), work = item.Value.ToString() };
                works.Add(work);
            }
            return works;
        }

        /*  WalletChangePassword: Changes the password for wallet to password
        *   Response:
            {  
              "changed" : "1"
            }
        */
        public dynamic WalletChangePassword(string wallet, string password)
        {
            string jsonIn = "{\"action\":\"password_change\"," +
                             "\"wallet\":\"" + wallet + "\"," +
                             "\"password\":\"" + password + "\"" +
                             "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  WalletPasswordEnter: Enters the password in to wallet
        *   Response:
            {  
              "valid" : "1"
            }
        */
        public dynamic WalletPasswordEnter(string wallet, string password)
        {
            string jsonIn = "{\"action\":\"password_enter\"," +
                             "\"wallet\":\"" + wallet + "\"," +
                             "\"password\":\"" + password + "\"" +
                             "}";
            return (GetDynamicObj(jsonIn));
        }


        /*  WalletValidPW: Enters the password in to wallet
        *   Response:
            {  
              "valid" : "1"
            }
        */
        public dynamic WalletValidPW(string wallet)
        {
            string jsonIn = "{\"action\":\"password_valid\"," +
                             "\"wallet\":\"" + wallet + "\"" +
                             "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  WalletLockedCheck: Checks whether wallet is locked
        *   Response:
            {  
              "locked" : "0"
            }
        */
        public dynamic WalletLockedCheck(string wallet)
        {
            string jsonIn = "{\"action\":\"wallet_locked\"," +
                             "\"wallet\":\"" + wallet + "\"" +
                             "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  WorkCancel: Stop generating work for block
        *   Response:
            {  
            }
        */
        public dynamic WorkCancel(string hash)
        {
            string jsonIn = "{\"action\":\"work_cancel\"," +
                             "\"hash\":\"" + hash + "\"" +
                             "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  WorkGenerate: Generates work for block
        *   Response:
            {  
                "work": "2bf29ef00786a6bc"  
            }
        */
        public dynamic WorkGenerate(string hash)
        {
            string jsonIn = "{\"action\":\"work_generate\"," +
                             "\"hash\":\"" + hash + "\"" +
                             "}";
            return (GetDynamicObj(jsonIn));
        }


        /*  WorkGet: Retrieves work for account in wallet
        *   Response:
            {  
                "work": "2bf29ef00786a6bc"  
            }
        */
        public dynamic WorkGet(string wallet, string account)
        {
            string jsonIn = "{\"action\":\"work_get\"," +
                             "\"wallet\":\"" + wallet + "\"," +
                             "\"account\":\"" + account + "\"" +
                             "}";
            return (GetDynamicObj(jsonIn));
        }


        /*  WorkGet: Retrieves work for account in wallet
        *   Response:
            {  
                "success": ""  
            }
        */
        public dynamic WorkSet(string wallet, string account, string work)
        {
            string jsonIn = "{\"action\":\"work_set\"," +
                             "\"wallet\":\"" + wallet + "\"," +
                             "\"account\":\"" + account + "\"," +
                             "\"work\":\"" + work + "\"" +
                             "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  AddWorkPeer: Add specific IP address and port as work peer for node until restart
        *   Response:
            {  
                "success": ""  
            }
        */
        public dynamic AddWorkPeer(string address, string port)
        {
            string jsonIn = "{\"action\":\"work_peer_add\"," +
                             "\"address\":\"" + address + "\"," +
                             "\"port\":\"" + port + "\"" +
                             "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  RetrieveWorkPeers: enable_control required, version 8.0+
        *   Response:
            {  
                "work_peers": [   
                    "::ffff:172.17.0.1:7076"   
                ]   
            }
        */
        public List<string> RetrieveWorkPeers()
        {
            string jsonIn = "{\"action\":\"work_peers\"" +
                             "}";
            string jsonOut = Post(jsonIn);
            jsonOut = JsonCleanUp(jsonOut);
            WorkPeers workPeers = JsonConvert.DeserializeObject<WorkPeers>(jsonOut);
            return workPeers.work_peers;
        }


        /*  ClearWorkPeers: Clear work peers node list until restart
         *   Response:
             {  
                 "success": ""  
             }
         */
        public dynamic ClearWorkPeers()
        {
            string jsonIn = "{\"action\":\"work_peers_clear\"" +
                             "}";
            return (GetDynamicObj(jsonIn));
        }

        /*  WorkValidate: Check whether work is valid for block
         *   Response:
             {  
                 "valid": "1"  
             }
         */
        public dynamic WorkValidate(string work, string hash)
        {
            string jsonIn = "{\"action\":\"work_validate\"," +
                                             "\"work\":\"" + work + "\"," +
                             "\"hash\":\"" + hash + "\"" +
                             "}";
            return (GetDynamicObj(jsonIn));
        }

    }
}
