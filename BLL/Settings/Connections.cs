using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Settings
{
    public static class Connections
    {
        private static readonly BLL.Enums.DevServer Server = BLL.Enums.DevServer.Alaa;
        public static string GetServerAddress()
        {
            return Server switch
            {
                Enums.DevServer.Alaa => "http://192.168.0.199:5000",
                _ => "http://192.168.0.199:5000",
            };
        }
    }
}
