using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestRESTAPI3.Data.Models;
using TestRESTAPI3.Models;

namespace TestRESTAPI3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public AccountController(UserManager<AppUser> userManager,IConfiguration configuration)
        {
                _userManger = userManager;
            this.configuration = configuration;
        }
        private readonly UserManager<AppUser> _userManger;
        private readonly IConfiguration configuration;

        [HttpPost("Register")]
        public async Task<IActionResult> RegesterNewUser(dtoNewUser user)
        {
            if(ModelState.IsValid)
            {
                AppUser appUser = new()
                {
                    UserName = user.userName,
                    Email = user.email
                };

                IdentityResult result = await _userManger.CreateAsync(appUser, user.password);
                if (result.Succeeded)
                {
                    return Ok("Succes");
                }
                else
                {
                    foreach(var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public async Task <IActionResult> LogIn(dtoLogin login)
        {
            if(ModelState.IsValid)
            {
                AppUser? user = await _userManger.FindByNameAsync(login.userName);
                if(user != null)
                {
                    if(await _userManger.CheckPasswordAsync(user,login.password))
                    {
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()));
                        var roles = await _userManger.GetRolesAsync(user);
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role,role.ToString()));
                        }

   

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                        var sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            claims : claims,
                            issuer : configuration["JWT:Issuer"],
                            audience : configuration["JWT:Audience"],
                            expires : DateTime.Now.AddHours(1),
                            signingCredentials : sc
                            );

                        var _token = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };
                        return Ok(_token);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "username is invalid");
                }
            }
            return BadRequest(ModelState);
        }






    }
}
