using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Shared.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Job is required")]
        public string Job { get; set; }

        public int Code { get; set; }

        public string? Time { get; set; }

        public string? Date { get; set; }

        [NotMapped]
        public string FullName => $"{Name} {Surname} {Job}";
        public DateTime GetTimeAsDateTime()
        {
            // Parse the Time string to DateTime using a specific format
            return DateTime.ParseExact(Time, "HH:mm", CultureInfo.InvariantCulture);
        }

        // Method to convert Date string to DateTime object
        public DateTime GetDateAsDateTime()
        {
            // Parse the Date string to DateTime using a specific format
            return DateTime.ParseExact(Date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        }
    }

}
