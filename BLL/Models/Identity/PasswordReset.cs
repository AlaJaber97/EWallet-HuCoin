using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class PasswordReset
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
