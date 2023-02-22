using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using GrandTripAPI.Models;

#nullable enable
namespace GrandTripAPI.Data.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _ctx;
        private readonly JwtService _jwtService;

        public UserRepository(AppDbContext ctx, JwtService service)
        {
            _ctx = ctx;
            _jwtService = service;
        }

        public async Task<User?> GetBy(Expression<Func<User, bool>> predicate)
        {
            return await _ctx.Users.FirstOrDefaultAsync(predicate);
        }

        public async Task<User?> GetByWith(Expression<Func<User, bool>> predicate,
            params Expression<Func<User, object>>[] navigations)
        {
            return await navigations
                    .Aggregate(_ctx.Users.Where(predicate),
                        (current, navigation)
                            => current.Include(navigation))
                    .FirstOrDefaultAsync();
        }
        public async Task<int> AddUser(string username, string password)
        {
            var user = User.CreateNew(username, password);
            await _ctx.AddAsync(user);
            await _ctx.SaveChangesAsync();
            return user.Id;
        }

        public async Task Update(User user)
        {
            _ctx.Update(user);
            await _ctx.SaveChangesAsync();
        }

        public async Task Delete(User user)
        {
            var routes = _ctx.Routes.Where(r => r.Creator.Id == user.Id);
            var admin = await _ctx.Users.FirstAsync(u=>u.Role == "Admin");
            await routes.ForEachAsync(r => { r.Creator = admin; });
            _ctx.UpdateRange(routes);
            _ctx.Remove(user);
            await _ctx.SaveChangesAsync();
        }
        public async Task<List<User>> GetAll(bool includeRoutes = false)
        {
            return includeRoutes 
                ? await _ctx.Users.Include(u=>u.CreatedRoutes).ToListAsync()
                : await _ctx.Users.ToListAsync();
        }
        public string GenerateToken(int id) => _jwtService.GenerateToken(id);
        }
}