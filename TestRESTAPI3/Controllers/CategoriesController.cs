using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestRESTAP3.Data;
using TestRESTAPI3.Data;
using TestRESTAPI3.Data.Models;

namespace TestRESTAPI3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        public CategoriesController(AppDbContext db)
        {
            _db = db;
        }
        private readonly AppDbContext _db;

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var cats = await _db.categories.ToListAsync();
            return Ok(cats);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategories(int id)
        {
            var cats = await _db.categories.SingleOrDefaultAsync(x => x.Id == id);
            if (cats == null)
            {
                return NotFound($"Category Id {id} not exists ");
            }
            return Ok(cats);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(string category)
        {
            Category c = new() { Name = category };
            await _db.categories.AddAsync(c);
            _db.SaveChanges();
            return Ok(c);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            var c = await _db.categories.SingleOrDefaultAsync(x => x.Id == category.Id);
            if (c == null)
            {
                return NotFound($"Category Id {category.Id} not exists ");
            }
            c.Name = category.Name;
            _db.SaveChanges();
            return Ok(c);
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCategoryPatch([FromBody] JsonPatchDocument<Category> category, [FromRoute] int id)
        {
            var c = await _db.categories.SingleOrDefaultAsync(x => x.Id == id);
            if (c == null)
            {
                return NotFound($"Category Id {id} not exists ");
            }
            category.ApplyTo(c);
            await _db.SaveChangesAsync();
            return Ok(c);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveCategory(int id)
        {
            var c = await _db.categories.SingleOrDefaultAsync(x => x.Id == id);
            if (c == null)
            {
                return NotFound($"Category Id {id} not exists ");
            }
            _db.categories.Remove(c);
            _db.SaveChanges();
            return Ok(c);
        }



    }
}
