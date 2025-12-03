using AutoMapper;
using BuzzTalk.Business.Dtos;
using BuzzTalk.Data.Entities;
using BuzzTalk.Data.Repositories;

namespace BuzzTalk.Business.Services
{
    public interface IGroupService
    {
        Task<List<GroupDto>> GetAllGroup(int id);
        Task<GroupDto> GetGroup(int id);
        Task<GroupDto> Create(GroupDto group, List<int> users);
        Task<List<(int, string)>> GetGroupAllUserToken(int id, int userId);
    }
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GroupService(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        public async Task<GroupDto> Create(GroupDto group, List<int> users)
        {
            var res = await _groupRepository.CreateGroup(_mapper.Map<Group>(group), users);
            return _mapper.Map<GroupDto>(res);
        }

        public async Task<List<GroupDto>> GetAllGroup(int id)
        {
            try
            {
                var res = await _groupRepository.GetAllGroups(id);
                return _mapper.Map<List<GroupDto>>(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<GroupDto> GetGroup(int id)
        {
            var res = await _groupRepository.GetGroupById(id);
            return _mapper.Map<GroupDto>(res);
        }

        public async Task<List<(int, string)>> GetGroupAllUserToken(int id, int userId)
        {
            var tokens = await _groupRepository.GetGroupAllUserToken(id, userId);
            return tokens;
        }
    }
}
