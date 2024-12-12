using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Login.Entities
{
    public class Usuario
    {

        public int id { get; set; }
        
        public string NomeUsuario { get; set; }

        public string SenhaUsuario { get; set; }
    }
}