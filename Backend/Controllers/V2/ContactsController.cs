using AutoMapper;
using ContactsAppApi.Data;
using ContactsAppApi.Hubs;
using ContactsAppApi.Models;
using ContactsAppApi.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsAppApi.Controllers.V2
{
    [ApiController]
    [Route("/api/v2/[controller]")]
    [Produces("application/json")]
    public class ContactsController : ControllerBase
    {
        private readonly ContactsAppDbContext context;
        private readonly IMapper mapper;
        private readonly IHubContext<NotificationHub> hubContext;

        public ContactsController(
            ContactsAppDbContext context,
            IMapper mapper,
            IHubContext<NotificationHub> hubContext)
        {
            this.context = context;
            this.mapper = mapper;
            this.hubContext = hubContext;
        }

        /// <summary>
        /// Get all contacts
        /// </summary>
        /// <returns>All contacts</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ContactDTO>> GetAll(bool allPhones) 
        {
            IQueryable<Contact> contacts = context.Contacts;

            if (allPhones)
                contacts = contacts.Include(x => x.AdditionalPhones);

            var dtos = mapper.Map<List<ContactDTO>>(contacts);
            return dtos;
        }

        /// <summary>
        /// Get contact by ID
        /// </summary>
        /// <param name="id">Contact ID</param>
        /// <returns>Contact</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ContactDTO>> GetById(string id)
        {
            var contact = await context.Contacts
                .Include(x => x.AdditionalPhones)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (contact == null)
                return NotFound();

            var contactDTO = mapper.Map<ContactDTO>(contact);

            return contactDTO;
        }

        /// <summary>
        /// Create new contact
        /// </summary>
        /// <param name="contactDto">Contact object</param>
        /// <returns>Created contact</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ContactDTO>> Create(ContactDTO contactDto)
        {
            contactDto.Id = null;
            var contact = mapper.Map<Contact>(contactDto);

            try
            {
                context.Contacts.Add(contact);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (await PhoneTaken(contactDto.Phone))
                {
                    ModelState.AddModelError("phone", "This phone number is already taken!");
                    return BadRequest(ModelState);
                }
                throw;
            }

            //return CreatedAtAction(nameof(GetByIdAsync), new { contact.Id }, contact);
            contactDto = mapper.Map<ContactDTO>(contact);

            await hubContext.Clients.All.SendAsync("notification", "Contact created!", $"{contactDto.Name} {contactDto.Surname}");
            return Created($"api/v1/contacts/{contact.Id}", contactDto);
        }

        /// <summary>
        /// Delete contact by ID
        /// </summary>
        /// <param name="id">Contact ID</param>
        /// <returns>Nothing</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(string id)
        {
            var contact = await context.Contacts.FindAsync(id);

            if (contact == null)
                return NotFound();

            context.Contacts.Remove(contact);
            await context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Replace contact by ID
        /// </summary>
        /// <param name="id">Contact ID</param>
        /// <param name="contactDto">Contact object</param>
        /// <returns>Replaced contact</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ContactDTO>> Update(string id, ContactDTO contactDto)
        {
            contactDto.Id = id;
            var contact = mapper.Map<Contact>(contactDto);

            try
            {
                context.Contacts.Update(contact);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!await ContactExists(id))
                { 
                    return NotFound();
                }
                else if (await PhoneTaken(contactDto.Phone))
                {
                    ModelState.AddModelError("phone", "This phone number is already taken!");
                    return BadRequest(ModelState);
                }

                throw;
            }

            contactDto = mapper.Map<ContactDTO>(contact);

            return contactDto;
        }

        /// <summary>
        /// Get contact by phone number
        /// </summary>
        /// <param name="phone">Phone number</param>
        /// <returns>Contact</returns>
        [HttpGet("phone/{phone}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ContactDTO>> GetByPhone(string phone)
        {
            var contact = await context.Contacts
                .Include(x => x.AdditionalPhones)
                .FirstOrDefaultAsync(x => x.Phone == phone);

            if (contact == null)
            {
                var additionalPhone = await context.AdditionalPhones
                    .FirstOrDefaultAsync(x => x.Phone == phone);
                
                if (additionalPhone == null)
                    return NotFound();

                contact = await context.Contacts
                    .Include(x => x.AdditionalPhones)
                    .FirstOrDefaultAsync(x => x.Id == additionalPhone.ContactId);

                if (contact == null)
                    return NotFound();
            }

            var contactDto = mapper.Map<ContactDTO>(contact);

            return contactDto;
        }

        /// <summary>
        /// Find contact by phone number
        /// </summary>
        /// <param name="phone">Phone number</param>
        /// <returns>Contact</returns>
        [HttpGet("search/{phone}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ContactDTO>>> SearchByPhone(string phone)
        {
            var contacts = await context.Contacts
                .Where(x => x.Phone.StartsWith(phone))
                .ToListAsync(); 

            var contactDtos = mapper.Map<List<ContactDTO>>(contacts);

            return contactDtos;
        }

        private async Task<bool> PhoneTaken(string phone)
        {
            var contactWithPhoneFound = await context.Contacts.AnyAsync(x => x.Phone == phone);
            var additionalPhoneFound = await context.AdditionalPhones.AnyAsync(x => x.Phone == phone);
            return contactWithPhoneFound || additionalPhoneFound;
        }

        private Task<bool> ContactExists(string id)
        {
            return context.Contacts.AnyAsync(x => x.Id == id); 
        }
    }
}













///// <summary>
///// Update contact by ID
///// </summary>
///// <param name="id">Contact ID</param>
///// <param name="contactPatch">Contact JSON patch object</param>
///// <returns>Updated contact</returns>
//[HttpPatch("{id}")]
//[ProducesResponseType(StatusCodes.Status200OK)]
//[ProducesResponseType(StatusCodes.Status400BadRequest)]
//[ProducesResponseType(StatusCodes.Status404NotFound)]
//public async Task<ActionResult<ContactDTO>> Patch(string id, JsonPatchDocument<Contact> contactPatch)
//{
//    var contact = await context.Contacts.FindAsync(id);

//    if (contact == null)
//        return NotFound();

//    contactPatch.ApplyTo(contact);

//    context.Contacts.Update(contact);
//    await context.SaveChangesAsync();

//    return Ok(contact);
//}