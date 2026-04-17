namespace SkillTrackerFront.Components.DtoModels
{
    public class AddNotesDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? NotesText { get; set; }
        public string? Workspace { get; set; }
    }

    public class NotesActivityDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? NotesText { get; set; }
        public string? Workspace { get; set; }
        public bool isPinned { get; set; }
        public bool isUrcheive { get; set; }
    }


}
