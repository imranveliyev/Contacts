using System.Threading.Tasks;
using AutoMapper;
using ContactsAppApi.Data;
using ContactsAppApi.Models;
using ContactsAppApi.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsAppApi.Controllers.V2
{
    [Route("api/v2/[controller]")]
    [ApiController]
    public class AdditionalPhonesController : ControllerBase
    {
        private readonly ContactsAppDbContext context;
        private readonly IMapper mapper;

        public AdditionalPhonesController(
            ContactsAppDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        /// <summary>
        /// Add phone number
        /// </summary>
        /// <param name="additionalPhoneDTO">Phone object</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(AdditionalPhoneDTO additionalPhoneDTO)
        {
            var phone = mapper.Map<AdditionalPhone>(additionalPhoneDTO);

            var contact = await context.Contacts.FindAsync(phone.ContactId);
            
            if (contact == null)
                return BadRequest("Contact does not exists!");

            context.AdditionalPhones.Add(phone);
            await context.SaveChangesAsync();

            return Accepted(phone);
        }

        /// <summary>
        /// Replace phone number
        /// </summary>
        /// <param name="phone">Phone number</param>
        /// <param name="additionalPhoneDTO">Phone object</param>
        /// <returns></returns>
        [HttpPut("{phone}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Replace(string phone, AdditionalPhoneDTO additionalPhoneDTO)
        {
            additionalPhoneDTO.Phone = phone;

            var addphone = mapper.Map<AdditionalPhone>(additionalPhoneDTO);

            var contact = await context.Contacts.FindAsync(addphone.ContactId);

            if (contact == null)
                return BadRequest("Contact does not exists!");

            try
            {
                context.AdditionalPhones.Update(addphone);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                addphone = await context.AdditionalPhones.FindAsync(phone);
                if (addphone == null)
                    return NotFound();

                throw;
            }

            return Ok(addphone); 
        }

        /// <summary>
        /// Remove phone number
        /// </summary>
        /// <param name="phone">Phone number</param>
        /// <returns>Nothing</returns>
        [HttpDelete("{phone}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Remove(string phone)
        {
            var addPhone = await context.AdditionalPhones.FindAsync(phone);

            if (addPhone == null)
                return NotFound();

            context.AdditionalPhones.Remove(addPhone);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}