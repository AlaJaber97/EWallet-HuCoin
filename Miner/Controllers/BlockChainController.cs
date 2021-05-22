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
        public readonly Services.Recharger Recharger;
        public BlockChainController(Services.CryptoCurrency blockChain, Services.Recharger recharger)
        {
            this.BlockChain = blockChain;
            this.Recharger = recharger;
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
            var balance = BlockChain.GetBalance(ownerAddress);
            return Ok(balance);
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
            return Ok(BlockChain.GetCredential()?.PublicKey);
        }

        [HttpPost("recharge/balance")]
        public IActionResult RechargeBalance(int cardnumber, string ownerAddress)
        {
            var CardCharge = Recharger.ChargeCards.SingleOrDefault(item => item.Number == cardnumber);
            if (CardCharge != null)
            {
                var credential = BlockChain.GetCredential();
                var transaction = new Miner.Models.Transaction
                {
                    Sender = "0",
                    Recipient = ownerAddress,
                    Amount = (decimal)CardCharge.Value,
                    Fees =0,
                };
                transaction.Signature = Utils.RSA.SignatureGenerate(credential.PrivateKey, transaction.ToString());
                var message = BlockChain.CreateTransaction(transaction);
                if(message.Contains("Transcation Successed")) Recharger.UseCard(cardnumber);
                return Ok(message);
            }
            return BadRequest("Can not found this number card");
        }

        [HttpPost("new/recharge")]
        public IActionResult CreateRechargeCard(int count, double value)
        {
            return Ok(Recharger.CreateChargeCards(count, value));
        }
    }
}
