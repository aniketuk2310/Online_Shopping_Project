using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Shopping_WebAPICore.Models;

namespace Online_Shopping_WebAPICore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly Online_Shopping_DbContext _context;

        public CategoryController(Online_Shopping_DbContext context)
        {
            _context = context;
        }

        // GET: api/Category
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        

        //[HttpGet("{cat}")]
        //public async Task<ActionResult<IEnumerable<Product>>> SortCat(int cat)
        //{
        //    return await _context.Products.Where(p => p.Category = cat).ToListAsync();

        //}
    }
}
