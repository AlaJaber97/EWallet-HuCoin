using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(JwtBearerDefaults.AuthenticationScheme)]
    public class EWalletController : ControllerBase
    {
        public readonly API.Data.APIContext _context;
        public EWalletController(API.Data.APIContext context)
        {
            this._context = context;
        }
        [HttpPost("send/transaction")]
        public async Task<IActionResult> SendTransaction(BLL.Models.TransactionClient transactionClient)
        {
            var userSender = _context.Users.Include(user => user.Wallet).ThenInclude(wallet => wallet.Credential)
                                .SingleOrDefault(user => user.PhoneNumber == transactionClient.SenderPhoneNumber);
            if (userSender?.Wallet?.Credential == null) return NotFound($"can not found sender with phone number {transactionClient.RecipientPhoneNumber}");
            
            var userRecipient = _context.Users.Include(user => user.Wallet).ThenInclude(wallet => wallet.Credential)
                                .SingleOrDefault(user => user.PhoneNumber == transactionClient.RecipientPhoneNumber);
            if (userRecipient?.Wallet?.Credential == null) return NotFound($"can not found recipient with phone number {transactionClient.RecipientPhoneNumber}");
            var transactionMiner = new BLL.Models.TransactionMiner
            {
                Sender = userSender.Wallet.Credential.PublicKey,
                Recipient = userRecipient.Wallet.Credential.PublicKey,
                Amount = transactionClient.Amount,
                Fees = transactionClient.Fees,
            };
            transactionMiner.Signature = Utils.RSA.SignatureGenerate(userSender.Wallet.Credential.PrivateKey, transactionMiner.ToString());

            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsJsonAsync($"{BLL.Settings.Connections.GetMinerAddress()}/api/blockchain/new/transaction", transactionMiner);
            var result = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, result);
        }

        [HttpPost("get/address_user")]
        [AllowAnonymous]
        public IActionResult GetAddressUser(string PhoneNumber)
        {
            var userRecipient = _context.Users.Include(user => user.Wallet).ThenInclude(wallet => wallet.Credential)
                                .SingleOrDefault(user => user.PhoneNumber == PhoneNumber);
            if (userRecipient?.Wallet?.Credential == null) return NotFound($"can not found recipient with phone number {PhoneNumber}");

            return Ok(userRecipient.Wallet.Credential.PublicKey);
        }

        [HttpPost("get/transactions")]
        public async Task<IActionResult> GetTransactions(string PhoneNumber)
        {
            var userRecipient = _context.Users.Include(user => user.Wallet).ThenInclude(wallet => wallet.Credential)
                                    .SingleOrDefault(user => user.PhoneNumber == PhoneNumber);
            if (userRecipient?.Wallet?.Credential == null) return NotFound($"can not found recipient with phone number {PhoneNumber}");

            var ownerAddress = userRecipient.Wallet.Credential.PublicKey;

            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{BLL.Settings.Connections.GetMinerAddress()}/api/blockchain/get/transactions?ownerAddress={ownerAddress}", null);
            var result = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, result);
        }

        [HttpPost("get/balance")]
        public async Task<IActionResult> GetBalance(string ownerAddress)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{BLL.Settings.Connections.GetMinerAddress()}/api/blockchain/get/balance?ownerAddress={ownerAddress}", null);
            var result = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, result);
        }
    }
}
