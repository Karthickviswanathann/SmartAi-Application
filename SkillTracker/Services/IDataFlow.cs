using Smart_Project_Capacity___Effort_Analyzer.Models;
using Smart_Project_Capacity___Effort_Analyzer.Models.ApiDtos;

namespace Smart_Project_Capacity___Effort_Analyzer.Services
{
    public interface IDataFlow
    {
        public Task<RespModel> Login(LoginDto login);

        public Task<RespModel> Register(RegisterMaster userMas);

    }
}
