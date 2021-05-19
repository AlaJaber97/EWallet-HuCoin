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
        public async Task<IActionResult> SendTransaction(BLL.Models.Transaction transaction)
        {
            var userRecipient = _context.Users.Include(user => user.Wallet).ThenInclude(wallet => wallet.Credential)
                                .SingleOrDefault(user => user.PhoneNumber == transaction.RecipientPhoneNumber);
            if (userRecipient?.Wallet?.Credential == null) return NotFound($"can not found recipient with phone number {transaction.RecipientPhoneNumber}");

            transaction.Recipient = userRecipient.Wallet.Credential.PublicKey;

            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsJsonAsync($"{BLL.Settings.Connections.GetMinerAddress()}/api/blockchain/new/transaction", transaction);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return Ok("Transaction Successfully!");
            else
                return Problem(result);
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
            var response = await httpClient.PostAsJsonAsync($"{BLL.Settings.Connections.GetMinerAddress()}/api/blockchain/get/transactions", ownerAddress);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return Ok(result);
            else
                return Problem(result);
        }

        [HttpPost("get/balance")]
        public async Task<IActionResult> GetBalance(string ownerAddress)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsJsonAsync($"{BLL.Settings.Connections.GetMinerAddress()}/api/blockchain/get/balance", ownerAddress);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return Ok(result);
            else
                return Problem(result);
        }
    }
}
