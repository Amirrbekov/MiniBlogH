using Microsoft.AspNetCore.Identity;

namespace MiniBlogWeb.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<IdentityUser>> GetAll();
}
