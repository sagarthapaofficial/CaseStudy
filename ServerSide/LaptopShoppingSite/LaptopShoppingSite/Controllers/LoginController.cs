using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System;
using LaptopShoppingSite.DAL.DomainClasses;
using LaptopShoppingSite.DAL.DAO;
using LaptopShoppingSite.DAL.Helpers;
using LaptopShoppingSite.DAL;
using Microsoft.AspNetCore.Authorization;

namespace LaptopShoppingSite.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        StoreContext _db;
        IConfiguration configuration;
       //does dependency injection of the context class and configuration class.
        
        public LoginController(StoreContext context, IConfiguration config)
        {
            _db = context;
            this.configuration = config;
        }

        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<CustomerHelper>> Index(CustomerHelper helper)
        {
            CustomerDAO dao = new CustomerDAO(_db);
            Customer Customer = await dao.GetByEmail(helper.email);
           
            //if user found
            if (Customer != null)
            {
                //we verify the password.
                if (VerifyPassword(helper.password, Customer.Hash, Customer.Salt))
                {
                    helper.password = "";
                    var appSettings = configuration.GetSection("AppSettings").GetValue<string>("Secret");
                    
                    // authentication successful so generate jwt token
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(appSettings);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, Customer.Id.ToString())
                    }),

                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    string returnToken = tokenHandler.WriteToken(token);
                    helper.token = returnToken;
                }
                else
                {
                    helper.token = "Customername or password invalid - login failed";
                }
            }
            else
            {
                helper.token = "no such Customer - login failed";
            }
            return helper;
        }
        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 10000);
            return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) == storedHash;
        }



    }
}
