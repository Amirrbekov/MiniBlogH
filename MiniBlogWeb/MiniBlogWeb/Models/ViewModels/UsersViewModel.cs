using MiniBlogWeb.Models.Domain;

namespace MiniBlogWeb.Models.ViewModels;

public class UsersViewModel
{
    public List<User> Users { get; set; }

    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool AdminroleChechbox { get; set; }
}
