using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/jobs")]
    public class JobsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public JobsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetJobs()
        {
            try
            {
                var jobs = await _context.Jobs.Select(j => j.Job).ToListAsync();
                return Ok(jobs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve job options: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddJob([FromBody] string newJobTitle)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newJobTitle))
                {
                    return BadRequest("Job title cannot be empty.");
                }

                // Check if the job already exists
                var existingJob = await _context.Jobs.FirstOrDefaultAsync(j => j.Job == newJobTitle);
                if (existingJob != null)
                {
                    return Conflict("Job title already exists.");
                }

                // Add the new job to the database
                var newJob = new Jobs { Job = newJobTitle };
                _context.Jobs.Add(newJob);
                await _context.SaveChangesAsync();

                // Return the updated list of jobs
                var jobs = await _context.Jobs.Select(j => j.Job).ToListAsync();
                return Ok(jobs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to add new job: {ex.Message}");
            }
        }
        [HttpDelete("{jobTitle}")]
        public async Task<IActionResult> DeleteJob(string jobTitle)
        {
            try
            {
                var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Job == jobTitle);
                if (job == null)
                {
                    return NotFound($"Job '{jobTitle}' not found.");
                }

                _context.Jobs.Remove(job);
                await _context.SaveChangesAsync();

                return Ok($"Job '{jobTitle}' removed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to remove job: {ex.Message}");
            }
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetJobs()
        {
            try
            {
                // Delete all existing jobs
                var jobs = await _context.Jobs.ToListAsync();
                _context.Jobs.RemoveRange(jobs);
                await _context.SaveChangesAsync();

                // Return an empty list of jobs
                return Ok(new List<string>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to reset jobs: {ex.Message}");
            }
        }
    }
}

    