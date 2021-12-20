using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using brotherscase;
using brotherscase.Domain;
using brotherscase.Models;
using brotherscase.Data;

namespace brotherscase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly AddressDbContext _context;
        private readonly AddressRepository _repository;

        public AddressController(AddressDbContext context)
        {
            _context = context;
            _repository = new AddressRepository(context);
        }

        /// <summary>
        /// Measures distance between two addresses in Kilometers.
        /// </summary>
        /// <param name="firstAddressId">Example: 5</param>
        /// <param name="secondAddressId">Example: 7</param>
        /// <returns></returns>
        [HttpGet("{firstAddressId}/{secondAddressId}")]
        public async Task<ActionResult<double>> GetDistance(int firstAddressId, int secondAddressId)
        {
            var firstAddress = await _context.Address.FindAsync(firstAddressId);
            var secondAddress = await _context.Address.FindAsync(secondAddressId);

            if (firstAddress == null || secondAddress == null)
            {
                return NotFound();
            }

            return _repository.GetDistanceInKilometers(firstAddress, secondAddress);
        }

        /// <summary>
        /// Retrieves all Addresses in database.
        /// </summary>
        /// <remarks>Parameters are for filtering, leave empty to retrieve all.<br />
        /// OrderBy is for sorting.</remarks>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddress([FromQuery] AddressParameters addressParameters)
        {
            return await _repository.GetAddresses(addressParameters).ToListAsync();            
        }
        /// <summary>
        /// Retrieve Address with specific ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetAddress(int id)
        {
            var address = await _context.Address.FindAsync(id);

            if (address == null)
            {
                return NotFound();
            }

            return address;
        }

        /// <summary>
        /// Update existing Address.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddress(int id, Address address)
        {
            if (id != address.AddressId)
            {
                return BadRequest();
            }

            _context.Entry(address).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
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

        /// <summary>
        /// Create new Address.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Address>> PostAddress(Address address)
        {
            _context.Address.Add(address);
            await _context.SaveChangesAsync();
                         
            return CreatedAtAction("GetAddress", new { id = address.AddressId }, address);
        }

        /// <summary>
        /// Delete an Address.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var address = await _context.Address.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            _context.Address.Remove(address);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool AddressExists(int id)
        {
            return _context.Address.Any(e => e.AddressId == id);
        }
    }
}
