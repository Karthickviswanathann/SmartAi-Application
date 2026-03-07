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

            var userRes = new UserDto
            {
                Id=validuser.Id,
                Username = validuser.Name,
                Email=validuser.Email,
                Role=validuser.Role.ToString(),
                IsActive=validuser.IsActive,

            };

            if (validuser != null) {

                var token=GenerateToken(userRes);
                respo.respCode = "200";
                respo.respDesc = "User Login Succesfully";
                respo.respType = "Error";
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

        public async Task<RespModel> Register(RegisterMaster userMas)
        {
            RespModel respo = new RespModel();

            var hashPassword = ConvertMd5(userMas.Password);

            var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            bool userExist = _dbContext.Usermaster.Any(x => x.Name == userMas.Name && x.PasswordHash == hashPassword && x.Email==userMas.Email);

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
                    Name = userMas.Name,
                    PasswordHash = hashPassword,
                    Email = userMas.Email,
                    IsActive = userMas.IsActive,
                    Role = userMas.Role,
                    CreatedDate=userMas.CreatedDate

                };
                                     
                _dbContext.Usermaster.Add(addUser);
                _dbContext.SaveChanges();
                respo.respCode = "200";
                respo.respDesc = "User Created Succesfully";
                respo.respType = "Succes";
            }

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





        public string GenerateToken(UserDto user)
        {
            var tokenkey = Encoding.UTF8.GetBytes(_config.GetRequiredSection("JWT:SecurityKey").Value);
            var TokenHandler = new JwtSecurityTokenHandler();

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Username),
                new Claim("UserId",user.Id.ToString()),
                new Claim("Email",user.Email),
                new Claim("Role",user.Role),
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




    }
}
