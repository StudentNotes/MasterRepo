using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using StudentNotes.Repositories.Base;
using StudentNotes.Repositories.DbModels;
using StudentNotes.Repositories.Infrastructure;
using StudentNotes.Repositories.RepositoryInterfaces;

namespace StudentNotes.Repositories.Repositories
{
    public class FileRepository : RepositoryBase<File>, IFileRepository
    {
        public FileRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public List<File> GetByTag(string tags)
        {
            var resultFiles = new List<File>();

            var tagList = tags.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (var tag in tagList)
            {
                string query = $"SELECT * FROM [SNDataBase].[dbo].[File] WHERE CONTAINS (FileTags, '\"{tag}\"')";

                var result = base.DbContext.File.SqlQuery(query).ToList();
                foreach (var resultItem in result)
                {
                    var file = new File()
                    {
                        FileId = resultItem.FileId,
                        Name = resultItem.Name,
                        Category = resultItem.Category,
                        Path = resultItem.Path,
                        Size = resultItem.Size,
                        UploadDate = resultItem.UploadDate,
                        IsShared = resultItem.IsShared,
                        FileTags = resultItem.FileTags,
                        UserId = resultItem.UserId
                    };

                    if (!resultFiles.Any(f => f.FileId == file.FileId))
                    {
                        resultFiles.Add(file);
                    }
                }
            }

            return resultFiles;
        }
    }
}
