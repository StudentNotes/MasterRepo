using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.ViewModels.LoggedIn;
using StudentNotes.Repositories.DbModels;
using StudentNotes.Repositories.Infrastructure;
using StudentNotes.Repositories.RepositoryInterfaces;

namespace StudentNotes.Logic.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly IUserVisitedSchoolRepository _userVisitedSchoolRepository;
        private readonly IStudySubjectRepository _studySubjectRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly ISemesterSubjectRepository _semesterSubjectRepository;
        private readonly ISemesterUserRepository _semesterUserRepository;

        private readonly IUnitOfWork _unitOfWork;

        public SchoolService(ISchoolRepository schoolRepository,
            IUserVisitedSchoolRepository userVisitedSchoolRepository, IGradeRepository gradeRepository, IStudySubjectRepository studySubjectRepository, ISemesterRepository semesterRepository, 
            ISemesterSubjectRepository semesterSubjectRepository, ISemesterUserRepository semesterUserRepository, IUnitOfWork unitOfWork)
        {
            _schoolRepository = schoolRepository;
            _userVisitedSchoolRepository = userVisitedSchoolRepository;
            _gradeRepository = gradeRepository;
            _studySubjectRepository = studySubjectRepository;
            _semesterRepository = semesterRepository;
            _semesterSubjectRepository = semesterSubjectRepository;
            _semesterUserRepository = semesterUserRepository;
            _unitOfWork = unitOfWork;
        }

        public School GetSchoolByName(string schoolName)
        {
            var school = _schoolRepository.Get(s => s.Name == schoolName);
            return school;
        }

        public School GetSchoolById(int schoolId)
        {
            return _schoolRepository.GetById(schoolId);
        }

        public IEnumerable<School> GetByUser(int userId)
        {
            var UsrSchls = _userVisitedSchoolRepository.GetMany(uvs => uvs.UserId == userId).ToList();

            return UsrSchls.Select(UsrSchl => _schoolRepository.Get(s => s.SchoolId == UsrSchl.SchoolId)).ToList();
        }

        public IEnumerable<School> GetAllSchools()
        {
            return _schoolRepository.GetAll().ToList();
        }

        public IEnumerable<School> GetSchoolsByTerm(string term)
        {
            return _schoolRepository.GetMany(s => s.Name.StartsWith(term));
        }

        public int AddSchoolAndSave(string name, string description)
        {
            _schoolRepository.AddAndSave(new School()
            {
                Name = name,
                Description = description
            });

            return 0;
        }

        public int AddSchool(string name, string description)
        {
            _schoolRepository.Add(new School()
            {
                Name = name,
                Description = description
            });

            return 0;
        }

        public bool UserAddedToSchool(int userId, int schoolId)
        {
            var school = _userVisitedSchoolRepository.GetMany(us => us.SchoolId == schoolId).ToList();
            if (school.Any(us => us.UserId == userId))
            {
                return true;
            }
            return false;
        }

        public int AddGradeToSchool(int schoolId, string year)
        {
            var school = _schoolRepository.GetById(schoolId);
            school.Grade.Add(new Grade()
            {
                Year = year
            });

            _schoolRepository.Commit();
            return 0;
        }

        public int RemoveGradeFromSchool(int schoolId, string year)
        {
            _gradeRepository.Delete(g => g.Year == year && g.SchoolId == schoolId);
            _gradeRepository.Commit();

            return 0;
        }

        public int RemoveSchool(int schoolId)
        {
            var gradesToDelete = _gradeRepository.GetMany(g => g.SchoolId == schoolId);
            var gradeIds = gradesToDelete.Select(x => x.GradeId).ToList();
            var studySubjectsToDelete = _studySubjectRepository.GetMany(ss => gradeIds.Contains(ss.GradeId)).ToList();

            var studySubjectIds = studySubjectsToDelete.Select(x => x.StudySubjectId).ToList();
            var semestersToDelete = _semesterRepository.GetMany(s => studySubjectIds.Contains(s.StudySubjectId)).ToList();

            foreach (var semester in semestersToDelete)
            {
                _semesterRepository.Delete(semester);
            }
            foreach (var studySubject in studySubjectsToDelete)
            {
                _studySubjectRepository.Delete(studySubject);
            }
            foreach (var grade in gradesToDelete)
            {
                _gradeRepository.Delete(grade);
            }


            _schoolRepository.Delete(s => s.SchoolId == schoolId);

            _unitOfWork.Commit();
            return 0;
        }

        public IEnumerable<Grade> GetAllSchoolGrades(int schoolId)
        {
            return _gradeRepository.GetMany(gr => gr.SchoolId == schoolId);
        }

        public MyUniversitiesViewModel GetStudySubjectsBySchoolAndUserId(int schoolId, int userId)
        {
            MyUniversitiesViewModel model = new MyUniversitiesViewModel();
            var school = _schoolRepository.GetById(schoolId);
            var semesters = _semesterUserRepository.GetMany(su => su.UserId == userId).ToList();
            var semesterList = semesters.Select(semester => semester.Semester).ToList();
            var studySubjectList = semesterList.Select(g => g.StudySubject).ToList();

            foreach (var studySubject in studySubjectList)
            {
                if (studySubject.Grade.SchoolId == schoolId)
                {
                    if (model.StudySubjects.Contains(studySubject))
                    {
                        continue;
                    }
                    model.Grades.Add(studySubject.Grade);
                    model.StudySubjects.Add(studySubject);
                }             
            }

            return model;
        }

        public IEnumerable<StudySubject> GetStudySubjectByGradeId(int gradeId)
        {
            return _studySubjectRepository.GetMany(ss => ss.GradeId == gradeId);
        }

        public School GetSchoolByGradeId(int gradeId)
        {
            return _schoolRepository.Get(s => s.Grade.Any(g => g.GradeId == gradeId));
        }

        public IEnumerable<StudySubject> GetAllStudySubjectsFromGrade(int gradeId)
        {
            return _studySubjectRepository.GetMany(s => s.GradeId == gradeId);
        }

        public int RemoveStudySubjectById(int studySubjectId)
        {
            _semesterRepository.Delete(sem => sem.StudySubjectId == studySubjectId);
            _studySubjectRepository.Delete(ss => ss.StudySubjectId == studySubjectId);
            _studySubjectRepository.Commit();

            return 0;
        }

        public int AddStudySubjectToGrade(int gradeId, string studySubjectName, string studySubjectDescription, int semesterCount)
        {
            List<Semester> semesters = new List<Semester>();
            for (int i = 0; i < semesterCount; i++)
            {
                semesters.Add(new Semester()
                {
                    SemesterNumber = (i+1)
                });
            }
            _studySubjectRepository.AddAndSave(new StudySubject()
            {
                Name = studySubjectName,
                Description = studySubjectDescription,
                Semester = semesters,
                GradeId = gradeId
            });

            return 0;
        }

        public bool UserJoinedStudySubject(int userId, int studySubjectId)
        {
            var studySubject = _studySubjectRepository.GetById(studySubjectId);
            if (studySubject == null)
            {
                return false;
            }
            var semesters = studySubject.Semester.ToList();
            foreach (var semester in semesters)
            {
                if (semester.SemesterUser.Any(su => su.UserId == userId))
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<Semester> GetSemestersByStudySubjectId(int studySubjectId)
        {
            return _semesterRepository.GetMany(s => s.StudySubjectId == studySubjectId);
        }

        public IEnumerable<SemesterSubject> GetSemesterSubjectsBySemesterId(int semesterId)
        {
            return _semesterSubjectRepository.GetMany(s => s.SemesterId == semesterId);
        }

        public int AddSemesterSubject(int semesterId, string semesterSubjectName)
        {
            _semesterSubjectRepository.AddAndSave(new SemesterSubject()
            {
                Name = semesterSubjectName,
                SemesterId = semesterId
            });

            return 0;
        }

        public int RemoveSemesterSubjectById(int semesterSubjectId)
        {
            _semesterSubjectRepository.Delete(s => s.SemesterSubjectId == semesterSubjectId);
            _semesterSubjectRepository.Commit();
            return 0;
        }

        public int AddUserToSchool(int userId, int schoolId)
        {
            _userVisitedSchoolRepository.Add(new UserVisitedSchool()
            {
                UserId = userId,
                SchoolId = schoolId
            });
            return 0;
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
