using CandidateManagementSystem.BL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateManagementSystem.BL.Services.Interfaces
{
    public interface IFileProcessingService
    {
        Task<FileUploadResultDto> ProcessFileAsync(Stream fileStream);
    }
}
