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
    }

}
