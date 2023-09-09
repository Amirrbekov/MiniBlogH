using MiniBlogWeb.Models.Domain;

namespace MiniBlogWeb.Models.ViewModels;

public class HomeViewModel
{
    public IEnumerable<BlogPost> BlogPosts { get; set; }
    public IEnumerable<Tag> Tags { get; set; }
}
