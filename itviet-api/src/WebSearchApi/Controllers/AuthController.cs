using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Helpers;
using WebApi.Helpers.Domains;
using WebApi.Models;
using WebApi.Models.Dtos;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly AppSettings _settings;
        public AuthController(IOptions<AppSettings> settings, IMapper mapper, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _settings = settings.Value;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
        {
            //check params missing
            if (string.IsNullOrEmpty(userForLoginDto.Username) || string.IsNullOrEmpty(userForLoginDto.Password))
            {
                return BadRequest("InvalidLogin");
            }

            var user = await _userManager.FindByNameAsync(userForLoginDto.Username);

            //check use exists
            if (user == null)
            {
                return Ok(new ApiOkResponse(null, 401, false, "InvalidLogin"));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, userForLoginDto.Password, false);

            //check password incorrect
            if (!result.Succeeded)
            {
                return Ok(new ApiOkResponse(null, 401, false, "InvalidLogin"));
            }

            //update last login
            user.LastLogin = DateTime.Now;
            await _userManager.UpdateAsync(user);

            //set token response info
            var tokenResponse = await GenerateToken(user);


            return Ok(new ApiOkResponse(tokenResponse, 201, true, string.Empty));
        }

        [NonAction]
        private async Task<TokenResponse> GenerateToken(UserModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.TokenSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(double.Parse(_settings.TokenExpiryDay)),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var strRoles = await _userManager.GetRolesAsync(user);

            var tokenResponse = new TokenResponse
            {
                AccessToken = tokenHandler.WriteToken(token),
                TokenExpiration = token.ValidTo,
                UserName = user.UserName,
                DisplayName = user.FullName,
                Position = user.OfficerPosition,
                Roles = strRoles,
                UserId = user.Id,
                OrganizeId = user.OrganizeId
            };

            return tokenResponse;
        }
    }
}
