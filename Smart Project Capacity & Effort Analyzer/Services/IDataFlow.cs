using Smart_Project_Capacity___Effort_Analyzer.Models;
using Smart_Project_Capacity___Effort_Analyzer.Models.ApiDtos;

namespace Smart_Project_Capacity___Effort_Analyzer.Services
{
     public interface IDataFlow
     {
         Task<RespModel> Login(LoginDto login);
         Task<RespModel> Register(SignInDto userMas);
         Task<RespModel> UpdatePass(string userName, string password);
         Task<RespModel> GetColor(HttpContext context);
         Task<RespModel> DeleteNotes(int Id, HttpContext context);
        Task<RespModel> PostNotesActivity(string? IsPinned, string? IsUrceive, int Id, HttpContext context);
         Task<RespModel> PostColor(string? themeColor, string? ElementColor,HttpContext context);
         Task<RespModel> PostNotes(Notes notes,HttpContext context);
         Task<RespModel> GetNotes(HttpContext context);
     }
}
