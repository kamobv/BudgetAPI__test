using BudgetAPI.Data;
using BudgetAPI.Filters;
using BudgetAPI.Helpers;
using BudgetAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly BudgetContext _context;
        public AuthController(BudgetContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register register)
        {
            if (_context.Users.Any(u => u.Email == register.Email))
            {
                ModelState.AddModelError("Email", "This email is already exsisit!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User newUser = new User
            {
                FullName = register.FullName,
                Password = Helper.HashPassword(register.Password),
                Email = register.Email,
                CreateAt = DateTime.Now,
                Token = Guid.NewGuid().ToString()
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                newUser.Token
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = _context.Users.FirstOrDefault(c => c.Email == login.Email);

            if (user != null)
            {
                if (Helper.CehckPassword(login.Password, user.Password))
                {
                    user.Token = Guid.NewGuid().ToString();
                    user.TokenExpireDate = DateTime.Now.AddMinutes(3);
                    await _context.SaveChangesAsync();

                    return Ok(new
                    {
                        user.Token,
                        user.TokenExpireDate
                    });
                }
            }

            return BadRequest(new { Error = "Email or Password is wrong!" });
        }

        [TypeFilter(typeof(Auth))]
        public async Task<IActionResult> LogOut()
        {
            string token = "";
            Request.Headers.TryGetValue("token", out var headerValue).ToString();
            token = headerValue.ToString();

            User user = _context.Users.FirstOrDefault(c => c.Token == token);

            if (user != null)
            {
                user.Token = null;
                user.TokenExpireDate = null;
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
