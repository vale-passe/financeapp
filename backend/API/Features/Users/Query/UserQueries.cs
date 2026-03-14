using API.Features.Users.Entities;
using API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Users.Query;

[ExtendObjectType(OperationTypeNames.Query)]
public abstract class UserQueries(DataContext context)
{
    public IQueryable<User> GetUsers() =>  context.Users;
    public async Task<User?> GetUser(Guid id) => 
        await context.Users.FirstOrDefaultAsync(u => u.Id == id);
}