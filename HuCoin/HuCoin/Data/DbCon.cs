using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HuCoin.Data
{
    public class DbCon : DbContext
    {
        public DbSet<HuCoin.Models.Beneficiary> Beneficiaries { get; set; }
        public DbCon()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var fullpath = Path.Combine(Xamarin.Essentials.FileSystem.CacheDirectory, "DBLite.db3");
            if (!File.Exists(fullpath)) File.Create(fullpath);
            optionsBuilder.UseSqlite($"Filename={fullpath}");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
