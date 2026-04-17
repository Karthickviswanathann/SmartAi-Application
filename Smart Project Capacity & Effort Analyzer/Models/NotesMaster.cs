namespace Smart_Project_Capacity___Effort_Analyzer.Models
{
    public class NotesMaster
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Title { get; set; }
        public string? NotesText { get; set; }
        public string? Workspace { get; set; }
        public bool IsPinned  { get; set; }
        public bool IsUrcheive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public UserMaster User { get; set; }

    }


    public class Notes
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? NotesText { get; set; }
        public string? Workspace { get; set; }


    }
}
