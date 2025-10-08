using API.Dtos;
using API.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tour.Controllers
{
    [ApiController]
    [Route("api/checkpoints")]
    public class CheckpointController : ControllerBase
    {
        private readonly ICheckpointService _checkpointService;

        public CheckpointController(ICheckpointService checkpointService)
        {
            _checkpointService = checkpointService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCheckpoint([FromBody] CheckpointDto checkpointDto)
        {
            if (!string.IsNullOrEmpty(checkpointDto.ImageBase64))
            {
                var imageBytes = Convert.FromBase64String(checkpointDto.ImageBase64);
                var fileName = $"{Guid.NewGuid()}.jpg";
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "checkpoints");
                Directory.CreateDirectory(folderPath);
                var filePath = Path.Combine(folderPath, fileName);
                await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);
                checkpointDto.ImageUrl = $"/images/checkpoints/{fileName}";
            }

            var createdCheckpoint = await _checkpointService.CreateCheckpointAsync(checkpointDto);
            return Ok(createdCheckpoint);
        }

        [Authorize]
        [HttpGet("tour-checkpoints/${tourId}")]
        public async Task<IActionResult> GetTourCheckpoints(int tourId)
        {
            var checkpoints = await _checkpointService.GetTourCheckpoints(tourId);
            return Ok(checkpoints);
        }

        [Authorize]
        [HttpGet("checkpoint/${checkpointId}")]
        public async Task<IActionResult> GetCheckpoint(int checkpointId)
        {
            var checkpoints = await _checkpointService.GetCheckpointById(checkpointId);
            return Ok(checkpoints);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteCheckpoint(int checkpointId)
        {
            try
            {

                await _checkpointService.DeleteCheckpointAsync(checkpointId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateCheckpoint([FromBody] CheckpointDto checkpointDto)
        {
            if (!string.IsNullOrEmpty(checkpointDto.ImageBase64))
            {
                var imageBytes = Convert.FromBase64String(checkpointDto.ImageBase64);
                var fileName = $"{Guid.NewGuid()}.jpg";
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "checkpoints");
                Directory.CreateDirectory(folderPath);
                var filePath = Path.Combine(folderPath, fileName);
                await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);
                checkpointDto.ImageUrl = $"/images/checkpoints/{fileName}";
            }
            var checkpoint = await _checkpointService.UpdateAsync(checkpointDto);
            return Ok(checkpoint);
        }

    }
}
