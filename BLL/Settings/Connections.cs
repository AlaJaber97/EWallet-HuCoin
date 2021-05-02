using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Settings
{
    public static class Connections
    {
        public static readonly BLL.Enums.DevServer Server = BLL.Enums.DevServer.Ammar;
        public static string GetServerAddress()
        {
            return Server switch
            {
                Enums.DevServer.Ansam => "http://192.168.0.171:5000",
                Enums.DevServer.Ammar => "http://192.168.0.199:5000",
                _ => "http://192.168.0.171:5000",
            };
        }
    }
}
