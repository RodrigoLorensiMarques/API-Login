using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data.Context;
using WebApi.Entities;
using Microsoft.IdentityModel.Tokens;
using WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApi.DTOs;

namespace WebApi.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public UserController(AppDbContext context, TokenService tokenService)
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
        public async Task<IActionResult> CreateUserCustomer(CreateUserDTO input)
        {
            try
            {
                var userDatabase = await _context.Users.AsNoTracking().Where(x => x.Name == input.Name).ToListAsync();

                if (userDatabase.Any())
                {
                    return BadRequest("Nome de usuário não disponível");
                }

                int CountCharPassword = input.Password.Count();

                if(CountCharPassword < 6)
                {
                    return BadRequest("Senha deve possuir pelo menos 6 caracteres");
                }

                string PasswordHash = BCrypt.Net.BCrypt.HashPassword(input.Password);

                User newUser = new User();
                newUser.Name = input.Name;
                newUser.FirstName = input.FirstName;
                newUser.LastName = input.LastName;
                newUser.Email = input.Email;
                newUser.Password = PasswordHash;
                newUser.Role = "customer";
                newUser.CreateDate = DateTime.UtcNow;

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                return Ok("Usuário cadastrado com sucesso!");
            
            }
            catch (Exception)
            {
                return StatusCode(500, "01X29 - Ocorreu um erro interno ao processar sua solicitação");
            }
        }


        [HttpPost("administrator")]
        public async Task<IActionResult> CreateUserAdmin(CreateUserDTO input)
        {
            try
            {
                var userDatabase = await _context.Users.AsNoTracking().Where(x => x.Name == input.Name).ToListAsync();

                if (userDatabase.Any())
                {
                    return BadRequest("Nome de usuário não disponível");
                }

                int CountCharPassword = input.Password.Count();

                if(CountCharPassword < 6)
                {
                    return BadRequest("Senha deve possuir pelo menos 6 caracteres");
                }
                
                string PasswordHash = BCrypt.Net.BCrypt.HashPassword(input.Password);

                User newUserAdmin = new User();
                newUserAdmin.Name = input.Name;
                newUserAdmin.FirstName = input.FirstName;
                newUserAdmin.LastName = input.LastName;
                newUserAdmin.Email = input.Email;
                newUserAdmin.Password = PasswordHash;
                newUserAdmin.Role = "administrator";
                newUserAdmin.CreateDate = DateTime.UtcNow;

                _context.Users.Add(newUserAdmin);
                await _context.SaveChangesAsync();
                return Ok("Usuário cadastrado com sucesso!");  
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
