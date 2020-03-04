using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserManager.API.Model
{
    public class UserChangePwd
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [RegularExpression(@"(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{6,16}")]
        public string Password { get; set; }
        [Required]
        [RegularExpression(@"(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{6,16}")]
        public string NewPwd { get; set; }
    }
}
