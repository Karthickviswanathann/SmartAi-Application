using SkillTrackerFront.Components.DtoModels;

namespace SkillTrackerFront.Services.Interface
{
    public interface IDataFlow
    {
        Task<RespModel> Login(string username, string password);
        Task<RespModel> Register(Register register);
        Task<RespModel> PostBehaviour(string? ThemeColor, string? ElementColor, string token);
        Task<RespModel> PostNotes(AddNotesDto notes, string token);
        Task<RespModel> GetBehaviour(string token);
        Task<RespModel> GetNotes(string token);
    }
}
