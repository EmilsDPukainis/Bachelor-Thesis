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
        
    }

}
