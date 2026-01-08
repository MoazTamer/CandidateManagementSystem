using CandidateManagementSystem.BL.DTOs;
using CandidateManagementSystem.BL.Services.Interfaces;
using CandidateManagementSystem.DAL.Entities;
using CandidateManagementSystem.DAL.Repositories.Candidate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateManagementSystem.BL.Services.implementation
{
    public class FileProcessingService : IFileProcessingService
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IValidationService _validationService;

        public FileProcessingService(
            ICandidateRepository candidateRepository,
            IValidationService validationService)
        {
            _candidateRepository = candidateRepository;
            _validationService = validationService;
        }
        public async Task<FileUploadResultDto> ProcessFileAsync(Stream fileStream)
        {
            var result = new FileUploadResultDto();
            var candidatesToAdd = new List<Candidate>();
            var lineNumber = 0;

            using (var reader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string headerLine = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(headerLine))
                {
                    result.Errors.Add("File is empty or missing header");
                    return result;
                }

                var headers = headerLine.Split(new[] { "\t\t" }, StringSplitOptions.None);
                if (headers.Length != 5 ||
                    !headers[0].Equals("#Name", StringComparison.OrdinalIgnoreCase) ||
                    !headers[1].Equals("#NickName", StringComparison.OrdinalIgnoreCase) ||
                    !headers[2].Equals("#Email", StringComparison.OrdinalIgnoreCase) ||
                    !headers[3].Equals("#YearsOfExperience", StringComparison.OrdinalIgnoreCase) ||
                    !headers[4].Equals("#MaxNumSkills", StringComparison.OrdinalIgnoreCase))
                {
                    result.Errors.Add(
                        "Invalid header format. Expected columns (double-tab separated):\n" +
                        "#Name\t\t#Nickname\t\t#Email\t\t#YearsOfExperience\t\t#MaxNumSkills"
                    );

                    return result;
                }

                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    lineNumber++;
                    result.TotalRecords++;

                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var fields = line.Split(new[] { "\t\t" }, StringSplitOptions.None);

                    if (fields.Length != 5)
                    {
                        result.InvalidRecords++;
                        result.Errors.Add($"Line {lineNumber}: Invalid number of fields");
                        continue;
                    }

                    var name = fields[0].Trim();
                    var nickname = fields[1].Trim();
                    var email = fields[2].Trim();
                    var yearsOfExperienceStr = fields[3].Trim();
                    var maxNumSkillsStr = fields[4].Trim();

                    if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(email))
                    {
                        result.InvalidRecords++;
                        result.Errors.Add($"Line {lineNumber}: Missing required fields");
                        continue;
                    }

                    if (!_validationService.ValidateEmail(email))
                    {
                        result.InvalidRecords++;
                        result.Errors.Add($"Line {lineNumber}: Invalid email format");
                        continue;
                    }

                    if (!_validationService.ValidateNickname(nickname))
                    {
                        result.InvalidRecords++;
                        result.Errors.Add($"Line {lineNumber}: Invalid nickname (must contain digit followed by special character: §,®,™,©,ʬ,@)");
                        continue;
                    }

                    if (!int.TryParse(yearsOfExperienceStr, out int yearsOfExperience) || yearsOfExperience < 0)
                    {
                        result.InvalidRecords++;
                        result.Errors.Add($"Line {lineNumber}: Invalid years of experience");
                        continue;
                    }

                    int? maxNumSkills = null;
                    if (!string.IsNullOrEmpty(maxNumSkillsStr))
                    {
                        if (int.TryParse(maxNumSkillsStr, out int parsedMaxSkills) && parsedMaxSkills >= 0)
                        {
                            maxNumSkills = parsedMaxSkills;
                        }
                        else
                        {
                            result.InvalidRecords++;
                            result.Errors.Add($"Line {lineNumber}: Invalid max num skills");
                            continue;
                        }
                    }

                    var candidate = new Candidate
                    {
                        Name = name,
                        NickName = nickname,
                        Email = email,
                        YearsOfExperience = yearsOfExperience,
                        MaxNumSkills = maxNumSkills
                    };

                    candidatesToAdd.Add(candidate);
                }
            }

            if (candidatesToAdd.Any())
            {
                var emails = candidatesToAdd.Select(c => c.Email).ToList();
                var existingCandidates = await _candidateRepository.GetByEmailsAsync(emails);
                var existingEmails = new HashSet<string>(existingCandidates
                    .Select(c => c.Email), StringComparer.OrdinalIgnoreCase);

                foreach (var candidate in candidatesToAdd)
                {
                    if (existingEmails.Contains(candidate.Email))
                    {
                        result.SkippedDuplicates++;
                    }
                    else
                    {
                        await _candidateRepository.AddAsync(candidate);
                        result.LoadedRecords++;
                    }
                }
            }

            return result;
        }

    }
}
