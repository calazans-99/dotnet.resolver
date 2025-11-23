using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mentorax.Api.Data;
using Mentorax.Api.Models;
using Mentorax.Api.Repositories.Interfaces;

namespace Mentorax.Api.Repositories.Implementations
{
    public class TarefaMentoriaRepository : ITarefaMentoriaRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TarefaMentoria> _dbSet;

        public TarefaMentoriaRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TarefaMentoria>();
        }

        public async Task<IEnumerable<TarefaMentoria>> GetAllAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TarefaMentoria?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(TarefaMentoria entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TarefaMentoria entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity is null)
                return;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TarefaMentoria>> GetByMentorshipPlanIdAsync(Guid planId)
        {
            return await _dbSet
                .Where(t => t.MentorshipPlanId == planId)
                .OrderBy(t => t.Description)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<(IEnumerable<TarefaMentoria> Items, long TotalCount)>
            GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _dbSet.AsQueryable();

            var totalCount = await query.LongCountAsync();

            var items = await query
                .OrderBy(t => t.Description)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<(IEnumerable<TarefaMentoria> Items, long TotalCount)>
            GetPagedByMentorshipPlanIdAsync(Guid planId, int pageNumber, int pageSize)
        {
            var query = _dbSet
                .Where(t => t.MentorshipPlanId == planId);

            var totalCount = await query.LongCountAsync();

            var items = await query
                .OrderBy(t => t.Description)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
