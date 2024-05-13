using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;

namespace Shared.Models
{
    public class CheckIns
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public int CheckedIn { get; set; }
        public int CheckedOut { get; set; }
        public required User User { get; set; }

        public string? CheckInTime { get; set; }
        public string? CheckOutTime { get; set; }
        public string? CheckInDate { get; set; }
        public string? Status { get; set; }
        public string? TotalHours { get; set; }
        public string? Location { get; set; }
        public DateTime GetTotalHoursAsDateTime()
        {
            // Parse the Time string to DateTime using a specific format
            return DateTime.ParseExact(TotalHours, "HH", CultureInfo.InvariantCulture);
        }
        public DateTime GetCheckInTimeAsDateTime()
        {
            // Parse the Time string to DateTime using a specific format
            return DateTime.ParseExact(CheckInTime, "HH:mm", CultureInfo.InvariantCulture);
        }

        public DateTime GetCheckOutTimeAsDateTime()
        {
            // Parse the Time string to DateTime using a specific format
            return DateTime.ParseExact(CheckOutTime, "HH:mm", CultureInfo.InvariantCulture);
        }

        // Method to convert Date string to DateTime object
        public DateTime GetDateAsDateTime()
        {
            // Parse the Date string to DateTime using a specific format
            return DateTime.ParseExact(CheckInDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        }

    }

}
