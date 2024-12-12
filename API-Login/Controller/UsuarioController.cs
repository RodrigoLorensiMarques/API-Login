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
            var usuarioBanco = _context.Usuarios.SingleOrDefault(x => x.NomeUsuario == nome);

            if (usuarioBanco != null)
            {
                bool verified = BCrypt.Net.BCrypt.Verify(senha, usuarioBanco.SenhaUsuario);

                if (verified == true)
                {
                    return Ok("Acesso liberado");
                }
                return Unauthorized("Credenciais incorretas");
            }

            else 
            {
                return NotFound("Credenciais incorretas");
            }
        }
        

        [HttpPost]
        public IActionResult Cadastrar (Usuario usuarioRecebido)
        {
            var usuarioBanco = _context.Usuarios.Where(x => x.NomeUsuario == usuarioRecebido.NomeUsuario);

            if (usuarioBanco.IsNullOrEmpty())
            {
                string PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuarioRecebido.SenhaUsuario);

                usuarioRecebido.SenhaUsuario = PasswordHash;

                _context.Usuarios.Add(usuarioRecebido);
                _context.SaveChanges();
                return Ok("Usuário cadastrado");
            }

            else
            {
                return BadRequest("Nome de usuário não disponível");
            }
        }
    }
}