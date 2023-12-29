using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vb.Data;
using Vb.Data.Entity;

namespace VbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly VbDbContext dbContext;

        public AccountsController(VbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<List<Account>> Get()
        {
            return await dbContext.Set<Account>()
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Account> GetById(int id)
        {
            var account = await dbContext.Set<Account>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return account;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Account account)
        {
            if (ModelState.IsValid)
            {
                await dbContext.Set<Account>().AddAsync(account);
                await dbContext.SaveChangesAsync();

                return Ok(account);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Account updatedAccount)
        {
            var existingAccount = await dbContext.Set<Account>().FindAsync(id);

            if (existingAccount == null)
            {
                return NotFound();
            }

            existingAccount.AccountNumber = updatedAccount.AccountNumber;
            existingAccount.IBAN = updatedAccount.IBAN;
            existingAccount.Balance = updatedAccount.Balance;
            existingAccount.CurrencyType = updatedAccount.CurrencyType;
            existingAccount.Name = updatedAccount.Name;
            existingAccount.OpenDate = updatedAccount.OpenDate;

            await dbContext.SaveChangesAsync();

            return Ok(existingAccount);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var accountToDelete = await dbContext.Set<Account>().FindAsync(id);

            if (accountToDelete == null)
            {
                return NotFound();
            }

            accountToDelete.IsActive = false;
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
