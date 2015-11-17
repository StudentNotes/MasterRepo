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
            var allUserFiles = _fileRepository.GetMany(f => f.UserId == userId).ToList();
            var allUserFileIds = allUserFiles.Select(f => f.FileId).ToList();

            var semesterSubjectFiles = _semesterSubjectFileRepository.GetMany(ss => allUserFileIds.Contains(ss.FileId)).Select(f => f.FileId).ToList();

            allUserFiles.RemoveAll(file => semesterSubjectFiles.Contains(file.FileId));

            return allUserFiles;
        }

        public IEnumerable<File> GetFilesBySemesterSubjectId(int semesterSubjectId)
        {
            var fileIds =
                _semesterSubjectFileRepository.GetMany(ssf => ssf.SemesterSubjectId == semesterSubjectId)
                    .Select(f => f.FileId).ToList();
            var files = _fileRepository.GetMany(f => fileIds.Contains(f.FileId));
            return files;
        }

        public IEnumerable<File> GetSemesterSubjectFilesByUserId(int semesterSubjectId, int userId)
        {
            var semesterSubjectFileIds =
                _semesterSubjectFileRepository.GetMany(f => f.SemesterSubjectId == semesterSubjectId).Select(f => f.FileId);

            var userFiles = from f in _fileRepository.GetMany(sf => semesterSubjectFileIds.Contains(sf.FileId))
                where f.UserId == userId
                select f;

            return userFiles;
        }

        public IEnumerable<File> GetSharedUserFiles(int userId)
        {
            var sharedFiles = _fileRepository.GetMany(u => u.UserId == userId && u.IsShared == false);
            return sharedFiles;
        }

        public File GetFileById(int fileId)
        {
            return _fileRepository.GetById(fileId);
        }

        public void SaveFile()
        {
            _unitOfWork.Commit();
        }
    }
}
