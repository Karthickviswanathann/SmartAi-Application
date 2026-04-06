

namespace SkillTrackerFront.Components.DtoModels
{
    public class UserMaster
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }


    }

    public class Register
    {

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }


    }

}
