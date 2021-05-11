using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class Credential
    {
        public Guid Id { get; set; }

        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }
}
