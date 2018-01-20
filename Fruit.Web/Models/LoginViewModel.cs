using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fruit.Web.Models
{
    public class LoginViewModel
    {
        [Display(Name="登录名")]
        public string Username { get; set; }

        [Display(Name = "密码"), DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name="记住我")]
        public bool RemeberMe { get; set; }
    }
}