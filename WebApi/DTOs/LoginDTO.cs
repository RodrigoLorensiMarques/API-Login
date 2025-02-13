using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.DTOs
{
    public class LoginDTO
    {
        public LoginDTO(string token) {
            Token = token;
        }
        public string Token { get; set; }
    }
}