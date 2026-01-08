using CandidateManagementSystem.BL.DTOs;
using CandidateManagementSystem.BL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CandidateManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private readonly ICandidateService _candidateService;
        private readonly IFileProcessingService _fileProcessingService;
        private readonly ILogger<CandidatesController> _logger;

        public CandidatesController(
            ICandidateService candidateService,
            IFileProcessingService fileProcessingService,
            ILogger<CandidatesController> logger)
        {
            _candidateService = candidateService;
            _fileProcessingService = fileProcessingService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandidateDto>>> GetAll()
        {
            try
            {
                var candidates = await _candidateService.GetAllCandidatesAsync();
                return Ok(candidates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving candidates");
                return StatusCode(500, "An error occurred while retrieving candidates");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CandidateDto>> GetById(int id)
        {
            try
            {
                var candidate = await _candidateService.GetCandidateByIdAsync(id);
                return Ok(candidate);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving candidate with ID {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the candidate");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CandidateDto>> Create([FromBody] CandidateDto candidateDto)
        {
            try
            {
                var created = await _candidateService.CreateCandidateAsync(candidateDto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating candidate");
                return StatusCode(500, "An error occurred while creating the candidate");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CandidateDto>> Update(int id, [FromBody] CandidateDto candidateDto)
        {
            try
            {
                var updated = await _candidateService.UpdateCandidateAsync(id, candidateDto);
                return Ok(updated);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating candidate with ID {Id}", id);
                return StatusCode(500, "An error occurred while updating the candidate");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _candidateService.DeleteCandidateAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting candidate with ID {Id}", id);
                return StatusCode(500, "An error occurred while deleting the candidate");
            }
        }

        [HttpPost("upload")]
        public async Task<ActionResult<FileUploadResultDto>> UploadFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded");

                if (!file.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    return BadRequest("Only .txt files are allowed");

                using (var stream = file.OpenReadStream())
                {
                    var result = await _fileProcessingService.ProcessFileAsync(stream);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing file upload");
                return StatusCode(500, "An error occurred while processing the file");
            }
        }

        [HttpPost("{candidateId}/skills")]
        public async Task<ActionResult<SkillDto>> AddSkill(int candidateId, [FromBody] SkillDto skillDto)
        {
            try
            {
                var created = await _candidateService.AddSkillToCandidateAsync(candidateId, skillDto);
                return Ok(created);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding skill to candidate {CandidateId}", candidateId);
                return StatusCode(500, "An error occurred while adding the skill");
            }
        }

        [HttpPut("skills/{skillId}")]
        public async Task<ActionResult<SkillDto>> UpdateSkill(int skillId, [FromBody] SkillDto skillDto)
        {
            try
            {
                var updated = await _candidateService.UpdateSkillAsync(skillId, skillDto);
                return Ok(updated);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating skill with ID {SkillId}", skillId);
                return StatusCode(500, "An error occurred while updating the skill");
            }
        }

        [HttpDelete("skills/{skillId}")]
        public async Task<ActionResult> DeleteSkill(int skillId)
        {
            try
            {
                await _candidateService.DeleteSkillAsync(skillId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting skill with ID {SkillId}", skillId);
                return StatusCode(500, "An error occurred while deleting the skill");
            }
        }
    }
}
