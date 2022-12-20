﻿using System;
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

        public string GenerateToken(int id)
        {
            return _jwtService.GenerateToken(id);
        }
    }
}