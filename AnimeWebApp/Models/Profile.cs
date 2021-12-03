using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimeWebApp.Models
{
    public class Profile
    {
        [Key]
        [ForeignKey("User")]
        public Guid Id { get; set; }
        public int? BirthYear { get; set; }
        public int? BirthMounth { get; set; }
        public int? BirthDay { get; set; }
        public string? Sex { get; set; }
        public string? City { get; set; }
        public string? Description { get; set; }
        
        //public string RegistrationDate { get; set; }
        
        public User User { get; set; }
    }
}