using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Xamarin.Essentials;

namespace Backend.Controllers
{
    [ApiController]

    [Route("api/checkins")]
    public class CheckInsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public CheckInsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<CheckIns>>> GetCheckInsByUserId(int userId)
        {
            // Retrieve check-ins for the specified user ID
            var checkIns = await _dbContext.CheckIns
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (checkIns == null || !checkIns.Any())
            {
                return NotFound("No check-ins found for the specified user ID.");
            }

            return Ok(checkIns);
        }

        [HttpPost]
        public async Task<IActionResult> CheckInUser([FromBody] CheckInRequest request)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Code == request.Code);
            if (user == null)
            {
                return NotFound("Invalid code. User not found.");
            }

            try
            {
                var userCheckIn = _dbContext.CheckIns.FirstOrDefault(c => c.UserId == user.Id && c.CheckedOut == 0);

                if (userCheckIn == null)
                {
                    // Create a new check-in entry
                    var newCheckIn = new CheckIns
                    {
                        UserId = user.Id,
                        CheckedIn = 1,
                        CheckInDate = DateTime.Today.ToString("dd-MM-yyyy"),
                        CheckInTime = DateTime.Now.ToString("HH:mm"),
                        User = user,
                        Location = request.Location // Store location data

                    };

                    _dbContext.CheckIns.Add(newCheckIn);
                    _dbContext.SaveChanges();

                    return Ok("Check-in successful.");
                }
                else
                {
                    // Update existing check-in entry
                    userCheckIn.CheckedOut = 1;
                    userCheckIn.CheckOutTime = DateTime.Now.ToString("HH:mm");

                    // Check if the check-out date is different from the check-in date
                    if (DateTime.Today.ToString("dd-MM-yyyy") != userCheckIn.CheckInDate)
                    {
                        // Update status to indicate late check-out
                        userCheckIn.Status = "Late";

                        _dbContext.SaveChanges();

                        return Ok("Check-Out successful\nAlthough you've checked out on a different day");

                    }
                    else
                    {

                        // Calculate total hours
                        TimeSpan totalHours = DateTime.Parse(userCheckIn.CheckOutTime).Subtract(DateTime.Parse(userCheckIn.CheckInTime));

                        // Convert total hours to string with custom format (hours only)
                        string formattedTotalHours = totalHours.ToString("hh"); // Using "hh" for hours only in 12-hour format, or "HH" for 24-hour format
                        // Determine status based on total hours
                        string status;
                        if (totalHours.TotalHours < 8)
                        {
                            status = "Underworking";
                        }
                        else if (totalHours.TotalHours > 9)
                        {
                            status = "Overworking";
                        }
                        else
                        {
                            status = "Normal";
                        }
                        userCheckIn.TotalHours = formattedTotalHours;
                        userCheckIn.Status = status;

                    }

                    _dbContext.SaveChanges();

                    return Ok("Check-out successful.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking in/out user: {ex.Message}");
                return StatusCode(500, "Failed to check-in/out user.");
            }
        }



        public class CheckInRequest
        {
            public int Code { get; set; }
            public string Location { get; set; }
        }






    }
}
