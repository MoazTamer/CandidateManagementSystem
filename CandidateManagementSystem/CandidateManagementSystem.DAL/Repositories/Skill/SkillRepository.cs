using CandidateManagementSystem.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateManagementSystem.DAL.Repositories.Skill
{
    public class SkillRepository : ISkillRepository
    {
        private readonly ApplicationDbContext _context;

        public SkillRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Entities.Skill> AddAsync(Entities.Skill skill)
        {
            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();
            return skill;
        }

        public async Task DeleteAsync(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill != null)
            {
                _context.Skills.Remove(skill);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Entities.Skill>> GetByCandidateIdAsync(int candidateId)
        {
            return await _context.Skills
                .AsNoTracking()
                .Where(s => s.CandidateId == candidateId)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Entities.Skill?> GetByIdAsync(int id)
        {
            return await _context.Skills
                .AsNoTracking()
                .Include(s => s.Candidate)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<int> GetSkillCountByCandidateIdAsync(int candidateId)
        {
            return await _context.Skills
                .CountAsync(s => s.CandidateId == candidateId);
        }

        public async Task UpdateAsync(Entities.Skill skill)
        {
            _context.Skills.Update(skill);
            await _context.SaveChangesAsync();
        }
    }
}
