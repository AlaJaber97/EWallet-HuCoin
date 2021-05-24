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
            var message = BlockChain.CreateTransaction(transaction);
            return Ok(message);
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

        [HttpPost("recharge/balance")]
        public IActionResult RechargeBalance(int number_card, string ownerAddress)
        {
            var CardCharge = Recharger.GetChargeCards().FirstOrDefault(item => item.NumberCard == number_card);
            if(CardCharge != null)
            {
                var transaction = new Models.Transaction
                {
                    Sender = "0",
                    Recipient = ownerAddress,
                    Amount = CardCharge.Value,
                    Fees = 0
                };
                if (Recharger.UseCard(CardCharge))
                {
                    BlockChain.RechargeBalance(transaction);
                    return Ok($"recharge your balance with {CardCharge.Value} HU coin");
                }
                else
                {
                    return Problem("can not use this card");
                }
            }
            return BadRequest("can not found this number card");
        }
        [HttpPost("new/recharge")]
        public IActionResult CreateChargeCards(int count, int value)
        {
            return Ok(Recharger.CreateChargeCards(count, value));
        }
        [HttpPost("list/recharge")]
        public IActionResult GetChargeCards()
        {
            return Ok(Recharger.GetChargeCards());
        }
        [HttpPost("cashout")]
        public IActionResult Cashout(decimal amount, string ownerAddress)
        {
            var transaction = new Models.Transaction
            {
                Sender = ownerAddress,
                Recipient = "0",
                Amount = amount,
                Fees = 0
            };
            BlockChain.RechargeBalance(transaction);
            return Ok($"The cash withdrawal of {amount} HU was successful");
        }

    }
}
