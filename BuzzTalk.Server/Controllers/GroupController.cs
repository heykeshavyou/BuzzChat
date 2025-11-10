using AutoMapper;
using BuzzTalk.Business.Dtos;
using BuzzTalk.Business.Services;
using BuzzTalk.Server.Hubs;
using BuzzTalk.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BuzzTalk.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly IHubContext<BuzzChatHub, IbuzzChatHub> _hubContext;

        public GroupController(IGroupService groupService, IMapper mapper, IWebHostEnvironment env, IHubContext<BuzzChatHub, IbuzzChatHub> hubContext)
        {
            _groupService = groupService;
            _mapper = mapper;
            _env = env;
            _hubContext = hubContext;
            if (string.IsNullOrEmpty(_env.WebRootPath))
            {
                _env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }
        }
        [HttpGet("GetAllGroups")]
        public async Task<IActionResult> GetAllGroup()
        {
            try
            {
                var id = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var res = await _groupService.GetAllGroup(id);
                var groups = _mapper.Map<List<GroupModel>>(res);
                return Ok(groups);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("Group/{id}")]
        public async Task<IActionResult> GetGroup(int id)
        {
            try
            {
                var res = await _groupService.GetGroup(id);
                var group = _mapper.Map<GroupModel>(res);
                return Ok(group);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [AllowAnonymous]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateGroup([FromForm] CreateGroupModel model)
        {
            try
            {
                var id = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                string path = null;
                if (model.icon.Length > 0)
                {
                    string folder = "GroupIcon";
                    string uploadPath = Path.Combine(_env.WebRootPath, folder);
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    path = uploadPath + '/' + model.icon.FileName;
                    using (FileStream filestream = System.IO.File.Create(path))
                    {
                        model.icon.CopyTo(filestream);
                        filestream.Flush();
                    }
                }
                var group = new GroupDto()
                {
                    Name = model.Name,
                    Icon = (path.IsNullOrEmpty())?null: "GroupIcon" + '/' + model.icon.FileName,
                    CreatedBy = id,
                };
                var res = await _groupService.Create(group, model.users);
                var groupModel = _mapper.Map<GroupModel>(res);
                if (groupModel != null)
                {
                    foreach (var user in groupModel.Users)
                    {
                        var connectedUser = BuzzChatHub._connectedUsers.FirstOrDefault(x => x.Key == user.Id);
                        if (connectedUser.Value != null && user.Id != id)
                        {
                            await _hubContext.Clients.Client(connectedUser.Value.ConnectionId).NewGroupCreated(groupModel);
                        }
                    }
                }
                return Ok(groupModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
