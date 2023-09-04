using Microsoft.EntityFrameworkCore;
using MiniBlogWeb.Data;
using MiniBlogWeb.Models.Domain;

namespace MiniBlogWeb.Repositories;

public class TagRepository : ITagRepository
{
    private readonly BlogDbContext _context;
    public TagRepository(BlogDbContext context)
    {
        _context = context;
    }
    public async Task<Tag> AddAsync(Tag tag)
    {
        await _context.Tags.AddAsync(tag);
        await _context.SaveChangesAsync();

        return tag;
    }

    public async Task<Tag?> DeleteAsync(Guid id)
    {
        var existingTag = await _context.Tags.FindAsync(id);

        if (existingTag != null)
        {
            _context.Tags.Remove(existingTag);
            await _context.SaveChangesAsync();
            return existingTag;
        }
        return null;
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        return await _context.Tags.ToListAsync();
    }

    public Task<Tag?> GetAsync(Guid id)
    {
        return _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Tag?> UpdateAsync(Tag tag)
    {
        var existingTag = await _context.Tags.FindAsync(tag.Id);

        if (existingTag != null)
        {
            existingTag.Name = tag.Name;
            existingTag.DisplayName = tag.DisplayName;

            await _context.SaveChangesAsync();

            return existingTag;
        }

        return null;
    }
}
