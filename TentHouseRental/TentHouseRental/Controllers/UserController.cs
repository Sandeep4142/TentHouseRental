using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TentHouseRental.BussinessLogic;

namespace TentHouseRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ITentHouseRentalService houseRentalService;
        private readonly IConfiguration config;

        public UserController(ITentHouseRentalService houseRentalService, IConfiguration config)
        {
            this.houseRentalService = houseRentalService;
            this.config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int userId = await houseRentalService.IsUserExist(user.Email, user.Password);

            if (userId > 0)
            {
                var token = GenerateToken();
                var response = Ok(new { token = token });
                return response;
            }
            else if (userId == 0)
            {
                return Unauthorized("Invalid password");
            }
            else
            {
                return NotFound("User not found");
            }
        }

        private string GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(config["Jwt:Issuer"], config["Jwt:Audience"], null,
                expires: DateTime.Now.AddHours(1), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
