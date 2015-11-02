using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Repositories.DbModels;
using StudentNotes.Repositories.RepositoryInterfaces;

namespace StudentNotes.Logic.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly IUserVisitedSchoolRepository _userVisitedSchoolRepository;

        public SchoolService(ISchoolRepository schoolRepository,
            IUserVisitedSchoolRepository userVisitedSchoolRepository)
        {
            _schoolRepository = schoolRepository;
            _userVisitedSchoolRepository = userVisitedSchoolRepository;
        }

        public School GetSchoolByName(string schoolName)
        {
            var school = _schoolRepository.Get(s => s.Name == schoolName);
            return school;
        }

        public IEnumerable<School> GetByUser(int userId)
        {
            var UsrSchls = _userVisitedSchoolRepository.GetMany(uvs => uvs.UserId == userId).ToList();

            List<School> schools = new List<School>();

            foreach (var UsrSchl in UsrSchls)
            {
                schools.Add(_schoolRepository.Get(s => s.SchoolId == UsrSchl.SchoolId));
            }

            return schools;
        }

        public IEnumerable<School> GetAllSchhools()
        {
            return _schoolRepository.GetAll().ToList();
        }

        public int AddSchool(string name, string description)
        {
            _schoolRepository.AddAndSave(new School()
            {
                Name = name,
                Description = description
            });

            return 0;
        }
    }
}
