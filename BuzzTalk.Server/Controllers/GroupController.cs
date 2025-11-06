using AutoMapper;
using BuzzTalk.Business.Dtos;
using BuzzTalk.Business.Services;
using BuzzTalk.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public GroupController(IGroupService groupService, IMapper mapper)
        {
            _groupService = groupService;
            _mapper = mapper;
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


    }
}
