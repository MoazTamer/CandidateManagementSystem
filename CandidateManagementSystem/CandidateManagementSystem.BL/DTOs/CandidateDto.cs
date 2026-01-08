using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateManagementSystem.BL.DTOs
{
    public class CandidateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public int YearsOfExperience { get; set; }
        public int? MaxNumSkills { get; set; }
        public List<SkillDto> Skills { get; set; } = new List<SkillDto>();
    }
}
