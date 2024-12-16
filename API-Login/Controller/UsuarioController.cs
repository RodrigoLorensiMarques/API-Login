using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using API_Login.Context;
using API_Login.Entities;
using Microsoft.IdentityModel.Tokens;
using API_Login.Services;
using Microsoft.AspNetCore.Authorization;

namespace API_Login.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly LoginContext _context;
        private readonly TokenService _tokenService;

        public UsuarioController(LoginContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
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
                    var token = _tokenService.GenerateJwtToken(usuarioBanco.NomeUsuario, usuarioBanco.id);
                    return Ok(new {message="Acesso Liberado", token });
                }
                return Unauthorized("Credenciais incorretas");
            }

            else 
            {
                return NotFound("Credenciais incorretas");
            }
        }

        [HttpPost]
        public IActionResult Cadastrar(Usuario usuarioRecebido)
        {
            var usuarioBanco = _context.Usuarios.Where(x => x.NomeUsuario == usuarioRecebido.NomeUsuario);

            if (usuarioBanco.Any())
            {
                return BadRequest("Nome de usuário não disponível");
            }

            else
            {
                string PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuarioRecebido.SenhaUsuario);

                usuarioRecebido.SenhaUsuario = PasswordHash;

                _context.Usuarios.Add(usuarioRecebido);
                _context.SaveChanges();
                return Ok("Usuário cadastrado");
            }
        }

        [Authorize]
        [HttpGet("protected")]
        public IActionResult Protected()
        {
            var username = User.Identity.Name;
            return Ok($"Acesso permitido para {username}");
        }


    }
}
