using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoPointAPI.Data;

namespace AutoPointAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly DbA91afaAutopointContext _context;

        public CartController(DbA91afaAutopointContext context)
        {
            _context = context;
        }

        // GET: api/Cart
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartProduct>>> GetCartProducts()
        {
            return await _context.CartProducts.ToListAsync();
        }

        [HttpGet("byUser/{id}")]
        public async Task<ActionResult<IEnumerable<CartProduct>>> GetCartProducts(int id)
        {
            return await _context.CartProducts.Where(c => c.UserId == id).ToListAsync();
        }

        // GET: api/Cart/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CartProduct>> GetCartProduct(int id)
        {
            var cartProduct = await _context.CartProducts.FindAsync(id);

            if (cartProduct == null)
            {
                return NotFound();
            }

            return cartProduct;
        }

        // PUT: api/Cart/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartProduct(int id, CartProduct cartProduct)
        {
            if (id != cartProduct.Id)
            {
                return BadRequest();
            }

            _context.Entry(cartProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartProductExists(id))
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

        // POST: api/Cart
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CartProduct>> PostCartProduct(CartProduct cartProduct)
        {
            _context.CartProducts.Add(cartProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCartProduct", new { id = cartProduct.Id }, cartProduct);
        }

        // DELETE: api/Cart/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartProduct(int id)
        {
            var cartProduct = await _context.CartProducts.FindAsync(id);
            if (cartProduct == null)
            {
                return NotFound();
            }

            _context.CartProducts.Remove(cartProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Cart/5
        [HttpDelete("byUserAndProduct/{userID}&{productID}")]
        public async Task<IActionResult> DeleteCartProduct(int userID, int productID)
        {
            var cartProduct = await _context.CartProducts.FirstOrDefaultAsync( c => c.UserId == userID && c.ProductId == productID);

            if (cartProduct == null)
            {
                return NotFound();
            }

            _context.CartProducts.Remove(cartProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("byUser/{userID}")]
        public async Task<IActionResult> DeleteCartProductsForUser(int userID)
        {
            var cartProducts = _context.CartProducts.Where(c => c.UserId == userID).ToList();

            if (cartProducts.Count <= 0)
            {
                return NotFound();
            }

            foreach (var cartProduct in cartProducts)
            {
                _context.CartProducts.Remove(cartProduct);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartProductExists(int id)
        {
            return _context.CartProducts.Any(e => e.Id == id);
        }
    }
}
