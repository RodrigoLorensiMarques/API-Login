using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Login.Entities
{
    public class User
    {

        public int id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Role{ get; set; }
    }
}