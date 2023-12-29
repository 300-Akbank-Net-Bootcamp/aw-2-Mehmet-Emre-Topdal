using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vb.Data;
using Vb.Data.Entity;

namespace VbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly VbDbContext dbContext;

        public AddressesController(VbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<List<Address>> Get()
        {
            return await dbContext.Set<Address>()
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Address> GetById(int id)
        {
            var address = await dbContext.Set<Address>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return address;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Address address)
        {
            if (ModelState.IsValid)
            {
                await dbContext.Set<Address>().AddAsync(address);
                await dbContext.SaveChangesAsync();

                return Ok(address);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Address updatedAddress)
        {
            var existingAddress = await dbContext.Set<Address>().FindAsync(id);

            if (existingAddress == null)
            {
                return NotFound();
            }

            existingAddress.Address1 = updatedAddress.Address1;
            existingAddress.Address2 = updatedAddress.Address2;
            existingAddress.Country = updatedAddress.Country;
            existingAddress.City = updatedAddress.City;
            existingAddress.County = updatedAddress.County;
            existingAddress.PostalCode = updatedAddress.PostalCode;
            existingAddress.IsDefault = updatedAddress.IsDefault;

            await dbContext.SaveChangesAsync();

            return Ok(existingAddress);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var addressToDelete = await dbContext.Set<Address>().FindAsync(id);

            if (addressToDelete == null)
            {
                return NotFound();
            }

            addressToDelete.IsActive = false;
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
