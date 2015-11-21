using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ViewModels.LoggedIn;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ServiceInterfaces
{
    public interface ISchoolService
    {
        #region SchoolServices

        School GetSchoolByName(string schoolName);
        School GetSchoolById(int schoolId);
        School GetSchoolByGradeId(int gradeId);
        IEnumerable<School> GetByUser(int userId);
        IEnumerable<School> GetAllSchools();
        IEnumerable<School> GetSchoolsByTerm(string term); 
        int AddSchoolAndSave(string name, string description);
        int AddSchool(string name, string description);
        bool UserAddedToSchool(int userId, int schoolId);

        #endregion

        #region GradeServices
        int AddGradeToSchool(int schoolId, string year);
        int RemoveGradeFromSchool(int schoolId, string year);
        int RemoveSchool(int schoolId);
        IEnumerable<Grade> GetAllSchoolGrades(int schoolId);
        IEnumerable<Grade> GetAllGrades();

            #endregion

        #region StudySubjectServies

        MyUniversitiesViewModel GetStudySubjectsBySchoolAndUserId(int schoolId, int userId); 
        IEnumerable<StudySubject> GetStudySubjectByGradeId(int gradeId);
        IEnumerable<StudySubject> GetAllStudySubjectsFromGrade(int gradeId);
        int RemoveStudySubjectById(int studySubjectId);
        int AddStudySubjectToGrade(int gradeId, string studySubjectName, string studySubjectDescription,
            int semesterCount);

        bool UserJoinedStudySubject(int userId, int studySubjectId);

        #endregion

        #region SemesterServices

        IEnumerable<Semester> GetSemestersByStudySubjectId(int studySubjectId);
        IEnumerable<SecureUserModel> GetUsersBySemesterId(int semesterId);

            #endregion

        #region SemesterSubjectServices

        IEnumerable<SemesterSubject> GetSemesterSubjectsBySemesterId(int semesterId);
        int AddSemesterSubject(int semesterId, string semesterSubjectName);
        int RemoveSemesterSubjectById(int semesterSubjectId);

        #endregion

        #region UserVisitedSchoolServices

        int AddUserToSchool(int userId, int schoolId);

        #endregion

        void Commit();
    }
}
