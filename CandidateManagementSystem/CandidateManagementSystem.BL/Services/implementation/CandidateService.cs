using CandidateManagementSystem.BL.DTOs;
using CandidateManagementSystem.BL.Services.Interfaces;
using CandidateManagementSystem.DAL.Entities;
using CandidateManagementSystem.DAL.Repositories.Candidate;
using CandidateManagementSystem.DAL.Repositories.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateManagementSystem.BL.Services.implementation
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IValidationService _validationService;

        public CandidateService(
            ICandidateRepository candidateRepository,
            ISkillRepository skillRepository,
            IValidationService validationService)
        {
            _candidateRepository = candidateRepository;
            _skillRepository = skillRepository;
            _validationService = validationService;
        }
        public async Task<SkillDto> AddSkillToCandidateAsync(int candidateId, SkillDto skillDto)
        {
            var candidate = await _candidateRepository.GetByIdAsync(candidateId);
            if (candidate == null)
                throw new KeyNotFoundException($"Candidate with ID {candidateId} not found");

            if (!_validationService.ValidateSkillName(skillDto.Name))
                throw new ArgumentException("Invalid skill name. Skill name cannot contain numbers");

            // Validate skill count
            var currentSkillCount = await _skillRepository.GetSkillCountByCandidateIdAsync(candidateId);
            if (!_validationService.ValidateSkillCount(currentSkillCount, candidate.MaxNumSkills))
                throw new InvalidOperationException($"Cannot add skill. Maximum number of skills ({candidate.MaxNumSkills}) reached");

            // Validate gain date
            if (!_validationService.ValidateSkillGainDate(skillDto.GainDate, candidate.YearsOfExperience))
            {
                var maxDate = DateTime.Today.AddYears(-candidate.YearsOfExperience);
                throw new ArgumentException($"Invalid gain date. Skill gain date must be on or before {maxDate:yyyy-MM-dd} (based on {candidate.YearsOfExperience} years of experience)");
            }

            var skill = new Skill
            {
                CandidateId = candidateId,
                Name = skillDto.Name,
                GainDate = skillDto.GainDate.Date
            };

            var created = await _skillRepository.AddAsync(skill);
            return MapSkillToDto(created);

        }

        public async Task<CandidateDto> CreateCandidateAsync(CandidateDto candidateDto)
        {
            // Validate nickname
            if (!_validationService.ValidateNickname(candidateDto.NickName))
                throw new ArgumentException("Invalid nickname. Must contain digit followed by special character (§, ®, ™, ©, ʬ, @)");

            // Validate email
            if (!_validationService.ValidateEmail(candidateDto.Email))
                throw new ArgumentException("Invalid email format");

            // Check for duplicate email
            if (await _candidateRepository.ExistsByEmailAsync(candidateDto.Email))
                throw new InvalidOperationException("A candidate with this email already exists");

            var candidate = new Candidate
            {
                Name = candidateDto.Name,
                NickName = candidateDto.NickName,
                Email = candidateDto.Email,
                YearsOfExperience = candidateDto.YearsOfExperience,
                MaxNumSkills = candidateDto.MaxNumSkills
            };

            var created = await _candidateRepository.AddAsync(candidate);
            return MapToDto(created);
        }

        public async Task DeleteCandidateAsync(int id)
        {
            var candidate = await _candidateRepository.GetByIdAsync(id);
            if (candidate == null)
                throw new KeyNotFoundException($"Candidate with ID {id} not found");

            await _candidateRepository.DeleteAsync(id);
        }

        public async Task DeleteSkillAsync(int skillId)
        {
            var skill = await _skillRepository.GetByIdAsync(skillId);
            if (skill == null)
                throw new KeyNotFoundException($"Skill with ID {skillId} not found");

            await _skillRepository.DeleteAsync(skillId);
        }

        public async Task<IEnumerable<CandidateDto>> GetAllCandidatesAsync()
        {
            var candidates = await _candidateRepository.GetAllAsync();
            return candidates.Select(MapToDto);
        }

        public async Task<CandidateDto> GetCandidateByIdAsync(int id)
        {
            var candidate = await _candidateRepository.GetByIdAsync(id);
            if (candidate == null)
                throw new KeyNotFoundException($"Candidate with ID {id} not found");

            return MapToDto(candidate);
        }


        public async Task<CandidateDto> UpdateCandidateAsync(int id, CandidateDto candidateDto)
        {
            var candidate = await _candidateRepository.GetByIdAsync(id);
            if (candidate == null)
                throw new KeyNotFoundException($"Candidate with ID {id} not found");

            // Validate nickname
            if (!_validationService.ValidateNickname(candidateDto.NickName))
                throw new ArgumentException("Invalid nickname. Must contain digit followed by special character (§, ®, ™, ©, ʬ, @)");

            // Validate email
            if (!_validationService.ValidateEmail(candidateDto.Email))
                throw new ArgumentException("Invalid email format");

            // Check for duplicate email (excluding current candidate)
            var existingCandidate = await _candidateRepository.GetByEmailAsync(candidateDto.Email);
            if (existingCandidate != null && existingCandidate.Id != id)
                throw new InvalidOperationException("A candidate with this email already exists");

            // Validate skill count if reducing maxNumSkills
            if (candidateDto.MaxNumSkills.HasValue)
            {
                var currentSkillCount = candidate.Skills.Count;
                if (currentSkillCount > candidateDto.MaxNumSkills.Value)
                    throw new InvalidOperationException($"Cannot set MaxNumSkills to {candidateDto.MaxNumSkills.Value}. Candidate already has {currentSkillCount} skills");
            }

            candidate.Name = candidateDto.Name;
            candidate.NickName = candidateDto.NickName;
            candidate.Email = candidateDto.Email;
            candidate.YearsOfExperience = candidateDto.YearsOfExperience;
            candidate.MaxNumSkills = candidateDto.MaxNumSkills;

            await _candidateRepository.UpdateAsync(candidate);

            var updated = await _candidateRepository.GetByIdAsync(id);
            return MapToDto(updated);
        }


        public async Task<SkillDto> UpdateSkillAsync(int skillId, SkillDto skillDto)
        {
            var skill = await _skillRepository.GetByIdAsync(skillId);
            if (skill == null)
                throw new KeyNotFoundException($"Skill with ID {skillId} not found");

            var candidate = await _candidateRepository.GetByIdAsync(skill.CandidateId);

            if (!_validationService.ValidateSkillName(skillDto.Name))
                throw new ArgumentException("Invalid skill name. Skill name cannot contain numbers");

            if (!_validationService.ValidateSkillGainDate(skillDto.GainDate, candidate.YearsOfExperience))
            {
                var maxDate = DateTime.Today.AddYears(-candidate.YearsOfExperience);
                throw new ArgumentException($"Invalid gain date. Skill gain date must be on or before {maxDate:yyyy-MM-dd} (based on {candidate.YearsOfExperience} years of experience)");
            }

            skill.Name = skillDto.Name;
            skill.GainDate = skillDto.GainDate.Date;

            await _skillRepository.UpdateAsync(skill);

            var updated = await _skillRepository.GetByIdAsync(skillId);
            return MapSkillToDto(updated);
        }


        private CandidateDto MapToDto(Candidate candidate)
        {
            return new CandidateDto
            {
                Id = candidate.Id,
                Name = candidate.Name,
                NickName = candidate.NickName,
                Email = candidate.Email,
                YearsOfExperience = candidate.YearsOfExperience,
                MaxNumSkills = candidate.MaxNumSkills,
                Skills = candidate.Skills?.Select(MapSkillToDto).ToList() ?? new List<SkillDto>()
            };
        }

        private SkillDto MapSkillToDto(Skill skill)
        {
            return new SkillDto
            {
                Id = skill.Id,
                CandidateId = skill.CandidateId,
                Name = skill.Name,
                GainDate = skill.GainDate
            };
        }
    }
}