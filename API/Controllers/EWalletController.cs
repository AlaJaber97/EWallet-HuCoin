using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost("send/transaction")]
        public async Task<IActionResult> SendTransaction(BLL.Models.TransactionMiner transactionMiner)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsJsonAsync($"{BLL.Settings.Connections.GetMinerAddress()}/blockchain/new/transaction", transactionMiner);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return Ok(result);
            else
                return Problem(result);
        }

        [HttpPost("get/transactions")]
        public async Task<IActionResult> GetTransactions(string ownerAddress)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsJsonAsync($"{BLL.Settings.Connections.GetMinerAddress()}/blockchain/get/transactions", ownerAddress);
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
            var response = await httpClient.PostAsJsonAsync($"{BLL.Settings.Connections.GetMinerAddress()}/blockchain/get/balance", ownerAddress);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return Ok(result);
            else
                return Problem(result);
        }
    }
}
