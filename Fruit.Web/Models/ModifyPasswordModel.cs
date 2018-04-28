using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fruit.Web.Models
{
    public class ModifyPasswordModel
    {
         
        public string UserCode { get; set; }

      
        public string oldPassword { get; set; }
        public string newPassword { get; set; }

 
    }
}