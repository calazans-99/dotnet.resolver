using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mentorax.Api.Data;
using Mentorax.Api.Models;
using Mentorax.Api.Repositories.Interfaces;
using System.Collections.Generic;
using System;

namespace Mentorax.Api.Repositories.Implementations
{
    public class MentorRepository : IMentorRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly DbSet<Mentor> _dbSet;

        public MentorRepository(ApplicationDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<Mentor>();
        }

        public async Task AddAsync(Mentor entity)
        {
            _dbSet.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var item = await GetByIdAsync(id);
            if (item == null) return;

            _dbSet.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task<Mentor?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<(IEnumerable<Mentor> Items, long TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _dbSet.AsQueryable();

            var totalCount = await query.LongCountAsync();

            var items = await query
                .OrderBy(m => m.FullName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<long> CountAsync()
        {
            return await _dbSet.LongCountAsync();
        }

        public async Task UpdateAsync(Mentor entity)
        {
            _dbSet.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> EmailExistsAsync(string email, Guid? excludeId = null)
        {
            return await _dbSet.AnyAsync(m =>
                m.Email == email &&
                (excludeId == null || m.Id != excludeId)
            );
        }

        public async Task<Mentor?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Email == email);
        }

        public async Task<IEnumerable<Mentor>> GetAllAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync();
        }

    }
}
