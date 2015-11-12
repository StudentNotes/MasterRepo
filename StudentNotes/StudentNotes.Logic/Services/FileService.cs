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
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly ISemesterSubjectFileRepository _semesterSubjectFileRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FileService(IFileRepository fileRepository, ISemesterSubjectFileRepository semesterSubjectFileRepository, IUnitOfWork unitOfWork)
        {
            _fileRepository = fileRepository;
            _semesterSubjectFileRepository = semesterSubjectFileRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<File> GetPrivateFiles(int userId)
        {
            var privateFiles = _fileRepository.GetMany(f => f.UserId == userId);

            return privateFiles;
        }

        public IEnumerable<File> GetFilesBySemesterSubjectId(int semesterSubjectId)
        {
            var fileIds =
                _semesterSubjectFileRepository.GetMany(ssf => ssf.SemesterSubjectId == semesterSubjectId)
                    .Select(f => f.FileId).ToList();
            var files = _fileRepository.GetMany(f => fileIds.Contains(f.FileId));
            return files;
        }

        public void SaveFile()
        {
            _unitOfWork.Commit();
        }
    }
}
