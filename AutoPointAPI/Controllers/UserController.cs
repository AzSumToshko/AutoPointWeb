﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoPointAPI.Data;
using Scrypt;

namespace AutoPointAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DbA91afaAutopointContext _context;
        private readonly ScryptEncoder _encoder;

        public UserController(DbA91afaAutopointContext context)
        {
            _encoder = new ScryptEncoder();
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpGet("byEmail/{email}")]
        public async Task<ActionResult<User>> GetUserByEmail(string email)
        {
            User user = null;
            try
            {
                user = await _context.Users.FirstAsync(u => u.Email.Equals(email));
            }
            catch (Exception ex)
            {
                return NotFound();
            }
            
            return user;
        }

        [HttpGet("byEmailAndPassword/{email}&{password}")]
        public async Task<ActionResult<User>> GetUserByEmailAndPassword(string email, string password)
        {
            bool isPasswordEncoded = _encoder.IsValid(password);

            email = email.Replace("%40","@");

            if (isPasswordEncoded)
            {
                foreach(var user in _context.Users)
                {
                    if (user.Email.Equals(email) && user.Password.Equals(password))
                    {
                        return user;
                    }
                }
            }
            else
            {
                foreach (var user in _context.Users)
                {
                    if (user.Email.Equals(email) && _encoder.Compare(password, user.Password))
                    {
                        return user;
                    }
                }
            }

            return NotFound();
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            if (!_encoder.IsValid(user.Password))
            {
                user.Password = _encoder.Encode(user.Password);
            }
            
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            user.Password = _encoder.Encode(user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
