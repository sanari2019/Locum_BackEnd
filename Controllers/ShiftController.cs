using Microsoft.AspNetCore.Mvc;
using Locum_Backend;
using Locum_Backend.Repositories;
using Locum_Backend.Models;
using Locum_Backend.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using System.Text;
// using Microsoft.IdentityModel.Tokens;
// using System.IdentityModel.Tokens.Jwt;
using Microsoft.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;

namespace Locum_Backend.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly ShiftRepository _shiftRepository;

        public ShiftController(ShiftRepository shiftRepository)
        {
            _shiftRepository = shiftRepository;
        }

        [HttpGet]
        public IActionResult GetShifts()
        {
            var shifts = _shiftRepository.GetShifts();
            return Ok(shifts);
        }

        [HttpGet("{id}")]
        public IActionResult GetShift(int id)
        {
            var shift = _shiftRepository.GetShiftById(id);
            if (shift == null)
            {
                return NotFound();
            }
            return Ok(shift);
        }

        [HttpPost]
        public IActionResult CreateShift([FromBody] Shift shift)
        {
            if (shift == null)
            {
                return BadRequest();
            }
            _shiftRepository.InsertShift(shift);
            return CreatedAtAction(nameof(GetShift), new { id = shift.Id }, shift);
        }

        // Add other actions as needed
    }
}