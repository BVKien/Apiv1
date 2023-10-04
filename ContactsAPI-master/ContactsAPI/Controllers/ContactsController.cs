using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers
{ 
    [ApiController]
    [Route("api/contacts")]
    public class ContactsController : Controller
    {
        private readonly ContactsAPIDbContext dbContext;

        public ContactsController(ContactsAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {

              return Ok(await dbContext.Contacts.ToListAsync());
        }
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult>GetContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            else
                return Ok(contact);
        }
        [HttpPost]
        public async Task<IActionResult> AddContact([FromBody] ContactViewModel addcontactRequest)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                FullName = addcontactRequest.FullName,
                Email = addcontactRequest.Email,
                Phone = addcontactRequest.Phone,
                Address = addcontactRequest.Address
            };
            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();
            return Ok(contact);
        }
       
        [Route("{id:guid}")]
        [HttpPut]
        public async Task<IActionResult> UpdateContact ([FromRoute] Guid id,[FromBody] ContactViewModel contact)
        {
            var myContact=await dbContext.Contacts.FindAsync(id);
            if (myContact != null)
            {
                myContact.FullName = contact.FullName;
                myContact.Email = contact.Email;
                myContact.Phone = contact.Phone;
                myContact.Address = contact.Address;
                await dbContext.SaveChangesAsync();
                return Ok(myContact);
            }
            return NotFound();
        }
        [Route("{id:guid}")]
        [HttpDelete]
        public async Task<IActionResult> Deletecontact([FromRoute]Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if (contact != null)
            {
                dbContext.Contacts.Remove(contact);
                await dbContext.SaveChangesAsync();
                return Ok(contact);
            }
            else
                return NotFound();
        }
    }
}
