using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class ResetPassword
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
