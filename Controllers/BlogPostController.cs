using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blog.Models;

namespace cSharp_webserver_personal_blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly BlogPostContext _context;

        public BlogPostController(BlogPostContext context)
        {
            _context = context;
        }


        // GET: api/BlogPost
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPostItem>>> GetBlogPostItems()
        {
          if (_context.BlogPostItems == null)
          {
              return NotFound();
          }
            return await _context.BlogPostItems.ToListAsync();
        }

        // GET: api/BlogPost/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogPostItem>> GetBlogPostItem(int id)
        {
          if (_context.BlogPostItems == null)
          {
              return NotFound();
          }
            var blogPostItem = await _context.BlogPostItems.FindAsync(id);

            if (blogPostItem == null)
            {
                return NotFound();
            }

            return blogPostItem;
        }

        // PUT: api/BlogPost/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlogPostItem(int id, BlogPostItem blogPostItem)
        {
            if (id != blogPostItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(blogPostItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogPostItemExists(id))
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

        // POST: api/BlogPost
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BlogPostItem>> PostBlogPostItem(BlogPostItem blogPostItem)
        {
          if (_context.BlogPostItems == null)
          {
              return Problem("Entity set 'BlogPostContext.TodoItems'  is null.");
          }
            _context.BlogPostItems.Add(blogPostItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBlogPostItem), new { id = blogPostItem.Id }, blogPostItem);
        }

        // DELETE: api/BlogPost/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogPostItem(int id)
        {
            if (_context.BlogPostItems == null)
            {
                return NotFound();
            }
            var blogPostItem = await _context.BlogPostItems.FindAsync(id);
            if (blogPostItem == null)
            {
                return NotFound();
            }

            _context.BlogPostItems.Remove(blogPostItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BlogPostItemExists(int id)
        {
            return (_context.BlogPostItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

/*
Created via scaffold

Make sure that all of your changes so far are saved.

    Control-click the TodoAPI project and select Open in Terminal. The terminal opens at the TodoAPI project folder. Run the following commands:

.NET CLI

dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design -v 7.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design -v 7.0.0
dotnet add package Microsoft.EntityFrameworkCore.SqlServer -v 7.0.0
dotnet tool uninstall -g dotnet-aspnet-codegenerator
dotnet tool install -g dotnet-aspnet-codegenerator

The preceding commands:

    Add NuGet packages required for scaffolding.
    Install the scaffolding engine (dotnet-aspnet-codegenerator) after uninstalling any possible previous version.

Build the project.

Run the following command:
.NET CLI

dotnet-aspnet-codegenerator controller -name TodoItemsController -async -api -m TodoItem -dc TodoContext -outDir Controllers

The preceding command scaffolds the TodoItemsController.
*/
