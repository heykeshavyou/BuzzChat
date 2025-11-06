using AutoMapper;
using BuzzTalk.Business.Dtos;
using BuzzTalk.Data.Repositories;

namespace BuzzTalk.Business.Services
{
    public interface IGroupService
    {
        Task<List<GroupDto>> GetAllGroup(int id);
        Task<GroupDto> GetGroup(int id);
    }
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GroupService(IGroupRepository groupRepository,IMapper mapper)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        public async Task<List<GroupDto>> GetAllGroup(int id)
        {
            try
            {
            var res = await _groupRepository.GetAllGroups(id);
            return _mapper.Map<List<GroupDto>>(res);
            }catch(Exception ex)
            {
                throw ex;
            }

        }

        public async Task<GroupDto> GetGroup(int id)
        {
            var res = await _groupRepository.GetGroupById(id);
            return _mapper.Map<GroupDto>(res);
        }
    }
}
