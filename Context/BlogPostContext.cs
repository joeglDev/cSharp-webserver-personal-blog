using Microsoft.EntityFrameworkCore;
namespace Blog.Models;

public class BlogPostContext : DbContext
{
     public  DbSet<BlogPostItem>? BlogPostItems { get; set; } 

    public BlogPostContext(DbContextOptions<BlogPostContext> options)
        : base(options)
    {
        Initialize();
    }

   

    public void Initialize()

    
    { 
        if (BlogPostItems != null)
        if (!BlogPostItems.Any())
        {

            BlogPostItem huskys = new BlogPostItem(0, "Hiroji", "Why are Huskys so cute!", "Huskies are often considered adorable due to their distinct features and charming personalities. With their striking blue or multicolored eyes, thick coats, and expressive faces, huskies possess a natural allure that captures the hearts of many. Their fluffy appearance, combined with their playful and friendly nature, creates an instant appeal. Whether it's their mischievous antics, their infectious energy, or their affectionate demeanor, huskies possess a certain charm that brings joy and warmth to those who encounter them. Their cuteness goes beyond their physical attributes and resonates with their spirited and endearing personalities, making them irresistible to many animal lovers.", 0);
            huskys.TimeStamp = DateTime.Now;

            BlogPostItems.Add(huskys);
            SaveChanges();
        }
    }
}