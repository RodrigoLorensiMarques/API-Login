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

namespace API_Login.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly LoginContext _context;
        private readonly string _secretKey;

        public UsuarioController(LoginContext context, IConfiguration configuration)
        {
            _context = context;
            _secretKey = configuration.GetSection("JwtSettings:SecretKey").Value;
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
                    var token = GenerateJwtToken(usuarioBanco.NomeUsuario, usuarioBanco.id);
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

        private string GenerateJwtToken(string username, int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim ("id",userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
