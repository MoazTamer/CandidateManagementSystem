using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateManagementSystem.BL.Services.Interfaces
{
    public interface IValidationService
    {
        bool ValidateNickname(string nickname);
        bool ValidateEmail(string email);
        bool ValidateSkillName(string skillName);
        bool ValidateSkillGainDate(DateTime gainDate, int yearsOfExperience);
        bool ValidateSkillCount(int currentCount, int? maxSkills);
    }
}
