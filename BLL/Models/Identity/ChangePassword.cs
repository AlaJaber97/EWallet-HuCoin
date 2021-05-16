using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class ChangePassword
    {
        public string UserName { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
