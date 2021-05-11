using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public Guid CredentialId { get; set; }
        public Credential Credential { get; set; }
    }
}
