using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Settings
{
    public static class Connections
    {
        public static readonly BLL.Enums.DevServer Server = BLL.Enums.DevServer.AzureProduction;
        public static readonly BLL.Enums.MinerServer Miner = BLL.Enums.MinerServer.Production;
        public static readonly BLL.Enums.DbServer DbServer = BLL.Enums.DbServer.AzureDatabase;
        public static string GetServerAddress()
        {
            return Server switch
            {
                Enums.DevServer.Ansam => "http://192.168.0.171:5000",
                Enums.DevServer.Ammar => "http://192.168.0.199:5000",
                Enums.DevServer.AzureProduction => "https://hucoinapi.azurewebsites.net",
                _ => "http://192.168.0.171:5000",
            };
        }
        public static string GetMinerAddress()
        {
            return Miner switch
            {
                BLL.Enums.MinerServer.Development=> "http://localhost:5378",
                //BLL.Enums.MinerServer.Miner_JO => "http://localhost:5555",
                //BLL.Enums.MinerServer.Miner_SA => "http://localhost:4444",
                BLL.Enums.MinerServer.Production => "https://hucoinminer.azurewebsites.net",
                _ => throw new NotImplementedException(),
            };
        }
        public static string GetConnectionStringDatabase()
        {
            return DbServer switch
            {
                BLL.Enums.DbServer.LocalDatabase=> "Server=(localdb)\\mssqllocaldb;Database=HuCoinDB;Trusted_Connection=True;",
                BLL.Enums.DbServer.AzureDatabase=> "Server=tcp:hucoindbserver.database.windows.net,1433;Initial Catalog=HuCoinDB;Persist Security Info=False;User ID=std.hu.edu.jo;Password=hucoin_97;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;",
              _ => throw new NotImplementedException(),
            };
        }
    }
}
