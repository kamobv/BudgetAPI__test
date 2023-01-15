using BudgetAPI.Data;
using BudgetAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetAPI.Filters
{
    public class Auth : ActionFilterAttribute
    {
        private readonly BudgetContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Auth(IHttpContextAccessor httpContextAccessor, BudgetContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string token = "";
            _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("token", out var headerValue).ToString();
            token = headerValue.ToString();

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new BadRequestObjectResult(new
                {
                    Error = "Token is missing!"
                });
                return;
            }

            User user = _context.Users.FirstOrDefault(c => c.Token == token && DateTime.Now <= c.TokenExpireDate);

            if (user == null)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    Error = "Token is wrong!"
                });
                return;
            }
        }
    }
}
