using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateManagementSystem.DAL.Repositories.Skill
{
    public interface ISkillRepository
    {
        Task<Entities.Skill> GetByIdAsync(int id);
        Task<IEnumerable<Entities.Skill>> GetByCandidateIdAsync(int candidateId);
        Task<Entities.Skill> AddAsync(Entities.Skill skill);
        Task UpdateAsync(Entities.Skill skill);
        Task DeleteAsync(int id);
        Task<int> GetSkillCountByCandidateIdAsync(int candidateId);
    }
}
