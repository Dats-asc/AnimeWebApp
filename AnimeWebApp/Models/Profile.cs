using System;

namespace AnimeWebApp.Models
{
    public class Profile
    {
        public Guid Id { get; set; }
        public int? BirthYear { get; set; }
        public int? BirthMounth { get; set; }
        public int? BirthDay { get; set; }
        public string? Sex { get; set; }
        public string? City { get; set; }
        public string? Description { get; set; }
        
        public User ProfileOwner { get; set; }
    }
}