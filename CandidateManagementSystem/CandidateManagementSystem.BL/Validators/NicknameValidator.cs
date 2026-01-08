using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CandidateManagementSystem.BL.Validators
{
    public class NicknameValidator
    {
        private static readonly Regex NicknamePattern = new Regex(@"\d[§®™©ʬ@]", RegexOptions.Compiled);

        public static bool IsValid(string nickname)
        {
            if (string.IsNullOrWhiteSpace(nickname))
                return false;

            return NicknamePattern.IsMatch(nickname);
        }
    }
}
