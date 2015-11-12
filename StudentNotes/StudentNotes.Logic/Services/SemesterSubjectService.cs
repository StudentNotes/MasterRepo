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
    public class SemesterSubjectService : ISemesterSubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SemesterSubjectService(ISubjectRepository subjectRepository, IUnitOfWork unitOfWork)
        {
            _subjectRepository = subjectRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Subject> GetAllSubjects()
        {
            List<Subject> subjectList = _subjectRepository.GetAll().ToList();
            return subjectList;
        }

        public void AddAndSaveSubject(string subjectName)
        {
            _subjectRepository.AddAndSave(new Subject()
            {
                Name = subjectName
            });
        }

        public void AddSubject(string subjectName)
        {
            _subjectRepository.Add(new Subject()
            {
                Name = subjectName
            });
        }

        public void DeleteSubjectAndSave(string subjectName)
        {
            _subjectRepository.Delete(s => s.Name == subjectName);
            _subjectRepository.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
