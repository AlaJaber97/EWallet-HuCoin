using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miner.Models
{
    public class Block
    {
        public int Index { get; set; }
        public DateTime Timestemp { get; set; }
        public List<Transaction> Transactions { get; set; }
        public int Proof { get; set; }
        public string PreviousHash { get; set; }
        public override string ToString() =>
            $"{Index} [{Timestemp:yyyy-MM-dd HH:mm:ss}] Proof: {Proof} | PrevHash: {PreviousHash} | Trx: {Transactions.Count}";
    }
}
