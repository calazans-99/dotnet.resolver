using Microsoft.EntityFrameworkCore;
using Mentorax.Api.Data;
using Mentorax.Api.Models;
using Mentorax.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mentorax.Api.Repositories.Implementations
{

    public class QuestionarioRepository : IQuestionarioRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Questionario> _dbSet;

        public QuestionarioRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Questionario>();
        }


        public async Task AddAsync(Questionario entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Questionario entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var item = await GetByIdAsync(id);
            if (item == null) return;

            _dbSet.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<Questionario?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<IEnumerable<Questionario>> GetAllAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<long> CountAsync()
        {
            return await _dbSet.LongCountAsync();
        }

        public async Task<IEnumerable<Questionario>> GetByMenteeIdAsync(Guid mentoradoId)
        {
            return await _dbSet
                .Where(q => q.MenteeId == mentoradoId)
                .OrderBy(q => q.SubmittedAt)
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<bool> ExistsForMenteeAsync(Guid mentoradoId)
        {
            return await _dbSet
                .AsNoTracking()
                .AnyAsync(q => q.MenteeId == mentoradoId);
        }


        public async Task<(IEnumerable<Questionario> Items, long TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _dbSet.AsQueryable();

            var totalCount = await query.LongCountAsync();

            var items = await query
                .OrderBy(q => q.SubmittedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return (items, totalCount);
        }


        public async Task<(IEnumerable<Questionario> Items, long TotalCount)> GetPagedByMenteeIdAsync(Guid mentoradoId, int pageNumber, int pageSize)
        {
            var query = _dbSet
                .Where(q => q.MenteeId == mentoradoId);

            var totalCount = await query.LongCountAsync();

            var items = await query
                .OrderBy(q => q.SubmittedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
