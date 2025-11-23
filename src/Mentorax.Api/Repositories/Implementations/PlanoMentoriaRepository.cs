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
    public class PlanoMentoriaRepository : IPlanoMentoriaRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<PlanoMentoria> _dbSet;

        public PlanoMentoriaRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<PlanoMentoria>();
        }

        // ----------------------------------------------------
        // CRUD BÁSICO
        // ----------------------------------------------------

        public async Task<IEnumerable<PlanoMentoria>> GetAllAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PlanoMentoria?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(PlanoMentoria entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PlanoMentoria entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _dbSet.FindAsync(id);
            if (existing is null)
                return;

            _dbSet.Remove(existing);
            await _context.SaveChangesAsync();
        }

        // ----------------------------------------------------
        // FILTROS POR MENTORADO (MENTEE)
        // ----------------------------------------------------

        /// <summary>
        /// Retorna todos os planos de mentoria de um determinado mentorado (mentee).
        /// </summary>
        public async Task<IEnumerable<PlanoMentoria>> GetByMenteeIdAsync(Guid menteeId)
        {
            return await _dbSet
                .Where(p => p.MenteeId == menteeId)
                .OrderBy(p => p.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Lista paginada de planos de mentoria.
        /// </summary>
        public async Task<(IEnumerable<PlanoMentoria> Items, long TotalCount)>
            GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _dbSet.AsQueryable();

            var totalCount = await query.LongCountAsync();

            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return (items, totalCount);
        }

        /// <summary>
        /// Lista paginada de planos filtrada por mentorado (mentee).
        /// </summary>
        public async Task<(IEnumerable<PlanoMentoria> Items, long TotalCount)>
            GetPagedByMenteeIdAsync(Guid menteeId, int pageNumber, int pageSize)
        {
            var query = _dbSet
                .Where(p => p.MenteeId == menteeId);

            var totalCount = await query.LongCountAsync();

            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
