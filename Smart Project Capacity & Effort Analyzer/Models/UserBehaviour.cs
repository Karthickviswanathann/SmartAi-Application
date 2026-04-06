namespace Smart_Project_Capacity___Effort_Analyzer.Models
{
    public class UserBehaviour
    {

        public int Id { get; set; }

        public int UserId { get; set; }

        public UserMaster User { get; set; }

        public string ThemeColor { get; set; }
        public string ElementColor { get; set; }
    }
}
