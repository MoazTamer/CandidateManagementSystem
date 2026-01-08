using CandidateManagementSystem.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateManagementSystem.DAL.Repositories.Candidate
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly ApplicationDbContext _context;
        public CandidateRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Entities.Candidate> AddAsync(Entities.Candidate candidate)
        {
            _context.Candidates.Add(candidate);
            await _context.SaveChangesAsync();
            return candidate;
        }

        public async Task DeleteAsync(int id)
        {
            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate != null)
            {
                _context.Candidates.Remove(candidate);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Candidates
                .AnyAsync(c => c.Email == email);
        }

        public async Task<IEnumerable<Entities.Candidate>> GetAllAsync()
        {
            return await _context.Candidates
                .AsNoTracking()
                .Include(c => c.Skills)
                .ToListAsync();
        }

        public async Task<Entities.Candidate?> GetByEmailAsync(string email)
        {
            return await _context.Candidates
                .AsNoTracking()
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<IEnumerable<Entities.Candidate>> GetByEmailsAsync(IEnumerable<string> emails)
        {
            return await _context.Candidates
                .AsNoTracking()
                .Where(c => emails.Contains(c.Email))
                .ToListAsync();
        }

        public async Task<Entities.Candidate?> GetByIdAsync(int id)
        {
            return await _context.Candidates
                .AsNoTracking()
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(Entities.Candidate candidate)
        {
            _context.Candidates.Update(candidate);
            await _context.SaveChangesAsync();
        }
    }
}
