using System.Collections.Generic;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ServiceInterfaces
{
    public interface ISemesterSubjectService
    {
        Semester GetSemesterById(int semesterId);
        SemesterSubject GetSemesterSubjectById(int semesterSubjectId);
        IEnumerable<SemesterSubject> GetSemesterSubjects(int semesterId);
        IEnumerable<Subject> GetAllSubjects(); 
        void AddAndSaveSubject(string subjectName);
        void AddSubject(string subjectName);
        void DeleteSubjectAndSave(string subjectName);

        IEnumerable<FileTagPattern> GetAllFileTagPatterns(); 
        void AddTagAndSave(string tagName);
        void DeleteTagAndSave(string tagName);
        void Commit();
    }
}
