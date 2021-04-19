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
            return Ok("It's work on..");
        }

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
