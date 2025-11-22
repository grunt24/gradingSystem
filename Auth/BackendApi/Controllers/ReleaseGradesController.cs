using BackendApi.Context;
using BackendApi.Core.Models;
using BackendApi.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReleaseGradesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthRepository _authRepository;

        public ReleaseGradesController(AppDbContext context, IAuthRepository authRepository)
        {
            _context = context;
            _authRepository = authRepository;
        }

        /// <summary>
        /// Release or hide all Midterm grades for students.
        /// </summary>
        [HttpPut("release-midterm")]
        public async Task<IActionResult> ReleaseAllMidtermGrades([FromQuery] bool isVisible = true)
        {
            var currentUser = await _authRepository.GetCurrentUserAsync();

            if (currentUser.Role != UserRole.Admin )
                return Forbid("Only Admin users can release grades.");

            var midtermGrades = await _context.MidtermGrades.ToListAsync();

            if (!midtermGrades.Any())
                return NotFound("No midterm grades found.");

            foreach (var grade in midtermGrades)
                grade.IsVisible = isVisible;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = $"All midterm grades {(isVisible ? "released" : "hidden")} successfully.",
                UpdatedCount = midtermGrades.Count
            });
        }

        /// <summary>
        /// Release or hide all Finals grades for students.
        /// </summary>
        [HttpPut("release-finals")]
        public async Task<IActionResult> ReleaseAllFinalsGrades([FromQuery] bool isVisible = true)
        {
            var currentUser = await _authRepository.GetCurrentUserAsync();

            if (currentUser.Role != UserRole.Admin)
                return Forbid("Only Admin users can release grades.");

            var finalsGrades = await _context.FinalsGrades.ToListAsync();

            if (!finalsGrades.Any())
                return NotFound("No finals grades found.");

            foreach (var grade in finalsGrades)
                grade.IsVisible = isVisible;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = $"All finals grades {(isVisible ? "released" : "hidden")} successfully.",
                UpdatedCount = finalsGrades.Count
            });
        }
        [HttpPut("toggle-visibility")]
        public async Task<IActionResult> ToggleGradeVisibility(
    [FromQuery] int userId,
    [FromQuery] bool isVisible,
    [FromQuery] string gradeType) // "midterm" or "finals"
        {
            var currentUser = await _authRepository.GetCurrentUserAsync();

            if (currentUser.Role != UserRole.Admin)
                return Forbid("Only Admin users can update grade visibility.");

            if (gradeType.ToLower() == "midterm")
            {
                var grade = await _context.MidtermGrades
                    .FirstOrDefaultAsync(g => g.StudentId == userId);

                if (grade == null)
                    return NotFound($"No midterm grade found for StudentId {userId}.");

                grade.IsVisible = isVisible;
            }
            else if (gradeType.ToLower() == "finals")
            {
                var grade = await _context.FinalsGrades
                    .FirstOrDefaultAsync(g => g.StudentId == userId);

                if (grade == null)
                    return NotFound($"No finals grade found for StudentId {userId}.");

                grade.IsVisible = isVisible;
            }
            else
            {
                return BadRequest("Invalid gradeType. Use 'midterm' or 'finals'.");
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = $"Grade visibility updated for StudentId {userId}",
                IsVisible = isVisible,
                GradeType = gradeType
            });
        }

    }
}
