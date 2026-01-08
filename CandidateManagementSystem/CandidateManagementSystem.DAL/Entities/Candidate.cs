using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateManagementSystem.DAL.Entities
{
    public class Candidate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public int YearsOfExperience { get; set; }
        public int? MaxNumSkills { get; set; }

        public ICollection<Skill> Skills { get; set; } = new List<Skill>();
    }
}
