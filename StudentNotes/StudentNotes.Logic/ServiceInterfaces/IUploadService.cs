using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.LogicModels;

namespace StudentNotes.Logic.ServiceInterfaces
{
    public interface IUploadService
    {
        int UploadPrivateNote(Note note, int userId);
        void SaveUpload();
        void Commit();
    }
}
