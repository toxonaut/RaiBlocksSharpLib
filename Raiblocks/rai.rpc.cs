using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Raiblocks
{
    class Rai
    {
        string url_base;

        public Rai(string url_base)
        {
            this.url_base = url_base;
        }
        public string rpc(string request)
        {
            string resultat="";
            try
            {
                 resultat = Post2(url_base);
            }
            catch (Exception x)
            {
                 
            }
            return resultat;
        }

        public string Post(string url, string payload)
        {
            WebHeaderCollection headers = new WebHeaderCollection();
            headers.Add("action", "account_balance");
            headers.Add("account", "xrb_3e3j5tkog48pnny9dmfzj1r16pg8t1e76dz5tmac6iq689wyjfpi00000000");

            Uri APIurl = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(APIurl);
            request.KeepAlive = true;
            request.Method = "POST";
            request.Headers.Add(headers);

            string responseString;
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    responseString = reader.ReadToEnd();
                }
            }
            return responseString;
        }

        public string Post2(string json)
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
        public string AccountBalance(string account)
        {
            string json = "{\"action\":\"account_balance\"," +
                              "\"account\":\"" + account + "\"}";

            return (Post2(json));
        }
    }
}
