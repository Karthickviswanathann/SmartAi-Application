using static Smart_Project_Capacity___Effort_Analyzer.Enums.MainEnum;

namespace Smart_Project_Capacity___Effort_Analyzer.Models.ApiDtos
{
    public class LoginDto
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }

    public class SignInDto
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

    }
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

    }

    public class RegisterMaster
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;


    }

}
