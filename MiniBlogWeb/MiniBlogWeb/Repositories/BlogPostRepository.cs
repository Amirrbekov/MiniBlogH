
using Azure;
using Microsoft.EntityFrameworkCore;
using MiniBlogWeb.Data;
using MiniBlogWeb.Models.Domain;

namespace MiniBlogWeb.Repositories;

public class BlogPostRepository : IBlogPostRepository
{
    private readonly BlogDbContext _context;
    public BlogPostRepository(BlogDbContext context)
    {
        _context = context;
    }
    public async Task<BlogPost> AddAsync(BlogPost blogPost)
    {
        await _context.BlogPosts.AddAsync(blogPost);
        await _context.SaveChangesAsync();

        return blogPost;
    }

    public async Task<BlogPost?> DeleteAsync(Guid id)
    {
        var existingBlogPost = await _context.BlogPosts.FindAsync(id);

        if (existingBlogPost != null)
        {
            _context.BlogPosts.Remove(existingBlogPost);
            await _context.SaveChangesAsync();
            return existingBlogPost;
        }
        return null;
    }

    public async Task<IEnumerable<BlogPost>> GetAllAsync() => await _context.BlogPosts.Include(x => x.Tags).ToListAsync();

    public async Task<BlogPost?> GetAsync(Guid id) => await _context.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(t => t.Id == id);

    public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
    {
        var existingtag = await _context.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);

        if (existingtag != null)
        {
            existingtag.Id = blogPost.Id;
            existingtag.Heading = blogPost.Heading;
            existingtag.PageTitle = blogPost.PageTitle;
            existingtag.Content = blogPost.Content;
            existingtag.ShortDescription = blogPost.ShortDescription;
            existingtag.Author = blogPost.Author;
            existingtag.FeaturedImageUrl = blogPost.FeaturedImageUrl;
            existingtag.UrlHandle = blogPost.UrlHandle;
            existingtag.Visible = blogPost.Visible;
            existingtag.PublishedDate = blogPost.PublishedDate;
            existingtag.Tags = blogPost.Tags;

            await _context.SaveChangesAsync();

            return existingtag;
        }

        return null;
    }
}
