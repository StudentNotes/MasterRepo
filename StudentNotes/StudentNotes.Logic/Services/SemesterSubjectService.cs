using System;
using System.Collections.Generic;
using System.Linq;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Repositories.DbModels;
using StudentNotes.Repositories.Infrastructure;
using StudentNotes.Repositories.RepositoryInterfaces;

namespace StudentNotes.Logic.Services
{
    public class SemesterSubjectService : ISemesterSubjectService
    {
        private readonly ISemesterRepository _semesterRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ISemesterSubjectRepository _semesterSubjectRepository;
        private readonly IFileTagPatternRepository _fileTagPatternRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SemesterSubjectService(ISemesterRepository semesterRepository, ISubjectRepository subjectRepository, ISemesterSubjectRepository semesterSubjectRepository, 
            IFileTagPatternRepository fileTagPatternRepository, IUnitOfWork unitOfWork)
        {
            _semesterRepository = semesterRepository;
            _subjectRepository = subjectRepository;
            _semesterSubjectRepository = semesterSubjectRepository;
            _fileTagPatternRepository = fileTagPatternRepository;
            _unitOfWork = unitOfWork;
        }

        public Semester GetSemesterById(int semesterId)
        {
            return _semesterRepository.GetById(semesterId);
        }

        public SemesterSubject GetSemesterSubjectById(int semesterSubjectId)
        {
            return _semesterSubjectRepository.GetById(semesterSubjectId);
        }

        public IEnumerable<SemesterSubject> GetSemesterSubjects(int semesterId)
        {
            var semesterSubjects = _semesterSubjectRepository.GetMany(ss => ss.SemesterId == semesterId);
            return semesterSubjects;
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

        public IEnumerable<FileTagPattern> GetAllFileTagPatterns()
        {
            var fileTagPatterns = _fileTagPatternRepository.GetAll();
            return fileTagPatterns;
        }

        public void AddTagAndSave(string tagName)
        {
            _fileTagPatternRepository.Add(new FileTagPattern()
            {
                Name = tagName
            });
            _fileTagPatternRepository.Commit();
        }

        public void DeleteTagAndSave(string tagName)
        {
            _fileTagPatternRepository.Delete(t => t.Name == tagName);
            _fileTagPatternRepository.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
