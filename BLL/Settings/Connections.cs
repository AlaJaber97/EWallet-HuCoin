using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Settings
{
    public static class Connections
    {
        public static readonly BLL.Enums.DevServer Server = BLL.Enums.DevServer.Ammar;
        public static readonly BLL.Enums.MinerServer Miner = BLL.Enums.MinerServer.Development;
        public static string GetServerAddress()
        {
            return Server switch
            {
                Enums.DevServer.Ansam => "http://192.168.0.171:5000",
                Enums.DevServer.Ammar => "http://192.168.0.199:5000",
                _ => "http://192.168.0.171:5000",
            };
        }
        public static string GetMinerAddress()
        {
            return Miner switch
            {
                BLL.Enums.MinerServer.Development=> "http://localhost:5378",
                BLL.Enums.MinerServer.Miner_JO => "http://localhost:5555",
                BLL.Enums.MinerServer.Miner_SA => "http://localhost:4444",
                _ => throw new NotImplementedException(),
            };
        }
    }
}
