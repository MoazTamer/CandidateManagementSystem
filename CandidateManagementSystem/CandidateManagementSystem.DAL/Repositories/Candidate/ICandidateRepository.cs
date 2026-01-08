using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateManagementSystem.DAL.Repositories.Candidate
{
    public interface ICandidateRepository
    {
        Task<Entities.Candidate?> GetByIdAsync(int id);
        Task<Entities.Candidate> GetByEmailAsync(string email);
        Task<IEnumerable<Entities.Candidate>> GetAllAsync();
        Task<Entities.Candidate> AddAsync(Entities.Candidate candidate);
        Task UpdateAsync(Entities.Candidate candidate);
        Task DeleteAsync(int id);
        Task<bool> ExistsByEmailAsync(string email);
        Task<IEnumerable<Entities.Candidate>> GetByEmailsAsync(IEnumerable<string> emails);

    }
}
