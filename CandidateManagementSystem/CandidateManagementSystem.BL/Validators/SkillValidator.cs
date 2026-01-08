using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CandidateManagementSystem.BL.Validators
{
    public class SkillValidator
    {
        private static readonly Regex NoDigitsPattern = new Regex(@"^\D+$", RegexOptions.Compiled);

        public static bool IsValidSkillName(string skillName)
        {
            if (string.IsNullOrWhiteSpace(skillName))
                return false;

            return NoDigitsPattern.IsMatch(skillName);
        }

        public static bool IsValidGainDate(DateTime gainDate, int yearsOfExperience)
        {
            var maxAllowedDate = DateTime.Today.AddYears(-yearsOfExperience);
            return gainDate.Date <= maxAllowedDate;
        }
    }
}
