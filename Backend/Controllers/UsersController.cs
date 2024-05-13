
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public UsersController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _dbContext.Users.ToListAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve users: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            if (user == null)
            {
                return BadRequest("Item data is missing");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Surname))
            {
                return BadRequest("Name or Surname is missing");
            }

            if (_dbContext.Users.Any(u => u.Name == user.Name))
            {
                return Conflict("Name already exists");
            }
            if (_dbContext.Users.Any(u => u.Surname == user.Surname))
            {
                return Conflict("Surname already exists");
            }

            Random random = new Random();
            int registrationCode;
            bool isCodeUnique = false;

            do
            {
                registrationCode = random.Next(1000, 9999);  // Generate a 4-digit random integer

                // Check if the generated code already exists in the database
                isCodeUnique = !_dbContext.Users.Any(u => u.Code == registrationCode);
            }
            while (!isCodeUnique);

            var newUser = new User
            {
                Name = user.Name,
                Surname = user.Surname,
                Job = user.Job,
                Code = registrationCode , // Assign the generated registration code
                Time = DateTime.Now.ToString("HH:mm"), // Set current time in 24-hour format
                Date = DateTime.Today.ToString("dd-MM-yyyy") // Set current date in dd-MM-yyyy format

            };


            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

            return Ok(new
            {
                Message = "User registration successful",
                RegistrationCode = registrationCode
            });
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                var user = await _dbContext.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound($"User with ID '{userId}' not found.");
                }

                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();

                return Ok($"User with ID '{userId}' removed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to remove user: {ex.Message}");
            }
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetUsers()
        {
            try
            {
                // Delete all existing users
                var users = await _dbContext.Users.ToListAsync();
                _dbContext.Users.RemoveRange(users);
                await _dbContext.SaveChangesAsync();

                // Return an empty list of users
                return Ok(new List<string>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to reset users: {ex.Message}");
            }
        }
    }
}