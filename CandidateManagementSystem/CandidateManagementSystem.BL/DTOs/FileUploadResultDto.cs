using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateManagementSystem.BL.DTOs
{
    public class FileUploadResultDto
    {
        public int TotalRecords { get; set; }
        public int LoadedRecords { get; set; }
        public int SkippedDuplicates { get; set; }
        public int InvalidRecords { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
