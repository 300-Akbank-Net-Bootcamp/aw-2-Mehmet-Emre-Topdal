using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vb.Data;
using Vb.Data.Entity;

namespace VbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTransactionsController : ControllerBase
    {
        private readonly VbDbContext dbContext;

        public AccountTransactionsController(VbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<List<AccountTransaction>> Get()
        {
            return await dbContext.Set<AccountTransaction>()
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<AccountTransaction> GetById(int id)
        {
            var transaction = await dbContext.Set<AccountTransaction>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return transaction;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AccountTransaction transaction)
        {
            if (ModelState.IsValid)
            {
                await dbContext.Set<AccountTransaction>().AddAsync(transaction);
                await dbContext.SaveChangesAsync();

                return Ok(transaction);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] AccountTransaction updatedTransaction)
        {
            var existingTransaction = await dbContext.Set<AccountTransaction>().FindAsync(id);

            if (existingTransaction == null)
            {
                return NotFound();
            }

            existingTransaction.ReferenceNumber = updatedTransaction.ReferenceNumber;
            existingTransaction.TransactionDate = updatedTransaction.TransactionDate;
            existingTransaction.Amount = updatedTransaction.Amount;
            existingTransaction.Description = updatedTransaction.Description;
            existingTransaction.TransferType = updatedTransaction.TransferType;

            await dbContext.SaveChangesAsync();

            return Ok(existingTransaction);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var transactionToDelete = await dbContext.Set<AccountTransaction>().FindAsync(id);

            if (transactionToDelete == null)
            {
                return NotFound();
            }

            transactionToDelete.IsActive = false;
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
