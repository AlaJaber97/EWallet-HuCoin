using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BLL.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string FamilyName { get; set; }
        public string Email { get; set; }
        public int UniversityID { get; set; }
        public byte[] Image { get; set; }

        public Guid WalletId { get; set; }
        public Wallet Wallet { get; set; }

        [NotMapped]
        public string Password { get; set; }
    }
}
