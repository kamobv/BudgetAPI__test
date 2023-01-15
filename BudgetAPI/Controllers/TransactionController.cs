using BudgetAPI.Data;
using BudgetAPI.Filters;
using BudgetAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [TypeFilter(typeof(Auth))]
    public class TransactionController : ControllerBase
    {
        private readonly BudgetContext _context;
        public TransactionController(BudgetContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> GetTransactionsList(TransactionSearchModel transactionSearchModel)
        {
            string token = "";
            Request.Headers.TryGetValue("token", out var headerValue).ToString();
            token = headerValue.ToString();

            User user = _context.Users.FirstOrDefault(u => u.Token == token);

            var transactionList = await _context.Transactions.Include(c => c.Category)
                                        .Where(t => t.Category.UserId == user.Id &&
                                        (transactionSearchModel.CategoryId != null ? t.CategoryId == transactionSearchModel.CategoryId : true) &&
                                        (transactionSearchModel.Type != null ? t.Type == transactionSearchModel.Type : true) &&
                                        (transactionSearchModel.StartDate != null ? t.Date >= transactionSearchModel.StartDate : true) &&
                                        (transactionSearchModel.EndDate != null ? t.Date <= transactionSearchModel.EndDate : true))
                                        .OrderByDescending(t => t.Date)
                                        .Skip((transactionSearchModel.Page - 1) * 10)
                                        .Take(10)
                                        .ToListAsync();

            var response = transactionList.Select(t => new
            {
                t.Id,
                t.Type,
                t.Amoun,
                t.Desc,
                t.CategoryId,
                t.Date
            }).ToList();

            return Ok(response);
        }
        public async Task<IActionResult> GetTransactionById(int id)
        {
            string token = "";
            Request.Headers.TryGetValue("token", out var headerValue).ToString();
            token = headerValue.ToString();

            User user = _context.Users.FirstOrDefault(u => u.Token == token);

            var transaction = await _context.Transactions.Include(t => t.Category)
                                                   .Where(t => t.Category.UserId == user.Id && t.Id == id)
                                                   .FirstOrDefaultAsync();
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                transaction.Id,
                transaction.Type,
                transaction.Amoun,
                transaction.Desc,
                transaction.CategoryId,
                transaction.Date
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create(TransactionDto transactionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string token = "";
            Request.Headers.TryGetValue("token", out var headerValue).ToString();
            token = headerValue.ToString();

            User user = await _context.Users.FirstOrDefaultAsync(u => u.Token == token);

            if (!_context.Categories.Any(c => c.UserId == user.Id && c.Id == transactionDto.CategoryId))
            {
                return BadRequest("CategoryId is not valid!");
            }

            Transaction newTransaction = new Transaction
            {
                CategoryId = transactionDto.CategoryId,
                Type = transactionDto.Type,
                Amoun = transactionDto.Amount,
                Desc = transactionDto.Desc,
                CraeteAt = System.DateTime.Now,
                Date = System.DateTime.Now
            };

            await _context.Transactions.AddAsync(newTransaction);
            await _context.SaveChangesAsync();

            return Created("", transactionDto);
        }
        [HttpPut]
        public async Task<IActionResult> Edit(int id, TransactionDto transactionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string token = "";
            Request.Headers.TryGetValue("token", out var headerValue).ToString();
            token = headerValue.ToString();

            User user = await _context.Users.FirstOrDefaultAsync(u => u.Token == token);

            var transactionFromDb = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id && t.Category.UserId == user.Id);

            if (transactionFromDb == null)
            {
                return NotFound();
            }

            if (!_context.Categories.Any(c => c.UserId == user.Id && c.Id == transactionDto.CategoryId))
            {
                return BadRequest("CategoryId is not valid!");
            }

            transactionFromDb.CategoryId = transactionDto.CategoryId;
            transactionFromDb.Type = transactionDto.Type;
            transactionFromDb.Amoun = transactionDto.Amount;
            transactionFromDb.Desc = transactionDto.Desc;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                transactionFromDb.Id,
                transactionFromDb.CategoryId,
                transactionFromDb.Type,
                transactionFromDb.Amoun,
                transactionFromDb.Desc
            });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            string token = "";
            Request.Headers.TryGetValue("token", out var headerValue).ToString();
            token = headerValue.ToString();

            User user = await _context.Users.FirstOrDefaultAsync(u => u.Token == token);

            var transactionFromDb = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id && t.Category.UserId == user.Id);

            if (transactionFromDb == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transactionFromDb);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
