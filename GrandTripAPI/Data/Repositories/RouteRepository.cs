﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GrandTripAPI.Models;
using Microsoft.EntityFrameworkCore;

#nullable enable
namespace GrandTripAPI.Data.Repositories
{
    public class RouteRepository
    {
        private readonly AppDbContext _ctx;

        public RouteRepository(AppDbContext context)
        {
            _ctx = context;
        }
        public async Task<Route?> GetBy(Expression<Func<Route,bool>> predicate)
        {
            return await _ctx.Routes.FirstOrDefaultAsync(predicate);
        }

        public async Task<Route?> GetByWith(Expression<Func<Route, bool>> predicate, 
            params Expression<Func<Route, object>>[] navigations)
        { 
            return await navigations.Aggregate(_ctx.Routes.AsQueryable(),
                    (current, navigation) => current.Include(navigation))
                .Where(predicate).FirstOrDefaultAsync();
        }
        public async Task<RouteTheme?> GetTheme(string themeName)
        {
            return await _ctx.Themes
                .Where(t => t.Name == themeName)
                .Include(t => t.Routes)
                .FirstOrDefaultAsync();
        }
        public async Task<RouteSeason?> GetSeason(string seasonName)
        {
            return await _ctx.Seasons
                .Where(s => s.Name == seasonName)
                .Include(s => s.Routes)
                .FirstOrDefaultAsync();
        }

        public async Task<int> AddRoute(Route route)
        {
            await _ctx.AddAsync(route);
            await _ctx.SaveChangesAsync();

            return route.RouteId;
        }

        public async void UpdateRoute(Route route)
        {
            _ctx.Update(route);
            await _ctx.SaveChangesAsync();
        }

        public async void DeleteRoute(Route route)
        {
            _ctx.Routes.Remove(route);
            await _ctx.SaveChangesAsync();
        }
    }
}