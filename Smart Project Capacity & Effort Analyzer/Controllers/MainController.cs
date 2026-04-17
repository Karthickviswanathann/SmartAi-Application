using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart_Project_Capacity___Effort_Analyzer.Context;
using Smart_Project_Capacity___Effort_Analyzer.Models;
using Smart_Project_Capacity___Effort_Analyzer.Models.ApiDtos;
using Smart_Project_Capacity___Effort_Analyzer.Services;

namespace Smart_Project_Capacity___Effort_Analyzer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly IDataFlow _dataFlow;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public MainController(IDataFlow dataFlow, IConfiguration configuration)
        {
           _dataFlow = dataFlow;
            _configuration = configuration;
        }



        [AllowAnonymous]
        [HttpPost("Login")]

        public async Task<IActionResult> Login(LoginDto login)
        {
            var res = await _dataFlow.Login(login);
            if(res.respCode != "200")
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [AllowAnonymous]
        [HttpPost("Register")]

        public async Task<IActionResult> Register(SignInDto userMaster)
        {
            var res = await _dataFlow.Register(userMaster);
            if (res.respCode != "200")
            {
                return BadRequest(res);
            }
            return Ok(res);
        }


        [HttpGet("GetBehaviour")]
        
        public async Task<IActionResult> GetBehaviour()
        {
            var res = await _dataFlow.GetColor(HttpContext);

            if (res.respCode != "200")
            {
                return BadRequest(res);
            }

            return Ok(res);
        }


        [HttpPost("UpdateColors")]

        public async Task<IActionResult> UpdateColors(string? themeColor,string? ElementColor)
        {
            var res = await _dataFlow.PostColor(themeColor, ElementColor, HttpContext);

            if (res.respCode != "200")
            {
                return BadRequest(res);
            }

            return Ok(res);
        }


        
        [HttpPost("UpdateNotesActivity")]

        public async Task<IActionResult> UpdateNotesActivity(string? Pinned, string? Urcheive,int noteId)
        {
            var res = await _dataFlow.PostNotesActivity(Pinned, Urcheive, noteId, HttpContext);

            if (res.respCode != "200")
            {
                return BadRequest(res);
            }

            return Ok(res);
        }


        [HttpGet("GetNotes")]

        public async Task<IActionResult> GetNotes()
        {
            var res = await _dataFlow.GetNotes(HttpContext);

            if (res.respCode != "200")
            {
                return BadRequest(res);
            }

            return Ok(res);
        }




        [HttpPost("PostNotes")]
        public async Task<IActionResult> PostNotes(Notes notes)
        {
            var res = await _dataFlow.PostNotes(notes, HttpContext);

            if (res.respCode != "200")
            {
                return BadRequest(res);
            }

            return Ok(res);
        }

        [HttpDelete("DeleteNotes")]

        public async Task<IActionResult> DeleteNotes(int noteId)
        {
            var res = await _dataFlow.DeleteNotes(noteId, HttpContext);

            if (res.respCode != "200")
            {
                return BadRequest(res);
            }

            return Ok(res);
        }





        [HttpPost("UpdateUser")]
        public async Task<IActionResult> ForgetPassword(string username,string password)
        {
            var res = await _dataFlow.UpdatePass(username,password);

            if (res.respCode!="200")
            {
                return BadRequest(res);
            }

            return Ok(res);

        }

    }
}
