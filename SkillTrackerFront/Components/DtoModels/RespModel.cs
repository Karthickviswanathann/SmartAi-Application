namespace SkillTrackerFront.Components.DtoModels
{
    public class RespModel
    {
        public string respCode { get; set; }
        public string respDesc { get; set; }
        public string respType { get; set; }
        public object? Data { get; set; }

    }
    public class RespModel1
    {
        public string respCode { get; set; }
        public string respDesc { get; set; }
        public string respType { get; set; }
        public List<NotesActivityDto> data { get; set; }

    }
}
