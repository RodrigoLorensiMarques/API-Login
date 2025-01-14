using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Context;
using WebApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DTOs;

namespace WebApi.Controller
{

    [ApiController]
    [Route("[controller]")]
    public class RecoveryController : ControllerBase
    {
        public readonly AppDbContext _context;
        public RecoveryController (AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO input)
        {
            try
            {
                int CountCharPassword = input.NewPassword.Count();
                if (CountCharPassword <6)
                {
                    return BadRequest("Senha deve possuir pelo menos 6 caracteres");
                }

                var userName = User.Identity.Name;
                var userDatabase = await _context.Users.FirstOrDefaultAsync(x => x.Name == userName);

                bool verified = BCrypt.Net.BCrypt.Verify(input.CurrentPassword, userDatabase.Password);

                if (verified != true)
                {
                    return BadRequest($"A senha informada é diferente da senha atual");
                }

                var passwordHash = BCrypt.Net.BCrypt.HashPassword(input.NewPassword);
                userDatabase.Password = passwordHash;

                _context.Users.Update(userDatabase);
                await _context.SaveChangesAsync();

                return Ok("Senha alterada com sucesso!");
            }
            catch (Exception)
            {
                return StatusCode(500, "01X29 - Ocorreu um erro interno ao processar sua solicitação");
            }
        }
    }
}