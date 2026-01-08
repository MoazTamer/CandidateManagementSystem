using CandidateManagementSystem.BL.Services.Interfaces;
using CandidateManagementSystem.BL.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateManagementSystem.BL.Services.implementation
{
    public class ValidationService : IValidationService
    {
        public bool ValidateEmail(string email)
        {
            return EmailValidator.IsValid(email);
        }

        public bool ValidateNickname(string nickname)
        {
            return NicknameValidator.IsValid(nickname);
        }

        public bool ValidateSkillCount(int currentCount, int? maxSkills)
        {
            if (!maxSkills.HasValue)
                return true;

            return currentCount < maxSkills.Value;
        }

        public bool ValidateSkillGainDate(DateTime gainDate, int yearsOfExperience)
        {
            return SkillValidator.IsValidGainDate(gainDate, yearsOfExperience);
        }

        public bool ValidateSkillName(string skillName)
        {
            return SkillValidator.IsValidSkillName(skillName);
        }
    }
}
