using Mentorax.Api.Models;

namespace Mentorax.Api.Repositories.Interfaces
{
    public interface IMentorRepository
    {
        Task<Mentor?> GetByIdAsync(Guid id);
        Task<IEnumerable<Mentor>> GetAllAsync();
        Task AddAsync(Mentor entity);
        Task UpdateAsync(Mentor entity);
        Task DeleteAsync(Guid id);

        Task<(IEnumerable<Mentor> Items, long TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task<bool> EmailExistsAsync(string email, Guid? excludeId = null);
        Task<Mentor?> GetByEmailAsync(string email);
    }
}
