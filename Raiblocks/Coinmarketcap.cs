using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Raiblocks
{
    public class CoinmarketcapCurrencies
    {
        [JsonProperty]
        public string id { get; set; }
        [JsonProperty]
        public string name { get; set; }
        [JsonProperty]
        public string symbol { get; set; }
        [JsonProperty]
        public int rank { get; set; }
        [JsonProperty]
        public double? price_usd { get; set; }
        [JsonProperty]
        public double? price_btc { get; set; }
        [JsonProperty]
        public double? market_cap_usd { get; set; }
        [JsonProperty]
        public double? percent_change_1h { get; set; }
        [JsonProperty]
        public double? percent_change_24h { get; set; }
        [JsonProperty]
        public double? percent_change_7d { get; set; }
    }
    public class Rates
    {
        public double USD { get; set; }
    }

    public class Forex
    {
        [JsonProperty("base")]
        public string baseCurrency { get; set; }
        [JsonProperty]
        public string date { get; set; }
        [JsonProperty("rates")]
        public Rates rates { get; set; }
    }

    public class CoinmarketcapAPI
    {
        private const string URL = "https://api.coinmarketcap.com/v1/ticker/?limit=50";

        List<CoinmarketcapCurrencies> model = new List<CoinmarketcapCurrencies>();
        Forex forex;

        public CoinmarketcapAPI()
        {
            Refresh();
        }
        public void Refresh()
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    model = JsonConvert.DeserializeObject<List<CoinmarketcapCurrencies>>(reader.ReadToEnd());
                }
                model.Add(new CoinmarketcapCurrencies() { name = "USD", price_usd = 1, symbol = "USD" });
                model.Add(new CoinmarketcapCurrencies() { name = "EUR", price_usd = GetEURPrice(), symbol = "EUR" });
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }

        public double? GetEURPrice()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.fixer.io/latest?symbols=USD,EUR");
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    forex = JsonConvert.DeserializeObject<Forex>(reader.ReadToEnd());
                }

            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
            return forex.rates.USD;
        }



        public double GetUSD(string CurrencySymbol)
        {
            foreach (CoinmarketcapCurrencies cc in model)
            {
                if (cc.symbol.ToLower() == CurrencySymbol.ToLower())
                    return (double)cc.price_usd;
            }
            return 0;
        }
        public List<CoinmarketcapCurrencies> GetAllCurrencies()
        {
            return model;
        }

    }
}