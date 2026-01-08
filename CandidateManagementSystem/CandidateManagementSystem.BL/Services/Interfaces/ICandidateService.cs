using CandidateManagementSystem.BL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateManagementSystem.BL.Services.Interfaces
{
    public interface ICandidateService
    {
        Task<IEnumerable<CandidateDto>> GetAllCandidatesAsync();
        Task<CandidateDto> GetCandidateByIdAsync(int id);
        Task<CandidateDto> CreateCandidateAsync(CandidateDto candidateDto);
        Task<CandidateDto> UpdateCandidateAsync(int id, CandidateDto candidateDto);
        Task DeleteCandidateAsync(int id);
        Task<SkillDto> AddSkillToCandidateAsync(int candidateId, SkillDto skillDto);
        Task<SkillDto> UpdateSkillAsync(int skillId, SkillDto skillDto);
        Task DeleteSkillAsync(int skillId);
    }
}
