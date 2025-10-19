using AutoMapper;
using BuzzTalk.Business.Dtos;
using BuzzTalk.Business.Services;
using BuzzTalk.Server.Hubs;
using BuzzTalk.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BuzzTalk.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IHubContext<BuzzChatHub,IbuzzChatHub> _hubContext;

        public AccountController(IAccountService accountService,IMapper mapper,IConfiguration config,IHubContext<BuzzChatHub,IbuzzChatHub> hubContext)
        {
            _accountService = accountService;
            _mapper = mapper;
            _config = config;
           _hubContext = hubContext;
        }
        [HttpPost("SignIn")]
        public async Task<IActionResult> Sign(SigninModel model)
        {
            var user = _mapper.Map<UserDto>(model);
            var result = await _accountService.SignIn(user);
            if (result.Item1)
            {
                UserModelHub user1 = new()
                {
                    Id = result.Item3,
                    Name=model.Name,
                    Email=model.Email,
                    Username=model.Username
                };
                await _hubContext.Clients.All.NewUserSignIn(user1);
                return Ok(result.Item2);
            }
            return BadRequest(result.Item2);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var result = await _accountService.Login( login.username,login.password);
            var user = _mapper.Map<UserModel>(result);
            user.Token = GenerateToken(user);
            if (result != null)
            {
                return Ok(user);
            }
            return BadRequest("Invalid username or password");
        }
        private string GenerateToken(UserModel user)
        {
           var secureKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
           var credentials = new SigningCredentials(secureKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("GetAllUsers")]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            var id = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var users = await _accountService.GetAllUsers(id);
            var data = _mapper.Map<List<UserModel>>(users);
            if (users != null)
            {
                return Ok(_mapper.Map<List<UserModelHub>>(data));
            }
            return NotFound("No users found");
        }

    }
}
