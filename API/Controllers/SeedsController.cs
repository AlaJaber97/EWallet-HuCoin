using API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedsController : ControllerBase
    {
        private readonly APIContext _context;
        public SeedsController(APIContext context)
        {
            this._context = context;
        }

        [HttpGet("Status")]
        public IActionResult GetStatusServer()
        {
            return Ok(
                $"API IP Server: {BLL.Settings.Connections.GetServerAddress()}"+
                "\n" +
                $"Miner IP Server: {BLL.Settings.Connections.GetMinerAddress()}"+
                "\n" +
                $"Database Server: {BLL.Settings.Connections.GetConnectionStringDatabase()}"
                );
        }
        [HttpGet("Fill")]
        public IActionResult FillDB()
        {
            try
            {
                _context.Users.Add(new BLL.Models.User
                {
                    FirstName = "Ammar'",
                    SecondName = "Mohammed",
                    FamilyName = "Dolat",
                    Email= "ammardolat63@yahoo.com",
                    PhoneNumber = "+962777192116",
                    PhoneNumberConfirmed = true,
                    UserName = "+962777192116",
                    Wallet = new BLL.Models.Wallet
                    {
                        Credential = Utils.RSA.KeyGenerate()
                    }
                });
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.ToString(), instance: "FillDB", statusCode: StatusCodes.Status500InternalServerError);
            }
            return Ok("Successed Fill Database");
        }
        /// <summary>
        /// to use this action write in your browser "http://localhost:52600/api/seeds/create"
        /// </summary>
        /// <returns></returns>
        [HttpGet("Create")]
        public IActionResult CreateDB()
        {
            try
            {
                _context.Database.EnsureCreated();
            }
            catch(Exception ex)
            {
                return Problem(detail: ex.ToString(),instance: "CreateDB", statusCode: StatusCodes.Status500InternalServerError);
            }
            return Ok("Successed Created Database");
        }

        [HttpGet("Delete")]
        public IActionResult DeleteDB()
        {
            try
            {
                _context.Database.EnsureDeleted();
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.ToString(), instance: "DeleteDB", statusCode: StatusCodes.Status500InternalServerError);
            }
            return Ok("Successed Deleted Database");
        }
    }
}
