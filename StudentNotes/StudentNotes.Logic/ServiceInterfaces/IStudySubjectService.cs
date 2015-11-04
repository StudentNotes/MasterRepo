using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ServiceInterfaces
{
    public interface IStudySubjectService
    {
        IEnumerable<Subject> GetAllSubjects(); 
        void AddAndSaveSubject(string subjectName);
        void AddSubject(string subjectName);
        void DeleteSubjectAndSave(string subjectName);
        void Commit();
    }
}
