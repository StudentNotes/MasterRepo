using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Repositories.DbModels;
using StudentNotes.Repositories.Infrastructure;
using StudentNotes.Repositories.RepositoryInterfaces;

namespace StudentNotes.Logic.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupUserRepository _groupUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GroupService(IGroupRepository groupRepository, IGroupUserRepository groupUserRepository, IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _groupUserRepository = groupUserRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Group> GetUserGroups(int userId)
        {
            var userGroups = _groupUserRepository.GetMany(gu => gu.UserId == userId).ToList();
            var userGroupIds = userGroups.Select(ug => ug.GroupId);

            return userGroupIds.Select(userGroupId => _groupRepository.Get(g => g.GroupId == userGroupId)).ToList();
        }
    }
}
