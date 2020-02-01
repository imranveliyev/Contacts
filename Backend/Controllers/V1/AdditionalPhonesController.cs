using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactsAppApi.Data;
using ContactsAppApi.Models;

namespace ContactsAppApi.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AdditionalPhonesController : ControllerBase
    {
        private readonly ContactsAppDbContext _context;

        public AdditionalPhonesController(ContactsAppDbContext context)
        {
            _context = context;
        }

        // GET: api/AdditionalPhones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdditionalPhone>>> GetAdditionalPhones()
        {
            return await _context.AdditionalPhones.ToListAsync();
        }

        // GET: api/AdditionalPhones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdditionalPhone>> GetAdditionalPhone(string id)
        {
            var additionalPhone = await _context.AdditionalPhones.FindAsync(id);

            if (additionalPhone == null)
            {
                return NotFound();
            }

            return additionalPhone;
        }

        // PUT: api/AdditionalPhones/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdditionalPhone(string id, AdditionalPhone additionalPhone)
        {
            if (id != additionalPhone.Phone)
            {
                return BadRequest();
            }

            _context.Entry(additionalPhone).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdditionalPhoneExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AdditionalPhones
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<AdditionalPhone>> PostAdditionalPhone(AdditionalPhone additionalPhone)
        {
            _context.AdditionalPhones.Add(additionalPhone);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AdditionalPhoneExists(additionalPhone.Phone))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAdditionalPhone", new { id = additionalPhone.Phone }, additionalPhone);
        }

        // DELETE: api/AdditionalPhones/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AdditionalPhone>> DeleteAdditionalPhone(string id)
        {
            var additionalPhone = await _context.AdditionalPhones.FindAsync(id);
            if (additionalPhone == null)
            {
                return NotFound();
            }

            _context.AdditionalPhones.Remove(additionalPhone);
            await _context.SaveChangesAsync();

            return additionalPhone;
        }

        private bool AdditionalPhoneExists(string id)
        {
            return _context.AdditionalPhones.Any(e => e.Phone == id);
        }
    }
}
