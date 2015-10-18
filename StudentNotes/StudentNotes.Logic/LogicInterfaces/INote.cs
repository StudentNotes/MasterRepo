using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentNotesDal.LogicInterfaces
{
    public interface INote
    {
        int UploadNote();
        int DownloadNote();
        int DeleteNote();
        int MoveNote();
        int AddTag(string tag);
        int RemoveTag(string tag);
    }
}
