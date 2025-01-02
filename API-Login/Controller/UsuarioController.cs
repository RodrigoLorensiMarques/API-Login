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
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Acesso(string nome, string senha)
        {
            var usuarioBanco = await _context.Usuarios.AsNoTracking().SingleOrDefaultAsync(x => x.NomeUsuario == nome);

            if (usuarioBanco != null)
            {
                bool verified = BCrypt.Net.BCrypt.Verify(senha, usuarioBanco.SenhaUsuario);

                if (verified == true)
                {
                    var token = _tokenService.GenerateJwtToken(usuarioBanco.NomeUsuario, usuarioBanco.id, usuarioBanco.Role);
                    return Ok(new {message="Acesso Liberado", token });
                }
                return Unauthorized("Credenciais incorretas");
            }

            else 
            {
                return NotFound("Credenciais incorretas");
            }
        }

        [HttpPost("comum")]
        public async Task<IActionResult> Cadastrar(Usuario usuarioRecebido)
        {
            var usuarioBanco = await _context.Usuarios.AsNoTracking().Where(x => x.NomeUsuario == usuarioRecebido.NomeUsuario).ToListAsync();

            if (usuarioBanco.Any())
            {
                return BadRequest("Nome de usuário não disponível");
            }

            else
            {
                string PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuarioRecebido.SenhaUsuario);

                usuarioRecebido.SenhaUsuario = PasswordHash;

                usuarioRecebido.Role = "Comum";

                _context.Usuarios.Add(usuarioRecebido);
                _context.SaveChangesAsync();
                return Ok("Usuário cadastrado");
            }
        }

        [HttpPost("administrator")]
        public async Task<IActionResult> CadastrarAdmin(Usuario usuarioRecebido)
        {
            var usuarioBanco = await _context.Usuarios.AsNoTracking().Where(x => x.NomeUsuario == usuarioRecebido.NomeUsuario).ToListAsync();

            if (usuarioBanco.Any())
            {
                return BadRequest("Nome de usuário não disponível");
            }

            else
            {
                string PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuarioRecebido.SenhaUsuario);

                usuarioRecebido.SenhaUsuario = PasswordHash;

                usuarioRecebido.Role = "Administrator";

                _context.Usuarios.Add(usuarioRecebido);
                _context.SaveChangesAsync();
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

        [Authorize(Roles ="Administrator")]
        [HttpGet("administrativo")]
        public IActionResult Administrativo()
        {
            var username = User.Identity.Name;
            return Ok($"Acesso permitido para {username}");
        }


    }
}
