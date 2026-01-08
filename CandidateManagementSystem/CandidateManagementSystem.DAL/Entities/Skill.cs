using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateManagementSystem.DAL.Entities
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime GainDate { get; set; }


        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }
    }
}
