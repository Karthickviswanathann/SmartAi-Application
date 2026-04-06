using Microsoft.AspNetCore.Authentication.BearerToken;
using Newtonsoft.Json;
using SkillTrackerFront.Components.DtoModels;
using SkillTrackerFront.Services.Interface;
using System.Net.Http.Headers;
using System.Text;

namespace SkillTrackerFront.Services
{
    public class DataFlow:IDataFlow
    {

        private readonly IConfiguration _configuration;
        public DataFlow(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<RespModel> Login(string username,string password)
        {
            RespModel respModel = new RespModel();

            var client = new HttpClient();

            var appurl = _configuration.GetRequiredSection("ApiUrl").Value;
            var url = appurl + "login";

            var request = new
            {
                username = username,
                password = password
            };


            var result = await client.PostAsJsonAsync(url, request);

            var respoCnt = await result.Content.ReadAsStringAsync();

            respModel = JsonConvert.DeserializeObject<RespModel>(respoCnt);

            return respModel;
      }


        public async Task<RespModel> Register(Register register)
        {
            RespModel respModel = new RespModel();

            var client = new HttpClient();

            var appurl = _configuration.GetRequiredSection("ApiUrl").Value;

            var url = appurl + "Register";

          
            var result = await client.PostAsJsonAsync(url, register);

            var respoCnt = await result.Content.ReadAsStringAsync();

            respModel = JsonConvert.DeserializeObject<RespModel>(respoCnt);

            return respModel;
        }


        public async Task<RespModel> PostBehaviour(string? ThemeColor,string? ElementColor,string token)
        {
            RespModel respModel = new RespModel();

            var client = new HttpClient();

            var appurl = _configuration.GetRequiredSection("ApiUrl").Value;


            var url = appurl + $"UpdateColors?themeColor={ThemeColor}&ElementColor={ElementColor}";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var result = await client.PostAsync(url, null);

            var respoCnt = await result.Content.ReadAsStringAsync();

            respModel = JsonConvert.DeserializeObject<RespModel>(respoCnt);

            return respModel;
        }


        public async Task<RespModel> PostNotes(AddNotesDto notes, string token)
        {
            RespModel respModel = new RespModel();

            var client = new HttpClient();

            var appurl = _configuration.GetRequiredSection("ApiUrl").Value;
            var url = appurl + $"PostNotes";
            var json = JsonConvert.SerializeObject(notes);

            var reqCont = new StringContent(json, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);       
            var result = await client.PostAsync(url, reqCont);

            var respoCnt = await result.Content.ReadAsStringAsync();

            respModel = JsonConvert.DeserializeObject<RespModel>(respoCnt);

            return respModel;
        }


        public async Task<RespModel> GetBehaviour(string token)
        {
            RespModel respModel = new RespModel();

            var client = new HttpClient();

            var appurl = _configuration.GetRequiredSection("ApiUrl").Value;
            var url = appurl + $"GetBehaviour";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var result = await client.GetAsync(url);
            var respoCnt = await result.Content.ReadAsStringAsync();
            respModel = JsonConvert.DeserializeObject<RespModel>(respoCnt);

            return respModel;
        }


        public async Task<RespModel> GetNotes(string token)
        {
            RespModel respModel = new RespModel();

            var client = new HttpClient();

            var appurl = _configuration.GetRequiredSection("ApiUrl").Value;
            var url = appurl +"GetNotes";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var result = await client.GetAsync(url);
            var respoCnt = await result.Content.ReadAsStringAsync();
            respModel = JsonConvert.DeserializeObject<RespModel>(respoCnt);

            return respModel;
        }

    }
}
