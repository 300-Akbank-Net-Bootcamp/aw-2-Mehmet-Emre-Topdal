using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vb.Data;
using Vb.Data.Entity;

namespace VbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EftTransactionsController : ControllerBase
    {
        private readonly VbDbContext dbContext;

        public EftTransactionsController(VbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<List<EftTransaction>> Get()
        {
            return await dbContext.Set<EftTransaction>()
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<EftTransaction> GetById(int id)
        {
            var eftTransaction = await dbContext.Set<EftTransaction>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return eftTransaction;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EftTransaction eftTransaction)
        {
            if (ModelState.IsValid)
            {
                await dbContext.Set<EftTransaction>().AddAsync(eftTransaction);
                await dbContext.SaveChangesAsync();

                return Ok(eftTransaction);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EftTransaction updatedEftTransaction)
        {
            var existingEftTransaction = await dbContext.Set<EftTransaction>().FindAsync(id);

            if (existingEftTransaction == null)
            {
                return NotFound();
            }

            existingEftTransaction.ReferenceNumber = updatedEftTransaction.ReferenceNumber;
            existingEftTransaction.TransactionDate = updatedEftTransaction.TransactionDate;
            existingEftTransaction.Amount = updatedEftTransaction.Amount;
            existingEftTransaction.Description = updatedEftTransaction.Description;
            existingEftTransaction.SenderAccount = updatedEftTransaction.SenderAccount;
            existingEftTransaction.SenderIban = updatedEftTransaction.SenderIban;
            existingEftTransaction.SenderName = updatedEftTransaction.SenderName;


            await dbContext.SaveChangesAsync();

            return Ok(existingEftTransaction);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eftTransactionToDelete = await dbContext.Set<EftTransaction>().FindAsync(id);

            if (eftTransactionToDelete == null)
            {
                return NotFound();
            }

            eftTransactionToDelete.IsActive = false;
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
