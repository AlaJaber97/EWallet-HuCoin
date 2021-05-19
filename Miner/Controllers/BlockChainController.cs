using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockChainController : ControllerBase
    {
        public readonly Services.CryptoCurrency BlockChain;
        public BlockChainController(Services.CryptoCurrency blockChain)
        {
            this.BlockChain = blockChain;
        }
        [HttpGet("get/transactions")]
        public IActionResult GetTransactions()
        {
            return Ok(BlockChain.GetTransactions());
        }
        [HttpPost("get/transactions")]
        public IActionResult GetTransactions(string ownerAddress)
        {
            return Ok(BlockChain.GetTransactionsByAddress(ownerAddress));
        }
        [HttpPost("new/transaction")]
        public IActionResult CreateTransaction(Models.Transaction transaction)
        {
            return Ok(BlockChain.CreateTransaction(transaction));
        }
        [HttpPost("get/balance")]
        public IActionResult GetBalance(string ownerAddress)
        {
            return Ok(BlockChain.GetBalance(ownerAddress));
        }
        [HttpGet("chain")]
        public IActionResult GetBlocks()
        {
            return Ok(BlockChain.GetBlocks());
        }
        [HttpPost("main")]
        public IActionResult Mine()
        {
            return Ok(BlockChain.Mine());
        }
        [HttpGet("get/nodes")]
        public IActionResult GetNodes()
        {
            return Ok(BlockChain.GetNodes());
        }
        [HttpPost("register/nodes")]
        public IActionResult RegisterNodes(string[] nodes)
        {
            BlockChain.RegisterNodes(nodes);
            return Ok($"you add {nodes.Length} Nodes....");
        }

        [HttpGet("resolve/nodes")]
        public IActionResult Resolve(string[] nodes)
        {
            return Ok(BlockChain.Consensus());
        }

        [HttpGet("miner/key")]
        public IActionResult GetMinerKey()
        {
            return Ok(BlockChain.GetCredential());
        }

    }
}
