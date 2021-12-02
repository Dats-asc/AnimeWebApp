namespace AnimeWebApp.Models
{
    public class ProfileViewModel
    {
        public int? BirthYear { get; set; }
        public int? BirthMounth { get; set; }
        public int? BirthDay { get; set; }
        public string? Sex { get; set; }
        public string? City { get; set; }
        public string? Description { get; set; }
        
        public User ProfileOwner { get; set; }
    }
}