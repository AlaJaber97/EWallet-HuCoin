using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class APIContext : IdentityDbContext<BLL.Models.User>
    {
        public DbSet<BLL.Models.Wallet> Wallets { get; set; }
        public DbSet<BLL.Models.Credential> Credentials { get; set; }
        public APIContext(DbContextOptions options):base(options)
        {

        }
    }
}
