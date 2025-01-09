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
    public class UserController : ControllerBase
    {
        private readonly LoginContext _context;
        private readonly TokenService _tokenService;

        public UserController(LoginContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<IActionResult> Acess(string name, string password)
        {
            try
            {
                var userDatabase = await _context.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Name == name);

                if (userDatabase != null)
                {
                    bool verified = BCrypt.Net.BCrypt.Verify(password, userDatabase.Password);

                    if (verified == true)
                    {
                        var token = _tokenService.GenerateJwtToken(userDatabase.Name, userDatabase.Id, userDatabase.Role);
                        return Ok(new {message="Acesso Liberado", token });
                    }
                    return BadRequest("Credenciais incorretas ou usuário não existe");
                }
                else 
                {
                    return BadRequest("Credenciais incorretas ou usuário não existe");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "01X35 - Ocorreu um erro interno ao processar sua solicitação");
            }
        }

        [HttpPost("customer")]
        public async Task<IActionResult> CreateUserCustomer(User newUser)
        {
            try
            {
                var userDatabase = await _context.Users.AsNoTracking().Where(x => x.Name == newUser.Name).ToListAsync();

                if (userDatabase.Any())
                {
                    return BadRequest("Nome de usuário não disponível");
                }

                else
                {
                    string PasswordHash = BCrypt.Net.BCrypt.HashPassword(newUser.Password);

                    newUser.Password = PasswordHash;

                    newUser.Role = "customer";

                    newUser.CreateDate = DateTime.UtcNow;

                    _context.Users.Add(newUser);
                    await _context.SaveChangesAsync();
                    return Ok("Usuário cadastrado com sucesso!");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "01X29 - Ocorreu um erro interno ao processar sua solicitação");
            }
        }


        [HttpPost("administrator")]
        public async Task<IActionResult> CreateUserAdmin(User newUserAdmin)
        {
            try
            {
                var userDatabase = await _context.Users.AsNoTracking().Where(x => x.Name == newUserAdmin.Name).ToListAsync();

                if (userDatabase.Any())
                {
                    return BadRequest("Nome de usuário não disponível");
                }

                else
                {
                    string PasswordHash = BCrypt.Net.BCrypt.HashPassword(newUserAdmin.Password);

                    newUserAdmin.Password = PasswordHash;

                    newUserAdmin.Role = "administrator";

                    newUserAdmin.CreateDate = DateTime.UtcNow;

                    _context.Users.Add(newUserAdmin);
                    await _context.SaveChangesAsync();
                    return Ok("Usuário cadastrado com sucesso!");
                    }
            }
            catch (Exception)
            {
                return StatusCode(500, "01X24 - Ocorreu um erro interno ao processar sua solicitação");
            }
        }


        [Authorize]
        [HttpGet("protected")]
        public IActionResult Protected()
        {
            try
            {
                var userName = User.Identity.Name;
                return Ok($"Acesso permitido para {userName}");
            }
            catch (Exception)
            {
                return StatusCode(500, "01X23 - Ocorreu um erro interno ao processar sua solicitação");
            }
        }

        [Authorize(Roles ="administrator")]
        [HttpGet("administrator")]
        public IActionResult Administrator()
        {
            try
            {
                var userName = User.Identity.Name;
                return Ok($"Acesso permitido para {userName}");
            }
            catch (Exception)
            {
                return StatusCode(500, "01X22 - Ocorreu um erro interno ao processar sua solicitação");
            }
        }

    }
}
