using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Raiblocks
{
    public class AccountBalance
    {
        public string account { get; set; }
        public string balance { get; set; }
        public string pending { get; set; }
    }
    public class AccountBlock
    {
        public string hash { get; set; }
        public string amount { get; set; }
        public string source { get; set; }
    }

    public class AccountBlocks
    {
        public string account { get; set; }
        public List<AccountBlock> blocks { get; set; }
    }
    public class Block
    {
        public string hash { get; set; }
        public string block_account { get; set; }
        public string amount { get; set; }

        public string type { get; set; }
        public string account { get; set; }
        public string previous { get; set; }
        public string destination { get; set; }
        public string source { get; set; }
        public string representative { get; set; }
        public string balance { get; set; }
        public string work { get; set; }
        public string signature { get; set; }
    }
    public class BlockAdditional
    {
        public string block_account { get; set; }
        public string amount { get; set; }
        public Block contents { get; set; }
    }


    public class BlocksAdditionalResponse
    {
        public GenericContainer blocks { get; set; }
    }
    public class BlocksMultipleResponse
    {
        public GenericContainer blocks { get; set; }
    }
    public class BlocksResponse
    {
        public Block contents { get; set; }
    }

    public class ChainBlocks
    {
        public List<string> blocks { get; set; }
    }
    public class Frontier
    {
        public string account { get; set; }
        public string hash { get; set; }
    }

    public class FrontiersResponse
    {
        public GenericContainer frontiers { get; set; }
    }

    public class Work
    {
        public string account { get; set; }
        public string work { get; set; }
    }

    public class GenericContainer
    {
        [JsonExtensionData]
        public IDictionary<string, JToken> _extraStuff;
    }
    public class WorksResponse
    {
        public GenericContainer works { get; set; }
    }

    public class WorkPeers
    {
        public List<string> work_peers { get; set; }
    }
    public class PeerVersion
    {
        public string address { get; set; }
        public string version { get; set; }
    }
    public class PeersResponse
    {
        public GenericContainer peers { get; set; }
    }

    public class PendingBlocks
    {
        public List<string> blocks { get; set; }
    }
    public class History
    {
        public string hash { get; set; }
        public string type { get; set; }
        public string account { get; set; }
        public string amount { get; set; }
    }
    public class NodeVersions
    {
        public string rpc_version { get; set; }
        public string store_version { get; set; }
        public string node_vendor { get; set; }
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
}
