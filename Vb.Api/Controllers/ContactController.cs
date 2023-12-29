using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vb.Data;
using Vb.Data.Entity;

namespace VbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly VbDbContext dbContext;

        public ContactsController(VbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<List<Contact>> Get()
        {
            return await dbContext.Set<Contact>()
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Contact> GetById(int id)
        {
            var contact = await dbContext.Set<Contact>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return contact;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Contact contact)
        {
            if (ModelState.IsValid)
            {
                await dbContext.Set<Contact>().AddAsync(contact);
                await dbContext.SaveChangesAsync();

                return Ok(contact);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Contact updatedContact)
        {
            var existingContact = await dbContext.Set<Contact>().FindAsync(id);

            if (existingContact == null)
            {
                return NotFound();
            }

            existingContact.ContactType = updatedContact.ContactType;
            existingContact.Information = updatedContact.Information;
            existingContact.IsDefault = updatedContact.IsDefault;

            await dbContext.SaveChangesAsync();

            return Ok(existingContact);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var contactToDelete = await dbContext.Set<Contact>().FindAsync(id);

            if (contactToDelete == null)
            {
                return NotFound();
            }

            contactToDelete.IsActive = false;
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
