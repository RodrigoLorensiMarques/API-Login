using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using API_Login.Context;
using API_Login.Entities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API_Login.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly LoginContext _context;

        public UsuarioController(LoginContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Acesso(string nome, string senha)
        {
            var NomeUsuarioBanco = _context.Usuarios.Where(x => x.NomeUsuario == nome).Select(x => x.NomeUsuario).SingleOrDefault();


            if (NomeUsuarioBanco != null)
            {
                var SenhaUsuarioBanco = _context.Usuarios.Where(x => x.NomeUsuario == nome).Select(x => x.SenhaUsuario).SingleOrDefault();

                if (SenhaUsuarioBanco.ToString() == senha.ToString())
                {
                    return Ok("Acesso liberado");
                }

                else
                {
                    return Unauthorized("Senha incorreta");
                }
            }

            else{
                return NotFound("Usuário não localizado");
            }



        }






    }
}