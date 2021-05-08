using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miner.Models
{
    public class Blockchain
    {
        public List<Block> Chain { get; set; }
        public int Length => Chain?.Count ?? 0;
        public Blockchain(List<Block> chain)
        {
            this.Chain = chain;
        }
    }
}
