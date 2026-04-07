using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PasswordHashing;
using Smart_Project_Capacity___Effort_Analyzer.Context;
using Smart_Project_Capacity___Effort_Analyzer.Models;
using Smart_Project_Capacity___Effort_Analyzer.Models.ApiDtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Smart_Project_Capacity___Effort_Analyzer.Services
{
    public class DataFlow:IDataFlow
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _config;
        public DataFlow(AppDbContext dbContext, IConfiguration config)
        {
            _dbContext=dbContext;
            _config = config;
        }

        public async Task<RespModel> Login(LoginDto login)
        {
            RespModel respo = new RespModel();

            var hashPassword = ConvertMd5(login.Password);

            var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            var validuser = _dbContext.Usermaster.Where(x => x.Name == login.Username && x.PasswordHash == hashPassword).FirstOrDefault();

        

            if (validuser != null) {

                var userRes = new UserDto
                {
                    Id = validuser.Id,
                    Username = validuser.Name,
                    Email = validuser.Email,
                    //Role=validuser.Role.ToString(),
                    IsActive = validuser.IsActive,

                };

                var token=GenerateToken(userRes);
                respo.respCode = "200";
                respo.respDesc = "User Login Succesfully";
                respo.respType = "Success";
                respo.Data = token;

            }
            else
            {
                respo.respCode = "400";
                respo.respDesc = "Invalid User";
                respo.respType = "Error";

            }
            return respo; 
        }

        public async Task<RespModel> Register(SignInDto userMas)
        {
            RespModel respo = new RespModel();

            var hashPassword = ConvertMd5(userMas.Password);

            var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            bool userExist = _dbContext.Usermaster.Any(x => x.Name == userMas.Username && x.PasswordHash == hashPassword && x.Email==userMas.Email);

            if (userExist)
            {
                respo.respCode = "400";
                respo.respDesc = "User Already Exist";
                respo.respType = "Error";
                return respo;
            }

            else
            {

                var addUser = new UserMaster
                {
                    Name = userMas.Username,
                    PasswordHash = hashPassword,
                    Email = userMas.Email,
                    IsActive = true,
                    CreatedDate=DateTime.Now

                };
                                     
                _dbContext.Usermaster.Add(addUser);
                _dbContext.SaveChanges();
                respo.respCode = "200";
                respo.respDesc = "User Created Succesfully";
                respo.respType = "Succes";
            }

            return respo;
        }


        public async Task<RespModel> GetColor( HttpContext context)
        {

            RespModel respo = new RespModel();


            var token = decryptedToken(context);

            var existUserDetail = _dbContext.UserBehaviour.Where(x => x.UserId == Convert.ToInt16(token[0])).FirstOrDefault();

    
            respo.respCode = "200";
            respo.respDesc = "Get User Behaviour Successfully";
            respo.respType = "Success";
            respo.Data = existUserDetail;
            return respo;


        }


        public async Task<RespModel> GetNotes(HttpContext context)
        {

            RespModel respo = new RespModel();


            var token = decryptedToken(context);

            var existUserDetail = _dbContext.NotesMasters.Where(x => x.UserId == Convert.ToInt16(token[0]));
            var data = await existUserDetail.OrderByDescending(x => x.CreatedDate).ToListAsync();

            respo.respCode = "200";
            respo.respDesc = "Get User Behaviour Successfully";
            respo.respType = "Success";
            respo.Data = data;
            return respo;


        }

        public async Task<RespModel> PostColor(string? themeColor, string? ElementColor,HttpContext context)
        {

            RespModel respo = new RespModel();


            var token = decryptedToken(context);

            var existUserDetail = _dbContext.UserBehaviour.Where(x => x.UserId == Convert.ToInt16(token[0])).FirstOrDefault();


            if (existUserDetail == null)
            {
                existUserDetail = new UserBehaviour
                {
                    UserId = Convert.ToInt16(token[0]),
                    ThemeColor = themeColor,
                    ElementColor = ElementColor
                };

           
                _dbContext.UserBehaviour.Add(existUserDetail);
                _dbContext.SaveChanges();

                respo.respCode = "200";
                respo.respDesc = "User Behaviour  Added Successfully";
                respo.respType = "Success";
                return respo;
            }


            // update only if value passed
            if (!string.IsNullOrWhiteSpace(themeColor))
                existUserDetail.ThemeColor = themeColor;

            if (!string.IsNullOrWhiteSpace(ElementColor))
                existUserDetail.ElementColor = ElementColor;

            await _dbContext.SaveChangesAsync(); // no need Update()

            respo.respCode = "200";
            respo.respDesc = "User  Behaviour Updated Successfully";
            respo.respType = "Success";
            return respo;


        }
        public async Task<RespModel> PostNotes(Notes notes, HttpContext context)
        {

            RespModel respo = new RespModel();


            var token = decryptedToken(context);

            var existUserDetail = _dbContext.NotesMasters.Where(x => x.UserId == Convert.ToInt16(token[0]) && x.Id==notes.Id).FirstOrDefault();


            if (existUserDetail == null)
            {
                existUserDetail = new NotesMaster
                {
                    UserId = Convert.ToInt16(token[0]),
                    Title=notes.Title,
                    NotesText = notes.NotesText,
                    CreatedDate=DateTime.Now,
                    UpdatedDate = null
                };

          
                _dbContext.NotesMasters.Add(existUserDetail);
                _dbContext.SaveChanges();

                respo.respCode = "200";
                respo.respDesc = "User Notes Added Successfully";
                respo.respType = "Success";
                return respo;
            }

            existUserDetail.Title = notes.Title;
            existUserDetail.NotesText = notes.NotesText;
            existUserDetail.UpdatedDate = DateTime.Now;

            _dbContext.SaveChangesAsync();

            respo.respCode = "200";
            respo.respDesc = "User  Behaviour Updated Successfully";
            respo.respType = "Success";
            return respo;


        }


        public async Task<RespModel> DeleteNotes(int Id, HttpContext context)
        {

            RespModel respo = new RespModel();


            var token = decryptedToken(context);

            var existUserDetail = _dbContext.NotesMasters.Where(x => x.UserId == Convert.ToInt16(token[0]) && x.Id == Id).FirstOrDefault();
            _dbContext.NotesMasters.Remove(existUserDetail);
            _dbContext.SaveChangesAsync();

            respo.respCode = "200";
            respo.respDesc = "User  Behaviour Deleted Successfully";
            respo.respType = "Success";
            return respo;


        }

        public async Task<RespModel> UpdatePass (string userName,string password)
        {

            RespModel respo = new RespModel();


            var existUserDetail = _dbContext.Usermaster.Where(x => x.Name == userName).FirstOrDefault();
            
            if(existUserDetail == null)
            {
                respo.respCode = "400";
                respo.respDesc = "User Not Found";
                respo.respType = "Error";
                return respo;
            }

            existUserDetail.PasswordHash = ConvertMd5(password);

            _dbContext.Usermaster.Update(existUserDetail);
            _dbContext.SaveChangesAsync();

            respo.respCode = "200";
            respo.respDesc = "User Update Successfully";
            respo.respType = "Success";
            return respo;


        }



















































        /*---------------------------------------------------- Encryption Potion & Token Creation  -----------------------------------------------*/

        public string ConvertMd5(string value)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(value));

                StringBuilder builder = new StringBuilder("0x");
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("X2"));
                }

                return builder.ToString();
            }
        }



        public string Encryption(string EncText)
        {
            using var aesAlg = Aes.Create();

            string enkey = "42358357407474453245745740747545";
            aesAlg.Key = Encoding.UTF8.GetBytes(enkey);
            aesAlg.Mode = CipherMode.ECB;
            aesAlg.Padding = PaddingMode.PKCS7;
            byte[] encrypted;
            using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
            {
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(EncText);
                encrypted = encryptor.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);
            }
            //byte[] result = new byte[aesAlg.IV.Length + encrypted.Length];
            //Array.Copy(aesAlg.IV, 0, result, 0, aesAlg.IV.Length);
            //Array.Copy(encrypted, 0, result, aesAlg.IV.Length, encrypted.Length);
            return Convert.ToBase64String(encrypted);


        }




        private string EncryptClaims(IEnumerable<Claim> claims)
        {
            var jsonClaims = new JwtPayload(claims);
            var jsonClaimsString = JsonConvert.SerializeObject(jsonClaims);
            return Encryption(jsonClaimsString);
        }


        public string Decrypturls(string encryptedText)
        {
            using var aesAlg = Aes.Create();

            string enkey = "42358357407474453245745740747545";
            aesAlg.Key = Encoding.UTF8.GetBytes(enkey);
            aesAlg.Mode = CipherMode.ECB;
            aesAlg.Padding = PaddingMode.PKCS7;
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] iv = new byte[aesAlg.IV.Length];
            Array.Copy(encryptedBytes, 0, iv, 0, iv.Length);
            byte[] encryptedMessage = new byte[encryptedBytes.Length];
            Array.Copy(encryptedBytes, 0, encryptedMessage, 0, encryptedMessage.Length);

            using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, iv))
            {
                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedMessage, 0, encryptedMessage.Length);
                string decryptedText = Encoding.UTF8.GetString(decryptedBytes);


                try
                {
                    JObject jsonObject = JObject.Parse(decryptedText);
                    return jsonObject.ToString();
                }
                catch (JsonReaderException)
                {

                    return decryptedText;
                }
            }
        }


        public string GenerateToken(UserDto user)
        {
            var tokenkey = Encoding.UTF8.GetBytes(_config.GetRequiredSection("JWT:SecurityKey").Value);
            var TokenHandler = new JwtSecurityTokenHandler();

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Username),
                new Claim("UserId",user.Id.ToString()),
                new Claim("Email",user.Email),
                new Claim("IsActive",user.IsActive.ToString())

            };

            var encrptedclaim = EncryptClaims(claims);


            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                 new List<Claim>
                 {
                    new Claim ("encrptedClaims",encrptedclaim)
                 }
                ),

                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenkey),
                    SecurityAlgorithms.HmacSha512)

            };

            var token = TokenHandler.CreateToken(tokenDesc);

            string finaltoken =  TokenHandler.WriteToken(token);
            return finaltoken;


        }



        public List<string> decryptedToken(HttpContext context)
        {
            var claimsPrincipal = context.User as ClaimsPrincipal;
            var encrptedclaims = claimsPrincipal?.FindFirst("encrptedClaims")?.Value;

            if (encrptedclaims != null)
            {
                var decrptedclaims = Decrypturls(encrptedclaims);
                var decrptjson = JsonConvert.DeserializeObject<Dictionary<string, string>>(decrptedclaims);
                if (decrptjson != null)
                {
                    var userID = decrptjson["UserId"];
                    var email = decrptjson["Email"];
                    var status = decrptjson["IsActive"];
            

                    List<string> TokenVlaues = new List<string>
                     {

                       userID,
                       email,
                       status,
                      
                     };

                    return TokenVlaues;


                }
                return new List<string> { "0" };
            }
            return new List<string> { "0" };
        }

    }
}
