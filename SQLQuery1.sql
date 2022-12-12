select * from Categories

insert into Categories values('cloths')

select * from Products
insert into Products([PName],[Description],[UnitPrice],[UnitsInStock],[Status],[CategoryId]) values('t-shirt','xyz',500,100,1,1)

select * from Users
insert into Users values('Aniket','Kale','8766594551','Baramati,Pune',1,'aniket@gmail.com','aniket@2310')
insert into Users values('Sanket','Kale','8390571799','Baramati,Pune',2,'Sanket@gmail.com','sanket@2310')
insert into Users values('Shahrukh','Khan','8890571799','Baramati,Pune',2,'SRK@gmail.com','srk@2310')
insert into Users values('Salman','Khan','8890571799','Baramati,Pune',2,'SK@gmail.com','sk@2310')

drop database [ProjectDb]

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Shopping_WebAPICore.Models;

namespace Online_Shopping_WebAPICore.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        
        private readonly Online_Shopping_DbContext _context;

        public ProductController(Online_Shopping_DbContext context)
        {
            _context = context;
        }

        //GET : api/Product
        [HttpGet]
        //[Produces("application/json")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        //GET : api/Product/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.PId == id);
        }

        //[Produces(NoContent)]
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductExists(product.PId))
                {
                    return Conflict();
                }
                else
                    throw;
            }
            return CreatedAtAction("GetProduct", new { ProductId = product.PId }, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> PutProduct(int id, Product product)
        {
            if (id != product.PId)
            {
                return BadRequest();
            }
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                    throw;
            }
            return Ok(product);
        }

        //DELETE : api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
