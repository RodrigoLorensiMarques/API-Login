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
        public async Task<IActionResult> Acess(string name, string password)
        {
            var userDatabase = await _context.Users.AsNoTracking().SingleOrDefaultAsync(x => x.UserName == name);

            if (userDatabase != null)
            {
                bool verified = BCrypt.Net.BCrypt.Verify(password, userDatabase.UserPassword);

                if (verified == true)
                {
                    var token = _tokenService.GenerateJwtToken(userDatabase.UserName, userDatabase.id, userDatabase.Role);
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
        public async Task<IActionResult> Register(User newUser)
        {
            var userDatabase = await _context.Users.AsNoTracking().Where(x => x.UserName == newUser.UserName).ToListAsync();

            if (userDatabase.Any())
            {
                return BadRequest("Nome de usuário não disponível");
            }

            else
            {
                string PasswordHash = BCrypt.Net.BCrypt.HashPassword(newUser.UserPassword);

                newUser.UserPassword = PasswordHash;

                newUser.Role = "Comum";

                _context.Users.Add(newUser);
                _context.SaveChangesAsync();
                return Ok("Usuário cadastrado");
            }
        }

        [HttpPost("administrator")]
        public async Task<IActionResult> CreateUserAdmin(User newUserAdmin)
        {
            var userDatabase = await _context.Users.AsNoTracking().Where(x => x.UserName == newUserAdmin.UserName).ToListAsync();

            if (userDatabase.Any())
            {
                return BadRequest("Nome de usuário não disponível");
            }

            else
            {
                string PasswordHash = BCrypt.Net.BCrypt.HashPassword(newUserAdmin.UserPassword);

                newUserAdmin.UserPassword = PasswordHash;

                newUserAdmin.Role = "Administrator";

                _context.Users.Add(newUserAdmin);
                _context.SaveChangesAsync();
                return Ok("Usuário cadastrado");
            }
        }


        [Authorize]
        [HttpGet("protected")]
        public IActionResult Protected()
        {
            var userName = User.Identity.Name;
            return Ok($"Acesso permitido para {userName}");
        }

        [Authorize(Roles ="Administrator")]
        [HttpGet("administrativo")]
        public IActionResult Administrativo()
        {
            var userName = User.Identity.Name;
            return Ok($"Acesso permitido para {userName}");
        }


    }
}
