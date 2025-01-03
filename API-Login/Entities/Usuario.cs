using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Login.Entities
{
    public class User
    {

        public int id { get; set; }
        
        public string UserName { get; set; }

        public string UserPassword { get; set; }

        public string Role{ get; set; }
    }
}