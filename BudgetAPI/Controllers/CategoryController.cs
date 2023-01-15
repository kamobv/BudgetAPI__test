using BudgetAPI.Data;
using BudgetAPI.Filters;
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
    [TypeFilter(typeof(Auth))]
    public class CategoryController : ControllerBase
    {
        private readonly BudgetContext _context;
        public CategoryController(BudgetContext context)
        {
            _context = context;
        }
        public IActionResult GetAllCategores()
        {
            string token = "";
            Request.Headers.TryGetValue("token", out var headerValue).ToString();
            token = headerValue.ToString();

            User user = _context.Users.FirstOrDefault(c => c.Token == token);

            var categories = _context.Categories.Where(c => c.UserId == user.Id).ToList();

            var response = categories.Select(c => new
            {
                c.Id,
                c.Name
            }).ToList();

            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            string token = "";
            Request.Headers.TryGetValue("token", out var headerValue).ToString();
            token = headerValue.ToString();

            User user = _context.Users.FirstOrDefault(c => c.Token == token);

            if (_context.Categories.Any(c => c.Name == category.Name))
            {
                ModelState.AddModelError("Name", "There is this category already!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Category newCategory = new Category
            {
                Name = category.Name,
                UserId = user.Id
            };

            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();

            return Ok(new CategoryDto
            {
                Id = newCategory.Id,
                Name = newCategory.Name
            });
        }
        [HttpPut]
        public async Task<IActionResult> Edit([FromQuery] int id, [FromBody] Category category)
        {
            string token = "";
            Request.Headers.TryGetValue("token", out var headerValue).ToString();
            token = headerValue.ToString();

            User user = _context.Users.FirstOrDefault(c => c.Token == token);

            var categoryFromDb = _context.Categories.FirstOrDefault(c => c.Id == id && c.UserId == user.Id);

            if (categoryFromDb == null)
            {
                return BadRequest("Category not found!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            categoryFromDb.Name = category.Name;
            await _context.SaveChangesAsync();

            return Ok(new CategoryDto
            {
                Id = categoryFromDb.Id,
                Name = categoryFromDb.Name
            });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            string token = "";
            Request.Headers.TryGetValue("token", out var headerValue).ToString();
            token = headerValue.ToString();

            User user = _context.Users.FirstOrDefault(c => c.Token == token);

            var categoryFromDb = _context.Categories.FirstOrDefault(c => c.Id == id && c.UserId == user.Id);

            if (categoryFromDb == null)
            {
                return BadRequest("Category not found!");
            }

            _context.Categories.Remove(categoryFromDb);
            await _context.SaveChangesAsync();

            return Ok();
        }
        public IActionResult GetCategoryById(int id)
        {
            string token = "";
            Request.Headers.TryGetValue("token", out var headerValue).ToString();
            token = headerValue.ToString();

            User user = _context.Users.FirstOrDefault(c => c.Token == token);

            var categoryFromDb = _context.Categories.FirstOrDefault(c => c.Id == id && c.UserId == user.Id);

            if (categoryFromDb == null)
            {
                return BadRequest("Category not found!");
            }

            return Ok(new CategoryDto
            {
                Id = categoryFromDb.Id,
                Name = categoryFromDb.Name
            });
        }
    }
}
