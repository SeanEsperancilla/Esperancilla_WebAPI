using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Esperancilla_WebAPI.Data;
using Esperancilla_WebAPI.Model;

namespace Esperancilla_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        // GET: api/Product/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // GET: api/Product/search/{name}
        [HttpGet("search/{name}")]
        public async Task<IActionResult> SearchByName(string name)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Name.Contains(name));
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // PUT: api/Product/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product updatedProduct)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            product.Stock = updatedProduct.Stock;

            await _context.SaveChangesAsync();
            return Ok(product);
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok("Product deleted successfully!");
        }

        // GET: api/Product/SearchPartialProductName/{name}
        [HttpGet("SearchPartialProductName/{name}")]
        public async Task<IActionResult> SearchPartialProductName(string name)
        {
            var products = await _context.Products
                .Where(p => EF.Functions.Like(p.Name, $"%{name}%"))
                .ToListAsync();

            if (products.Count == 0)
            {
                return NotFound();
            }
            return Ok(products);
        }
    }
}
