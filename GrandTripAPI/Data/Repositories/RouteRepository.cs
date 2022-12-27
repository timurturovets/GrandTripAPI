using System;
using System.Collections.Generic;
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

        public async Task<List<Route?>> GetAll(string? theme = null, string? season = null)
        {
            var set = _ctx.Routes;

            if (string.IsNullOrEmpty(theme) && string.IsNullOrEmpty(season)) return await set
                    .Include(r => r.Dots)
                    .Include(r => r.Lines)
                    .ToListAsync();
                
            if (string.IsNullOrEmpty(theme)) return await set
                .Where(r => r.Season.Key == season)
                .Include(r => r.Dots)
                .Include(r => r.Lines)
                .ToListAsync();

            if (string.IsNullOrEmpty(season))
                return await set
                    .Where(r => r.Theme.Key == theme)
                    .Include(r => r.Dots)
                    .Include(r => r.Lines)
                    .ToListAsync();
            
            return await set
                .Where(r => r.Theme.Key == theme && r.Season.Key == season)
                .Include(r => r.Dots)
                .Include(r => r.Lines)
                .ToListAsync();
        }
        
        public async Task<Route?> GetByWith(Expression<Func<Route, bool>> predicate, 
            params Expression<Func<Route, object>>[] navigations)
        { 
            return await navigations.Aggregate(_ctx.Routes.AsQueryable(),
                    (current, navigation) => current.Include(navigation))
                .Where(predicate).FirstOrDefaultAsync();
        }
        public async Task<RouteTheme?> GetTheme(string themeName, bool includeRoutes = false)
        {
            return includeRoutes
                ? await _ctx.Themes
                    .Where(t => t.Key == themeName)
                    .Include(t => t.Routes)
                    .FirstOrDefaultAsync()
                : await _ctx.Themes
                    .Where(t => t.Key == themeName)
                    .FirstOrDefaultAsync();
        }
        public async Task<RouteSeason?> GetSeason(string seasonName, bool includeRoutes = false)
        {
            return includeRoutes
                ? await _ctx.Seasons
                    .Where(s => s.Key == seasonName)
                    .Include(s => s.Routes)
                    .FirstOrDefaultAsync()
                : await _ctx.Seasons
                    .Where(s => s.Key == seasonName)
                    .FirstOrDefaultAsync();
        }

        public async Task<int> AddRoute(Route route)
        {
            await _ctx.Routes.AddAsync(route);
            await _ctx.SaveChangesAsync();

            return route.RouteId;
        }

        public async Task UpdateRoute(Route route, bool replaceDotsAndLines = false)
        {
            if (replaceDotsAndLines)
            {
                _ctx.Dots.RemoveRange(_ctx.Dots.Where(d => d.RouteId == route.RouteId));
                _ctx.Lines.RemoveRange(_ctx.Lines.Where(l => l.RouteId == route.RouteId));
            }

            _ctx.Routes.Update(route);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteRoute(Route route)
        {
            _ctx.Routes.Remove(route);
            await _ctx.SaveChangesAsync();
        }

        public async Task<List<Route>> GetByIds(IEnumerable<int> ids)
        {
            List<Route> routes = new();
            foreach (var id in ids)
            {
                var route = await _ctx.Routes
                    .Where(r => r.RouteId == id)
                    .Include(r=>r.Dots)
                    .Include(r=>r.Lines)
                    .Include(r=>r.Theme)
                    .Include(r=>r.Season)
                    .FirstOrDefaultAsync();
                if (route is not null) routes.Add(route);
            }

            return routes;
        }
        public async Task<List<Route>> GetCreatedRoutes(User user)
        {
            return await _ctx.Routes.IgnoreAutoIncludes()
                .Include(r => r.Creator)
                .Where(r => r.Creator.Id == user.Id)
                .Include(r=>r.Dots)
                .Include(r=>r.Lines)
                .ToListAsync();

        }
    }
}