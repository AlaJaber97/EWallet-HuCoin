﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class APIContext : IdentityDbContext<BLL.Models.User>
    {
        public APIContext(DbContextOptions options):base(options)
        {

        }
    }
}
