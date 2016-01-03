using System.Data.Entity.ModelConfiguration;

namespace StudentNotes.Repositories.DbModels
{
    public class FileMap : EntityTypeConfiguration<File>
    {
        public FileMap()
        {
            HasKey(f => f.FileId);
        }
    }
}
