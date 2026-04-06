using static Smart_Project_Capacity___Effort_Analyzer.Enums.MainEnum;

namespace Smart_Project_Capacity___Effort_Analyzer.Models
{
    public class UserMaster
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public bool IsActive { get; set; } 

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;




    }
}
